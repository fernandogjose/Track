﻿using System;
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
using Track.Webapi.Middlewares;

namespace Track.Webapi {

    /// <summary>
    /// Startup
    /// </summary>
    public class Startup {

        /// <summary>
        /// Constructor
        /// </summary>
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        /// <summary>
        /// IConfiguration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// ConfigureServices
        /// </summary>
        public void ConfigureServices (IServiceCollection services) {

            //--- obter as chaves do config
            string mongoServerName = Configuration["DataMongo:DefaultConnection:ServerName"];
            string mongoDatabase = Configuration["DataMongo:DefaultConnection:Database"];
            string sqlConnection = Configuration["SQL:DefaultConnection:Database"];
            bool isDebug = true;
            bool.TryParse(Configuration["IsDebug"], out isDebug);

            //--- Configurar o serviço de documentação do Swagger
            services.AddSwaggerGen (c => {
                c.SwaggerDoc ("v1", new Info { Title = "Track API", Version = "v1", Description = "API REST para o track da clearsale", });

                string caminhoAplicacao = PlatformServices.Default.Application.ApplicationBasePath;
                string nomeAplicacao = PlatformServices.Default.Application.ApplicationName;
                string caminhoXmlDoc = Path.Combine (caminhoAplicacao, $"{nomeAplicacao}.xml");

                c.IncludeXmlComments (caminhoXmlDoc);
            });

            Bootstrap.Configure (services, mongoServerName, mongoDatabase, sqlConnection, isDebug);
            services.AddCors ();
            services.AddMvc ();
            services.AddSingleton<IConfiguration> (Configuration);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            //--- ativando os middleware de error e de validação do token
            app.UseMiddleware (typeof (ErrorMiddleware));            
            app.SecurityValidation();
            
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