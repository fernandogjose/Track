using System.Collections.Generic;
using Newtonsoft.Json;

namespace Track.Domain.ClearSale.Models {

    public class SendDataAccountResponse {

        [JsonProperty (PropertyName = "requestID")]
        public string RequestId { get; set; }

        [JsonProperty (PropertyName = "account")]
        public SendDataAccount Account { get; set; }
    }

    public class SendDataAccount {

        [JsonProperty (PropertyName = "code")]
        public string code { get; set; }

    }
}