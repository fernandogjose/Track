using System;
using System.Net;
using System.Threading.Tasks;
using Bogus;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Track.Data.Sql.ConfigurationData.Repositories;
using Track.Data.Sql.User.Repositories;
using Track.Domain.ClearSale.Interfaces.Proxies;
using Track.Domain.ClearSale.Interfaces.Services;
using Track.Domain.ClearSale.Models;
using Track.Domain.ClearSale.Services;
using Track.Domain.Common.Exceptions;
using Track.Domain.ConfigurationData.Interfaces.MongoRepositories;
using Track.Domain.ConfigurationData.Interfaces.Services;
using Track.Domain.ConfigurationData.Interfaces.SqlRepositories;
using Track.Domain.ConfigurationData.Models;
using Track.Domain.User.Interfaces.SqlRepositories;
using Track.Proxy.ClearSale;
using Xunit;

namespace Track.XUnit {

    public class ClearSaleServiceTest {

        private readonly IServiceCollection _serviceCollection;

        private readonly Mock<IClearSaleProxy> _clearSaleProxyMock;

        private readonly Mock<IUserSqlRepository> _userSqlRepositoryMock;

        private readonly Mock<IConfigurationDataMongoRepository> _configurationDataMongoRepositoryMock;

        private readonly Mock<IConfigurationDataCacheService> _configurationDataCacheServiceMock;

        private readonly Mock<IConfigurationDataSqlRepository> _configurationDataSqlRepositoryMock;

        private readonly IClearSaleService _clearSaleService;

        private readonly Faker _faker;
        private readonly string _name;
        private readonly string _email;

        private readonly int _randomInt;

