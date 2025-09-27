using MongoDB.Bson.Serialization.Attributes;

namespace RankingEngine.Domain
{
    public class CityBoundary
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string DisplayName { get; set; }
        public double LatCenter { get; set; }
        public double LonCenter { get; set; }
        public List<double> BoundingBox { get; set; }
        public GeoJson GeoJson { get; set; }
    }

    public class GeoJson
    {
        public string Type { get; set; }
        public List<List<double>> Coordinates { get; set; }
    }
}
