using MongoDB.Bson.Serialization.Attributes;

namespace LatissimusDorsi.Server.Models
{
    public class UserProjection
    {
        [BsonId]
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Objective { get; set; } = string.Empty;
        public string ProfileImage { get; set; } = string.Empty;
    }
}
