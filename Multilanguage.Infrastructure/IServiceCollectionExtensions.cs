using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Multilanguage.Application.Abstract;
using Multilanguage.Infrastructure.Data;
using Multilanguage.Infrastructure.Localizer;
using Multilanguage.Infrastructure.Options;

namespace Multilanguage.Infrastructure
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCognitiveSeriviceOptions(configuration);
            services.AddMultilanguageOptions();
            services.AddMultilanguageDbContext();
            services.AddLocalization();
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

        public static IServiceCollection AddCognitiveSeriviceOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CognitiveServiceOptions>(configuration.GetSection("AzureCognitive"));
            return services;
        }
        public static IServiceCollection AddLocalization(this IServiceCollection services)
        {
            services.AddScoped<IStringLocalizerCogniteveService, StringLocalizerCognitiveService>();
            services.AddScoped<IStringLocalizerFacade, StringLocalizerFacade>();
            services.AddScoped<IStringLocalizerService, StringLocalizerService>();
            return services;
        }
    }
}
