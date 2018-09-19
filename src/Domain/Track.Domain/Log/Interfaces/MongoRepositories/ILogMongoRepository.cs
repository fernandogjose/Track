using System.Threading.Tasks;
using Track.Domain.ConfigurationData.Models;

namespace Track.Domain.ConfigurationData.Interfaces.MongoRepositories 
{
    public interface ILogMongoRepository 
    {
        void AddAsync (LogRequest logRequest);
    }
}