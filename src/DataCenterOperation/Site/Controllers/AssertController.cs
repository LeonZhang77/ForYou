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
        private readonly IAssertX86ServerUserInformationService _assertX86ServerUserInformationService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private ILogger<AssertController> _logger;

        public AssertController(
            IAssertX86ServerService assertX86ServerService,
            IAssertX86ServerUserInformationService assertX86ServerUserInformationService,
            IHostingEnvironment hostingEnvironment,
            ILogger<AssertController> logger
            )
        {
            _assertX86ServerService = assertX86ServerService;
            _assertX86ServerUserInformationService = assertX86ServerUserInformationService;
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
            string filePath = Request.QueryString.Value.Substring(1);
                
            List<string[]> str_values_List = Util.Excel_Utils.GetSheetValuesFromExcel(filePath);
            List<AssertX86ServerViewModel> tempList = new List<AssertX86ServerViewModel>();
            AssertX86ServerVerifyListViewModel returnModel = new AssertX86ServerVerifyListViewModel();
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
                    returnModel.ErrorItems.Add(model);
                }
                else
                {
                    tempList.Add(model);
                }
            }
            foreach (var item in tempList)
            {
                if ( tempList.FindAll(m => m.FixedAssertNumber == item.FixedAssertNumber).Count >= 2)
                {
                    returnModel.ErrorItems.Add(item);
                }
                else
                {
                    Assert_X86Server server = await _assertX86ServerService.GetAssertX86ServerByColumn(item.FixedAssertNumber, ENUMS.Type.FixedAssertNumber);
                    
                    if (server != null)
                    {
                        returnModel.ReadyToModify.Add(item);
                    }
                    else
                    {
                        returnModel.ReadyToAdd.Add(item);
                    }
                }
                
            }

            if (System.IO.File.Exists(filePath))
            {
                //删除临时文件
                System.IO.File.Delete(filePath); 
            }            
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
                var entity = await _assertX86ServerService.GetAssertX86ServerByColumn(model.FixedAssertNumber, ENUMS.Type.FixedAssertNumber);
                entity.FixedAssertNumber = model.FixedAssertNumber;
                    entity.Name = model.Name;
                    entity.SerialNumber = model.SerialNumber;
                    entity.HD = model.HD;
                    entity.OS = model.OS;
                    entity.EngineNumber = model.EngineNumber;
                    entity.RackLocation = model.RackLocation;
                    entity.BeginU = model.BeginU;
                    entity.EndU = model.EndU;
                    entity.VirtualizedResourcePool = model.VirtualizedResourcePool;
                    entity.BusinessSystem = model.BusinessSystem;
                    entity.IP = model.IP;
                    entity.NetcardNumber = model.NetcardNumber;
                    entity.HBANumber = model.HBANumber;
                    entity.StorageSize = model.StorageSize;
                    entity.MaintenanceInformation = model.MaintenanceInformation;
                    entity.InstallDate = model.InstallDate;
                    entity.Band = model.Band;
                    entity.CPU = model.CPU;
                    entity.Memory= model.Memory;
                
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

        [HttpGet]
        public async Task<IActionResult> X86Server_Modify(){
            //var guid = new Guid(Request.Form["id"]);
            var guid = new Guid(Request.QueryString.Value.Substring(1));
            Assert_X86Server x86Server = await _assertX86ServerService.GetAssertX86ServerById(guid);
            AssertX86ServerViewModel model = new AssertX86ServerViewModel();
                model.ID = x86Server.Id;
                model.FixedAssertNumber = x86Server.FixedAssertNumber;
                model.Name = x86Server.Name;
                model.SerialNumber = x86Server.SerialNumber;
                model.Band = x86Server.Band;
                model.CPU = x86Server.CPU;
                model.Memory = x86Server.Memory;
                model.HD = x86Server.HD;
                model.OS = x86Server.OS;
                model.EngineNumber = x86Server.EngineNumber;
                model.RackLocation = x86Server.RackLocation;
                model.BeginU = x86Server.BeginU;
                model.EndU = x86Server.EndU;
                model.VirtualizedResourcePool = x86Server.VirtualizedResourcePool;
                model.BusinessSystem = x86Server.BusinessSystem;
                model.IP = x86Server.IP;
                model.NetcardNumber = x86Server.NetcardNumber;
                model.HBANumber = x86Server.HBANumber;
                model.StorageSize = x86Server.StorageSize;
                model.MaintenanceInformation = x86Server.MaintenanceInformation;
                if (String.IsNullOrEmpty(x86Server.InstallDate.ToString()))
                {
                    model.InstallDate = null;
                }
                else
                {
                    model.InstallDate = x86Server.InstallDate;
                }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> X86Server_Modify(AssertX86ServerViewModel model)
        {
            Assert_X86Server entity = await _assertX86ServerService.GetAssertX86ServerById(model.ID);
            
                    entity.Name = model.Name;
                    entity.SerialNumber = model.SerialNumber;
                    entity.HD = model.HD;
                    entity.OS = model.OS;
                    entity.EngineNumber = model.EngineNumber;
                    entity.RackLocation = model.RackLocation;
                    entity.BeginU = model.BeginU;
                    entity.EndU = model.EndU;
                    entity.VirtualizedResourcePool = model.VirtualizedResourcePool;
                    entity.BusinessSystem = model.BusinessSystem;
                    entity.IP = model.IP;
                    entity.NetcardNumber = model.NetcardNumber;
                    entity.HBANumber = model.HBANumber;
                    entity.StorageSize = model.StorageSize;
                    entity.MaintenanceInformation = model.MaintenanceInformation;
                    entity.InstallDate = model.InstallDate;
                    entity.Band = model.Band;
                    entity.CPU = model.CPU;
                    entity.Memory = model.Memory;
                    await _assertX86ServerService.UpdateAssertX86Server(entity);

            return Redirect("./X86Server");
        }
    
        [HttpGet]
        public async Task<IActionResult> X86Server_Users(){
            
            AssertX86ServerUsersViewModel model = null;
            if(String.IsNullOrEmpty(Request.QueryString.Value))
            {
                model = new AssertX86ServerUsersViewModel();
            }
            else
            {
                var fixAssertNumber = Request.QueryString.Value.Substring(1);
                var item = await _assertX86ServerService.GetAssertX86ServerByColumn(fixAssertNumber,ENUMS.Type.FixedAssertNumber);
                model = new AssertX86ServerUsersViewModel();
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
                
                List<Assert_X86ServerUserInformation> users = await _assertX86ServerUserInformationService.GetUsersByServerGuid(item.Id);
                model.Users = JsonConvert.SerializeObject(users);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> X86Server_Users(AssertX86ServerUsersViewModel requestModel)
        {
            
            ICollection<Assert_X86ServerUserInformation> users = AssertX86ServerUsersViewModel.GetUsers(requestModel.Users);
            Assert_X86ServerUserInformation currentEntity = null;
            List<Assert_X86ServerUserInformation> readyToRemove = await _assertX86ServerUserInformationService.GetUsersByServerFixedAssertNumber(requestModel.FixedAssertNumber);
            foreach(var item in readyToRemove)
            {
                var result = await _assertX86ServerUserInformationService.RemoveUserByGuidAsync(item.Id);
            }
            //_assertX86ServerUserInformationService.RemoveUsersByServerFixedAssertNumber(requestModel.FixedAssertNumber);
            foreach(var item in users)
            {
                item.FixedAssertNumber = requestModel.FixedAssertNumber;
                currentEntity = await _assertX86ServerUserInformationService.AddUserAsync(item);                                
            }

            return Redirect("/Assert/X86Server");
        }
    }
}