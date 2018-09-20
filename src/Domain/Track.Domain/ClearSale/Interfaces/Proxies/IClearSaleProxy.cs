using System.Threading.Tasks;
using Track.Domain.ClearSale.Models;

namespace Track.Domain.ClearSale.Interfaces.Proxies {

    public interface IClearSaleProxy {

        Task<SendDataLoginResponse> SendDataLoginAsync (SendDataLoginRequest sendDataLoginRequest);

        Task<SendDataResetPasswordResponse> SendDataResetPasswordAsync (SendDataResetPasswordClearSaleRequest sendDataResetPasswordClearSaleRequest);

        Task<SendDataAccountResponse> SendDataAccountCreateAsync (SendDataAccountRequest sendDataAccountRequest);

        Task<SendDataAccountResponse> SendDataAccountUpdateAsync (SendDataAccountRequest sendDataAccountRequest);

    }
}