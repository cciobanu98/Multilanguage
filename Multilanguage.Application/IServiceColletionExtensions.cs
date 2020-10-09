using Microsoft.Extensions.DependencyInjection;
using Multilanguage.Application.Abstract;
using Multilanguage.Application.Localizer;

namespace Multilanguage.Application
{
    public static class IServiceColletionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddLocalizer();
            return services;
        }

        public static IServiceCollection AddLocalizer(this IServiceCollection services)
        {
            services.AddScoped<IStringLocalizerFacade, StringLocalizerFacade>();
            services.AddScoped<IStringLocalizerService, StringLocalizerService>();
            return services;
        }
    }
}
