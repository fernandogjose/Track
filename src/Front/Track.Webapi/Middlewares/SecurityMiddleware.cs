using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Track.Domain.Common.Exceptions;

namespace Track.Webapi.Middlewares {

    /// <summary>
    /// Middleware para validar o token nas requisições
    /// </summary>
    public class SecurityMiddleware {

        private readonly RequestDelegate _next;

        private readonly IConfiguration _configuration;

        private static string DecodeToken (string value) {
            byte[] byteToken = System.Convert.FromBase64String (value);
            string response = System.Text.Encoding.UTF8.GetString (byteToken);
            return response;
        }

        /// <summary>
        /// Construtor
        /// </summary>
        public SecurityMiddleware (RequestDelegate next, IConfiguration configuration) {
            _next = next;
            _configuration = configuration;
        }

        /// <summary>
        /// Invoke
        /// </summary>
        public async Task Invoke (HttpContext context) {

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