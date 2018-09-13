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

    public class ClearSaleProxy : IClearSaleProxy {

        private readonly string _urlApiAccountClearSale;

        private readonly string _urlApiTokenClearSale;

        private readonly string _clearSaleLogin;

        private readonly string _clearSalePassword;

        private static AuthenticationResponse AuthenticationResponse;

        private void ValidateConfigValues () {
            if (string.IsNullOrEmpty (_urlApiAccountClearSale)) {
                throw new CustomException ("A chave urlApiAccountClearSale não está configurada no banco de dados", HttpStatusCode.NotAcceptable);
            }

            if (string.IsNullOrEmpty (_urlApiTokenClearSale)) {
                throw new CustomException ("A chave urlApiAccountClearSale não está configurada no banco de dados", HttpStatusCode.NotAcceptable);
            }

            if (string.IsNullOrEmpty (_clearSaleLogin)) {
                throw new CustomException ("A chave urlApiAccountClearSale não está configurada no banco de dados", HttpStatusCode.NotAcceptable);
            }

            if (string.IsNullOrEmpty (_clearSalePassword)) {
                throw new CustomException ("A chave urlApiAccountClearSale não está configurada no banco de dados", HttpStatusCode.NotAcceptable);
            }
        }

        public ClearSaleProxy (string urlApiAccountClearSale, string urlApiTokenClearSale, string clearSaleLogin, string clearSalePassword) {
            _urlApiAccountClearSale = urlApiAccountClearSale;
            _urlApiTokenClearSale = urlApiTokenClearSale;
            _clearSaleLogin = clearSaleLogin;
            _clearSalePassword = clearSalePassword;

            ValidateConfigValues ();
        }

        private readonly ConfigurationDataCacheService _configurationDataCacheService;

        private async Task<string> SendPost (string url, string request) {
            HttpClient client = new HttpClient ();

            if (AuthenticationResponse != null && !string.IsNullOrEmpty (AuthenticationResponse.Token)) {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ("Bearer", AuthenticationResponse.Token);
            }

            var result = await client.PostAsync (url, new StringContent (request, Encoding.UTF8, "application/json"));
            var contents = await result.Content.ReadAsStringAsync ();

            return contents;
        }

        public async Task<SendDataLoginResponse> SendDataLoginAsync (SendDataLoginRequest sendDataLoginRequest) {

            //--- obter o token
            await GetToken ();

            //--- converter o objeto para json
            string sendDataLoginRequestJson = JsonConvert.SerializeObject (sendDataLoginRequest);

            //--- post
            string sendDataLoginResponseJson = await SendPost ($"{_urlApiAccountClearSale}/Login", sendDataLoginRequestJson);

            //--- deserializa o json para o o objeto de retorno
            SendDataLoginResponse sendDataLoginResponse = JsonConvert.DeserializeObject<SendDataLoginResponse> (sendDataLoginResponseJson);

            //--- retorna 
            return sendDataLoginResponse;
        }
        public async Task<SendDataAccountResponse> SendDataAccountAsync (SendDataAccountRequest sendDataAccountRequest) {

            //--- obter o token
            await GetToken ();

            //--- converter o objeto para json
            string sendDataAccountRequestJson = JsonConvert.SerializeObject (sendDataAccountRequest);

            //--- post
            string sendDataLoginResponseJson = await SendPost (_urlApiAccountClearSale, sendDataAccountRequestJson);

            //--- deserializa o json para o o objeto de retorno
            SendDataAccountResponse sendDataAccountResponse = JsonConvert.DeserializeObject<SendDataAccountResponse> (sendDataLoginResponseJson);

            //--- retorna 
            return sendDataAccountResponse;
        }

        private async Task GetToken () {

            if (AuthenticationResponse != null && !string.IsNullOrEmpty (AuthenticationResponse.Token)) {
                return;
            }

            Dictionary<string, string> authenticateRequest = new Dictionary<string, string> ();
            authenticateRequest.Add ("name", _clearSaleLogin);
            authenticateRequest.Add ("password", _clearSalePassword);

            string authenticateRequestJson = JsonConvert.SerializeObject (authenticateRequest);
            string authenticationResponse = await SendPost (_urlApiTokenClearSale, authenticateRequestJson);

            AuthenticationResponse = JsonConvert.DeserializeObject<AuthenticationResponse> (authenticationResponse);
        }
    }
}