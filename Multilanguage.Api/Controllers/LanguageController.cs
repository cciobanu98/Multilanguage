using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Multilanguage.Application.Abstract;
using System;
using System.Threading.Tasks;

namespace Multilanguage.Api.Controllers
{
    [Route("api/language")]
    public class LanguageController : BaseController
    {
        private readonly IStringLocalizerService _stringLocalizerService;
        public LanguageController(IStringLocalizerService stringLocalizerService)
        {
            _stringLocalizerService = stringLocalizerService;
        }

        [HttpPost("set")]
        public ActionResult SetCookie(string culture)
        {
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
