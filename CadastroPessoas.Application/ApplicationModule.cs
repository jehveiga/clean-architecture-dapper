using CadastroPessoas.Application.Business;
using Microsoft.Extensions.DependencyInjection;

namespace CadastroPessoas.Application
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddBusiness();

            return services;
        }

        public static IServiceCollection AddBusiness(this IServiceCollection services)
        {
            services.AddTransient<IUserBusiness, UserBusiness>();

            return services;
        }
    }
}
