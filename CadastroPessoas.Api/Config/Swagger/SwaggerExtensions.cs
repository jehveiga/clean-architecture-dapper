using Asp.Versioning.ApiExplorer;

namespace CadastroPessoas.Api.Config.Swagger;

public static class SwaggerExtensions
{

    public static IServiceCollection SwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen();
        services.ConfigureOptions<ConfigureSwaggerOptions>();

        return services;
    }

    public static WebApplication SwaggerUi(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            IApiVersionDescriptionProvider apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                IEnumerable<ApiVersionDescription> descriptions = apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse();

                foreach (ApiVersionDescription description in descriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description?.GroupName.ToUpperInvariant());
                }

            });
        }

        return app;
    }
}
