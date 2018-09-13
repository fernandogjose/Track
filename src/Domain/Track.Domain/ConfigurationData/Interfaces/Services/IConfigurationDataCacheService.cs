using Track.Domain.ConfigurationData.Models;

namespace Track.Domain.ConfigurationData.Interfaces.Services
{
    public interface IConfigurationDataCacheService
    {
         Configuration GetByKey (string key);
    }
}