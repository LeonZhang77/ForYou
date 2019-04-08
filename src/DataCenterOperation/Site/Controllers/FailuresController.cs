using System;
using System.Linq;
using System.Threading.Tasks;
using DataCenterOperation.Services;
using DataCenterOperation.Site.Extensions;
using DataCenterOperation.Site.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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

        public async Task<IActionResult> Index(string keyword, int? pageIndex, int? pageSize)
        {
            keyword = keyword ?? string.Empty;
            pageIndex = pageIndex ?? 1;
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            pageSize = pageSize ?? 20;
            pageSize = pageSize < 1 ? 20 : pageSize;

            int itemIndexStart = (pageIndex.Value - 1) * pageSize.Value;
            var failures = await _failureService.GetAsync(keyword, pageIndex.Value, pageSize.Value);
            var items = failures.Select(f => new FailureListItem
            {
                Index = ++itemIndexStart,
                Id = f.Id,
                DeviceName = f.DeviceName,
                FailureCause = f.FailureCause,
                DateRecorded = f.WhenRecorded,
                DateSolved = f.WhenSolved
            });

            var count = await _failureService.CountAsync(keyword);

            var model = new FailureSearchViewModel
            {
                Failures = items,
                Keyword = keyword,
                PageIndex = pageIndex.Value,
                PageSize = pageSize.Value,
                Count = count
            };

            return View(model);
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

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return BadRequest();

            var failure = await _failureService.GetAsync(id.Value);
            if (failure == null) return NotFound();

            var model = FailureDetailsViewModel.CreateFromEntity(failure);

            return View(model);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return BadRequest();

            var failure = await _failureService.GetAsync(id.Value);
            if (failure == null) return NotFound();

            var model = FailureEditViewModel.CreateFromEntity(failure);

            return View("Create", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid? id, FailureEditViewModel model)
        {
            if (id == null) return BadRequest();
            if (id.Value != model.Id) return BadRequest();

            var failure = await _failureService.UpdateAsync(model);
            if (failure == null) return NotFound();

            return RedirectToAction("Details", new { id = failure.Id });
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return BadRequest();

            var failure = await _failureService.GetAsync(id.Value);
            if (failure == null) return NotFound();

            var model = FailureDetailsViewModel.CreateFromEntity(failure);

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DoDelete(Guid? id)
        {
            if (id == null) return BadRequest();

            await _failureService.DeleteAsync(id.Value);

            return RedirectToAction("Index");
        }
    }
}