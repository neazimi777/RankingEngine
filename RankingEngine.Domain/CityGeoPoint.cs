namespace RankingEngine.Domain
{
    public class CityGeoPoint
    {
        public string Id { get; set; } 
        public int Resolution { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string City { get; set; } 
        public string Country { get; set; } 
        public bool IsReaded { get; set; } = false;
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    }
}
