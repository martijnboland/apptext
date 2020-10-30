using LiteDB.Identity.Database;
using LiteDB.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AppText.Host.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<LiteDbUser> _userManager;
        private readonly SignInManager<LiteDbUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ILiteDbIdentityContext _liteDbIdentityContext;

        public AccountController(
            UserManager<LiteDbUser> userManager,
            SignInManager<LiteDbUser> signInManager,
            ILogger<AccountController> logger,
            IConfiguration configuration,
            ILiteDbIdentityContext liteDbIdentityContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _configuration = configuration;
            _liteDbIdentityContext = liteDbIdentityContext;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            var collection = _liteDbIdentityContext.LiteDatabase.GetCollection<LiteDbUser>();
            if (collection.Count() == 0)
            {
                return RedirectToAction("Create");
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user == null)
                {
                    _logger.LogWarning("User {0} not found", model.Username);
                }
                else
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User logged in.");

                        return RedirectToLocal(returnUrl);
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return Redirect("~/");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Create()
        {
            // Only display create form when no user exists
            var collection = _liteDbIdentityContext.LiteDatabase.GetCollection<LiteDbUser>();
            if (collection.Count() > 0)
            {
                return Redirect("~/");
            }
            var model = new CreateViewModel
            {
                Username = _configuration["AdminUser"]
            };
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new LiteDbUser { UserName = model.Username, Email = "admin@apptext.io" };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return Redirect("~/");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return Redirect("~/");
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
