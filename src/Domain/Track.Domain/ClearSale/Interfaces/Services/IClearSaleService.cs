using Track.Domain.ClearSale.Models;

namespace Track.Domain.ClearSale.Interfaces.Services
{
    public interface IClearSaleService
    {
         SendDataLoginResponse SendDataLogin(SendDataLoginRequest sendDataLoginRequest);
    }
}