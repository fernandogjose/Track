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

    public class BaseResponse {
        public string Contents { get; set; }
        public HttpResponseMessage HttpResponseMessage { get; set; }

    }
}