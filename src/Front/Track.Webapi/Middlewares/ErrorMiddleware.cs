using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Track.Domain.Common.Exceptions;
using Track.Domain.ConfigurationData.Interfaces.Services;
using Track.Domain.ConfigurationData.Models;
using Track.Domain.Log.Enums;

namespace Track.Webapi.Middlewares {

    /// <summary>
    /// Interceptador dos erros da api
    /// </summary>
    public class ErrorMiddleware {
        private readonly RequestDelegate next;

        private readonly ILogService _logService;

        private Task HandleExceptionAsync (HttpContext context, Exception exception) {

            //--- Obter o erro
            HttpStatusCode code = HttpStatusCode.InternalServerError;
            string namespaceClass = "";
            string method = "";

            if (exception is ArgumentException) code = HttpStatusCode.BadRequest;
            else if (exception is CustomException) {

                CustomException customException = ((CustomException) exception);
                code = customException.HttpStatusCode;
                namespaceClass = customException.NamespaceClass;
                method = customException.Method;
            }

            //--- Logar a Exception no MongoDB
            LogRequest logRequest = new LogRequest {
                StatusCode = StatusCode.Error.ToString(),
                LogDate = DateTime.Now,
                HttpStatusCode = code.ToString (),
                NamespaceClass = namespaceClass,
                Method = method,
                Message = exception.Message,
                InnerException = exception.InnerException != null ? exception.InnerException.Message : ""
            };

            _logService.AddAsync (logRequest);

            //--- constroi o response
            var result = JsonConvert.SerializeObject (new { error = exception.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) code;
            return context.Response.WriteAsync (result);
        }

        /// <summary>
        /// Construtor
        /// </summary>
        public ErrorMiddleware (RequestDelegate next, ILogService logService) {
            this.next = next;
            this._logService = logService;
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

    }
}