using AppText.AdminApp.Configuration;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;

namespace AppText.AdminApp.Controllers
{
    [Route("")]
    public class AdminController : Controller
    {
        private readonly AppTextAdminConfigurationOptions _options;

        public AdminController(AppTextAdminConfigurationOptions options)
        {
            _options = options;
        }

        [HttpGet("{*catchall}")]
        public IActionResult AppTextAdmin()
        {
            var appBaseUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            var model = new AdminAppModel(_options, appBaseUrl);
            return View(model);
        }
    }

    public class AdminAppModel
    {
        private readonly AppTextAdminConfigurationOptions _options;
        private readonly string _appBaseUrl;

        public AdminAppModel(AppTextAdminConfigurationOptions options, string appBaseUrl)
        {
            _options = options;
            _appBaseUrl = appBaseUrl;
        }

        public string ApiBaseUrl
        {
            get { return _options.ApiBaseUrl ?? $"{_appBaseUrl}/{_options.RoutePrefix}"; }
        }

        public string AuthType
        {
            get { return _options.AuthType.ToString(); }
        }

        public string OidcSettings
        {
            get
            {
                if (_options.OidcSettings.Any())
                {
                    return JsonConvert.SerializeObject(_options.OidcSettings, Formatting.Indented);
                }
                return null;
            }
        }
    }
}
