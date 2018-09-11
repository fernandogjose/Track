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

namespace Track.Data.Mongo.Repositories {
    public class ConfigurationDataMongoRepository : BaseMongoRepository, IConfigurationDataMongoRepository {

        public ConfigurationDataMongoRepository (string serverName, string database) : base (serverName, database) { }

        public string GetByKey (string key) {
            IMongoCollection<BsonDocument> collection = _database.GetCollection<BsonDocument> ("DadosConfiguracao");
            FilterDefinitionBuilder<BsonDocument> builder = Builders<BsonDocument>.Filter;
            FilterDefinition<BsonDocument> filter = builder.Eq ("Nome", key); // & builder.Eq ("ProductName", "WH-208");
            BsonDocument resultFilter = collection.Find (filter).FirstOrDefault ();

            if (resultFilter == null)
                return null;

            Configuration configurationData = Mapper (resultFilter);

            return JsonConvert.SerializeObject (configurationData);
        }

        private static Configuration Mapper (BsonDocument bsonDocument) {
            Configuration configurationData = new Configuration ();
            configurationData.Nome = bsonDocument.GetValue ("Nome").ToString ();
            configurationData.Valor = bsonDocument.GetValue ("Valor").ToString ();
            configurationData.DataMudanca = Convert.ToDateTime (bsonDocument.GetValue ("DataMudanca").ToString ());
            return configurationData;
        }

        public async Task Add (Configuration configurationData) {
            IMongoCollection<BsonDocument> collection = _database.GetCollection<BsonDocument> ("DadosConfiguracao");
            BsonDocument configurationDataBsonDocument = configurationData.ToBsonDocument ();
            collection.DeleteOne (Builders<BsonDocument>.Filter.Eq ("Nome", configurationData.Nome));
            await collection.InsertOneAsync (configurationData.ToBsonDocument ());
        }
    }
}