using System.Threading.Tasks;
using Track.Domain.ConfigurationData.Models;

namespace Track.Domain.ConfigurationData.Interfaces.MongoRepositories 
{
    public interface ILogMongoRepository 
    {
        Task AddAsync (LogRequest logRequest);
    }
}