using System;
using System.Net;
using System.Threading.Tasks;
using Track.Domain.ClearSale.Interfaces.Proxies;
using Track.Domain.ClearSale.Interfaces.Services;
using Track.Domain.ClearSale.Models;
using Track.Domain.Common.Exceptions;
using Track.Domain.ConfigurationData.Services;
using Track.Domain.ConfigurationData.Interfaces.Services;
using Track.Domain.ConfigurationData.Interfaces.MongoRepositories;
using Track.Domain.ConfigurationData.Models;

namespace Track.Domain.ClearSale.Services {
    public class ClearSaleService : IClearSaleService {
        private readonly IClearSaleProxy _clearSaleProxy;

        private readonly IConfigurationDataMongoRepository _configurationDataMongoRepository;

        private readonly IConfigurationDataCacheService _configurationDataCacheService;

        public ClearSaleService (IClearSaleProxy clearSaleProxy, IConfigurationDataMongoRepository configurationDataMongoRepository, IConfigurationDataCacheService configurationDataCacheService) {
            _clearSaleProxy = clearSaleProxy;
            _configurationDataMongoRepository = configurationDataMongoRepository;
            _configurationDataCacheService = configurationDataCacheService;
        }

        private void IsSendDataLogin () {

            //--- obter do cache (memória -> mongo -> banco)
            Configuration podeExecutarClearSale = _configurationDataCacheService.GetByKey ("CanSendDataLoginClearSale");

            //--- verifica se pode executar, caso contrário retorna um erro de negocio (Não implementado)
            if (podeExecutarClearSale == null || string.IsNullOrEmpty (podeExecutarClearSale.Valor) || podeExecutarClearSale.Valor != "true")
                throw new CustomException ("O envio de dados para o ClearSale está desligado", HttpStatusCode.NotImplemented);
        }

        private static void IsValidSendDataLoginRequest (SendDataLoginRequest sendDataLoginRequest) {
            if (sendDataLoginRequest == null) {
                throw new CustomException ("Request inválido", HttpStatusCode.BadRequest);
            }

            if (string.IsNullOrEmpty (sendDataLoginRequest.Code)) {
                throw new CustomException ("E-mail é obrigatório", HttpStatusCode.BadRequest);
            }

            if (string.IsNullOrEmpty (sendDataLoginRequest.SessionId)) {
                throw new CustomException ("SessionId é obrigatório", HttpStatusCode.BadRequest);
            }
        }

        public async Task<SendDataLoginResponse> SendDataLoginAsync (SendDataLoginRequest sendDataLoginRequest) {
            IsValidSendDataLoginRequest (sendDataLoginRequest);
            IsSendDataLogin ();
            SendDataLoginResponse sendDataLoginResponse = await _clearSaleProxy.SendDataLoginAsync (sendDataLoginRequest);
            return sendDataLoginResponse;
        }

        public async Task<SendDataAccountResponse> SendDataAccountAsync(SendDataAccountRequest sendDataAccountRequest)
        {
            SendDataAccountResponse sendDataAccountResponse = await _clearSaleProxy.SendDataAccountAsync (sendDataAccountRequest);
            return sendDataAccountResponse;
        }
    }
}