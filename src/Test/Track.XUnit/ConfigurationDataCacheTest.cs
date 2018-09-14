using System;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using Track.Domain.ClearSale.Interfaces.Proxies;
using Track.Domain.ClearSale.Interfaces.Services;
using Track.Domain.ClearSale.Models;
using Track.Domain.ConfigurationData.Interfaces.MongoRepositories;
using Track.Domain.ConfigurationData.Interfaces.Services;
using Track.Domain.ConfigurationData.Interfaces.SqlRepositories;
using Track.Domain.ConfigurationData.Models;
using Track.Domain.ConfigurationData.Services;
using Xunit;

namespace Track.XUnit {
    public class ConfigurationDataCacheTest {

        private readonly IServiceCollection _serviceCollection;

        private readonly Mock<IClearSaleProxy> _clearSaleProxyMock;

        private readonly Mock<IConfigurationDataMongoRepository> _configurationDataMongoRepositoryMock;

        private readonly Mock<IConfigurationDataCacheService> _configurationDataCacheServiceMock;

        private readonly Mock<IConfigurationDataSqlRepository> _configurationDataSqlRepositoryMock;

        private readonly IConfigurationDataCacheService _configurationDataCacheService;

        private readonly Faker _faker;
        private readonly string _name;
        private readonly string _email;
        private readonly int _randomInt;
        private readonly string _randomWord;

        public ConfigurationDataCacheTest () {
            //--- mock
            _clearSaleProxyMock = new Mock<IClearSaleProxy> ();
            _configurationDataMongoRepositoryMock = new Mock<IConfigurationDataMongoRepository> ();
            _configurationDataSqlRepositoryMock = new Mock<IConfigurationDataSqlRepository> ();
            _configurationDataCacheServiceMock = new Mock<IConfigurationDataCacheService> ();

            //--- configuração do DI
            _serviceCollection = new ServiceCollection ();
            _serviceCollection.AddMemoryCache ();
            _serviceCollection.AddSingleton<IClearSaleProxy> (_clearSaleProxyMock.Object);
            _serviceCollection.AddSingleton<IConfigurationDataMongoRepository> (_configurationDataMongoRepositoryMock.Object);
            _serviceCollection.AddSingleton<IConfigurationDataSqlRepository> (_configurationDataSqlRepositoryMock.Object);
            _serviceCollection.AddSingleton<IConfigurationDataCacheService, ConfigurationDataCacheService> ();

            //--- obter o service
            var services = _serviceCollection.BuildServiceProvider ();
            _configurationDataCacheService = services.GetService<IConfigurationDataCacheService> ();

            // Dados Fake
            _faker = new Faker ();
            _name = _faker.Person.FirstName;
            _email = _faker.Person.Email;
            _randomInt = _faker.Random.Int (0, int.MaxValue);
            _randomWord = _faker.Random.String (15);
        }

        [Fact]
        public void MustReturnAConfigurationObjectWhenTheKeyIsFoundInMongo () {
            DateTime dataComparacao = DateTime.Now.AddMinutes (-30);

            Configuration configuration = new Configuration {
                _id = "CanSendDataLoginClearSale",
                Nome = "CanSendDataLoginClearSale",
                Valor = "true",
                DataMudanca = DateTime.Now
            };

            string jsonConfiguration = JsonConvert.SerializeObject (configuration);

            _configurationDataMongoRepositoryMock
                .Setup (r => r.GetByKey ("CanSendDataLoginClearSale"))
                .Returns (jsonConfiguration);

            var response = _configurationDataCacheService.GetByKey ("CanSendDataLoginClearSale");
            Assert.Equal (response._id, configuration._id);
            Assert.Equal (response.Nome, configuration.Nome);
            Assert.Equal (response.Valor, configuration.Valor);
            Assert.True (response.DataMudanca > dataComparacao);
        }

        [Fact]
        public void MustReturnAConfigurationObjectWhenTheKeyIsFoundInSQL () {
            DateTime dataComparacao = DateTime.Now.AddMinutes (-30);

            Configuration configuration = new Configuration {
                _id = "CanSendDataLoginClearSale",
                Nome = "CanSendDataLoginClearSale",
                Valor = "true",
                DataMudanca = DateTime.Now
            };

            string jsonConfiguration = JsonConvert.SerializeObject (configuration);

            _configurationDataMongoRepositoryMock
                .Setup (r => r.GetByKey ("CanSendDataLoginClearSale"));

            _configurationDataSqlRepositoryMock
                .Setup (r => r.GetByKey ("CanSendDataLoginClearSale"))
                .Returns (configuration);

            var response = _configurationDataCacheService.GetByKey ("CanSendDataLoginClearSale");
            Assert.Equal (response._id, configuration._id);
            Assert.Equal (response.Nome, configuration.Nome);
            Assert.Equal (response.Valor, configuration.Valor);
            Assert.True (response.DataMudanca > dataComparacao);
        }

        [Fact]
        public void MustReturnNullWhenTheKeyWasNotFoundOnCacheOrMongoOrSQL () {

            _configurationDataMongoRepositoryMock
                .Setup (r => r.GetByKey ("CanSendDataLoginClearSale"));

            _configurationDataSqlRepositoryMock
                .Setup (r => r.GetByKey ("CanSendDataLoginClearSale"))
                .Returns(new Configuration());

            var response = _configurationDataCacheService.GetByKey ("CanSendDataLoginClearSale");
            Assert.True(string.IsNullOrEmpty(response.Valor));
        }
    }
}