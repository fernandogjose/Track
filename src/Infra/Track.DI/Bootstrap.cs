using Microsoft.Extensions.DependencyInjection;
using Track.Data.Mongo.ConfigurationData.Repositories;
using Track.Data.Sql.ConfigurationData.Repositories;
using Track.Data.Sql.User.Repositories;
using Track.Domain.ClearSale.Interfaces.Proxies;
using Track.Domain.ClearSale.Interfaces.Services;
using Track.Domain.ClearSale.Services;
using Track.Domain.ConfigurationData.Interfaces.MongoRepositories;
using Track.Domain.ConfigurationData.Interfaces.Services;
using Track.Domain.ConfigurationData.Interfaces.SqlRepositories;
using Track.Domain.ConfigurationData.Models;
using Track.Domain.ConfigurationData.Services;
using Track.Domain.User.Interfaces.SqlRepositories;
using Track.Proxy.ClearSale;

namespace Track.DI {
    public class Bootstrap {

        public static void Configure (IServiceCollection services, string mongoServerName, string mongoDatabase, string sqlConnection, bool isDebug) {

            //--- Services
            services.AddSingleton<IClearSaleService, ClearSaleService> ();
            services.AddSingleton<ILogService, LogService> ();

            //--- Caches
            services.AddMemoryCache ();
            services.AddSingleton<IConfigurationDataCacheService, ConfigurationDataCacheService> ();

            //--- Mongo Repositories
            services.AddSingleton<IConfigurationDataMongoRepository> (p => new ConfigurationDataMongoRepository (mongoServerName, mongoDatabase));
            services.AddSingleton<ILogMongoRepository> (p => new LogMongoRepository (mongoServerName, mongoDatabase));

            //--- SQL Repositories
            services.AddSingleton<IConfigurationDataSqlRepository> (p => new ConfigurationDataSqlRepository (sqlConnection));
            services.AddSingleton<IUserSqlRepository> (p => new UserSqlRepository (sqlConnection));

            //--- Obter chaves de configuração
            var servicesCollection = services.BuildServiceProvider ();
            var _configurationDataCacheService = servicesCollection.GetService<IConfigurationDataCacheService> ();
            Configuration urlApiAccountClearSale = _configurationDataCacheService.GetByKey ("UrlApiAccountClearSale");
            Configuration urlApiTokenClearSale = _configurationDataCacheService.GetByKey ("UrlApiTokenClearSale");
            Configuration clearSaleLogin = _configurationDataCacheService.GetByKey ("ClearSaleLogin");
            Configuration clearSalePassword = _configurationDataCacheService.GetByKey ("ClearSalePassword");

            //--- Proxies
            var logService = servicesCollection.GetService<ILogService> ();
            services.AddSingleton<IClearSaleProxy> (p => new ClearSaleProxy (urlApiAccountClearSale.Valor, urlApiTokenClearSale.Valor, clearSaleLogin.Valor, clearSalePassword.Valor, logService, isDebug));
        }
    }
}