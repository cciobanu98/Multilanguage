using Microsoft.Extensions.Localization;
using Multilanguage.Domain.Models;
using System.Globalization;
using System.Threading.Tasks;

namespace Multilanguage.Application.Abstract
{
    public interface IStringLocalizerService
    {
        Task<LocalizedString> Get(string name, TranslationType type);

        void WithCulture(CultureInfo culture);
    }
}
