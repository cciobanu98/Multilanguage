using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Multilanguage.Domain.Models;
using System;

namespace Multilanguage.Infrastructure.Data.EntityConfigurations
{
    class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.HasData(new Language() //Default app language
            {
                Name = "British English",
                Code = "en-GB",
                Id = Guid.Parse("a02f7209-8e72-4e45-8d2a-29d892e1f66e")
            });
        }
    }
}
