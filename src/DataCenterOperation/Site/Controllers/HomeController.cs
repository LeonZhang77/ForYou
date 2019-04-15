using DataCenterOperation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DataCenterOperation.Site.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger _logger;
        private readonly IUserService _userService;

        public HomeController(ILoggerFactory loggerFactory, IUserService userService)
            :base(userService)
        {
            _logger = loggerFactory.CreateLogger<HomeController>();
            _userService = userService;
        }

        [Authorize]
        public IActionResult Index()
        {
            ViewBag.AccessPremission = this.GetUser().Result.UserPermission;
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
