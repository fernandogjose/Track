using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using Track.Domain.ConfigurationData.Interfaces.MongoRepositories;
using Track.Domain.ConfigurationData.Models;

namespace Track.Data.Mongo.ConfigurationData.Repositories {
    public class LogMongoRepository : BaseMongoRepository, ILogMongoRepository {

        public LogMongoRepository (string serverName, string database) : base (serverName, database) { }
        
        public void AddAsync (LogRequest logRequest) {
            IMongoCollection<BsonDocument> collection = _database.GetCollection<BsonDocument> ("LogTrackApi");
            BsonDocument configurationDataBsonDocument = logRequest.ToBsonDocument ();
            collection.InsertOne (logRequest.ToBsonDocument ());
        }
    }
}