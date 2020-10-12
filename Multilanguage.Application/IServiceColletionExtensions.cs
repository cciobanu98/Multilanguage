using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Multilanguage.Application.Abstract;
using Multilanguage.Application.Localizer;
using Multilanguage.Application.Options;

namespace Multilanguage.Application
{
    public static class IServiceColletionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMultilanguageOption(configuration);
            services.AddLocalizer();
            return services;
        }

        public static IServiceCollection AddLocalizer(this IServiceCollection services)
        {
            services.AddScoped<IStringLocalizerFacade, StringLocalizerFacade>();
            services.AddScoped<IStringLocalizerService, StringLocalizerService>();
            return services;
        }

        public static IServiceCollection AddMultilanguageOption(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MultilanguageOptions>(configuration.GetSection("Multilanguage"));
            return services;
        }
    }
}
