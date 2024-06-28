using EnhanceRustPlus.Business.Interfaces;
using EnhanceRustPlus.Business.Services;
using EnhanceRustPlus.EfCore.Context;
using EnhanceRustPlus.EfCore.Implementation;
using EnhanceRustPlus.EfCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EnhanceRustPlus.Configuration
{
    /// <summary>
    /// Extension methods for configuring services in the application.
    /// </summary>
    public static class ServiceConfigurationExtension
    {
        /// <summary>
        /// Adds the business services to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepositoryManager<>), typeof(RepositoryManager<>));
            services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));

            services.AddSingleton<ICleanupService, CleanupService>();
            services.AddSingleton<ICredentialService, CredentialService>();
            services.AddSingleton<IEncryptionService, EncryptionService>();
            services.AddSingleton<ISetupService, SetupService>();
            services.AddSingleton<IUserService, UserService>();

            return services;
        }

        /// <summary>
        /// Adds the database context to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="isDevelopment">A flag indicating whether the application is running in development mode.</param>
        /// <returns>The updated service collection.</returns>
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
