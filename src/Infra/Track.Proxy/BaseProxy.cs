using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Track.Domain.ClearSale.Models;
using Track.Domain.Common.Exceptions;
using Track.Domain.ConfigurationData.Interfaces.Services;
using Track.Domain.ConfigurationData.Models;
using Track.Domain.Log.Enums;

namespace Track.Proxy {

    public class BaseProxy {
        private readonly ILogService _logService;

        private readonly bool _isDebug;

        private HttpClient GetHeader (AuthenticationResponse authenticationResponse) {
            HttpClient client = new HttpClient ();

            if (_isDebug) {
                HttpClientHandler handler = new HttpClientHandler () {
                    Proxy = new WebProxy ("http://10.128.131.16:3128"),
                    UseProxy = true,
                };

                client = new HttpClient (handler);

                _logService.AddAsync (new LogRequest { Message = "debug = true" });
            } else {
                _logService.AddAsync (new LogRequest { Message = "debug = false" });
            }

            if (authenticationResponse != null && !string.IsNullOrEmpty (authenticationResponse.Token)) {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ("Bearer", authenticationResponse.Token);
            }

            return client;
        }

        public BaseProxy (ILogService logService, bool isDebug) {
            _logService = logService;
            _isDebug = isDebug;
        }

        private async Task LogRequest (string url, string request, string method, string tokenApi) {
            LogRequest logRequest = new LogRequest {
                StatusCode = StatusCode.Info.ToString (),
                Message = $"Url: {url} - Request: {request} - Token: {tokenApi}",
                Method = method,
                NamespaceClass = "Track.Proxy.BaseProxy",
                LogDate = DateTime.Now
            };

            await _logService.AddAsync (logRequest);
        }

        public async Task<string> HttpPostAsync (string url, string request, AuthenticationResponse authenticationResponse, string method) {

            //--- loga a requisição
            await LogRequest (url, request, method, authenticationResponse == null? "GetToken": authenticationResponse.Token);

            //--- monta o header
            HttpClient client = GetHeader (authenticationResponse);

            //--- faz a chamada da api por post
            var result = await client.PostAsync (url, new StringContent (request, Encoding.UTF8, "application/json"));
            var contents = await result.Content.ReadAsStringAsync ();

            //--- loga se não retornar nada na requisição
            if (string.IsNullOrEmpty (contents)) {
                throw new CustomException ("O serviço na api da clearsale esta com erro, verificar o problema com a clearsale", HttpStatusCode.InternalServerError, "Track.Proxy.BaseProxy", "HttpPostAsync");
            }

            return contents;
        }

        public async Task<string> HttpPutAsync (string url, string request, AuthenticationResponse authenticationResponse, string method) {

            //--- loga a requisição
            await LogRequest (url, request, method, authenticationResponse == null? "GetToken": authenticationResponse.Token);

            //--- monta o header
            HttpClient client = GetHeader (authenticationResponse);

            //--- faz a chamada da api por put
            var result = await client.PutAsync (url, new StringContent (request, Encoding.UTF8, "application/json"));
            var contents = await result.Content.ReadAsStringAsync ();

            //--- loga se não retornar nada na requisição
            if (string.IsNullOrEmpty (contents)) {
                throw new CustomException ("O serviço na api da clearsale esta com erro, verificar o problema com a clearsale", HttpStatusCode.InternalServerError, "Track.Proxy.BaseProxy", "HttpPostAsync");
            }

            return contents;
        }
    }
}