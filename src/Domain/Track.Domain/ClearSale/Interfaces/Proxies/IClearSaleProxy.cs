using System.Threading.Tasks;
using Track.Domain.ClearSale.Models;

namespace Track.Domain.ClearSale.Interfaces.Proxies {
    
    public interface IClearSaleProxy {
        
        Task<SendDataLoginResponse> SendDataLoginAsync (SendDataLoginRequest sendDataLoginRequest);

        Task<SendDataAccountResponse> SendDataAccountAsync (SendDataAccountRequest sendDataAccountRequest);
    }
}