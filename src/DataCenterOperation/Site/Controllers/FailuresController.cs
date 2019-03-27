using DataCenterOperation.Services;
using DataCenterOperation.Site.Extensions;
using DataCenterOperation.Site.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DataCenterOperation.Site.Controllers
{
    [Authorize]
    public class FailuresController : Controller
    {
        private IFailureService _failureService;
        private ILogger<FailuresController> _logger;

        public FailuresController(IFailureService failureService,
            ILogger<FailuresController> logger)
        {
            _failureService = failureService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return Content("Index");
        }

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
        public async Task<IActionResult> Create(FailureCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Please check the inputs.");
                return View(model);
            }

            try
            {
                _logger.LogInformation($"{User.GetUsername()} is recording a device failure...");

                var failure = await _failureService.RecordFailureAsync(model);

                _logger.LogInformation($"A device failure is recorded with this Id: {failure.Id} by {User.GetUsername()}.");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "", model);

                throw ex;
            }
        }
    }
}