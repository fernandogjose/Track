using System;
using Track.Domain.Log.Enums;

namespace Track.Domain.ConfigurationData.Models {
    public class LogRequest {

        public StatusCode StatusCode { get; set; }

        public string HttpStatusCode { get; set; }

        public string Message { get; set; }

        public string InnerException { get; set; }

        public string Method { get; set; }

        public string NamespaceClass { get; set; }

        public DateTime LogDate {get; set;}
    }
}