using AppText.AdminApp.Configuration;
using AppText.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
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
            var appBaseUrl = $"//{Request.Host}{Request.PathBase}";
            var model = new AdminAppModel(_options, appBaseUrl, Request.PathBase);
            return View(model);
        }
    }

    public class AdminAppModel
    {
        private readonly AppTextAdminConfigurationOptions _options;
        private readonly string _appBaseUrl;
        private readonly string _pathBase;

        public AdminAppModel(AppTextAdminConfigurationOptions options, string appBaseUrl, string pathBase)
        {
            _options = options;
            _appBaseUrl = appBaseUrl;
            _pathBase = pathBase.EnsureStartsWith("/").EnsureEndsWith("/");
        }

        public string ClientBaseRoute
        {
            get { return $"{_pathBase}{_options.RoutePrefix}".EnsureEndsWith("/"); }
        }

        public string ApiBaseUrl
        {
            get
            {
                var defaultApiBaseUrl = $"{_appBaseUrl}/{_options.RoutePrefix}".EnsureDoesNotEndWith("/");
                return _options.ApiBaseUrl ?? defaultApiBaseUrl;
            }
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
