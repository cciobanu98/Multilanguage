using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Multilanguage.Application.Abstract;
using Multilanguage.Infrastructure.Azure;
using Multilanguage.Infrastructure.Data;
using Multilanguage.Infrastructure.Options;

namespace Multilanguage.Infrastructure
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMultilanguageOptions();
            services.AddMultilanguageDbContext();
            services.AddAzureCognitiveService(configuration);
            return services;
        }

        public static IServiceCollection AddMultilanguageDbContext(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, MultilanguageDbContext>();

            services.AddDbContext<MultilanguageDbContext>((serviceProvider, options) =>
            {
                var connectionString = serviceProvider.GetRequiredService<IOptions<MultilanguageConnectionOptions>>().Value.MultilanguageConnection;
                options.UseSqlServer(connectionString);
            });
            return services;
        }

        public static IServiceCollection AddMultilanguageOptions(this IServiceCollection services)
        {
            services.AddOptions<MultilanguageConnectionOptions>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.Bind(settings);
                });
            return services;
        }
        public static IServiceCollection AddAzureCognitiveService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CognitiveServiceOptions>(configuration.GetSection("AzureCognitive"));
            services.AddScoped<IStringLocalizerCogniteveService, StringLocalizerCognitiveService>();
            return services;
        }
    }
}
