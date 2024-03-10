using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace LatissimusDorsi.Server.Models
{
    [BsonIgnoreExtraElements]
    public class TrainingSession
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; } = String.Empty;

        [BsonElement("TrainerId")]
        public string trainerId { get; set; } = String.Empty;

        [BsonElement("Users")]
        public List<string> users { get; set; } = new List<string>();

        [BsonElement("Date")]
        public DateTime startDate { get; set; } = new DateTime();

        [BsonElement("City")]
        public string city { get; set; } = String.Empty;

        [BsonElement("Gym")]
        public string gym {  get; set; } = String.Empty;

        [BsonElement("AvailableSlots")]
        public int slots { get; set; } 
    }
}
