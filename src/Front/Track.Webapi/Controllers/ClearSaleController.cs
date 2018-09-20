using System;
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
    [Route ("clearsale")]
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
        /// <response code="200">Objeto com o status do envio</response>
        /// <response code="400">Solicitação inválida.</response>
        /// <response code="500">Erro inesperado.</response>
        /// <returns>Objeto com o status do envio</returns>
        [HttpPost]
        [Route ("sendDataLoginAsync")]
        [ProducesResponseType (typeof (SendDataLoginResponse), 200)]
        public async Task<ActionResult> SendDataLoginAsync ([FromBody] SendDataLoginRequest sendDataLoginRequest) {
            SendDataLoginResponse sendDataLoginResponse = await _clearSaleService.SendDataLoginAsync (sendDataLoginRequest);
            return this.Ok (sendDataLoginResponse);
        }

        /// <summary>
        /// Envia os dados do login para o ClearSale
        /// </summary>
        /// <param name="sendDataAccountRequest">Objeto com os dados do usuário</param>
        /// <response code="200">Objeto com o status do envio</response>
        /// <response code="400">Solicitação inválida.</response>
        /// <response code="500">Erro inesperado.</response>
        /// <returns>Objeto com o status do envio</returns>
        [HttpPost]
        [Route ("sendDataAccountCreateAsync")]
        [ProducesResponseType (typeof (SendDataAccountResponse), 200)]
        public async Task<ActionResult> SendDataAccountCreateAsync ([FromBody] SendDataAccountRequest sendDataAccountRequest) {
            SendDataAccountResponse sendDataAccountResponse = await _clearSaleService.SendDataAccountCreateAsync (sendDataAccountRequest);
            return this.Ok (sendDataAccountResponse);
        }

        /// <summary>
        /// Envia os dados do login para o ClearSale
        /// </summary>
        /// <param name="sendDataAccountRequest">Objeto com os dados do usuário</param>
        /// <response code="200">Objeto com o status do envio</response>
        /// <response code="400">Solicitação inválida.</response>
        /// <response code="500">Erro inesperado.</response>
        /// <returns>Objeto com o status do envio</returns>
        [HttpPost]
        [Route ("sendDataAccountUpdateAsync")]
        [ProducesResponseType (typeof (SendDataAccountResponse), 200)]
        public async Task<ActionResult> SendDataAccountUpdateAsync ([FromBody] SendDataAccountRequest sendDataAccountRequest) {
            SendDataAccountResponse sendDataAccountResponse = await _clearSaleService.SendDataAccountUpdateAsync (sendDataAccountRequest);
            return this.Ok (sendDataAccountResponse);
        }

        /// <summary>
        /// Envia os dados do recuperar senha
        /// </summary>
        /// <param name="sendDataResetPasswordRequest">Objeto com os dados do usuário</param>
        /// <response code="200">Objeto com o status do envio</response>
        /// <response code="400">Solicitação inválida.</response>
        /// <response code="500">Erro inesperado.</response>
        /// <returns>Objeto com o status do envio</returns>
        [HttpPost]
        [Route ("SendDataResetPasswordAsync")]
        [ProducesResponseType (typeof (SendDataResetPasswordResponse), 200)]
        public async Task<ActionResult> SendDataResetPasswordAsync ([FromBody] SendDataResetPasswordRequest sendDataResetPasswordRequest) {
            SendDataResetPasswordResponse sendDataResetPasswordResponse = await _clearSaleService.SendDataResetPasswordAsync (sendDataResetPasswordRequest);
            return this.Ok (sendDataResetPasswordResponse);
        }
    }
}