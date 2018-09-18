using System;
using System.Net;
using System.Threading.Tasks;
using Track.Domain.ClearSale.Interfaces.Proxies;
using Track.Domain.ClearSale.Interfaces.Services;
using Track.Domain.ClearSale.Models;
using Track.Domain.Common.Exceptions;
using Track.Domain.ConfigurationData.Interfaces.MongoRepositories;
using Track.Domain.ConfigurationData.Interfaces.Services;
using Track.Domain.ConfigurationData.Models;
using Track.Domain.ConfigurationData.Services;
using Track.Domain.User.Interfaces.SqlRepositories;
using Track.Domain.User.Models;

namespace Track.Domain.ClearSale.Services {
    public class ClearSaleService : IClearSaleService {
        private readonly IClearSaleProxy _clearSaleProxy;

        private readonly IConfigurationDataMongoRepository _configurationDataMongoRepository;

        private readonly IConfigurationDataCacheService _configurationDataCacheService;

        private readonly IUserSqlRepository _userSqlRepository;

        private void CanSendDataLoginClearSale () {

            //--- obter do cache (memória -> mongo -> banco)
            Configuration podeExecutarClearSale = _configurationDataCacheService.GetByKey ("CanSendDataLoginClearSale");

            //--- verifica se pode executar, caso contrário retorna um erro de negocio (Não implementado)
            if (podeExecutarClearSale == null || string.IsNullOrEmpty (podeExecutarClearSale.Valor) || podeExecutarClearSale.Valor != "true")
                throw new CustomException ("O envio de dados para o ClearSale está desligado", HttpStatusCode.NotImplemented);
        }

        private void CanSendDataResetPasswordClearSale () {

            //--- obter do cache (memória -> mongo -> banco)
            Configuration podeExecutarClearSale = _configurationDataCacheService.GetByKey ("CanSendDataResetPasswordClearSale");

            //--- verifica se pode executar, caso contrário retorna um erro de negocio (Não implementado)
            if (podeExecutarClearSale == null || string.IsNullOrEmpty (podeExecutarClearSale.Valor) || podeExecutarClearSale.Valor != "true")
                throw new CustomException ("O envio de dados para o ClearSale está desligado", HttpStatusCode.NotImplemented);
        }

        private void CanSendDataAccountClearSale () {

            //--- obter do cache (memória -> mongo -> banco)
            Configuration podeExecutarClearSale = _configurationDataCacheService.GetByKey ("CanSendDataAccountClearSale");

            //--- verifica se pode executar, caso contrário retorna um erro de negocio (Não implementado)
            if (podeExecutarClearSale == null || string.IsNullOrEmpty (podeExecutarClearSale.Valor) || podeExecutarClearSale.Valor != "true")
                throw new CustomException ("O envio de dados para o ClearSale está desligado", HttpStatusCode.NotImplemented);
        }

        private static void ValidateRequestObject (Object obj) {
            if (obj == null) {
                throw new CustomException ("Objeto request não pode ser null", HttpStatusCode.BadRequest);
            }
        }

        private static void ValidateString (string value, string name) {
            if (string.IsNullOrEmpty (value)) {
                throw new CustomException ($"{name} é obrigatório", HttpStatusCode.BadRequest);
            }
        }

        public ClearSaleService (IClearSaleProxy clearSaleProxy, IConfigurationDataMongoRepository configurationDataMongoRepository, IConfigurationDataCacheService configurationDataCacheService, IUserSqlRepository userSqlRepository) {
            _clearSaleProxy = clearSaleProxy;
            _configurationDataMongoRepository = configurationDataMongoRepository;
            _configurationDataCacheService = configurationDataCacheService;
            _userSqlRepository = userSqlRepository;
        }

        public async Task<SendDataLoginResponse> SendDataLoginAsync (SendDataLoginRequest sendDataLoginRequest) {
            ValidateRequestObject (sendDataLoginRequest);
            ValidateString (sendDataLoginRequest.Code, "Code");
            ValidateString (sendDataLoginRequest.SessionID, "SessionId");
            CanSendDataLoginClearSale ();
            SendDataLoginResponse sendDataLoginResponse = await _clearSaleProxy.SendDataLoginAsync (sendDataLoginRequest);
            return sendDataLoginResponse;
        }

        public async Task<SendDataResetPasswordResponse> SendDataResetPasswordAsync (SendDataResetPasswordRequest sendDataResetPasswordRequest) {
            ValidateRequestObject (sendDataResetPasswordRequest);

            sendDataResetPasswordRequest.Code = _userSqlRepository.GetUserIdByEmail (new GetUserIdByEmailRequest (sendDataResetPasswordRequest.Code)).ToString ();

            ValidateString (sendDataResetPasswordRequest.Code, "Code");
            ValidateString (sendDataResetPasswordRequest.SessionId, "SessionId");
            CanSendDataResetPasswordClearSale ();
            SendDataResetPasswordResponse sendDataResetPasswordResponse = await _clearSaleProxy.SendDataResetPasswordAsync (sendDataResetPasswordRequest);
            return sendDataResetPasswordResponse;
        }

        public async Task<SendDataAccountResponse> SendDataAccountCreateAsync (SendDataAccountRequest sendDataAccountRequest) {
            SendDataAccountResponse sendDataAccountResponse = await _clearSaleProxy.SendDataAccountCreateAsync (sendDataAccountRequest);
            return sendDataAccountResponse;
        }

        public async Task<SendDataAccountResponse> SendDataAccountUpdateAsync (SendDataAccountRequest sendDataAccountRequest) {
            SendDataAccountResponse sendDataAccountResponse = await _clearSaleProxy.SendDataAccountUpdateAsync (sendDataAccountRequest);
            return sendDataAccountResponse;
        }
    }
}