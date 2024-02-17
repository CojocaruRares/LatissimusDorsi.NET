using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace LatissimusDorsi.Server.Models
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

        [BsonElement("Age")]
        public int age { get; set; }     

        [BsonElement("Description")]
        public string description { get; set; } = String.Empty;     

        [BsonElement("Motto")]
        public string motto { get; set; } = String.Empty;

        [BsonElement("Gym")]
        public string gym { get; set; } = String.Empty;

        [BsonElement("Specialization")]
        public string specialization { get; set; } = String.Empty;

        [BsonElement("ProfileImage")]
        public string profileImage { get; set; } = String.Empty;


        public List<Workout> Workouts { get; set; } = new List<Workout>();
    }
}
