using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Multilanguage.Infrastructure;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Multilanguage
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();

            var config = LoadConfiguration();

            services.AddSingleton(config);
            services.AddLogging(configure => configure.AddConsole());
            services.AddMemoryCache();
            services.AddHttpClient();
            services.AddInfrastructure(config);
            services.AddTransient<MultilanguageApp>();

            var provider = services.BuildServiceProvider();
            var app = provider.GetRequiredService<MultilanguageApp>();

            await app.Run();
        }

        private static IConfiguration LoadConfiguration()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets(assembly);

            return builder.Build();
        }
    }
}
