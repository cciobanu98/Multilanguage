using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Threading.Tasks;

namespace Multilanguage.Application.Abstract
{
    public interface IStringLocalizerService
    {
        Task<LocalizedString> Get(string name);

        void WithCulture(CultureInfo culture);
    }
}
