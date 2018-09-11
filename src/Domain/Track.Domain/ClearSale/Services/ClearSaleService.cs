using Track.Domain.ClearSale.Interfaces.Proxies;
using Track.Domain.ClearSale.Interfaces.Services;
using Track.Domain.ClearSale.Models;

namespace Track.Domain.ClearSale.Services {
    public class ClearSaleService : IClearSaleService {
        private readonly IClearSaleProxy _clearSaleProxy;

        public ClearSaleService (IClearSaleProxy clearSaleProxy) {
            _clearSaleProxy = clearSaleProxy;
        }

        private bool IsToSendDataLogin () {
            //--- obter do mongodb
            // string isToSendDataLogin = 

            return true;
        }

        public Task<SendDataLoginResponse> SendDataLogin (SendDataLoginRequest sendDataLoginRequest) {

            SendDataLoginResponse sendDataLoginResponse = _clearSaleProxy.SendDataLogin (sendDataLoginRequest);
            return sendDataLoginResponse;
        }
    }
}