using LatissimusDorsi.Server.Data;
using LatissimusDorsi.Server.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LatissimusDorsi.Server.Services
{
    public class TrainingSessionService
    {
        private readonly IMongoCollection<TrainingSession> _sessionCollection;

        public TrainingSessionService(IOptions<DatabaseSettings> dbSettings)
        {
            MongoClient client = new MongoClient(dbSettings.Value.Connection);
            IMongoDatabase database = client.GetDatabase(dbSettings.Value.DatabaseName);
            _sessionCollection = database.GetCollection<TrainingSession>(dbSettings.Value.CollectionSessions);
        }

        public async Task CreateAsync(TrainingSession session)
        {
            await _sessionCollection.InsertOneAsync(session);
            return;
        }

        public async Task<List<TrainingSession>> GetAsync()
        {
            return await _sessionCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<TrainingSession> GetAsync(string id)
        {
            return await _sessionCollection.Find(session => session.id == id).FirstOrDefaultAsync();
        }

    }
}
