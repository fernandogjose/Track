using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Track.Domain.ClearSale.Models;
using Track.Domain.Common.Exceptions;

namespace Track.Proxy {

    public class BaseProxy {

        private HttpClient GetHeader (AuthenticationResponse authenticationResponse) {
            HttpClient client = new HttpClient ();

            if (authenticationResponse != null && !string.IsNullOrEmpty (authenticationResponse.Token)) {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ("Bearer", authenticationResponse.Token);
            }

            return client;
        }

        public async Task<string> HttpPostAsync (string url, string request, AuthenticationResponse authenticationResponse) {

            HttpClient client = GetHeader (authenticationResponse);

            var result = await client.PostAsync (url, new StringContent (request, Encoding.UTF8, "application/json"));
            var contents = await result.Content.ReadAsStringAsync ();

            //--- loga se não retornar nada na requisição
            if (string.IsNullOrEmpty (contents)) {
                throw new CustomException ("O serviço na api da clearsale esta com erro, verificar o problema com a clearsale", HttpStatusCode.InternalServerError, "Track.Proxy.BaseProxy", "HttpPostAsync");
            }

            return contents;
        }

        public async Task<string> HttpPutAsync (string url, string request, AuthenticationResponse authenticationResponse) {

            HttpClient client = GetHeader (authenticationResponse);

            var result = await client.PutAsync (url, new StringContent (request, Encoding.UTF8, "application/json"));
            var contents = await result.Content.ReadAsStringAsync ();

            return contents;
        }
    }
}