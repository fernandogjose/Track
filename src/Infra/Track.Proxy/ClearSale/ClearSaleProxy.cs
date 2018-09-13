using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Track.Domain.ClearSale.Interfaces.Proxies;
using Track.Domain.ClearSale.Models;
using Track.Domain.ConfigurationData.Services;

namespace Track.Proxy.ClearSale {
    public class ClearSaleProxy : IClearSaleProxy {
        private readonly ConfigurationDataCacheService _configurationDataCacheService;

        public ClearSaleProxy (ConfigurationDataCacheService configurationDataCacheService) {

            _configurationDataCacheService = configurationDataCacheService;
        }
        public Task<SendDataLoginResponse> SendDataLoginAsync (SendDataLoginRequest sendDataLoginRequest) {
            throw new NotImplementedException();
        }
        public Task<SendDataLoginResponse> SendDataAccountAsync (SendDataAccountRequest sendDataLoginRequest) {
            string url = GetURL ("UrlApiAccountClearSale");
            WebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create (url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add ("Authorization", "Bearer " + GetToken ().Token);

            using (var streamWriter = new StreamWriter (httpWebRequest.GetRequestStream ())) {
                string json = JsonConvert.SerializeObject (sendDataLoginRequest);

                streamWriter.Write (json);
                streamWriter.Flush ();
                streamWriter.Close ();
            }

            WebResponse httpResponse = (HttpWebResponse) httpWebRequest.GetResponse ();
            string statusCode = ((HttpWebResponse) httpResponse).StatusCode.ToString ().ToUpper ();

            Stream dataStream = httpResponse.GetResponseStream ();
            StreamReader reader = new StreamReader (dataStream);
            string responseFromServer = reader.ReadToEnd ();

            if (statusCode == "OK") {
                //  Log.Information($"Dados enviados para a Tempest com sucesso. StatusCode: {statusCode}");
            } else {
                //Log.Information($"Dados enviados para a Tempest com erro. StatusCode: {statusCode}");
            }

            SendDataLoginResponse response = new SendDataLoginResponse ();
            return Task.FromResult (response);
        }
        private string GetURL (string key) {
            string url = _configurationDataCacheService.GetByKey (key).Valor;
            return url;
        }
        private ClearSaleAuthResponse GetToken () {

            string url = GetURL ("UrlApiTokenClearSale");
            var data = new Dictionary<string, string> ();
            data.Add ("name", "NovaPontoCom");
            data.Add ("password", "hdjYd7E");

            WebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create (url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter (httpWebRequest.GetRequestStream ())) {
                string json = JsonConvert.SerializeObject (data);

                streamWriter.Write (json);
                streamWriter.Flush ();
                streamWriter.Close ();
            }

            WebResponse httpResponse = (HttpWebResponse) httpWebRequest.GetResponse ();
            string statusCode = ((HttpWebResponse) httpResponse).StatusCode.ToString ().ToUpper ();
            Stream dataStream = httpResponse.GetResponseStream ();
            StreamReader reader = new StreamReader (dataStream);
            string responseFromServer = reader.ReadToEnd ();
            ClearSaleAuthResponse resultCaptcha = JsonConvert.DeserializeObject<ClearSaleAuthResponse> (responseFromServer);

            return resultCaptcha;
        }
    }
}