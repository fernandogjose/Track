using System;
using System.Net;
using MongoDB.Driver;
using Track.Domain.Common.Exceptions;

namespace Track.Data.Mongo {
    public class BaseMongoRepository {
        public IMongoDatabase _database { get; }

        public BaseMongoRepository (string serverName, string database) {
            try {
                MongoClient client = new MongoClient (serverName);
                _database = client.GetDatabase (database);
            } catch (Exception ex) {
                string innerException = "";
                if (ex.InnerException != null) {
                    innerException = ex.InnerException.Message;
                }
                string message = $"Message: {ex.Message} - Inner Exception: {innerException}";
                throw new CustomException (message, HttpStatusCode.NotAcceptable, "Track.Data.Mongo/BaseMongoRepository", "BaseMongoRepository - Construtor");
            }

        }
    }
}