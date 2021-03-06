using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Track.Domain.ClearSale.Interfaces.Proxies;
using Track.Domain.ClearSale.Models;
using Track.Domain.Common.Exceptions;
using Track.Domain.ConfigurationData.Interfaces.Services;
using Track.Domain.ConfigurationData.Services;

namespace Track.Proxy.ClearSale {

    public class ClearSaleProxy : BaseProxy, IClearSaleProxy {

        private readonly string _urlApiAccountClearSale;

        private readonly string _urlApiTokenClearSale;

        private readonly string _clearSaleLogin;

        private readonly string _clearSalePassword;

        private static AuthenticationResponse AuthenticationResponse;

        private void ValidateConfigValues () {
            if (string.IsNullOrEmpty (_urlApiAccountClearSale)) {
                throw new CustomException ("A chave urlApiAccountClearSale não está configurada no banco de dados", HttpStatusCode.NotAcceptable, "Track.Proxy.ClearSale.ClearSaleProxy", "ValidateConfigValues");
            }

            if (string.IsNullOrEmpty (_urlApiTokenClearSale)) {
                throw new CustomException ("A chave urlApiTokenClearSale não está configurada no banco de dados", HttpStatusCode.NotAcceptable, "Track.Proxy.ClearSale.ClearSaleProxy", "ValidateConfigValues");
            }

            if (string.IsNullOrEmpty (_clearSaleLogin)) {
                throw new CustomException ("A chave ClearSaleLogin não está configurada no banco de dados", HttpStatusCode.NotAcceptable, "Track.Proxy.ClearSale.ClearSaleProxy", "ValidateConfigValues");
            }

            if (string.IsNullOrEmpty (_clearSalePassword)) {
                throw new CustomException ("A chave ClearSalePassword não está configurada no banco de dados", HttpStatusCode.NotAcceptable, "Track.Proxy.ClearSale.ClearSaleProxy", "ValidateConfigValues");
            }
        }

        public ClearSaleProxy (string urlApiAccountClearSale, string urlApiTokenClearSale, string clearSaleLogin, string clearSalePassword, ILogService logService, bool isDebug) : base (logService, isDebug) {
            _urlApiAccountClearSale = urlApiAccountClearSale;
            _urlApiTokenClearSale = urlApiTokenClearSale;
            _clearSaleLogin = clearSaleLogin;
            _clearSalePassword = clearSalePassword;

            ValidateConfigValues ();
        }

        public async Task<SendDataLoginResponse> SendDataLoginAsync (SendDataLoginRequest sendDataLoginRequest) {

            //--- obter o token
            await GetToken ();

            //--- converter o objeto para json
            string sendDataLoginRequestJson = JsonConvert.SerializeObject (sendDataLoginRequest);

            //--- post
            BaseResponse sendDataLoginResponseJson = await HttpPostAsync ($"{_urlApiAccountClearSale}/Login", sendDataLoginRequestJson, AuthenticationResponse, "SendDataLoginAsync");

            //--- deserializa o json para o o objeto de retorno
            SendDataLoginResponse sendDataLoginResponse = JsonConvert.DeserializeObject<SendDataLoginResponse> (sendDataLoginResponseJson.Contents);

            //--- retorna 
            return sendDataLoginResponse;
        }

        public async Task<SendDataResetPasswordResponse> SendDataResetPasswordAsync (SendDataResetPasswordClearSaleRequest sendDataResetPasswordClearSaleRequest) {

            //--- obter o token
            await GetToken ();

            //--- converter o objeto para json
            string sendDataResetPasswordClearSaleRequestJson = JsonConvert.SerializeObject (sendDataResetPasswordClearSaleRequest);

            //--- post
            BaseResponse sendDataResetPasswordResponseJson = await HttpPostAsync ($"{_urlApiAccountClearSale}/ResetPassword", sendDataResetPasswordClearSaleRequestJson, AuthenticationResponse, "SendDataResetPasswordAsync");

            //--- deserializa o json para o o objeto de retorno
            SendDataResetPasswordResponse sendDataResetPasswordResponse = JsonConvert.DeserializeObject<SendDataResetPasswordResponse> (sendDataResetPasswordResponseJson.Contents);

            //--- retorna 
            return sendDataResetPasswordResponse;
        }

