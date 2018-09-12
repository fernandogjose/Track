using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Track.Domain.ClearSale.Interfaces.Proxies;
using Track.Domain.ClearSale.Models;
using Track.Domain.ConfigurationData.Caches;

namespace Track.Proxy.ClearSale {
    public class ClearSaleProxy : IClearSaleProxy {
        private readonly ConfigurationDataCache _configurationDataCache;

        public ClearSaleProxy (ConfigurationDataCache configurationDataCache) {

            _configurationDataCache = configurationDataCache;
        }

        public Task<SendDataLoginResponse> SendDataLoginAsync (SendDataLoginRequest sendDataLoginRequest) {
            string url = GetURL ("UrlApiAccountClearSale");
            WebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create (url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add ("Authorization", GetToken());

            using (var streamWriter = new StreamWriter (httpWebRequest.GetRequestStream ())) {
                string json = JsonConvert.SerializeObject (sendDataLoginRequest);

                streamWriter.Write (json);
                streamWriter.Flush ();
                streamWriter.Close ();
            }

            WebResponse httpResponse = (HttpWebResponse) httpWebRequest.GetResponse ();
            string statusCode = ((HttpWebResponse) httpResponse).StatusCode.ToString ().ToUpper ();

            if (statusCode == "OK") {
                //  Log.Information($"Dados enviados para a Tempest com sucesso. StatusCode: {statusCode}");
            } else {
                //Log.Information($"Dados enviados para a Tempest com erro. StatusCode: {statusCode}");
            }

            SendDataLoginResponse response = new SendDataLoginResponse ();
            return Task.FromResult(response);
        }

        private string GetURL (string key) {
            string url = _configurationDataCache.GetByKey (key).Valor;
            return url;
        }
        private string GetToken () {
            string clearSaleURL = _configurationDataCache.GetByKey ("UrlApiTokenClearSale").Valor;
            return clearSaleURL;

        }
    }

}