using DataCenterOperation.Site.Extensions;
using DataCenterOperation.Site.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DataCenterOperation.Site.Controllers
{
    [Authorize]
    public class FailuresController : Controller
    {
        public IActionResult Create()
        {
            var theMoment = DateTime.Now;
            var model = new FailureCreateViewModel
            {
                WhoRecorded = User.GetUsername(),
                DateRecorded = theMoment,
                TimeRecorded = theMoment
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(FailureCreateViewModel model)
        {
            return View();
        }
    }
}