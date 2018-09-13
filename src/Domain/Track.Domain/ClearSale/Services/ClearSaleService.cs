using System;
using System.Net;
using System.Threading.Tasks;
using Track.Domain.ClearSale.Interfaces.Proxies;
using Track.Domain.ClearSale.Interfaces.Services;
using Track.Domain.ClearSale.Models;
using Track.Domain.ConfigurationData.Caches;
using Track.Domain.ConfigurationData.Interfaces.MongoRepositories;
using Track.Domain.ConfigurationData.Models;
using Track.Domain.Common.Exceptions;
using Track.Domain.ConfigurationData.Interfaces.Caches;

namespace Track.Domain.ClearSale.Services {
    public class ClearSaleService : IClearSaleService {
        private readonly IClearSaleProxy _clearSaleProxy;

        private readonly IConfigurationDataMongoRepository _configurationDataMongoRepository;

        private readonly IConfigurationDataCache _configurationDataCache;

        public ClearSaleService (IClearSaleProxy clearSaleProxy, IConfigurationDataMongoRepository configurationDataMongoRepository, IConfigurationDataCache configurationDataCache) {
            _clearSaleProxy = clearSaleProxy;
            _configurationDataMongoRepository = configurationDataMongoRepository;
            _configurationDataCache = configurationDataCache;
        }

        private void IsSendDataLogin () {

            //--- obter do cache (memória -> mongo -> banco)
            Configuration podeExecutarClearSale = _configurationDataCache.GetByKey ("PodeExecutarClearSale");

            //--- verifica se pode executar, caso contrário retorna um erro de negocio (Não implementado)
            if (podeExecutarClearSale == null || string.IsNullOrEmpty (podeExecutarClearSale.Valor) || podeExecutarClearSale.Valor != "true")
                throw new CustomException ("O envio de dados para o ClearSale está desligado", HttpStatusCode.NotImplemented);
        }

        private void IsValidSendDataLoginRequest(SendDataLoginRequest sendDataLoginRequest) {
            if(sendDataLoginRequest == null || sendDataLoginRequest.Account == null || string.IsNullOrEmpty(sendDataLoginRequest.Account.Email)) {
                throw new CustomException ("Request inválido", HttpStatusCode.BadRequest);
            }

            if(string.IsNullOrEmpty(sendDataLoginRequest.Account.Email)) {
                throw new CustomException ("E-mail é obrigatório", HttpStatusCode.BadRequest);
            }

            if(string.IsNullOrEmpty(sendDataLoginRequest.Account.Name)) {
                throw new CustomException ("Nome é obrigatório", HttpStatusCode.BadRequest);
            }
        }

        public async Task<SendDataLoginResponse> SendDataLoginAsync (SendDataLoginRequest sendDataLoginRequest) {
            IsSendDataLogin ();
            SendDataLoginResponse sendDataLoginResponse = await _clearSaleProxy.SendDataLoginAsync (sendDataLoginRequest);
            return sendDataLoginResponse;
        }
    }
}