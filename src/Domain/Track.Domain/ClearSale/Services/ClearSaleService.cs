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
                throw new CustomException ("O envio de dados para o ClearSale está desligado", HttpStatusCode.NotImplemented, "Track.Domain.ClearSale.Services.ClearSaleService", "CanSendDataLoginClearSale");
        }

        private void CanSendDataResetPasswordClearSale () {

            //--- obter do cache (memória -> mongo -> banco)
            Configuration podeExecutarClearSale = _configurationDataCacheService.GetByKey ("CanSendDataResetPasswordClearSale");

            //--- verifica se pode executar, caso contrário retorna um erro de negocio (Não implementado)
            if (podeExecutarClearSale == null || string.IsNullOrEmpty (podeExecutarClearSale.Valor) || podeExecutarClearSale.Valor != "true")
                throw new CustomException ("O envio de dados para o ClearSale está desligado", HttpStatusCode.NotImplemented, "Track.Domain.ClearSale.Services.ClearSaleService", "CanSendDataResetPasswordClearSale");
        }

        private void CanSendDataAccountClearSale () {

            //--- obter do cache (memória -> mongo -> banco)
            Configuration podeExecutarClearSale = _configurationDataCacheService.GetByKey ("CanSendDataAccountClearSale");

            //--- verifica se pode executar, caso contrário retorna um erro de negocio (Não implementado)
            if (podeExecutarClearSale == null || string.IsNullOrEmpty (podeExecutarClearSale.Valor) || podeExecutarClearSale.Valor != "true")
                throw new CustomException ("O envio de dados para o ClearSale está desligado", HttpStatusCode.NotImplemented, "Track.Domain.ClearSale.Services.ClearSaleService", "CanSendDataAccountClearSale");
        }

        private static void ValidateRequestObject (Object obj) {
            if (obj == null) {
                throw new CustomException ("Objeto request não pode ser null", HttpStatusCode.BadRequest, "Track.Domain.ClearSale.Services.ClearSaleService", "ValidateRequestObject");
            }
        }

        private static void ValidateString (string value, string name) {
            if (string.IsNullOrEmpty (value)) {
                throw new CustomException ($"{name} é obrigatório", HttpStatusCode.BadRequest, "Track.Domain.ClearSale.Services.ClearSaleService", "ValidateString");
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

            //--- verifica se o request é válido
            ValidateRequestObject (sendDataResetPasswordRequest);

            //--- recupera o id do cliente pelo email
            GetUserIdAndSessionIdByEmailResponse getUserIdAndSessionIdByEmailResponse = _userSqlRepository.GetUserIdAndSessionIdByEmail (new GetUserIdAndSessionIdByEmailRequest (sendDataResetPasswordRequest.Email));
            
            //--- cria o objeto de request para a api da clearsale
            SendDataResetPasswordClearSaleRequest sendDataResetPasswordClearSaleRequest = new SendDataResetPasswordClearSaleRequest{
                Code = getUserIdAndSessionIdByEmailResponse.UserId == 0 ? "" : getUserIdAndSessionIdByEmailResponse.UserId.ToString (),
                SessionId = getUserIdAndSessionIdByEmailResponse.SessionId
            };
            
            //--- valida se o id do cliente e a sessionid foram passadas
            ValidateString (sendDataResetPasswordClearSaleRequest.Code, "Code");
            ValidateString (sendDataResetPasswordClearSaleRequest.SessionId, "SessionId");

            //--- verifica se esta setado para enviar os dados para o clearsale
            CanSendDataResetPasswordClearSale ();

            //--- envia para o clearsale e retorna com o resultado
            SendDataResetPasswordResponse sendDataResetPasswordResponse = await _clearSaleProxy.SendDataResetPasswordAsync (sendDataResetPasswordClearSaleRequest);
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