        public async Task<SendDataAccountResponse> SendDataAccountCreateAsync (SendDataAccountRequest sendDataAccountRequest) {

            //--- obter o token
            await GetToken ();

            //--- converter o objeto para json
            string sendDataAccountRequestJson = JsonConvert.SerializeObject (sendDataAccountRequest);

            //--- post
            BaseResponse sendDataLoginResponseJson = await HttpPostAsync (_urlApiAccountClearSale, sendDataAccountRequestJson, AuthenticationResponse, "SendDataAccountCreateAsync");

            if (!IsResultSuccess(sendDataLoginResponseJson.HttpResponseMessage)) {
                if(IsExistingAccount(sendDataLoginResponseJson.Contents))
                {
                    sendDataLoginResponseJson = await HttpPutAsync (_urlApiAccountClearSale, sendDataAccountRequestJson, AuthenticationResponse, "SendDataAccountUpdateAsync");
                }
            }
            //--- deserializa o json para o o objeto de retorno
            SendDataAccountResponse sendDataAccountResponse = JsonConvert.DeserializeObject<SendDataAccountResponse> (sendDataLoginResponseJson.Contents);

            //--- retorna 
            return sendDataAccountResponse;
        }

        public async Task<SendDataAccountResponse> SendDataAccountUpdateAsync (SendDataAccountRequest sendDataAccountRequest) {

            //--- obter o token
            await GetToken ();

            //--- converter o objeto para json
            string sendDataAccountRequestJson = JsonConvert.SerializeObject (sendDataAccountRequest);

            //--- post
            BaseResponse sendDataLoginResponseJson = await HttpPutAsync (_urlApiAccountClearSale, sendDataAccountRequestJson, AuthenticationResponse, "SendDataAccountUpdateAsync");

            //--- deserializa o json para o o objeto de retorno
            SendDataAccountResponse sendDataAccountResponse = JsonConvert.DeserializeObject<SendDataAccountResponse> (sendDataLoginResponseJson.Contents);

            //--- retorna 
            return sendDataAccountResponse;
        }

        private async Task GetToken () {

            if (AuthenticationResponse != null &&
                !string.IsNullOrEmpty (AuthenticationResponse.Token) &&
                Convert.ToDateTime (AuthenticationResponse.ExpirationDate) >= DateTime.Now) {
                return;
            }

            Dictionary<string, string> authenticateRequest = new Dictionary<string, string> ();
            authenticateRequest.Add ("name", _clearSaleLogin);
            authenticateRequest.Add ("password", _clearSalePassword);

            string authenticateRequestJson = JsonConvert.SerializeObject (authenticateRequest);
            BaseResponse authenticationResponse = await HttpPostAsync (_urlApiTokenClearSale, authenticateRequestJson, AuthenticationResponse, "GetToken");

            if ((authenticationResponse.Contents.StartsWith ("{") && authenticationResponse.Contents.EndsWith ("}")) ||
                (authenticationResponse.Contents.StartsWith ("[") && authenticationResponse.Contents.EndsWith ("]"))) {
                AuthenticationResponse = JsonConvert.DeserializeObject<AuthenticationResponse> (authenticationResponse.Contents);
            } else {
                throw new Exception (authenticationResponse.Contents);
            }
        }

        private bool IsExistingAccount (string contents) {
            if (contents.Contains ("existing-account"))
                return true;
            else
                return false;
        }
        private bool IsResultSuccess (HttpResponseMessage statusResponse) {
            if ((int)statusResponse.StatusCode == 200)
                return true;
            else
                return false;
        }

    }
}