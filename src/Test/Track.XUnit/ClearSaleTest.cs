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

        private readonly IServiceCollection _serviceCollection;

        private readonly Mock<IClearSaleProxy> _clearSaleProxyMock;

        private readonly Mock<IConfigurationDataMongoRepository> _configurationDataMongoRepositoryMock;

        private readonly Mock<ConfigurationDataCache> _configurationDataCacheMock;

        private readonly Mock<IConfigurationDataSqlRepository> _configurationDataSqlRepositoryMock;

        private readonly IClearSaleService _clearSaleService;

        public ClearSaleTest () {
            //--- mock
            _clearSaleProxyMock = new Mock<IClearSaleProxy>();
            _configurationDataMongoRepositoryMock = new Mock<IConfigurationDataMongoRepository>();
            _configurationDataSqlRepositoryMock = new Mock<IConfigurationDataSqlRepository>();
            _configurationDataCacheMock = new Mock<ConfigurationDataCache>();            

            //--- configuração do DI
            _serviceCollection = new ServiceCollection();
            _serviceCollection.AddMemoryCache();
            _serviceCollection.AddSingleton<IClearSaleProxy>(_clearSaleProxyMock.Object);
            _serviceCollection.AddSingleton<IConfigurationDataMongoRepository>(_configurationDataMongoRepositoryMock.Object);
            _serviceCollection.AddSingleton<IConfigurationDataSqlRepository>(_configurationDataSqlRepositoryMock.Object);
            _serviceCollection.AddSingleton<ConfigurationDataCache>();
            _serviceCollection.AddSingleton<IClearSaleService, ClearSaleService>();
            
            //--- obter o service
            var services = _serviceCollection.BuildServiceProvider();
            _clearSaleService = services.GetService<IClearSaleService>();
        }

        [Fact]
        public void EnviarRequestVazio () {
            SendDataLoginRequest sendDataLoginRequest = new SendDataLoginRequest ();

            string expectedStatus = "Faulted";

            //--- Mock do serviço de cache buscando no mongodb
            _configurationDataMongoRepositoryMock
                .Setup (r => r.GetByKey ("PodeExecutarClearSale"))
                .Returns ("{\"_id\":\"PODEEXECUTARCLEARSALE\",\"IdDadosConfiguracao\":0,\"IdDadosConfiguracaoAmbiente\":0,\"Ambiente\":null,\"IdDadosConfiguracaoAplicacao\":0,\"Aplicacao\":null,\"IdDadosConfiguracaoGrupo\":0,\"Grupo\":null,\"Nome\":\"PodeExecutarClearSale\",\"Valor\":\"true\",\"DataMudanca\":\"2018-09-12T18:44:53.1802692-03:00\",\"FlagEditavel\":false,\"AlteradoPor\":null}");

            //--- enviar os dados para o login
            var retorno = _clearSaleService.SendDataLoginAsync (sendDataLoginRequest);
           
            Assert.Equal (retorno.Status.ToString(), expectedStatus);
        }
    }
}