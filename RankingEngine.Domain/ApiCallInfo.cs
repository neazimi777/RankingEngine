using MongoDB.Bson.Serialization.Attributes;

namespace RankingEngine.Domain
{
    public class ApiCallInfo
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public string RegisteryEmail { get; set; }
        public string RegisteryEmailPass { get; set; }
        public int MaxCallPerMonth { get; set; }
        public int MaxCallPerHour { get; set; }
        public DateTime LatestCallTime { get; set; }
        public int CallCount { get; set; }
        public TimeSpan DellayPerCall { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime CreationDate { get; set; }

    }
}
