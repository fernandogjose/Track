using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Track.Domain.Common.Exceptions;

namespace Track.Webapi.Middlewares {

    /// <summary>
    /// Interceptador dos erros da api
    /// </summary>
    public class ErrorMiddleware {
        private readonly RequestDelegate next;

        /// <summary>
        /// Construtor
        /// </summary>
        public ErrorMiddleware (RequestDelegate next) {
            this.next = next;
        }

        /// <summary>
        /// Invoke
        /// </summary>
        public async Task Invoke (HttpContext context) {
            try {
                await next (context);
            } catch (Exception ex) {
                await HandleExceptionAsync (context, ex);
            }
        }

        private static Task HandleExceptionAsync (HttpContext context, Exception exception) {
            var code = HttpStatusCode.InternalServerError;

            if (exception is ArgumentException) code = HttpStatusCode.BadRequest;
             else if (exception is CustomException) code = ((CustomException) exception).HttpStatusCode;

            //--- Fernando - Logar a Exception no MongoDB
            var result = JsonConvert.SerializeObject (new { error = exception.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) code;
            return context.Response.WriteAsync (result);
        }
    }
}