using System;
using System.Net;

namespace Track.Domain.Common.Exceptions
{
    public class CustomException : Exception {

        public HttpStatusCode HttpStatusCode { get; set; }

        public CustomException (string message, HttpStatusCode httpStatusCode) : base (message) {
            HttpStatusCode = httpStatusCode;
        }
    }
}