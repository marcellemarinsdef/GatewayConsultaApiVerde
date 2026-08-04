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

        // Detalhe do Verde (quando houver) anexado à mensagem — sem isso,
        // um 400 real da Defensoria virava só "Parâmetro inválido" pro
        // cliente, sem pista do motivo (maria-ia#20260202). Truncado pra
        // não estourar em corpo de erro gigante.
        protected IActionResult ErroApi(ApiException ex)
        {
            var mensagem = ex.Detalhe is { Length: > 0 }
                ? $"{ex.Message} — Verde respondeu: {ex.Detalhe[..Math.Min(ex.Detalhe.Length, 500)]}"
                : ex.Message;
            return Erro(ex.Code, mensagem, ex.StatusCode);
        }

        protected IActionResult ErroTimeout(string message) =>
            Erro("TIMEOUT", message, StatusCodes.Status504GatewayTimeout);

        protected IActionResult ErroParametroInvalido(string message) =>
            Erro("PARAMETRO_INVALIDO", message, StatusCodes.Status400BadRequest);
    }
}
