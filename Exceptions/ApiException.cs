namespace GatewayConsultaApiVerde.Exceptions
{
    public class ApiException : Exception
    {
        public int StatusCode { get; }

        public ApiException(int statusCode)
            : base(GetMessage(statusCode))
        {
            StatusCode = statusCode;
        }

        private static string GetMessage(int statusCode)
        {
            return statusCode switch
            {
                400 => "Parâmetro inválido",
                401 => "Não autorizado para acessar a API externa.",
                403 => "Acesso negado pela API externa.",
                404 => "Dado não encontrado.",
                422 => "Número inválido",
                500 => "Erro interno na API externa.",
                503 => "A API externa está indisponível.",
                _ => "Erro ao consultar na API externa."
            };
        }
    }
}
