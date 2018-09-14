using System.Collections.Generic;
using Newtonsoft.Json;

namespace Track.Domain.ClearSale.Models {
    public class SendDataResetPasswordResponse {

        [JsonProperty (PropertyName = "requestID")]
        public string RequestId { get; set; }

        [JsonProperty (PropertyName = "account")]
        public List<SendDataAccountPhones> Account { get; set; }
    }

    public class SendDataResetPasswordAccount {
        [JsonProperty (PropertyName = "code")]
        public string Code { get; set; }
    }
}