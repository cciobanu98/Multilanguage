using System.Threading.Tasks;

namespace Multilanguage.Application.Abstract
{
    public interface IStringLocalizerFacade
    {
        Task<string> Get(string key, string langCode);

        Task Remove(string key, string langCode);

        Task Set(string key, string value, string langCode);
    }
}
