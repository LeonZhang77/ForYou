using DataCenterOperation.Data.Entities;
using DataCenterOperation.Services;
using DataCenterOperation.Site.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace DataCenterOperation.Site.Controllers
{
    public class BaseController : Controller
    {
        private readonly IUserService _userService;
        private readonly string _userClaimName = "userid";

        public BaseController(IUserService userService)
        {
            _userService = userService;
        }

        protected async Task<UserAccessViewModel> GetUser()
        {
            UserAccessViewModel loginUser = new UserAccessViewModel();

            ClaimsPrincipal claimsPrincipal = this.HttpContext.User;
            if (claimsPrincipal != null && claimsPrincipal.Identity.IsAuthenticated)
            {
                Claim claim = claimsPrincipal.FindFirst(_userClaimName);
                var user = await _userService.GetUserAsync(new Guid(claim.Value));

                if (user != null)
                {
                    loginUser = UserAccessViewModel.GetUserAcessInfo(user);
                }
            }
            return loginUser;
        }
    }
}