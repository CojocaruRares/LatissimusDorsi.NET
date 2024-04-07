using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace LatissimusDorsi.Server.Models
{
    [BsonIgnoreExtraElements]
    public class Workout
    {
        [BsonElement("Name")]
        public string title { get; set; } = String.Empty;

        [BsonElement("Intensity")]
        public string intensity { get; set; } = String.Empty;

        [BsonElement("Exercises")]
        public Dictionary<string,List<Exercise>> exercises { get; set; } = new Dictionary<string,List<Exercise>>();

    }
}
