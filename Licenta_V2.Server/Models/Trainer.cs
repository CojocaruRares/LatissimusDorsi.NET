using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Licenta_V2.Server.Models
{
    [BsonIgnoreExtraElements]
    public class Trainer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; } = String.Empty;

        [BsonElement("Name")]
        public string name { get; set; } = String.Empty;

        [BsonElement("Address")]
        public string address { get; set; } = String.Empty;

        [BsonElement("Description")]
        public string description { get; set; } = String.Empty;
    }
}
