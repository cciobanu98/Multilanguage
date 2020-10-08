using System;

namespace Multilanguage.Domain.Models
{
    public class Translation : EntityBase
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public string Context { get; set; }

        public Guid LanguageId { get; set; }

        public Language Language { get; set; }

        public bool IsVerified { get; set; }

        public TranslationType Type { get; set; }
    }
}
