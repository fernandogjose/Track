using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using Track.DI;

namespace Track.Webapi {

    public class Startup {

        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices (IServiceCollection services) {

            //--- obter as chaves do config
            string mongoServerName = Configuration["DataMongo:DefaultConnection:ServerName"];
            string mongoDatabase = Configuration["DataMongo:DefaultConnection:Database"];
            string sqlConnection = Configuration["SQL:DefaultConnection:Database"];

            //--- Configurar o serviço de documentação do Swagger
            services.AddSwaggerGen (c => {
                c.SwaggerDoc ("v1", new Info { Title = "Track API", Version = "v1", Description = "API REST para o track da clearsale", });

                string caminhoAplicacao = PlatformServices.Default.Application.ApplicationBasePath;
                string nomeAplicacao = PlatformServices.Default.Application.ApplicationName;
                string caminhoXmlDoc = Path.Combine (caminhoAplicacao, $"{nomeAplicacao}.xml");

                c.IncludeXmlComments (caminhoXmlDoc);
            });

            Bootstrap.Configure (services, mongoServerName, mongoDatabase, sqlConnection);
            services.AddCors ();
            services.AddMvc ();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
    /// ClearSale API
    /// </summary>
        public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }

            app.UseCors (
                options => options
                .AllowAnyOrigin ()
                .AllowAnyHeader ()
                .AllowAnyMethod ()
            );

            app.UseMvc ();

            //--- Ativando middlewares para uso do Swagger
            app.UseSwagger ();
            app.UseSwaggerUI (c => {
                c.SwaggerEndpoint ("/swagger/v1/swagger.json",
                    "Track API");
            });
        }
    }
}