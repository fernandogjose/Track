using Microsoft.Extensions.DependencyInjection;
using Track.Domain.ClearSale.Interfaces.Proxies;
using Track.Domain.ClearSale.Interfaces.Services;
using Track.Domain.ClearSale.Services;
using Track.Proxy.ClearSale;

namespace Track.DI {
    public class Bootstrap {

        public static void Configure (IServiceCollection services) {

            //--- Services
            services.AddSingleton<IClearSaleService, ClearSaleService> ();

            //--- Proxies
            services.AddSingleton<IClearSaleProxy, ClearSaleProxy> ();

        }
    }
}