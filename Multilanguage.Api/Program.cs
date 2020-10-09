using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Multilanguage.Infrastructure.Data;
using Multilanguage.Infrastructure.Helpers;
using System.Linq;

namespace Multilanguage.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var seed = args.Any(x => x == "/seed");
            if (seed) args = args.Except(new[] { "/seed" }).ToArray();

            var hasCustomArgs = seed;

            var host = CreateHostBuilder(args).Build();
            if (hasCustomArgs)
            {
                if (seed)
                {
                    using (var scope = host.Services.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<MultilanguageDbContext>();
                        SeedHelper.SeedTestData(dbContext);
                    }
                }
                return;
            }
            host.Run();
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
