using Multilanguage.Domain.Models;
using System.Threading.Tasks;

namespace Multilanguage.Application.Abstract
{
    public interface IStringLocalizerFacade
    {
        Task<string> Get(string key, string langCode, TranslationType type);

        Task Remove(string key, string langCode, TranslationType type);

        Task<Translation> Set(string key, string value, string langCode, TranslationType type);
    }
}
