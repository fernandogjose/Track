using Track.Domain.ClearSale.Models;

namespace Track.Domain.ClearSale.Interfaces.Proxies
{
    public interface IClearSaleProxy
    {
         SendDataLoginResponse SendDataLogin(SendDataLoginRequest sendDataLoginRequest);
    }
}