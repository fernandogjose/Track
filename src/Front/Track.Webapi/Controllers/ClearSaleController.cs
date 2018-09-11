using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Track.Domain.ClearSale.Interfaces.Services;
using Track.Domain.ClearSale.Models;

namespace Track.Webapi.Controllers
{
    [Route("api/[controller]")]
    public class ClearSaleController : Controller
    {
        private readonly IClearSaleService _clearSaleService;

        public ClearSaleController(IClearSaleService clearSaleService)
        {
            _clearSaleService = clearSaleService;
        }

        [HttpPost]
        public Task<SendDataLoginResponse> SendDataLogin(SendDataLoginRequest sendDataLoginRequest)
        {
            return _clearSaleService.SendDataLogin(sendDataLoginRequest);
        }
}
}
