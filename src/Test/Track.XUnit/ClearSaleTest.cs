using System;
using System.Net;
using System.Threading.Tasks;
using Bogus;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Track.Data.Mongo.Repositories;
using Track.Data.Sql.ConfigurationData.Repositories;
using Track.Domain.ClearSale.Interfaces.Proxies;
using Track.Domain.ClearSale.Interfaces.Services;
using Track.Domain.ClearSale.Models;
using Track.Domain.ClearSale.Services;
using Track.Domain.Common.Exceptions;
using Track.Domain.ConfigurationData.Interfaces.MongoRepositories;
using Track.Domain.ConfigurationData.Interfaces.Services;
using Track.Domain.ConfigurationData.Interfaces.SqlRepositories;
using Track.Domain.ConfigurationData.Models;
using Track.Proxy.ClearSale;
using Xunit;

namespace Track.XUnit {

    public class ClearSaleTest {

        private readonly IServiceCollection _serviceCollection;

        private readonly Mock<IClearSaleProxy> _clearSaleProxyMock;

        private readonly Mock<IConfigurationDataMongoRepository> _configurationDataMongoRepositoryMock;

        private readonly Mock<IConfigurationDataCacheService> _configurationDataCacheServiceMock;

        private readonly Mock<IConfigurationDataSqlRepository> _configurationDataSqlRepositoryMock;

        private readonly IClearSaleService _clearSaleService;

        private readonly Faker _faker;
        private readonly string _name;
        private readonly string _email;

        public ClearSaleTest () {
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
            _serviceCollection.AddSingleton<IConfigurationDataCacheService> (_configurationDataCacheServiceMock.Object);
            _serviceCollection.AddSingleton<IClearSaleService, ClearSaleService> ();

            //--- obter o service
            var services = _serviceCollection.BuildServiceProvider ();
            _clearSaleService = services.GetService<IClearSaleService> ();

            // Dados Fake
            _faker = new Faker ();
            _name = _faker.Person.FirstName;
            _email = _faker.Person.Email;
        }

        private async Task<SendDataLoginResponse> GetSendDataLoginResponse () {
            SendDataLoginResponse sendDataLoginResponse = new SendDataLoginResponse {
                RequestId = "ssddad",
                Account = new SendDataLoginAccountResponse {
                Code = "123456"
                }
            };

            return sendDataLoginResponse;
        }

        [Fact]
        public void MustReturnCustomExceptionWhenKeyPodeExecutarClearsaleIsFalse () {
            SendDataLoginRequest sendDataLoginRequest = new SendDataLoginRequest {
                
            };
            // Task<SendDataLoginResponse> sendDataLoginResponse =  teste();

            const string messageExpected = "O envio de dados para o ClearSale está desligado";

            // //--- Mock do serviço de cache buscando no mongodb
            // _configurationDataMongoRepositoryMock
            //     .Setup (r => r.GetByKey ("PodeExecutarClearSale"))
            //     .Returns ("{\"_id\":\"PODEEXECUTARCLEARSALE\",\"IdDadosConfiguracao\":0,\"IdDadosConfiguracaoAmbiente\":0,\"Ambiente\":null,\"IdDadosConfiguracaoAplicacao\":0,\"Aplicacao\":null,\"IdDadosConfiguracaoGrupo\":0,\"Grupo\":null,\"Nome\":\"PodeExecutarClearSale\",\"Valor\":\"true\",\"DataMudanca\":\"2018-09-12T18:44:53.1802692-03:00\",\"FlagEditavel\":false,\"AlteradoPor\":null}");

            //--- Mock do serviço de cache
            _configurationDataCacheServiceMock
                .Setup (r => r.GetByKey ("PodeExecutarClearSale"))
                .Returns (new Configuration {
                    _id = "PodeExecutarClearSale",
                        Nome = "PodeExecutarClearSale",
                        Valor = "false"
                });

            // _clearSaleProxyMock
            //     .Setup (r => r.SendDataLoginAsync (sendDataLoginRequest))
            //     .Returns(sendDataLoginResponse);

            //--- enviar os dados para o a ClearSale
            var response = Assert.ThrowsAsync<CustomException> (() => _clearSaleService.SendDataLoginAsync (sendDataLoginRequest));
            Assert.Equal (response.Result.Message, messageExpected);
            Assert.Equal (response.Result.HttpStatusCode, HttpStatusCode.NotImplemented);
        }

        [Fact]
        public void MustReturnCustomExceptionWhenKeyPodeExecutarClearsaleIsNull () {
            SendDataLoginRequest sendDataLoginRequest = new SendDataLoginRequest {
                
            };
            // Task<SendDataLoginResponse> sendDataLoginResponse =  teste();

            const string messageExpected = "O envio de dados para o ClearSale está desligado";

            // //--- Mock do serviço de cache buscando no mongodb
            // _configurationDataMongoRepositoryMock
            //     .Setup (r => r.GetByKey ("PodeExecutarClearSale"))
            //     .Returns ("{\"_id\":\"PODEEXECUTARCLEARSALE\",\"IdDadosConfiguracao\":0,\"IdDadosConfiguracaoAmbiente\":0,\"Ambiente\":null,\"IdDadosConfiguracaoAplicacao\":0,\"Aplicacao\":null,\"IdDadosConfiguracaoGrupo\":0,\"Grupo\":null,\"Nome\":\"PodeExecutarClearSale\",\"Valor\":\"true\",\"DataMudanca\":\"2018-09-12T18:44:53.1802692-03:00\",\"FlagEditavel\":false,\"AlteradoPor\":null}");

            //--- Mock do serviço de cache
            _configurationDataCacheServiceMock
                .Setup (r => r.GetByKey ("PodeExecutarClearSale"));

            // _clearSaleProxyMock
            //     .Setup (r => r.SendDataLoginAsync (sendDataLoginRequest))
            //     .Returns(sendDataLoginResponse);

            //--- enviar os dados para o a ClearSale
            var response = Assert.ThrowsAsync<CustomException> (() => _clearSaleService.SendDataLoginAsync (sendDataLoginRequest));
            Assert.Equal (response.Result.Message, messageExpected);
            Assert.Equal (response.Result.HttpStatusCode, HttpStatusCode.NotImplemented);
        }
    }
}