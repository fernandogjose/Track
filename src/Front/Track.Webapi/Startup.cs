using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Track.DI;

namespace Track.Webapi {
    public class Startup {

        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            string mongoServerName = Configuration["DataMongo:DefaultConnection:ServerName"];
            string mongoDatabase = Configuration["DataMongo:DefaultConnection:Database"];
            string sqlConnection = Configuration["SQL:DefaultConnection:Database"];

            Bootstrap.Configure (services, mongoServerName, mongoDatabase, sqlConnection);
            services.AddCors ();
            services.AddMvc ();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }

            app.UseCors (
                options => options.WithOrigins ("//carrinho.casasbahia.com.br")
                .WithOrigins ("//www.casasbahia.com.br")
                .AllowAnyHeader ()
                .AllowAnyMethod ()
                .AllowAnyOrigin ()
            );

            app.UseMvc ();
        }
    }
}