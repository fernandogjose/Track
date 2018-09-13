using System.Threading.Tasks;
using Track.Domain.ClearSale.Models;

namespace Track.Domain.ClearSale.Interfaces.Services 
{
    public interface IClearSaleService 
    {        
        Task<SendDataLoginResponse> SendDataLoginAsync (SendDataLoginRequest sendDataLoginRequest);

        Task<SendDataAccountResponse> SendDataAccountAsync (SendDataAccountRequest sendDataAccountRequest);
    }
}