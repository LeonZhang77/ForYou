using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataCenterOperation.Services;
using DataCenterOperation.Site.Extensions;
using DataCenterOperation.Site.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;

namespace DataCenterOperation.Site.Controllers
{
    [Authorize]
    public class FailuresController : Controller
    {
        private IFailureService _failureService;
        private ILogger<FailuresController> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;

        public FailuresController(IFailureService failureService,
            IHostingEnvironment hostingEnvironment,
            ILogger<FailuresController> logger)
        {
            _failureService = failureService;
            _hostingEnvironment = hostingEnvironment;
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
                WhoRecorded = f.WhoRecorded,
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

        public async Task<string> Report_Upload(IFormCollection upload_files)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            var filePath = "";
            foreach (var item in upload_files.Files)
            {
                var filename = Guid.NewGuid();

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                { filePath = webRootPath + "\\upload\\FailersReport\\" + filename.ToString() + ".jpg"; }
                else
                { filePath = webRootPath + "/upload/FailersReport/" + filename.ToString() + ".jpg"; }


                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await item.CopyToAsync(stream);
                }
            }
            return filePath;
        }

        public string Report_Rename()
        {
            var new_file_name = Request.Form["id"];
            var old_file_Path = Request.Form["filePath"];
            var new_file_path = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                new_file_path = Path.GetDirectoryName(old_file_Path) + "\\" + new_file_name + Path.GetExtension(old_file_Path);
            }
            else
            {
                new_file_path = Path.GetDirectoryName(old_file_Path) + "/" + new_file_name + Path.GetExtension(old_file_Path);
            }

            System.IO.File.Move(old_file_Path, new_file_path);
            
            return new_file_path;
        }

        public int Report_Remove()
        {
            var returndata = 0;
            string webRootPath = _hostingEnvironment.WebRootPath;
            var file_name = Request.Form["id"];
            var file_path = "";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            { file_path = webRootPath + "\\upload\\FailersReport\\" + file_name + ".jpg"; }
            else
            { file_path = webRootPath + "/upload/FailersReport/" + file_name + ".jpg"; }

            if (System.IO.File.Exists(file_path)) 
            {
                System.IO.File.Delete(file_path);
                returndata = 1; 
            }

            return returndata;
        }

        public int Report_Show()
        {
            var returndata = 0;
            string webRootPath = _hostingEnvironment.WebRootPath;
            var file_name = Request.Form["id"];
            var file_path = "";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            { file_path = webRootPath + "\\upload\\FailersReport\\" + file_name + ".jpg"; }
            else
            { file_path = webRootPath + "/upload/FailersReport/" + file_name + ".jpg"; }

            if (System.IO.File.Exists(file_path)) { returndata = 1; }

            return returndata;
        }
    }
}