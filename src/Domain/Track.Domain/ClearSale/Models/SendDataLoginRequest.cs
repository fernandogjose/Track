using Newtonsoft.Json;

namespace Track.Domain.ClearSale.Models {

    public class SendDataLoginRequest {

        [JsonProperty (PropertyName = "code")]
        public string Code { get; set; }

        [JsonProperty (PropertyName = "sessionID")]
        public string SessionId { get; set; }
    }
}