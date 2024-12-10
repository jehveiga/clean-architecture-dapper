using CadastroPessoas.Domain.Interfaces.Repositories;
using CadastroPessoas.Infrastructure.Auth;
using CadastroPessoas.Infrastructure.Factory;
using CadastroPessoas.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using SwaggerDocExample.Identity.Configurations;
using System.Text;

namespace CadastroPessoas.Infrastructure
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
                                                            IConfiguration configuration)
        {
            return services.
                AddSqlFactory().
                AddRepositories().
                AddRedis(configuration).
                AddAuthentication(configuration).
                AddSerilog(configuration);
        }

        private static IServiceCollection AddSqlFactory(this IServiceCollection services)
        {
            services.AddScoped<SqlFactory>();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IPessoaRepository, PessoaRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            return services;
        }

        private static IServiceCollection AddRedis(this IServiceCollection services,
                                                    IConfiguration configuration)
        {
            // Adicionando serviço de cache a aplicação com Redis
            services.AddStackExchangeRedisCache(options =>
                    options.Configuration = configuration.GetConnectionString("Cache"));

            return services;
        }

        private static IServiceCollection AddSerilog(this IServiceCollection services,
                                                          IConfiguration configuration)
        {
            // Exemplo da configuracao ficar na appsettings.json
            Log.Logger = new LoggerConfiguration()
                                .ReadFrom.Configuration(configuration)
                                .Enrich.FromLogContext()
                                .CreateLogger();

            // Adicionando o Serilog no container de serviços, para ser usado na requisição
            return services;
        }

        private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            IConfigurationSection jwtAppSettingOptions = configuration.GetSection(nameof(JwtOptions));
            SymmetricSecurityKey securityKey = new(Encoding.ASCII.GetBytes(configuration.GetSection("JwtOptions:SecurityKey").Value ?? string.Empty));

            services.Configure<JwtOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtOptions.Issuer)] ?? string.Empty;
                options.Audience = jwtAppSettingOptions[nameof(JwtOptions.Audience)] ?? string.Empty;
                options.SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                options.AccessTokenExpiration = int.Parse(jwtAppSettingOptions[nameof(JwtOptions.AccessTokenExpiration)] ?? "0");
                options.RefreshTokenExpiration = int.Parse(jwtAppSettingOptions[nameof(JwtOptions.RefreshTokenExpiration)] ?? "0");
            });

            // Adicionando o serviço de autenticação no container de serviços com as configurações para geração do token e validação
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = configuration.GetSection("JwtOptions:Issuer").Value,
                        ValidAudience = configuration.GetSection("JwtOptions:Audience").Value,
                        IssuerSigningKey = securityKey,

                        ValidateLifetime = true,
                    };
                });

            // Adicionando serviço do Jwt na aplicação
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }

    }
}
