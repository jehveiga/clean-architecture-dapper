namespace CadastroPessoas.Api.Util;

public class ErrorResponse
{
    /// <summary>
    /// Tipo de erro que foi gerado
    /// </summary>
    /// <example>https://datatracker.ietf.org/doc/html/rfc9110#name-400-bad-request</example>
    public string? Type { get; }
    /// <summary>
    /// Descrição dos erros
    /// </summary>
    /// <example>Um ou mais erros de validação foram encontrados</example>
    public string? Title { get; }

    /// <summary>
    /// Status code http
    /// </summary>
    /// <example>400</example>
    public int Status { get; }


    /// <summary>
    /// Dicionário de erros encontrados
    /// </summary>
    public IDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>();

    public ErrorResponse(string type, int status, string title)
    {
        Type = type;
        Status = status;
        Title = title;
    }

    public ErrorResponse(string type, int status, string title, IDictionary<string, string[]> errors)
    {
        Type = type;
        Status = status;
        Title = title;
        Errors = errors;
    }
}
