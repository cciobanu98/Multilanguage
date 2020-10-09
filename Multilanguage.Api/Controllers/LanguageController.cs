using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Multilanguage.Application.Abstract;
using Multilanguage.Domain.Models;
using System;
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
        public async Task<ActionResult<string>> Get(string text)
        {
            var test = Response;
            var tes2t = Request;
            var message = await _stringLocalizerService.Get(text);
            return message.Value;
        }
    }
}
