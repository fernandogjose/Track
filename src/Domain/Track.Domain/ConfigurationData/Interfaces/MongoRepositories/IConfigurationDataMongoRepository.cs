using System.Threading.Tasks;
using Track.Domain.ConfigurationData.Models;

namespace Track.Domain.ConfigurationData.Interfaces.MongoRepositories 
{
    public interface IConfigurationDataMongoRepository 
    {
        string GetByKey (string key);

        Task Add (Configuration configuration);
    }
}