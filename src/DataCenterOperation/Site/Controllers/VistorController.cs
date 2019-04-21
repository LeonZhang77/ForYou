using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataCenterOperation.Data.Entities;
using DataCenterOperation.Services;
using DataCenterOperation.Site.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DataCenterOperation.Site.Controllers
{
    public class VistorController : Controller
    {
        private readonly IVistorRecordService _vistorRecordService;
        private readonly IVistorEntryRequestService _vistorEntryRequestService;
        private readonly IVistorEntourageService _vistorEntourageService;
        private readonly ILogger _logger;

        public VistorController(
            IVistorRecordService vistorRecordService,
            IVistorEntryRequestService vistorEntryRequestService,
            IVistorEntourageService vistorEntourageService,
            ILoggerFactory loggerFactory)
        {
            _vistorRecordService = vistorRecordService;
            _vistorEntryRequestService = vistorEntryRequestService;
            _vistorEntourageService = vistorEntourageService;
            _logger = loggerFactory.CreateLogger<HomeController>();
        }

        public async Task<IActionResult> History(string keyword, int? pageIndex, int? pageSize)
        {
            keyword = keyword ?? string.Empty;
            pageIndex = pageIndex ?? 1;
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            pageSize = pageSize ?? 20;
            pageSize = pageSize < 1 ? 20 : pageSize;

            var records = await _vistorRecordService.GetVistorRecordsAsync(keyword, pageIndex.Value, pageSize.Value);
            var items = records.Select(r => new EntryViewModel
            {
                ID = r.Id,
                VistorName = r.VistorName,
                NumberOfPeople = r.NumberOfPeople,
                EntryTime = r.EntryTime,
                Company = r.Company,
                Matter = r.Matter,
                ContactInfo = r.ContactInfo,
                VistorEntryRequestGuid = r.VistorEntryRequestGuid,
            });

            var count = await _vistorRecordService.CountAsync(keyword);

            var model = new HistorySearchViewModel
            {
                Items = items,
                Keyword = keyword,
                PageIndex = pageIndex.Value,
                PageSize = pageSize.Value,
                Count = count
            };

            return View(model);
        }

        public async Task<IActionResult> Add_ContactInfo()
        {
            var guid = new Guid(Request.Form["id"]);
            var contactInfo = Request.Form["contactInfo"];
            VistorRecord vistorRecord = await _vistorRecordService.UpdateContactInfo(guid, contactInfo);
            return Redirect("/Vistor/History");
        }

        public async Task<String> Details()
        {
            var id = new Guid(Request.Form["id"]);
            VistorEntryRequest entryRequest = await _vistorEntryRequestService.GetVistorEntryRequestsById(id);
            EntryRequestViewModel model = new EntryRequestViewModel();
            model.RequestPeoopleName = entryRequest.RequestPeoopleName;
            model.Company = entryRequest.Company;
            model.RequestDate = entryRequest.RequestDate;
            model.BeginTime = entryRequest.BeginTime;
            model.EndTime = entryRequest.EndTime;
            model.Area = entryRequest.Area;
            model.Belongings = entryRequest.Belongings;
            model.Matter_Short = entryRequest.Matter_Short;
            model.Matter_Details = entryRequest.Matter_Details;
            model.Admin_Confirm = entryRequest.Admin_Confirm;
            model.Manager_Confirm = entryRequest.Manager_Confirm;

            List<VistorEntourage> entourages = await _vistorEntourageService.GetVistorEntourageByRequestGUID(id);
            model.Entourage = JsonConvert.SerializeObject(entourages);

            string result = JsonConvert.SerializeObject(model);
            return (result);
        }

        [HttpGet]
        public IActionResult EntryRequest()
        {
            var theMoment = DateTime.Now;
            var request = new EntryRequestViewModel();
            request.RequestDate = theMoment;
            request.BeginTime = theMoment;
            request.EndTime = theMoment;

            return View(request);
        }

        [HttpPost]
        public IActionResult EntryRequest(EntryRequestViewModel requestModel)
        {
            string str_requestDate = requestModel.RequestDate.ToString("yyyy-MM-dd");
            string str_beginTime = requestModel.BeginTime.TimeOfDay.ToString();
            string str_endTime = requestModel.EndTime.TimeOfDay.ToString();

            var request = new VistorEntryRequest
            {
                RequestPeoopleName = requestModel.RequestPeoopleName,
                Company = requestModel.Company,
                RequestDate = requestModel.RequestDate,
                BeginTime = Convert.ToDateTime(str_requestDate + " " + str_beginTime),
                EndTime = Convert.ToDateTime(str_requestDate + " " + str_endTime),
                Area = requestModel.Area,
                Belongings = requestModel.Belongings,
                Matter_Short = requestModel.Matter_Short,
                Matter_Details = requestModel.Matter_Details,
                Admin_Confirm = requestModel.Admin_Confirm,
                Manager_Confirm = requestModel.Manager_Confirm,
            };


            ICollection<VistorEntourage> vistorEntourages = EntryRequestViewModel.GetEntourage(requestModel.Entourage);
            request.Entourage = vistorEntourages;

            _vistorEntryRequestService.AddVistorEntryRequest(request);

            return Redirect("./History");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
