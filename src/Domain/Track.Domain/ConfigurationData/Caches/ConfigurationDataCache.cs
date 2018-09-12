using System;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Track.Domain.ConfigurationData.Interfaces.MongoRepositories;
using Track.Domain.ConfigurationData.Interfaces.SqlRepositories;
using Track.Domain.ConfigurationData.Models;

namespace Track.Domain.ConfigurationData.Caches {
    public class ConfigurationDataCache {

        private readonly IMemoryCache _memoryCache;

        private readonly IConfigurationDataMongoRepository _configurationDataMongoRepository;

        private IConfigurationDataSqlRepository _configurationDataSqlRepository;

        public ConfigurationDataCache (IMemoryCache memoryCache, IConfigurationDataMongoRepository configurationDataMongoRepository, IConfigurationDataSqlRepository configurationDataSqlRepository) {
            _memoryCache = memoryCache;
            _configurationDataMongoRepository = configurationDataMongoRepository;
            _configurationDataSqlRepository = configurationDataSqlRepository;
        }

        private Configuration GetByCacheInMemory (string key) {

            //--- recupera do cache e verifica se é null
            string valueCached = _memoryCache.Get<string> (key);
            if (string.IsNullOrEmpty (valueCached)) {
                return null;
            }

            //--- verifica se a data de mudanca já expirou
            valueCached = ValidateExpirationDate (valueCached, 15);

            if (string.IsNullOrEmpty (valueCached)) {
                return null;
            }

            return JsonConvert.DeserializeObject<Configuration> (valueCached);
        }

        private Configuration GetByCacheInMongo (string key) {

            //--- recupera do cache e verifica se é null
            string valueCached = _configurationDataMongoRepository.GetByKey (key);
            if (string.IsNullOrEmpty (valueCached)) {
                return null;
            }

            //--- verifica se a data de mudanca já expirou
            valueCached = ValidateExpirationDate (valueCached, 30);

            if (string.IsNullOrEmpty (valueCached))
                return null;

            return JsonConvert.DeserializeObject<Configuration> (valueCached);
        }

        private string ValidateExpirationDate (string valueCached, int addMinutes) {
            Configuration configurationData = JsonConvert.DeserializeObject<Configuration> (valueCached);
            return Convert.ToDateTime(configurationData.DataMudanca).AddMinutes (addMinutes) < DateTime.Now ? "" : valueCached;
        }

        private void AddCache (Configuration configuration, string key) {
            string valueCached = JsonConvert.SerializeObject (configuration);
            _memoryCache.Set (key, valueCached);
            _configurationDataMongoRepository.Add (configuration);
        }

        public Configuration GetByKey (string key) {

            //--- obter da memória
            Configuration response = GetByCacheInMemory (key);
            if (response != null) {
                return response;
            }

            //--- obter do mongo
            response = GetByCacheInMongo (key);
            if (response != null) {
                return response;
            }

            //--- se não tem no cache, busca no banco de dados
            Configuration configurationData = _configurationDataSqlRepository.GetByKey (key);

            //--- atualiza o cache
            AddCache (configurationData, key);

            return configurationData;
        }
    }
}