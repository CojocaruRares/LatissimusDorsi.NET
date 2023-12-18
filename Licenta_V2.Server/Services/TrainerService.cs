using Licenta_V2.Server.Data;
using Licenta_V2.Server.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;


namespace Licenta_V2.Server.Services
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

    }
}
