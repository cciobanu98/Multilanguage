using Multilanguage.Domain.Models;
using Multilanguage.Infrastructure.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Multilanguage.Infrastructure.Helpers
{
    public static class SeedHelper
    {
        public static void SeedTestData(MultilanguageDbContext dbContext)
        {
            AddLanguageTestData(dbContext);
            dbContext.SaveChanges();
        }

        public static async Task SeedTestDataAsync(MultilanguageDbContext dbContext)
        {
            AddTestData(dbContext);
            await dbContext.SaveChangesAsync();
        }

        private static void AddTestData(MultilanguageDbContext dbContext)
        {
            AddLanguageTestData(dbContext);
        }

        private static void AddLanguageTestData(MultilanguageDbContext dbContext)
        {
            var roLanguage = new Language()
            {
                Code = "ro-RO",
                Name = "Rommanian",
                Id = Guid.Parse("f9044e38-eca2-4d91-a163-0e4fb64c2f00"),
            };
            if (!dbContext.Languages.Any(x => x.Code == roLanguage.Code))
            {
                dbContext.Languages.Add(roLanguage);
            }

            var ruLanguage = new Language()
            {
                Code = "ru-RU",
                Name = "Russian",
                Id = Guid.Parse("f9044e38-eca2-4d91-a163-0e4fb64c2f01"),
            };
            if (!dbContext.Languages.Any(x => x.Code == ruLanguage.Code))
            {
                dbContext.Languages.Add(ruLanguage);
            }

            var esLanguage = new Language()
            {
                Code = "es-ES",
                Name = "Spanish",
                Id = Guid.Parse("f9044e38-eca2-4d91-a163-0e4fb64c2f02"),
            };
            if (!dbContext.Languages.Any(x => x.Code == esLanguage.Code))
            {
                dbContext.Languages.Add(esLanguage);
            }
        }
    }
}
