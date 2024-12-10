using CadastroPessoas.Api.Config.ApiVersion;
using CadastroPessoas.Api.Config.Swagger;
using CadastroPessoas.Application;
using CadastroPessoas.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddInfrastructure(configuration)
                .AddApplication();

builder.Services.AddProblemDetails();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Adicionando classes de configuração de versionamento e Swagger personalizado
builder.Services.ConfigureApiVersioning();
builder.Services.SwaggerGen();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
app.SwaggerUi();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
