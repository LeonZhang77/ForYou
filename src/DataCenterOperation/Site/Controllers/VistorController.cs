using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DataCenterOperation.Services;
using DataCenterOperation.Site.Extensions;
using DataCenterOperation.Site.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataCenterOperation.Data;
using DataCenterOperation.Data.Entities;
using Newtonsoft.Json;

namespace DataCenterOperation.Site.Controllers
{
    public class VistorController : Controller
    {
        private readonly IVistorRecordService _vistorRecordService;
        private readonly IVistorEntryRequestService _vistorEntryRequestService;
        private readonly ILogger _logger;        

        public VistorController(
            IVistorRecordService vistorRecordService,
            IVistorEntryRequestService vistorEntryRequestService,
            ILoggerFactory loggerFactory)
        {
            _vistorRecordService = vistorRecordService;
            _vistorEntryRequestService = vistorEntryRequestService;
            _logger = loggerFactory.CreateLogger<HomeController>();
        }        

        public async Task<IActionResult> History()
        {
            List<VistorRecord> records = (await _vistorRecordService.GetAllVistorRecords());
            List<EntryViewModel> models = new List<EntryViewModel>();
            foreach (VistorRecord item in records)
            {
                EntryViewModel model = new EntryViewModel();
                model.ID = item.Id;
                model.VistorName = item.VistorName;
                model.NumberOfPeople = item.NumberOfPeople;
                model.EntryTime = item.EntryTime;
                model.Company = item.Company;
                model.Matter = item.Matter;
                model.ContactInfo = item.ContactInfo;
                model.VistorEntryRequestGuid = item.VistorEntryRequestGuid;
                models.Add(model);
            }

            return View(models);
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
            string result = JsonConvert.SerializeObject(model);
            return (result);
        }

        [HttpGet]
        public IActionResult EntryRequest()
        {
            var request = new EntryRequestViewModel();
           
            return View(request);
        }

        [HttpPost]
        public IActionResult EntryRequest(EntryRequestViewModel requestModel)
        {
            var request = new VistorEntryRequest
            {
              RequestPeoopleName = requestModel.RequestPeoopleName,
              Company = requestModel.Company,
              RequestDate = requestModel.RequestDate,
              BeginTime = requestModel.BeginTime,
              EndTime = requestModel.EndTime,
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

        [HttpGet]
        public IActionResult Entry()
        {
            var vistor = new EntryViewModel();
            //vistor.EntryTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm");
            //vistor.EntryTime.GetDateTimeFormats('t');
            //vistor.Matter = "";
            return View(vistor);
        }

        [HttpPost]
        public IActionResult Entry(EntryViewModel entry)
        {
            var vistor = _vistorRecordService.AddVistorAsync(entry.VistorName, entry.NumberOfPeople, entry.EntryTime, entry.Company, entry.Matter, entry.ContactInfo);
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
