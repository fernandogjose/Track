using MongoDB.Driver;

namespace Track.Data.Mongo
{
    public class BaseMongoRepository
    {
        public IMongoDatabase _database { get; }

        public BaseMongoRepository (string serverName, string database) {
            MongoClient client = new MongoClient (serverName);
            _database = client.GetDatabase (database);
        }
    }
}