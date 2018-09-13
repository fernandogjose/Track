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
<<<<<<< HEAD
using Track.Domain.Common.Exceptions;
=======
using Track.Domain.ConfigurationData.Interfaces.Services;
>>>>>>> 81fe6aa53e637a09b061b172b5d8170dbe515c42
using Track.Domain.ConfigurationData.Services;

namespace Track.Proxy.ClearSale {

    public class ClearSaleProxy : IClearSaleProxy {
<<<<<<< HEAD

        private readonly string _urlApiAccountClearSale;

        private readonly string _urlApiTokenClearSale;

        private readonly string _clearSaleLogin;
=======
        private readonly IConfigurationDataCacheService _configurationDataCacheService;

        public ClearSaleProxy (IConfigurationDataCacheService configurationDataCacheService) {
>>>>>>> 81fe6aa53e637a09b061b172b5d8170dbe515c42

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
<<<<<<< HEAD

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
=======
        public Task<SendDataLoginResponse> SendDataLoginAsync (SendDataLoginRequest sendDataLoginRequest) {
            throw new NotImplementedException ();
        }
        public async Task<SendDataLoginResponse> SendDataAccountAsync (SendDataAccountRequest sendDataLoginRequest) {

            HttpClient client = new HttpClient ();
            string url = GetURL ("UrlApiAccountClearSale");
            string json = JsonConvert.SerializeObject (sendDataLoginRequest);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ("Bearer", GetToken ().Token);
            var result = await client.PostAsync (url, new StringContent (json, Encoding.UTF8, "application/json"));
            var contents = await result.Content.ReadAsStringAsync ();
          
            SendDataLoginResponse response = new SendDataLoginResponse ();
            
            return response;
>>>>>>> 81fe6aa53e637a09b061b172b5d8170dbe515c42
        }

        public async Task<SendDataLoginResponse> SendDataLoginAsync (SendDataLoginRequest sendDataLoginRequest) {
            await GetToken ();

            string sendDataLoginRequestJson = JsonConvert.SerializeObject (sendDataLoginRequest);
            string sendDataLoginResponseJson = await SendPost ($"{_urlApiAccountClearSale}/Login", sendDataLoginRequestJson);

            SendDataLoginResponse sendDataLoginResponse = JsonConvert.DeserializeObject<SendDataLoginResponse> (sendDataLoginResponseJson);

            return sendDataLoginResponse;
        }

        public async Task<SendDataLoginResponse> SendDataAccountAsync (SendDataAccountRequest sendDataLoginRequest) {
            await GetToken ();

            string sendDataLoginRequestJson = JsonConvert.SerializeObject (sendDataLoginRequest);
            string sendDataLoginResponseJson = await SendPost ($"{_urlApiAccountClearSale}/Login", sendDataLoginRequestJson);

            SendDataLoginResponse sendDataLoginResponse = JsonConvert.DeserializeObject<SendDataLoginResponse> (sendDataLoginResponseJson);

            return sendDataLoginResponse;
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