using Microsoft.EntityFrameworkCore;
using Multilanguage.Application.Abstract;
using Multilanguage.Domain.Models;

namespace Multilanguage.Infrastructure.Data
{
    public class MultilanguageDbContext : DbContext, IUnitOfWork
    {
        public MultilanguageDbContext(DbContextOptions<MultilanguageDbContext> options) : base (options)
        {

        }

        public DbSet<Language> Languages { get; set; }

        public DbSet<Translation> Translations { get; set; }
    }
}
