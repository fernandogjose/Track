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

            //--- valida se o token esta ativo
            string tokenApi = _configuration.GetValue<string> ("Security:Token");
            if (string.IsNullOrEmpty (tokenApi) || tokenApi != tokenRequest) {
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