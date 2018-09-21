using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Track.Domain.Common.Exceptions;
using Track.Domain.ConfigurationData.Interfaces.Services;
using Track.Domain.ConfigurationData.Models;
using Track.Domain.Log.Enums;

namespace Track.Webapi.Middlewares {

    /// <summary>
    /// Middleware para validar o token nas requisições
    /// </summary>
    public class SecurityMiddleware {

        private readonly RequestDelegate _next;

        private readonly IConfiguration _configuration;

        private readonly ILogService _logService;

        private static string DecodeToken (string value) {
            byte[] byteToken = System.Convert.FromBase64String (value);
            string response = System.Text.Encoding.UTF8.GetString (byteToken);
            return response;
        }

        /// <summary>
        /// Construtor
        /// </summary>
        public SecurityMiddleware (RequestDelegate next, IConfiguration configuration, ILogService logService) {
            _next = next;
            _configuration = configuration;
            _logService = logService;
        }

        /// <summary>
        /// Invoke
        /// </summary>
        public async Task Invoke (HttpContext context) {

            await _logService.AddAsync (new LogRequest {
                StatusCode = StatusCode.Info.ToString(),
                Message = "Iniciou a aplicação"
            });

            //--- valida se o token esta sendo passado no headers
            StringValues tokenRequest;
            if (!context.Request.Headers.TryGetValue ("token", out tokenRequest)) {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync ("token não encontrado");
                throw new CustomException ("token não encontrado", HttpStatusCode.Unauthorized, "Track.Webapi.Middlewares.SecurityMiddleware", "Invoke");
            }

            //--- Decodifica a data que esta vindo no headers como token
            string tokenRequestDecoded = DecodeToken (tokenRequest[0]);
            DateTime tokenRequestDate = new DateTime ();
            DateTime.TryParse (tokenRequestDecoded, out tokenRequestDate);

            //--- valida se o token esta ativo, se a data não expirou
            if (tokenRequestDate < DateTime.Now.AddDays (-1)) {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync ("token inválido");
                throw new CustomException ("token inválido", HttpStatusCode.Unauthorized, "Track.Webapi.Middlewares.SecurityMiddleware", "Invoke");
            }

            await _logService.AddAsync (new LogRequest {
                StatusCode = StatusCode.Info.ToString(),
                Message = "Subiu a aplicação com sucesso na validação do token"
            });

            await _next.Invoke (context);
        }
    }

    /// <summary>
    /// UserKeyValidatorsExtension
    /// </summary>
    public static class SecurityExtension {

        /// <summary>
        /// ApplyUserKeyValidation
        /// </summary>
        public static IApplicationBuilder SecurityValidation (this IApplicationBuilder app) {
            app.UseMiddleware<SecurityMiddleware> ();
            return app;
        }
    }
}