
using H3;
using H3.Model;
using Microsoft.AspNetCore.Http;
using NetTopologySuite.Geometries;
using NetTopologySuite.Geometries.Prepared;
using RankingEngine.Domain;
using RankingEngine.Domain.Exceptions;
using RankingEngine.Domain.Repositories;
using RankingEngine.DomainService.Abstractions;
using System.Collections.Concurrent;


namespace RankingEngine.DomainService
{
    public class CityGeoPointService : ICityGeoPointService
    {
        private readonly ICityGeoPointRepository _cityGeoPointRepository;
        private readonly ICityBoundaryRepository _cityBoundaryRepository;
        private readonly IJobSchedulerService _jobSchedulerService;
        private readonly IApiService _apiService;
        public CityGeoPointService(ICityGeoPointRepository cityGeoPointRepository,
            ICityBoundaryRepository cityBoundaryRepository,
            IJobSchedulerService jobSchedulerService,
            IApiService apiService)
        {
            _cityGeoPointRepository = cityGeoPointRepository;
            _cityBoundaryRepository = cityBoundaryRepository;
            _jobSchedulerService = jobSchedulerService;
            _apiService = apiService;
        }

        public async Task<int> GenerateCityGeoPoints(string city, string country, int resolution = 9, CancellationToken ct = default)
        {
            var boundary = await _cityBoundaryRepository.GetCityBoundary(city, country, ct);
            if (boundary is null)
                throw new BaseException($"City:{city} Country:{country} boundary not found.", StatusCodes.Status400BadRequest);

            var geom = BuildGeometryFromBoundary(boundary);

            if (!geom.IsValid)
            {
                var fixedGeom = geom.Buffer(0);
                if (fixedGeom != null && fixedGeom.IsValid) geom = fixedGeom;
                else throw new BaseException("Boundary geometry invalid and cannot be fixed.", StatusCodes.Status400BadRequest);
            }

            var cells = PolyfillGeometry(geom, resolution);

            var docs = cells.Select(h =>
            {
                var c = h.ToLatLng();
                return new CityGeoPoint
                {
                    Id = h.ToString(),
                    City = city,
                    Country = country,
                    Resolution = resolution,
                    Lat = c.Latitude,
                    Lng = c.Longitude
                };
            }).ToList();

            if (docs.Count == 0) return 0;

            await _cityGeoPointRepository.AddRangeAsync(docs);

            return docs.Count;
        }

        private static Geometry BuildGeometryFromBoundary(CityBoundary cb)
        {
            if (cb?.GeoJson == null)
                throw new BaseException("Boundary has no GeoJson.", StatusCodes.Status400BadRequest);

            var gf = new GeometryFactory();

            if (cb.GeoJson.Type.Equals("Polygon", StringComparison.OrdinalIgnoreCase))
            {
                var rings = (cb.GeoJson.Coordinates?.Count == 1)
                            ? cb.GeoJson.Coordinates[0]
                            : cb.GeoJson.Coordinates?.FirstOrDefault();
                if (rings == null) throw new BaseException("Polygon coordinates missing.", StatusCodes.Status400BadRequest);

                return BuildPolygon(rings, gf);
            }
            else if (cb.GeoJson.Type.Equals("MultiPolygon", StringComparison.OrdinalIgnoreCase))
            {
                var polys = new List<Polygon>();
                foreach (var polygonRings in cb.GeoJson.Coordinates ?? new())
                {
                    polys.Add(BuildPolygon(polygonRings, gf));
                }
                return gf.CreateMultiPolygon(polys.ToArray());
            }
            else
            {
                throw new BaseException($"Unsupported GeoJson.Type '{cb.GeoJson.Type}'.", StatusCodes.Status400BadRequest);
            }
        }

        private static Polygon BuildPolygon(List<List<List<double>>> rings, GeometryFactory gf)
        {
            if (rings == null || rings.Count == 0)
                throw new BaseException("Polygon must have at least one ring.", StatusCodes.Status400BadRequest);

            var shell = ToLinearRing(rings[0], gf);
            var holes = new List<LinearRing>();

            for (int i = 1; i < rings.Count; i++)
                holes.Add(ToLinearRing(rings[i], gf));

            return gf.CreatePolygon(shell, holes.Count > 0 ? holes.ToArray() : null);
        }

        private static LinearRing ToLinearRing(List<List<double>> ringCoords, GeometryFactory gf)
        {
            if (ringCoords == null || ringCoords.Count < 4)
                throw new BaseException("Linear ring must have at least 4 positions (closed).", StatusCodes.Status400BadRequest);

            var coords = new List<Coordinate>(ringCoords.Count + 1);

            foreach (var pos in ringCoords)
            {
                if (pos == null || pos.Count < 2)
                    throw new BaseException("Each position must be [lon, lat].", StatusCodes.Status400BadRequest);
                double lon = pos[0];
                double lat = pos[1];
                coords.Add(new Coordinate(lon, lat));
            }

            if (!coords[0].Equals2D(coords[^1]))
                coords.Add(new Coordinate(coords[0].X, coords[0].Y));

            return gf.CreateLinearRing(coords.ToArray());
        }
        private static HashSet<H3Index> PolyfillGeometry(Geometry geom, int res, double? stepDegrees = null)
        {
            var acc = new ConcurrentDictionary<H3Index, byte>();

            if (geom is Polygon p)
            {
                PolyfillPolygon(p, res, acc, stepDegrees);
            }
            else if (geom is MultiPolygon mp)
            {
                for (int i = 0; i < mp.NumGeometries; i++)
                    PolyfillPolygon((Polygon)mp.GetGeometryN(i), res, acc, stepDegrees);
            }
            else
            {
                throw new BaseException("Only Polygon or MultiPolygon are supported.", StatusCodes.Status400BadRequest);
            }

            return new HashSet<H3Index>(acc.Keys);
        }
        private static void PolyfillPolygon(Polygon poly, int res, ConcurrentDictionary<H3Index, byte> acc, double? stepDegrees)
        {
            var env = poly.EnvelopeInternal;
            var prepared = PreparedGeometryFactory.Prepare(poly);
            double step = stepDegrees ?? AutoStepFor(res);

            foreach (double lat in Range(env.MinY, env.MaxY, step))
            {
                for (double lon = env.MinX; lon <= env.MaxX; lon += step)
                {
                    var h = H3Index.FromLatLng(new LatLng(lat, lon), res);
                    if (!h.IsValid) continue;

                    var center = h.ToLatLng();
                    var pt = new Point(center.Longitude, center.Latitude);

                    if (prepared.Covers(pt))
                        acc.TryAdd(h, 0);
                }
            }
        }
        private static IEnumerable<double> Range(double start, double end, double step)
        {
            for (double v = start; v <= end; v += step) yield return v;
        }
        private static double AutoStepFor(int res) => res switch
        {
            <= 7 => 0.006,
            8 => 0.003,
            9 => 0.002,
            10 => 0.0012,
            11 => 0.0007,
            _ => 0.0005
        };
    }

}




