using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace CadastroPessoas.Api.Config.Swagger;

/// IApiVersionDescriptionProvider através do nosso construtor, 
/// ela servirá para que tenhamos disponível de maneira automática todas as versões que temos na nossa API pois nela temos uma lista das mesmas.
public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (ApiVersionDescription description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
        }

        // Add Security with Jwt
        AddSecurityConfig(options);

        // Incluir a documentação XML da API
        string xmlApiFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        string xmlApiPath = Path.Combine(AppContext.BaseDirectory, xmlApiFile);
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlApiPath));

        // Incluir a documentação XML dos DTOs
        string xmlDtoFile = $"{Assembly.GetAssembly(type: typeof(Application.Dtos.v1.InputModels.InseriPessoaInputModel))?.GetName().Name}.xml"; // Altere conforme o nome da sua classe DTO
        string xmlDtoPath = Path.Combine(AppContext.BaseDirectory, xmlDtoFile);
        options.IncludeXmlComments(xmlDtoPath);

        options.EnableAnnotations(); // Habilitar anotações
    }

    private static OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
    {
        OpenApiInfo info = new()
        {
            Title = "Pessoas",
            Description = "API para retorno de dados de pessoa.",
            Contact = new OpenApiContact
            {
                Name = "Meu email de contato",
                Email = "meu_email@email.com"
            },
            License = new OpenApiLicense
            {
                Name = "Meu tipo de licença",
                Url = new Uri(uriString: "https://exemplo.com.licenca")
            },
            Version = description.ApiVersion.ToString()
        };

        if (description.IsDeprecated)
        {
            info.Description +=
                " Esta versão da API foi depreciada, por favor utilizar uma mais recente disponível.";
        }

        return info;
    }

    private void AddSecurityConfig(SwaggerGenOptions options)
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "Insira o token no padrão: Bearer {seu token}",
            Name = "Authorization",
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
                }
            });
    }
}
