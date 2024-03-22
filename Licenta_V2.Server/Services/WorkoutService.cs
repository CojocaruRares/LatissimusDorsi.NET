using LatissimusDorsi.Server.Data;
using LatissimusDorsi.Server.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LatissimusDorsi.Server.Services
{
    public class WorkoutService
    {
        private readonly IMongoCollection<Trainer> _trainerCollection;

        public WorkoutService(IOptions<DatabaseSettings> dbSettings)
        {
            MongoClient client = new MongoClient(dbSettings.Value.Connection);
            IMongoDatabase database = client.GetDatabase(dbSettings.Value.DatabaseName);
            _trainerCollection = database.GetCollection<Trainer>(dbSettings.Value.CollectionTrainers);
        }

        public async Task AddWorkoutAsync(string id, Workout workout)
        {
            var filter = Builders<Trainer>.Filter.Eq(t => t.id, id);
            var update = Builders<Trainer>.Update.Push(t => t.Workouts, workout);

            await _trainerCollection.UpdateOneAsync(filter, update);
        }

        public async Task<List<Workout>> GetWorkoutAsync(string id)
        {
            var trainer = await _trainerCollection.Find(t => t.id == id).FirstOrDefaultAsync();
            if (trainer == null)
                throw new Exception($"trainer with id {id} could not be found");
            else return trainer.Workouts;
        }

        public async Task DeleteWorkoutAsync(string id, int index)
        {
            var filter = Builders<Trainer>.Filter.Eq(t => t.id, id);
            
            var unsetUpdate = Builders<Trainer>.Update.Unset($"Workouts.{index}");
            await _trainerCollection.UpdateOneAsync(filter, unsetUpdate);

            var pullUpdate = Builders<Trainer>.Update.Pull(t => t.Workouts, null);
            await _trainerCollection.UpdateOneAsync(filter, pullUpdate);
        }

        private string CalculateDesiredIntensity(int age, int weight, int height, int genderFactor)
        {
            float intensityFactor = (220 - age) * ((float)weight / height);

            if (intensityFactor >= 60 + genderFactor && intensityFactor <= 70 + genderFactor)
                return "moderate";
            else if (intensityFactor > 70 + genderFactor)
                return "high";
            else
                return "low";
        }

        //get one workout that aligns with users needs
        public async Task<Workout> GetPerfectWorkoutAsync(string objective, int weight, int height, int age, byte gender)
        {
            int genderFactor = (gender==1) ? 10 : 0;
            float intensityFactor = (220 - age) * (weight / height);
            string desiredIntensity = CalculateDesiredIntensity(age, weight, height, genderFactor);
       

            var filter = Builders<Trainer>.Filter.Where(t => t.specialization == objective);
            var trainer = await _trainerCollection.Find(filter).FirstOrDefaultAsync();

            if (trainer != null && trainer.Workouts != null && trainer.Workouts.Count > 0)
            {
                var random = new Random();
                var suitableWorkouts = trainer.Workouts.Where(w => w.intensity == desiredIntensity).ToList();
                if (suitableWorkouts.Count > 0)
                {                               
                    return suitableWorkouts[random.Next(suitableWorkouts.Count)];
                }
                else
                {
                    suitableWorkouts = trainer.Workouts.Where(w => w.intensity == "moderate").ToList();
                    if (suitableWorkouts.Count > 0)
                    {
                        random = new Random();
                        return suitableWorkouts[random.Next(suitableWorkouts.Count)];
                    }
                }
            }
          
            throw new Exception($"No suitable workout found for '{objective}'.");
        }

    }
}