        public ClearSaleServiceTest () {
            //--- mock
            _clearSaleProxyMock = new Mock<IClearSaleProxy> ();
            _configurationDataMongoRepositoryMock = new Mock<IConfigurationDataMongoRepository> ();
            _configurationDataSqlRepositoryMock = new Mock<IConfigurationDataSqlRepository> ();
            _configurationDataCacheServiceMock = new Mock<IConfigurationDataCacheService> ();
            _userSqlRepositoryMock = new Mock<IUserSqlRepository> ();

            //--- configuração do DI
            _serviceCollection = new ServiceCollection ();
            _serviceCollection.AddMemoryCache ();
            _serviceCollection.AddSingleton<IClearSaleProxy> (_clearSaleProxyMock.Object);
            _serviceCollection.AddSingleton<IConfigurationDataMongoRepository> (_configurationDataMongoRepositoryMock.Object);
            _serviceCollection.AddSingleton<IConfigurationDataSqlRepository> (_configurationDataSqlRepositoryMock.Object);
            _serviceCollection.AddSingleton<IConfigurationDataCacheService> (_configurationDataCacheServiceMock.Object);
            _serviceCollection.AddSingleton<IClearSaleService, ClearSaleService> ();
            _serviceCollection.AddSingleton<IUserSqlRepository> (_userSqlRepositoryMock.Object);

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
        public void MustReturnCustomExceptionWhenKeyCanSendDataLoginClearSaleIsFalse () {
            SendDataLoginRequest sendDataLoginRequest = new SendDataLoginRequest {
                Code = _name,
                SessionID = _randomInt.ToString ()
            };

            const string messageExpected = "O envio de dados para o ClearSale está desligado";

            //--- Mock do serviço de cache
            _configurationDataCacheServiceMock
                .Setup (r => r.GetByKey ("CanSendDataLoginClearSale"))
                .Returns (new Configuration {
                    _id = "CanSendDataLoginClearSale",
                        Nome = "CanSendDataLoginClearSale",
                        Valor = "false"
                });

            //--- enviar os dados para o a ClearSale
            var response = Assert.ThrowsAsync<CustomException> (() => _clearSaleService.SendDataLoginAsync (sendDataLoginRequest));
            Assert.Equal (response.Result.Message, messageExpected);
            Assert.Equal (HttpStatusCode.NotImplemented, response.Result.HttpStatusCode);
        }

        [Fact]
        public void MustReturnCustomExceptionWhenKeyCanSendDataLoginClearSaleIsNull () {
            SendDataLoginRequest sendDataLoginRequest = new SendDataLoginRequest {
                Code = _name,
                SessionID = _email
            };

            const string messageExpected = "O envio de dados para o ClearSale está desligado";

            //--- Mock do serviço de cache
            _configurationDataCacheServiceMock
                .Setup (r => r.GetByKey ("CanSendDataLoginClearSale"));

            //--- enviar os dados para o a ClearSale
            var response = Assert.ThrowsAsync<CustomException> (() => _clearSaleService.SendDataLoginAsync (sendDataLoginRequest));
            Assert.Equal (response.Result.Message, messageExpected);
            Assert.Equal (HttpStatusCode.NotImplemented, response.Result.HttpStatusCode);
        }

        [Fact]
        public void MustSuccessfullySendDataToClearSale () {
            SendDataLoginRequest sendDataLoginRequest = new SendDataLoginRequest {
                Code = _name,
                SessionID = _randomInt.ToString ()
            };

            const string messageExpected = "O envio de dados para o ClearSale está desligado";

            //--- Mock do serviço de cache
            _configurationDataCacheServiceMock
                .Setup (r => r.GetByKey ("CanSendDataLoginClearSale"));

            //--- enviar os dados para o a ClearSale
            var response = Assert.ThrowsAsync<CustomException> (() => _clearSaleService.SendDataLoginAsync (sendDataLoginRequest));
            Assert.Equal (response.Result.Message, messageExpected);
            Assert.Equal (HttpStatusCode.NotImplemented, response.Result.HttpStatusCode);
        }

        [Fact]
        public void MustReturnBadRequestStatusWithTheMessageRequestInvalidoWhenRequestIsNullOrInvalid () {
            SendDataLoginRequest sendDataLoginRequest = null;

            const string messageExpected = "Objeto request não pode ser null";

            // //--- Mock do serviço de cache
            // _configurationDataCacheServiceMock
            //     .Setup (r => r.GetByKey ("CanSendDataLoginClearSale"));

            //--- enviar os dados para o a ClearSale
            var response = Assert.ThrowsAsync<CustomException> (() => _clearSaleService.SendDataLoginAsync (sendDataLoginRequest));
            Assert.Equal (messageExpected, response.Result.Message);
            Assert.Equal (HttpStatusCode.BadRequest, response.Result.HttpStatusCode);
        }

        [Fact]
        public void MustReturnBadRequestStatusWithTheMessageCodeEObrigatorioWhenCodeIsEmpty () {
            SendDataLoginRequest sendDataLoginRequest = new SendDataLoginRequest {
                SessionID = _email
            };

            const string messageExpected = "Code é obrigatório";

            //--- Mock do serviço de cache
            _configurationDataCacheServiceMock
                .Setup (r => r.GetByKey ("CanSendDataLoginClearSale"));

            //--- enviar os dados para o a ClearSale
            var response = Assert.ThrowsAsync<CustomException> (() => _clearSaleService.SendDataLoginAsync (sendDataLoginRequest));
            Assert.Equal (response.Result.Message, messageExpected);
            Assert.Equal (HttpStatusCode.BadRequest, response.Result.HttpStatusCode);
        }

        [Fact]
        public void MustReturnBadRequestStatusWithTheMessageSessionIdEObrigatorioWhenSessionIdIsEmpty () {
            SendDataLoginRequest sendDataLoginRequest = new SendDataLoginRequest {
                Code = _name
            };

            const string messageExpected = "SessionId é obrigatório";

            //--- Mock do serviço de cache
            _configurationDataCacheServiceMock
                .Setup (r => r.GetByKey ("CanSendDataLoginClearSale"));

            //--- enviar os dados para o a ClearSale
            var response = Assert.ThrowsAsync<CustomException> (() => _clearSaleService.SendDataLoginAsync (sendDataLoginRequest));
            Assert.Equal (response.Result.Message, messageExpected);
            Assert.Equal (HttpStatusCode.BadRequest, response.Result.HttpStatusCode);
        }

    }
}