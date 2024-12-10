namespace CadastroPessoas.Api.Config.ApiVersion;

public static class ApiVersioningExtensions
{
    // Adicionar o versionamento na API
    public static IServiceCollection ConfigureApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(option =>
        {
            // Versão padrão da api
            option.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);

            // Para assumir a mesma quando não for especificada pelo consumidor
            option.AssumeDefaultVersionWhenUnspecified = true;

            // Avisar nas chamadas quais as versões disponíveis para quem esteja recebendo esse retorno
            option.ReportApiVersions = true;
        })
        .AddMvc()
        .AddApiExplorer(
        setup =>
        {
            // Logo abaixo é configurando o formato de versionamento para ser a letra ‘v’ e mais dígitos que possam estar a frente para definir o número.
            setup.GroupNameFormat = "'v'VVV";

            // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
            // can also be used to control the format of the API version in route templates
            setup.SubstituteApiVersionInUrl = true;
        });

        return services;
    }
}

