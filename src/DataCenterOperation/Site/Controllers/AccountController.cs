using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DataCenterOperation.Data.Entities;
using DataCenterOperation.Services;
using DataCenterOperation.Site.Extensions;
using DataCenterOperation.Site.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DataCenterOperation.Site.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly ILogger _logger;
        private readonly string _cookieScheme = "DataCenterOperation.CookieScheme";

        public AccountController(
            IAccountService accountService,
            IUserService userService,
            ILoggerFactory loggerFactory)
        {
            _accountService = accountService;
            _userService = userService;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing cookie to ensure a clean login process
            await HttpContext.SignOutAsync(_cookieScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = await _accountService.LoginAsync(model.Username, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "用户名或密码不正确。");
                    return View(model);
                }

                if (user.Disabled)
                {
                    _logger.LogWarning(2, "此账户已被禁止登陆。");
                    return View("Disabled");
                }

                var userClaims = new List<Claim>{
                        new Claim("userid", user.Id.ToString()),
                        new Claim("username", model.Username),
                        new Claim("isadmin", user.IsAdmin.ToString()),
                        new Claim("updatedtime", user.UpdatedTime.Value.Ticks.ToString())
                    };
                var principal = new ClaimsPrincipal(new ClaimsIdentity(userClaims, "AuthenticationTypes.Password"));
                await HttpContext.SignInAsync(_cookieScheme, principal);

                _logger.LogInformation($"User {user.Username} logged in.");

                return RedirectToLocal(returnUrl, isAdmin: user.IsAdmin);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(_cookieScheme);
            _logger.LogInformation($"User {HttpContext.User?.GetUsername()} logged out. Redirecting to HOME...");

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ResetSystemAdmin()
        {
            if ("localhost".Equals(Request.Host.Host, StringComparison.OrdinalIgnoreCase))
            {
                await _accountService.ResetSystemAdmin();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Management()
        {
            var users = await _userService.GetAllUsers();

            return View(users);
        }

        public async Task<string> User_Add()
        {
            var returnValue = "/account/management";

            var username = Request.Form["username"];
            var password = Request.Form["password"];
            var role = Request.Form["role"];

            var entity = new User
            {
                Username = username,
                Password = password
            };
            entity.IsAdmin = role.Equals("Admin") ? true : false;

            await _userService.AddOrUpdateUser(entity);

            return returnValue;
        }

        public async Task<string> User_Remove()
        {
            var returnValue = "/account/management";

            var guid = new Guid(Request.Form["id"]);

            await _userService.RemoveUser(guid);

            return returnValue;
        }

        public async Task<string> User_Modify()
        {
            var returnValue = "/account/management";

            var guid = new Guid(Request.Form["id"]);
            var password = Request.Form["password"];
            var role = Request.Form["role"];

            var entity = await _userService.GetUserAsync(guid);
            entity.IsAdmin = role.Equals("Admin") ? true : false;
            entity.Password = password;

            await _userService.AddOrUpdateUser(entity);

            return returnValue;
        }

        #region helper methods

        private IActionResult RedirectToLocal(string returnUrl, bool isAdmin)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}
