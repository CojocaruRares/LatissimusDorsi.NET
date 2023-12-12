using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Licenta_V2.Server.Models
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; } = String.Empty;

        [BsonElement("Name")]
        public string name { get; set; } = String.Empty;

        [BsonElement("Address")]
        public string address { get; set; } = String.Empty;

        [BsonElement("Age")]
        public int age { get; set; }

        [BsonElement("Height")]
        public int height { get; set; }

        [BsonElement("Weight")]
        public int weight { get; set; }

        [BsonElement("Objective")]
        [BsonRepresentation(BsonType.String)]
        public string Objective { get; set; } = String.Empty;

        [BsonElement("Gender")]
        public byte Gender { get; set; }

        [BsonElement("BodyFatPercentage")]
        public int BodyFatPercentage { get; set; }

    }
}
