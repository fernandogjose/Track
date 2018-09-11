using Track.Domain.ConfigurationData.Models;

namespace Track.Domain.ConfigurationData.Interfaces.SqlRepositories
{
    public interface IConfigurationDataSqlRepository
    {
        Configuration GetByKey (string key);
    }
}