using Microsoft.Extensions.DependencyInjection;
using Track.Data.Mongo.Repositories;
using Track.Data.Sql.ConfigurationData.Repositories;
using Track.Domain.ClearSale.Interfaces.Proxies;
using Track.Domain.ClearSale.Interfaces.Services;
using Track.Domain.ClearSale.Services;
using Track.Domain.ConfigurationData.Caches;
using Track.Domain.ConfigurationData.Interfaces.MongoRepositories;
using Track.Domain.ConfigurationData.Interfaces.SqlRepositories;
using Track.Proxy.ClearSale;

namespace Track.DI {
    public class Bootstrap {

        public static void Configure (IServiceCollection services, string mongoServerName, string mongoDatabase, string sqlConnection) {

            //--- Services
            services.AddSingleton<IClearSaleService, ClearSaleService> ();

            //--- Caches
            services.AddSingleton<ConfigurationDataCache>();

            //--- Proxies
            services.AddSingleton<IClearSaleProxy, ClearSaleProxy> ();

            //--- Mongo Repositories
            services.AddSingleton<IConfigurationDataMongoRepository> (p => new ConfigurationDataMongoRepository (mongoServerName, mongoDatabase));

            //--- Mongo Repositories
            services.AddSingleton<IConfigurationDataSqlRepository> (p => new ConfigurationDataSqlRepository (sqlConnection));
        }
    }
}