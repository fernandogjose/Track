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
using Track.Domain.ConfigurationData.Interfaces.Services;
using Track.Domain.ConfigurationData.Services;

namespace Track.Proxy.ClearSale {
    public class ClearSaleProxy : IClearSaleProxy {
        private readonly IConfigurationDataCacheService _configurationDataCacheService;

        public ClearSaleProxy (IConfigurationDataCacheService configurationDataCacheService) {

            _configurationDataCacheService = configurationDataCacheService;
        }
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