using Microsoft.Extensions.Localization;
using Multilanguage.Application.Abstract;
using System.Globalization;
using System.Threading.Tasks;

namespace Multilanguage.Infrastructure.Localizer
{
    public class StringLocalizerService : IStringLocalizerService
    {
        private readonly IStringLocalizerFacade _stringLocalizerFacade;

        protected CultureInfo CurrentCulture => System.Threading.Thread.CurrentThread.CurrentCulture;

        public StringLocalizerService(IStringLocalizerFacade stringLocalizerFacade)
        {
            _stringLocalizerFacade = stringLocalizerFacade;
        }

        private async Task<string> GetString(string name)
        {
            var value = await _stringLocalizerFacade.Get(name, CurrentCulture.Name);
            return value;
        }

        public async Task<LocalizedString> Get(string name)
        {
            var value = await GetString(name);
            return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
        }

        public void WithCulture(CultureInfo culture)
        {
            CultureInfo.DefaultThreadCurrentCulture = culture;
        }
    }
}
