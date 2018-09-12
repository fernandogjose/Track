using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Track.Domain.ClearSale.Interfaces.Services;
using Track.Domain.ClearSale.Models;

namespace Track.Webapi.Controllers {
<<<<<<< HEAD
=======

    /// <summary>
    /// ClearSale API
    /// </summary>
>>>>>>> 9afc0e547672ac55c318430170f13385e5cfcd56
    [Route ("api/clearsale")]
    public class ClearSaleController : Controller {

        private readonly IClearSaleService _clearSaleService;

        /// <summary>
        /// Injeção de dependencia para o serviço do clearsale
        /// </summary>
        public ClearSaleController (IClearSaleService clearSaleService) {
            _clearSaleService = clearSaleService;
        }

        /// <summary>
        /// Envia os dados do login para o ClearSale
        /// </summary>
        /// <param name="sendDataLoginRequest">Objeto com os dados do usuário</param>
        /// <returns>Objeto com o status do envio</returns>
        [HttpPost]
        [Route ("sendDataLoginAsync")]
        public async Task<SendDataLoginResponse> SendDataLoginAsync ([FromBody] SendDataLoginRequest sendDataLoginRequest) {
            SendDataLoginResponse sendDataLoginResponse = await _clearSaleService.SendDataLoginAsync (sendDataLoginRequest);
            return sendDataLoginResponse;
        }
    }

}