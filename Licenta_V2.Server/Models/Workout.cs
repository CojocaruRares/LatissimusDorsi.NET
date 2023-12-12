using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Licenta_V2.Server.Models
{
    [BsonIgnoreExtraElements]
    public class Workout
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; } = String.Empty;

        [BsonElement("Name")]
        public string name { get; set; } = String.Empty;

        [BsonElement("Objective")]
        public string objective { get; set; } = String.Empty;

        [BsonElement("Exercises")]
        public List<Exercise> exercises { get; set; } = new List<Exercise>(7);

    }
}
