using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Track.Data.Mongo.Repositories;
using Track.Data.Sql.ConfigurationData.Repositories;
using Track.Domain.ClearSale.Interfaces.Proxies;
using Track.Domain.ClearSale.Interfaces.Services;
using Track.Domain.ClearSale.Models;
using Track.Domain.ClearSale.Services;
using Track.Domain.ConfigurationData.Caches;
using Track.Domain.ConfigurationData.Interfaces.MongoRepositories;
using Track.Domain.ConfigurationData.Interfaces.SqlRepositories;
using Track.Domain.ConfigurationData.Models;
using Track.Proxy.ClearSale;
using Xunit;

namespace Track.XUnit {

    public class ClearSaleTest {

        private readonly Mock<IClearSaleProxy> _clearsaleProxyMock;

        private readonly Mock<IConfigurationDataMongoRepository> _mongoRepositoryMock;

        private readonly Mock<ConfigurationDataCache> _configurationDataCacheMock;

        private readonly ClearSaleService _clearSaleService;

        private readonly Mock<IConfigurationDataSqlRepository> _sqlRepositoryMock;

        private readonly IServiceCollection _serviceCollection;

        //IMemoryCache memoryCache, IConfigurationDataMongoRepository configurationDataMongoRepository, IConfigurationDataSqlRepository configurationDataSqlRepository

        public ClearSaleTest () {

            _clearsaleProxyMock = new Mock<IClearSaleProxy> ();
            _mongoRepositoryMock = new Mock<IConfigurationDataMongoRepository> ();
            _configurationDataCacheMock = new Mock<ConfigurationDataCache> ();

            _serviceCollection = new ServiceCollection ();
            _serviceCollection.AddSingleton<IClearSaleProxy> (_clearsaleProxyMock.Object);
            _serviceCollection.AddSingleton<IConfigurationDataMongoRepository> (p => new ConfigurationDataMongoRepository ("mongoServerName", "mongoDatabase"));
            _serviceCollection.AddSingleton<ConfigurationDataCache> ();

            var services = _serviceCollection.BuildServiceProvider ();
            _clearSaleService = services.GetService<ClearSaleService> ();
        }

        [Fact]
        public void EnviarRequestVazio () {
            SendDataLoginRequest sendDataLoginRequest = new SendDataLoginRequest ();

            int expectedStatusCode = 200;

            try {

                _configurationDataCacheMock
                    .Setup (r => r.GetByKey ("PodeExecutarClearSale"))
                    .Returns (new Configuration {
                        Nome = "PodeExecutarClearSale",
                        Valor = "true"
                    });

                var retorno = _clearSaleService.SendDataLoginAsync (sendDataLoginRequest);
            } catch (Exception ex) {
                var teste = ex;
            }

            Assert.Equal (200, expectedStatusCode);
        }
    }
}