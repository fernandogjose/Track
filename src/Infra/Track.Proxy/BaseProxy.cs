using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Track.Domain.ClearSale.Models;

namespace Track.Proxy {

    public class BaseProxy {

        private HttpClient GetHeader (AuthenticationResponse authenticationResponse) {
            HttpClient client = new HttpClient ();

            if (authenticationResponse != null && !string.IsNullOrEmpty (authenticationResponse.Token)) {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ("Bearer", authenticationResponse.Token);
            }
            
            return client;
        }

        public async Task<string> SendPost (string url, string request, AuthenticationResponse authenticationResponse) {

            HttpClient client = GetHeader(authenticationResponse);

            var result = await client.PostAsync (url, new StringContent (request, Encoding.UTF8, "application/json"));
            var contents = await result.Content.ReadAsStringAsync ();

            return contents;
        }

        public async Task<string> SendPut (string url, string request, AuthenticationResponse authenticationResponse) {

            HttpClient client = GetHeader(authenticationResponse);

            var result = await client.PutAsync (url, new StringContent (request, Encoding.UTF8, "application/json"));
            var contents = await result.Content.ReadAsStringAsync ();

            return contents;
        }
    }
}