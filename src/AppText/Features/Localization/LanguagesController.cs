using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace AppText.Features.Localization
{
    [Route("languages")]
    public class LanguagesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var cultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures)
                .Where(ci => ci.Name != String.Empty)
                .Select(ci => new { Code = ci.Name, Description = ci.EnglishName });
            return Ok(cultures);
        }
    }
}