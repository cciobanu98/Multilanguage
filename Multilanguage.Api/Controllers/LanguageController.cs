using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Multilanguage.Application.Abstract;
using Multilanguage.Application.Extensions;
using Multilanguage.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Multilanguage.Api.Controllers
{
    [Route("api/language")]
    public class LanguageController : BaseController
    {
        private readonly IStringLocalizerService _stringLocalizerService;
        private readonly IUnitOfWork _unitOfWork;
        public LanguageController(IStringLocalizerService stringLocalizerService, IUnitOfWork unitOfWork)
        {
            _stringLocalizerService = stringLocalizerService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("set")]
        public ActionResult SetCookie(string culture)
        {
            var languageActive = _unitOfWork.Set<Language>().Any(x => x.Code == culture);
            if (!languageActive)
            {
                throw new NotSupportedException($"Culture {culture} it's not supported");
            }
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get(string text)
        {
            var error = await _stringLocalizerService.GetError(text); //Error message will be translated because it's dont't need to be verified
            var ui = await _stringLocalizerService.GetUI(text); //UI message need to be verified.
            return new List<string>() { error.Value, ui.Value };
        }
    }
}
