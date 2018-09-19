using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

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
                return;
            }

            string tokenRequestDecoded = DecodeToken(tokenRequest[0]);

            //--- valida se o token esta ativo, aqui a gente precisa validar se a data não expirou
            //--- temporariamente fica assim
            if (tokenRequestDecoded != "meuteste") {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync ("token inválido");
                return;
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