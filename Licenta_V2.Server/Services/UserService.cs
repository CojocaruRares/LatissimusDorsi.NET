using LatissimusDorsi.Server.Data;
using LatissimusDorsi.Server.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LatissimusDorsi.Server.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _userCollection;

        public UserService(IOptions<DatabaseSettings> dbSettings)
        {
            MongoClient client = new MongoClient(dbSettings.Value.Connection);
            IMongoDatabase database = client.GetDatabase(dbSettings.Value.DatabaseName);
            _userCollection = database.GetCollection<User>(dbSettings.Value.CollectionUsers);
        }

        public async Task CreateAsync(User user)
        {
            await _userCollection.InsertOneAsync(user);
            return;
        }

        public async Task<List<User>> GetAsync()
        {
            return await _userCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<User> GetAsync(string id)
        {
            return await _userCollection.Find(user => user.id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(string id, User user)
        {
            await _userCollection.ReplaceOneAsync(user => user.id == id, user);
            return;
        }

        public async Task DeleteAsync(string id)
        {
            await _userCollection.DeleteOneAsync(user => user.id == id);
            return;
        }

        public async Task<List<UserProjection>> GetPartialAsync()
        {
            var filter = Builders<User>.Filter.Empty;
            var projection = Builders<User>.Projection
                .Include(u => u.id)
                .Include(u => u.name)
                .Include(u => u.address)
                .Include(u => u.Objective)
                .Include(u => u.profileImage);

            var bsonUsers = await _userCollection.Find(filter).Project(projection).ToListAsync();

            var users = bsonUsers.Select(bson =>
            {
                return new UserProjection
                {
                    Id = bson.GetValue("_id").AsObjectId.ToString(),
                    Name = bson.GetValue("Name").AsString,
                    Address = bson.GetValue("Address").AsString,
                    Objective = bson.GetValue("Objective").AsString,
                    ProfileImage = bson.GetValue("ProfileImage").AsString
                };
            }).ToList();

            return users;
        }

    }
}
