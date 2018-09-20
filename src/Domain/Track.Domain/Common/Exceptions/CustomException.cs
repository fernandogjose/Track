using System;
using System.Net;

namespace Track.Domain.Common.Exceptions
{
    public class CustomException : Exception {

        public HttpStatusCode HttpStatusCode { get; private set; }

        public string NamespaceClass { get; private set; }

        public string Method { get; private set; }

        public CustomException (string message, HttpStatusCode httpStatusCode, string namespaceClass, string method) : base (message) {
            HttpStatusCode = httpStatusCode;
            NamespaceClass = namespaceClass;
            Method = method;
        }
    }
}