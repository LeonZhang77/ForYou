using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using DataCenterOperation.Services;
using DataCenterOperation.Site.Extensions;
using DataCenterOperation.Site.ViewModels;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataCenterOperation.Data;
using DataCenterOperation.Data.Entities;
using DataCenterOperation.Util;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace DataCenterOperation.Site.Controllers
{
    public class AssertController : Controller
    {
        private readonly IAssertX86ServerService _assertX86ServerService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private ILogger<AssertController> _logger;

        public AssertController(
            IAssertX86ServerService assertX86ServerService,
            IHostingEnvironment hostingEnvironment,
            ILogger<AssertController> logger
            )
        {
            _assertX86ServerService = assertX86ServerService;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }
        public IActionResult Overview()
        {
            return View();
        }

        public IActionResult Server()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> X86Server()
        {
            List<Assert_X86Server> servers = (await _assertX86ServerService.GetAllAssertX86Server());
            List<AssertX86ServerViewModel> models = new List<AssertX86ServerViewModel>();

            foreach (Assert_X86Server item in servers)
            {
                AssertX86ServerViewModel model = new AssertX86ServerViewModel();
                model.ID = item.Id;
                model.FixedAssertNumber = item.FixedAssertNumber;
                model.Name = item.Name;
                model.SerialNumber = item.SerialNumber;
                model.HD = item.HD;
                model.OS = item.OS;
                model.EngineNumber = item.EngineNumber;
                model.RackLocation = item.RackLocation;
                model.BeginU = item.BeginU;
                model.EndU = item.EndU;
                model.VirtualizedResourcePool = item.VirtualizedResourcePool;
                model.BusinessSystem = item.BusinessSystem;
                model.IP = item.IP;
                model.NetcardNumber = item.NetcardNumber;
                model.HBANumber = item.HBANumber;
                model.StorageSize = item.StorageSize;
                model.MaintenanceInformation = item.MaintenanceInformation;
                model.InstallDate = item.InstallDate;
                model.Band = item.Band;
                model.CPU = item.CPU;
                model.Memory = item.Memory;
                models.Add(model);
            }            
            return View(models);
        }

        [HttpGet]
        public IActionResult X86Server_Add()
        {            
            var request = new AssertX86ServerViewModel();
            request.InstallDate = DateTime.Now;

            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> X86Server_Add(AssertX86ServerViewModel model)
        {
            var entity = new Assert_X86Server
            {
                FixedAssertNumber = model.FixedAssertNumber,
                Name = model.Name,
                SerialNumber = model.SerialNumber,
                HD = model.HD,
                OS = model.OS,
                EngineNumber = model.EngineNumber,
                RackLocation = model.RackLocation,
                BeginU = model.BeginU,
                EndU = model.EndU,
                VirtualizedResourcePool = model.VirtualizedResourcePool,
                BusinessSystem = model.BusinessSystem,
                IP = model.IP,
                NetcardNumber = model.NetcardNumber,
                HBANumber = model.HBANumber,
                StorageSize = model.StorageSize,
                MaintenanceInformation = model.MaintenanceInformation,
                InstallDate = model.InstallDate,
                Band = model.Band,
                CPU = model.CPU
            };
            entity.Memory = model.Memory;
            await _assertX86ServerService.AddAssertX86Server(entity);
            return Redirect("./X86Server");
        }

        [HttpGet]
        public async Task<IActionResult> X86Server_Verify_List() {
            //    string filePath = Request.Form["filePath"];
            string filePath = Request.QueryString.Value.Substring(1);
                
            List<string[]> str_values_List = Util.Excel_Utils.GetSheetValuesFromExcel(filePath);
            AssertX86ServerVerifyListViewModel returnModel = new AssertX86ServerVerifyListViewModel();
            //foreach (var str_arr in str_values_List)
            for (var i = 2; i < str_values_List.Count; i++)
            {
                AssertX86ServerViewModel model = new AssertX86ServerViewModel();
                model.FixedAssertNumber = str_values_List[i][0];
                model.Name = str_values_List[i][1];
                model.SerialNumber = str_values_List[i][2];
                model.Band = str_values_List[i][3];
                model.CPU = str_values_List[i][4];
                model.Memory = str_values_List[i][5];
                model.HD = str_values_List[i][6];
                model.OS = str_values_List[i][7];
                model.EngineNumber = str_values_List[i][8];
                model.RackLocation = str_values_List[i][9];
                model.BeginU = str_values_List[i][10];
                model.EndU = str_values_List[i][11];
                model.VirtualizedResourcePool = str_values_List[i][12];
                model.BusinessSystem = str_values_List[i][13];
                model.IP = str_values_List[i][14];
                model.NetcardNumber = str_values_List[i][15];
                model.HBANumber = str_values_List[i][16];
                model.StorageSize = str_values_List[i][17];
                model.MaintenanceInformation = str_values_List[i][18];
                if (String.IsNullOrEmpty(str_values_List[i][19]))
                {
                    model.InstallDate = null;
                }
                else
                {
                    model.InstallDate = Convert.ToDateTime(str_values_List[i][18]);
                }

                if (model.FixedAssertNumber == "")
                {
                    returnModel.ReadyToAdd.Add(model);
                }
                else
                {
                    Assert_X86Server server = await _assertX86ServerService.GetAssertX86ServerByColumn(model.FixedAssertNumber, ENUMS.Type.FixedAssertNumber);

                    if (server != null)
                    {
                        returnModel.ReadyToModify.Add(model);
                    }
                    else
                    {
                        returnModel.ReadyToAdd.Add(model);
                    }
                }

            }

            //return Redirect("./OverView");
            return View(returnModel);

        }
        
        [HttpPost]
        public async Task<IActionResult> X86Server_Verify_List(AssertX86ServerVerifyListViewModel verifyListModel)
        {
            foreach (var model in verifyListModel.ReadyToAdd)
            {
                var entity = new Assert_X86Server
                {
                    FixedAssertNumber = model.FixedAssertNumber,
                    Name = model.Name,
                    SerialNumber = model.SerialNumber,
                    HD = model.HD,
                    OS = model.OS,
                    EngineNumber = model.EngineNumber,
                    RackLocation = model.RackLocation,
                    BeginU = model.BeginU,
                    EndU = model.EndU,
                    VirtualizedResourcePool = model.VirtualizedResourcePool,
                    BusinessSystem = model.BusinessSystem,
                    IP = model.IP,
                    NetcardNumber = model.NetcardNumber,
                    HBANumber = model.HBANumber,
                    StorageSize = model.StorageSize,
                    MaintenanceInformation = model.MaintenanceInformation,
                    InstallDate = model.InstallDate,
                    Band = model.Band,
                    CPU = model.CPU
                };
                entity.Memory = model.Memory;
                await _assertX86ServerService.AddAssertX86Server(entity);
            }

            foreach (var model in verifyListModel.ReadyToModify)
            {
                var entity = new Assert_X86Server
                {
                    FixedAssertNumber = model.FixedAssertNumber,
                    Name = model.Name,
                    SerialNumber = model.SerialNumber,
                    HD = model.HD,
                    OS = model.OS,
                    EngineNumber = model.EngineNumber,
                    RackLocation = model.RackLocation,
                    BeginU = model.BeginU,
                    EndU = model.EndU,
                    VirtualizedResourcePool = model.VirtualizedResourcePool,
                    BusinessSystem = model.BusinessSystem,
                    IP = model.IP,
                    NetcardNumber = model.NetcardNumber,
                    HBANumber = model.HBANumber,
                    StorageSize = model.StorageSize,
                    MaintenanceInformation = model.MaintenanceInformation,
                    InstallDate = model.InstallDate,
                    Band = model.Band,
                    CPU = model.CPU
                };
                entity.Memory = model.Memory;
                await _assertX86ServerService.UpdateAssertX86Server(entity);
            }

            return Redirect("./X86Server");
        }

        public async Task<string> X86Server_Upload(IFormCollection upload_files)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            var filePath = "";

            foreach (var item in upload_files.Files)
            {
                var filename = Guid.NewGuid();

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                { filePath = webRootPath + "\\upload\\" + filename.ToString() + ".xls"; }
                else 
                { filePath = webRootPath + "/upload/" + filename.ToString() + ".xls";  }


                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await item.CopyToAsync(stream);
                }
            }
            return filePath;
        }

        public async Task<string> X86Server_Remove()
        {
            var guid = new Guid(Request.Form["id"]);

            try
            {
                _logger.LogInformation($"{User.GetUsername()} is removing a X86 Server...");

                var x86Server = await _assertX86ServerService.RemoveAssertX86ServerByGuid(guid);

                return ("/Assert/X86Server");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "", guid);

                throw ex;
            }
        }
    }
}