using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Multilanguage.Application.Abstract;
using Multilanguage.Application.Options;
using Multilanguage.Domain.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Multilanguage.Application.Localizer
{
    public class StringLocalizerFacade : IStringLocalizerFacade
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizerCogniteveService _cogniteveService;
        private readonly ILogger<StringLocalizerFacade> _logger;
        private readonly IOptions<MultilanguageOptions> _multilanguageOptions;

        public StringLocalizerFacade(IMemoryCache memoryCache,
                                     IStringLocalizerCogniteveService cogniteveService,
                                     IUnitOfWork unitOfWork,
                                     ILogger<StringLocalizerFacade> logger,
                                     IOptions<MultilanguageOptions> multilanguageOptions)
        {
            _memoryCache = memoryCache;
            _cogniteveService = cogniteveService;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _multilanguageOptions = multilanguageOptions;
        }

        public async Task<string> Get(string key, string langCode, TranslationType type)
        {
            var value = GetTranslationFromCache(key, langCode, type);
            if (value == null)
            {
                var translation = await _unitOfWork.Set<Translation>().FirstOrDefaultAsync(x => x.Key == key && x.Language.Code == langCode && x.Type == type);
                if (translation == null)
                {
                    _logger.LogInformation("Get value from azure cognitive service");
                    var translatedText = await _cogniteveService.Get(key, langCode);
                    value = await Set(key, translatedText, langCode, type);
                    if (_multilanguageOptions.Value.NeedToBeVerfied != null &&
                         _multilanguageOptions.Value.NeedToBeVerfied.Any(x => x == type))
                    {
                        SetCacheForNotVerifiedItem(key, langCode, type);
                        return key;
                    }
                }
                else
                {
                    _logger.LogInformation("Get value from database");
                    value = translation;
                    if (_multilanguageOptions.Value.NeedToBeVerfied != null &&
                        _multilanguageOptions.Value.NeedToBeVerfied.Any(x => x == translation.Type) &&
                        !translation.IsVerified)
                    {
                        SetCacheForNotVerifiedItem(key, langCode, type);
                        return key;
                    }
                }
                SetCache(key, value, langCode, type);

            }
            else
            {
                _logger.LogInformation("Get value from cache");
            }
            return value.Value;
        }

        public async Task Remove(string key, string langCode, TranslationType type)
        {
            var translation = await _unitOfWork.Set<Translation>().FirstOrDefaultAsync(x => x.Key == key && x.Language.Code == langCode && x.Type == type);
            _unitOfWork.Set<Translation>().Remove(translation);
            _memoryCache.Remove(key + langCode);
        }

        public async Task<Translation> Set(string key, string value, string langCode, TranslationType type)
        {
            var language = await _unitOfWork.Set<Language>().FirstOrDefaultAsync(x => x.Code == langCode);
            if (language == null)
            {
                throw new ArgumentNullException(nameof(language));
            }
            Translation translation = new Translation() { Key = key, Language = language, Value = value, Type = type };
            _unitOfWork.Set<Translation>().Add(translation);
            await _unitOfWork.SaveChangesAsync();
            return translation;
        }

        private void SetCache(string key, Translation value, string langCode, TranslationType type)
        {
            var options = new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(30),
            };
            var cachekey = GetCacheKey(key, langCode, type);
            _memoryCache.Set(cachekey, value, options);
        }

        private void SetCacheForNotVerifiedItem(string key, string langCode, TranslationType type)
        {
            var translation = new Translation() { Key = key, Value = key, Type = type, IsVerified = false };
            SetCache(key, translation, langCode, type);
        }

        private object GetCacheKey(string key, string langCode, TranslationType type)
        {
            return new { key, langCode, type };
        }

        private Translation GetTranslationFromCache(string key, string langCode, TranslationType type)
        {
            var cachekey = GetCacheKey(key, langCode, type);
            Translation value = _memoryCache.Get(cachekey) as Translation;
            return value;
        }
    }
}
