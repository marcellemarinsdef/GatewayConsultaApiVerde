namespace GatewayConsultaApiVerde.Exceptions
{
    public class ApiException : Exception
    {
        public int StatusCode { get; }

        // código estável (não muda entre releases) — pro client tratar por
        // código, não parseando a mensagem em português (padroes-rest-nivel2)
        public string Code { get; }

        public ApiException(int statusCode)
            : base(GetMessage(statusCode))
        {
            StatusCode = statusCode;
            Code = GetCode(statusCode);
        }

        private static string GetCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "PARAMETRO_INVALIDO",
                401 => "NAO_AUTORIZADO",
                403 => "ACESSO_NEGADO",
                404 => "NAO_ENCONTRADO",
                422 => "DADO_INVALIDO",
                500 => "ERRO_INTERNO_EXTERNO",
                503 => "SERVICO_INDISPONIVEL",
                _ => "ERRO_DESCONHECIDO"
            };
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
