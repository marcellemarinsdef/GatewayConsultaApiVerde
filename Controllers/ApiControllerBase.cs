using GatewayConsultaApiVerde.Exceptions;
using GatewayConsultaApiVerde.Models;
using Microsoft.AspNetCore.Mvc;

namespace GatewayConsultaApiVerde.Controllers
{
    // Base compartilhada — centraliza o envelope de erro padrão
    // {error:{code,message}} (padroes-rest-nivel2) pra não duplicar em cada
    // controller. Nenhum controller monta erro "na mão" fora daqui.
    [ApiController]
    public abstract class ApiControllerBase : Controller
    {
        protected IActionResult Erro(string code, string message, int statusCode) =>
            StatusCode(statusCode, new ErrorResponseDTO
            {
                Error = new ErrorResponseDTO.ErrorDetail { Code = code, Message = message }
            });

        protected IActionResult ErroApi(ApiException ex) =>
            Erro(ex.Code, ex.Message, ex.StatusCode);

        protected IActionResult ErroTimeout(string message) =>
            Erro("TIMEOUT", message, StatusCodes.Status504GatewayTimeout);

        protected IActionResult ErroParametroInvalido(string message) =>
            Erro("PARAMETRO_INVALIDO", message, StatusCodes.Status400BadRequest);
    }
}
