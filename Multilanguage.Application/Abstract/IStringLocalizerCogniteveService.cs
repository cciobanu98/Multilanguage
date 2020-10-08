using System.Threading.Tasks;

namespace Multilanguage.Application.Abstract
{
    public interface IStringLocalizerCogniteveService
    {
        Task<string> Get(string key, string toLanguage);
    }
}
