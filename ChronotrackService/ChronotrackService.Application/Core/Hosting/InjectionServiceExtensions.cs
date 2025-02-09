using System;
using ChronotrackService.Application.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChronotrackService.Application
{
    public static class InjectionServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddSingleton<AppSettings>();

            services.AddDbContext<ChronotrackContext>(options =>
                options.UseMySql(appSettings.ConnChronotrack,
                ServerVersion.AutoDetect(appSettings.ConnChronotrack)));

            return services;
        }
    }
}
