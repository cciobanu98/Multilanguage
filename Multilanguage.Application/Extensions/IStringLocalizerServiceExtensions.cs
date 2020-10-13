using Microsoft.Extensions.Localization;
using Multilanguage.Application.Abstract;
using Multilanguage.Domain.Models;
using System.Threading.Tasks;

namespace Multilanguage.Application.Extensions
{
    public static class IStringLocalizerServiceExtensions
    {
        public static async Task<LocalizedString> GetError(this IStringLocalizerService stringLocalizer, string message)
        {
            return await stringLocalizer.Get(message, TranslationType.Error);
        }

        public static async Task<LocalizedString> GetUI(this IStringLocalizerService stringLocalizer, string message)
        {
            return await stringLocalizer.Get(message, TranslationType.UI);
        }
    }
}
