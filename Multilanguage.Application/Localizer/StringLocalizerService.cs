using Microsoft.Extensions.Localization;
using Multilanguage.Application.Abstract;
using Multilanguage.Domain.Models;
using System.Globalization;
using System.Threading.Tasks;

namespace Multilanguage.Application.Localizer
{
    public class StringLocalizerService : IStringLocalizerService
    {
        private readonly IStringLocalizerFacade _stringLocalizerFacade;

        protected CultureInfo CurrentCulture => System.Threading.Thread.CurrentThread.CurrentCulture;

        public StringLocalizerService(IStringLocalizerFacade stringLocalizerFacade)
        {
            _stringLocalizerFacade = stringLocalizerFacade;
        }

        private async Task<string> GetString(string name, TranslationType type)
        {
            var value = await _stringLocalizerFacade.Get(name, CurrentCulture.Name, type);
            return value;
        }

        public async Task<LocalizedString> Get(string name, TranslationType type)
        {
            var value = await GetString(name, type);
            return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
        }

        public void WithCulture(CultureInfo culture)
        {
            CultureInfo.DefaultThreadCurrentCulture = culture;
        }
    }
}
