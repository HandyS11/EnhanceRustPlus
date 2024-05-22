using EfCore;
using EfCore.Implementation;
using EfCore.Interfaces;
using EnhanceRustPlus.Interfaces;
using EnhanceRustPlus.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EnhanceRustPlus.Configuration
{
    public static class ServiceConfigurationExtension
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepositoryManager<>), typeof(RepositoryManager<>));
            services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));

            services.AddSingleton<ICleanupService, CleanupService>();
            services.AddSingleton<ISetupService, SetupService>();

            return services;
        }

        public static IServiceCollection AddDbContext(this IServiceCollection services, bool isDevelopment)
        {
            services.AddDbContext<DbContext, EnhanceRustPlusDbContext>(opt =>
            {
                opt.EnableDetailedErrors(isDevelopment);
                opt.EnableSensitiveDataLogging(isDevelopment);
            });

            return services;
        }
    }
}
