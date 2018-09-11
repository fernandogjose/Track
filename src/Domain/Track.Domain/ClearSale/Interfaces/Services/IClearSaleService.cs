using Track.Domain.ClearSale.Models;

namespace Track.Domain.ClearSale.Interfaces.Services {
    public interface IClearSaleService {
        Task<SendDataLoginResponse> SendDataLogin (SendDataLoginRequest sendDataLoginRequest);
    }
}