using Track.Domain.ConfigurationData.Models;

namespace Track.Domain.ConfigurationData.Interfaces.Caches
{
    public interface IConfigurationDataCache
    {
         Configuration GetByKey (string key);
    }
}