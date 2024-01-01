using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace LatissimusDorsi.Server.Models
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
        public string Objective { get; set; } = String.Empty;

        [BsonElement("Gender")]
        public byte Gender { get; set; }

        [BsonElement("BodyFatPercentage")]
        public int BodyFatPercentage { get; set; }

        [BsonElement("ProfileImage")]
        public string profileImage { get; set; } = String.Empty;

    }
}
