using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Track.Domain.ClearSale.Interfaces.Services;
using Track.Domain.ClearSale.Models;

namespace Track.Webapi.Controllers {
    [Route ("api/clearsale")]
    public class ClearSaleController : Controller {

        private readonly IClearSaleService _clearSaleService;

        public ClearSaleController (IClearSaleService clearSaleService) {
            _clearSaleService = clearSaleService;
        }

        [HttpPost]
        [Route ("sendDataLoginAsync")]
        public async Task<SendDataLoginResponse> SendDataLoginAsync (SendDataLoginRequest sendDataLoginRequest) {
            SendDataLoginResponse sendDataLoginResponse = await _clearSaleService.SendDataLoginAsync (sendDataLoginRequest);
            return sendDataLoginResponse;
        }
    }
}