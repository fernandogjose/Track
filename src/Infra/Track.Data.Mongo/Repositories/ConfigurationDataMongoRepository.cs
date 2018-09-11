using System;
using System.Collections.ObjectModel;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using Track.Domain.ConfigurationData.Interfaces.MongoRepositories;
using Track.Domain.ConfigurationData.Models;

namespace Track.Data.Mongo.Repositories
{
    public class ConfigurationDataMongoRepository : BaseMongoRepository, IConfigurationDataMongoRepository {

        public ConfigurationDataMongoRepository (string serverName, string database) : base (serverName, database) { }

        public string GetByKey (string key) {
            IMongoCollection<BsonDocument> collection = _database.GetCollection<BsonDocument> ("DadosConfiguracao");
            FilterDefinitionBuilder<BsonDocument> builder = Builders<BsonDocument>.Filter;
            FilterDefinition<BsonDocument> filter = builder.Eq ("Nome", key); // & builder.Eq ("ProductName", "WH-208");
            BsonDocument resultFilter = collection.Find (filter).FirstOrDefault ();
            ConfigurationDataRequest configurationData = Mapper (resultFilter);

            return JsonConvert.SerializeObject (configurationData);
        }

        private static ConfigurationDataRequest Mapper (BsonDocument bsonDocument) {
            ConfigurationDataRequest configurationData = new ConfigurationDataRequest ();
            configurationData.Nome = bsonDocument.GetValue ("Nome").ToString ();
            configurationData.Valor = bsonDocument.GetValue ("Valor").ToString ();
            configurationData.DataMudanca = Convert.ToDateTime (bsonDocument.GetValue ("DataMudanca").ToString ());
            return configurationData;
        }

        public void Add (ConfigurationDataRequest configurationDataRequest) {
            IMongoCollection<BsonDocument> collection = _database.GetCollection<BsonDocument> ("DadosConfiguracao");
            BsonDocument configurationDataBsonDocument = configurationDataRequest.ToBsonDocument ();
            collection.DeleteOne (Builders<BsonDocument>.Filter.Eq ("Nome", configurationDataRequest.Nome));
            collection.InsertOne (configurationDataRequest.ToBsonDocument ());
        }
    }
}