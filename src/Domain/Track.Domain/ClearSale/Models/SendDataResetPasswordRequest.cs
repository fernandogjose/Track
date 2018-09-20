using System.Collections.Generic;
using Newtonsoft.Json;

namespace Track.Domain.ClearSale.Models {

    public class SendDataResetPasswordRequest {

        public string Email { get; set; }
    }

    public class SendDataResetPasswordClearSaleRequest {

        [JsonProperty (PropertyName = "code")]
        public string Code { get; set; }

        [JsonProperty (PropertyName = "sessionID")]
        public string SessionId { get; set; }
    }
}