using Track.Domain.ClearSale.Interfaces.Proxies;
using Track.Domain.ClearSale.Models;

namespace Track.Proxy.ClearSale {
    public class ClearSaleProxy : IClearSaleProxy {
        public Task<SendDataLoginResponse> SendDataLogin (SendDataLoginRequest sendDataLoginRequest) {
            return new SendDataLoginResponse ();
        }
    }
}