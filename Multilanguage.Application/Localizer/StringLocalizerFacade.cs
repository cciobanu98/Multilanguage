﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<string> Get(string key, string langCode)
        {
            string value = _memoryCache.Get(key + langCode) as string;
            if (value == null)
            {
                var translation = await _unitOfWork.Set<Translation>().FirstOrDefaultAsync(x => x.Key == key && x.Language.Code == langCode);
                if (translation == null)
                {
                    _logger.LogInformation("Get value from azure cognitive service");
                    value = await _cogniteveService.Get(key, langCode);
                    await Set(key, value, langCode);
                    if (_multilanguageOptions.Value.NeedToBeVerfied != null && _multilanguageOptions.Value.NeedToBeVerfied.Count > 0)
                    {
                        return key;
                    }
                }
                else
                {
                    _logger.LogInformation("Get value from database");
                    value = translation.Value;
                    if (_multilanguageOptions.Value.NeedToBeVerfied != null &&
                        _multilanguageOptions.Value.NeedToBeVerfied.Any(x => x == translation.Type) &&
                        !translation.IsVerified)
                    {
                        return key;
                    }
                }
                SetCache(key, value, langCode);

            }
            else
            {
                _logger.LogInformation("Get value from cache");
            }
            return value;
        }

        public async Task Remove(string key, string langCode)
        {
            var translation = await _unitOfWork.Set<Translation>().FirstOrDefaultAsync(x => x.Key == key && x.Language.Code == langCode);
            _unitOfWork.Set<Translation>().Remove(translation);
            _memoryCache.Remove(key + langCode);
        }

        public async Task Set(string key, string value, string langCode)
        {
            var language = await _unitOfWork.Set<Language>().FirstOrDefaultAsync(x => x.Code == langCode);
            if (language == null)
            {
                throw new ArgumentNullException(nameof(language));
            }
            Translation translation = new Translation() { Key = key, Language = language, Value = value };
            _unitOfWork.Set<Translation>().Add(translation);
            await _unitOfWork.SaveChangesAsync();
        }

        private void SetCache(string key, string value, string langCode)
        {
            var options = new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(30),
            };
            _memoryCache.Set(key + langCode, value, options);
        }
    }
}
