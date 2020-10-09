using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Multilanguage.Infrastructure.Options;
using System.IO;

namespace Multilanguage.Infrastructure.Data
{
    public class MultilanguageDbContextDesignTimeFactory : IDesignTimeDbContextFactory<MultilanguageDbContext>
    {
        public MultilanguageDbContext CreateDbContext(string[] args)
        {
            string applicationSettingsFullPath = Path.Combine(Directory.GetCurrentDirectory(), @"../Multilanguage.Api");

            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(applicationSettingsFullPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets(Constants.UserSecretsId)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<MultilanguageDbContext>();
            var connectionString = config.Get<MultilanguageConnectionOptions>();
            optionsBuilder.UseSqlServer(connectionString.MultilanguageConnection);
            return new MultilanguageDbContext(optionsBuilder.Options);
        }
    }
}
