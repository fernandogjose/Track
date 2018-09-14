﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Track.Domain.ClearSale.Interfaces.Services;
using Track.Domain.ClearSale.Models;

namespace Track.Webapi.Controllers {

    /// <summary>
    /// ClearSale API
    /// </summary>
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

        /// <summary>
        /// Envia os dados do login para o ClearSale
        /// </summary>
        /// <param name="sendDataAccountRequest">Objeto com os dados do usuário</param>
        /// <returns>Objeto com o status do envio</returns>
        [HttpPost]
        [Route ("sendDataAccountAsync")]
        public async Task<SendDataAccountResponse> SendDataAccountAsync ([FromBody] SendDataAccountRequest sendDataAccountRequest) {
            SendDataAccountResponse sendDataAccountResponse = await _clearSaleService.SendDataAccountAsync (sendDataAccountRequest);
            return sendDataAccountResponse;
        }

        /// <summary>
        /// Envia os dados do recuperar senha
        /// </summary>
        /// <param name="sendDataResetPasswordRequest">Objeto com os dados do usuário</param>
        /// <returns>Objeto com o status do envio</returns>
        [HttpPost]
        [Route ("sendDataAccountAsync")]
        public async Task<SendDataResetPasswordResponse> SendDataAccountAsync ([FromBody] SendDataResetPasswordRequest sendDataResetPasswordRequest) {
            SendDataResetPasswordResponse sendDataResetPasswordResponse = await _clearSaleService.SendDataResetPasswordAsync (sendDataResetPasswordRequest);
            return sendDataResetPasswordResponse;
        }        
    }
}