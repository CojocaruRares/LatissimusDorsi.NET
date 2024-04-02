using LatissimusDorsi.Server.Data;
using LatissimusDorsi.Server.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

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

        //if the training sessions are too old ( at least a month ) delete them.
        public async Task CleanupSessionsAsync()
        {
            DateTime cutoffDate = DateTime.UtcNow.AddMonths(-1);

            var filter = Builders<TrainingSession>.Filter.Lt(session => session.startDate, cutoffDate);
            await _sessionCollection.DeleteManyAsync(filter);
        }


        public async Task<List<TrainingSession>> GetAvailableAsync()
        {
            var currentDate = DateTime.UtcNow.AddHours(1);
            var list = await _sessionCollection.Find(new BsonDocument()).ToListAsync();
            var filteredlist = new List<TrainingSession>();
            foreach (var session in list)
            {
                if(session.users.Count < session.slots && DateTime.Compare(session.startDate,currentDate) > 0)
                    filteredlist.Add(session);
            }
            return filteredlist;
        }

        public async Task<List<TrainingSession>> GetByTrainer(string trainerId)
        {
            var list = await _sessionCollection.Find(new BsonDocument()).ToListAsync();
            var filteredList = new List<TrainingSession>();
            foreach (var session in list)
            {
                if(session.trainerId == trainerId)
                    filteredList.Add(session);
            }
            return filteredList;
        }

        public async Task<bool> JoinSessionAsync(string sessionId, string userId)
        { 
            var currentDate = DateTime.UtcNow.AddHours(1);
            var filter = Builders<TrainingSession>.Filter.Eq(session => session.id,sessionId);
            var update = Builders<TrainingSession>.Update.AddToSet(session => session.users, userId);

            TrainingSession session = await _sessionCollection.Find(filter).FirstOrDefaultAsync();

            if (session.users.Count < session.slots && DateTime.Compare(session.startDate,currentDate) > 0) {
                await _sessionCollection.UpdateOneAsync(filter, update);
                return true;
            }
            return false;
        }

        public async Task<List<string>> GetUsersAsync(string sessionId)
        {
            var list = await _sessionCollection.Find(session => session.id == sessionId).FirstOrDefaultAsync();
            return list.users;

        }

        public async Task<List<TrainingSession>> GetSessionsByDateAndUidAsync(string userId, DateTime date)
        {
            var onlyDate = date.Date.AddDays(1);
            var filter = Builders<TrainingSession>.Filter.Where(session =>
            session.startDate.Date ==  onlyDate &&
            session.users.Contains(userId));

            return await _sessionCollection.Find(filter).ToListAsync();
        }

    }
}
