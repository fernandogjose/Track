using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Track.Domain.ClearSale.Interfaces.Proxies;
using Track.Domain.ClearSale.Interfaces.Services;
using Track.Domain.ClearSale.Models;
using Track.Domain.ConfigurationData.Interfaces.MongoRepositories;
using Track.Domain.ConfigurationData.Interfaces.Services;
using Track.Domain.ConfigurationData.Interfaces.SqlRepositories;
using Track.Domain.ConfigurationData.Models;
using Track.Domain.ConfigurationData.Services;
using Xunit;

namespace Track.XUnit
{
    public class ConfigurationDataCacheTest
    {


        private readonly IServiceCollection _serviceCollection;

        private readonly Mock<IClearSaleProxy> _clearSaleProxyMock;

        private readonly Mock<IConfigurationDataMongoRepository> _configurationDataMongoRepositoryMock;

        private readonly Mock<IConfigurationDataCacheService> _configurationDataCacheServiceMock;

        private readonly Mock<IConfigurationDataSqlRepository> _configurationDataSqlRepositoryMock;

        private readonly IClearSaleService _clearSaleService;

        private readonly Faker _faker;
        private readonly string _name;
        private readonly string _email;

        private readonly int _randomInt;

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
            _serviceCollection.AddSingleton<IConfigurationDataCacheService,ConfigurationDataCacheService>();

            //--- obter o service
            var services = _serviceCollection.BuildServiceProvider ();
            _clearSaleService = services.GetService<IClearSaleService> ();

            // Dados Fake
            _faker = new Faker ();
            _name = _faker.Person.FirstName;
            _email = _faker.Person.Email;
            _randomInt = _faker.Random.Int (0, int.MaxValue);
        }


         [Fact]
        public void MustReturnAConfigurationObjectWhenTheKeyIsFoundInMongo () {

            SendDataLoginRequest sendDataLoginRequest = new SendDataLoginRequest {
                Code = _name,
                SessionId = _email
            };

            Configuration configuration = new Configuration{
                    _id = "CanSendDataLoginClearSale",
                        Nome = "CanSendDataLoginClearSale",
                        Valor = "true"
                };

            //--- Mock do serviço de cache
            _configurationDataCacheServiceMock
                .Setup (r => r.GetByKey ("CanSendDataLoginClearSale"))
                .Returns (configuration);

            //--- enviar os dados para o a ClearSale
            var response = _clearSaleService.SendDataLoginAsync (sendDataLoginRequest);
            Assert.Same(response.Result, configuration);
            // Assert.Equal (response.Result.HttpStatusCode, HttpStatusCode.NotImplemented);
        }
    }
}