using LatissimusDorsi.Server.Data;
using LatissimusDorsi.Server.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;


namespace LatissimusDorsi.Server.Services
{
    public class TrainerService
    {
        private readonly IMongoCollection<Trainer> _trainerCollection;

        public TrainerService(IOptions<DatabaseSettings> dbSettings)
        {
            MongoClient client = new MongoClient(dbSettings.Value.Connection);
            IMongoDatabase database = client.GetDatabase(dbSettings.Value.DatabaseName);
            _trainerCollection = database.GetCollection<Trainer>(dbSettings.Value.CollectionTrainers);
        }

        public async Task CreateAsync(Trainer trainer)
        {
            await _trainerCollection.InsertOneAsync(trainer);
            return;
        }

        public async Task<List<Trainer>> GetAsync()
        {
            return await _trainerCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<Trainer> GetAsync(string id)
        {
            return await _trainerCollection.Find(trainer => trainer.id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(string id, Trainer trainer)
        {
            await _trainerCollection.ReplaceOneAsync(trainer => trainer.id == id, trainer);
            return;
        }

        public async Task DeleteAsync(string id)
        {
            await _trainerCollection.DeleteOneAsync(trainer => trainer.id == id);
            return;
        }

        public async Task AddWorkoutAsync(string trainerId, Workout workout)
        {
            var filter = Builders<Trainer>.Filter.Eq(t => t.id, trainerId);
            var update = Builders<Trainer>.Update.Push(t => t.Workouts, workout);

            await _trainerCollection.UpdateOneAsync(filter, update);
        }

    }
}
