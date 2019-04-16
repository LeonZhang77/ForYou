using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using DataCenterOperation.Services;
using DataCenterOperation.Data;
using DataCenterOperation.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace DataCenterOperation.Site.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class MysqlDBController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMysqlDBService _mysqlDBService;
        private readonly IVistorEntryRequestService _vistorEntryRequestService;

        private readonly DataCenterOperationDbContext _db;
        private ILogger<AssertController> _logger;

        public MysqlDBController(
            IHostingEnvironment hostingEnvironment,
            IMysqlDBService mysqlDBService,
            IVistorEntryRequestService vistorEntryRequestService,
            DataCenterOperationDbContext dbContext,
            ILogger<AssertController> logger
            )
        {
            _mysqlDBService = mysqlDBService;
            _vistorEntryRequestService = vistorEntryRequestService;
            _hostingEnvironment = hostingEnvironment;
            _db = dbContext;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<string> Backup()
        {
            string retunStr = "";
            string webRootPath = _hostingEnvironment.WebRootPath;
            var filename = Guid.NewGuid();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            { retunStr = webRootPath + "\\upload\\MysqlDB\\" + filename.ToString() + ".json"; }
            else
            { retunStr = webRootPath + "/upload/MysqlDB/" + filename.ToString() + ".json"; }

            Tables tables = new Tables();
            tables.Users = await _mysqlDBService.GetAllUsers();
            tables.vistorEntourages = await _mysqlDBService.GetAllVistorEntourage();
            tables.VistorEntryRequests = await _mysqlDBService.GetAllVistorEntryRequest();
            tables.vistorRecord = await _mysqlDBService.GetAllVistorRecord();
            tables.assert_X86Servers = await _mysqlDBService.GetAllAssertX86Server();
            tables.assert_X86ServerUserInformations = await _mysqlDBService.GetAllAssert_X86ServerUserInformation();
            tables.Falures = await _mysqlDBService.GetAllFailure();
            string readyToWrite = JsonConvert.SerializeObject(tables);

            System.IO.File.WriteAllText(retunStr, readyToWrite, System.Text.Encoding.UTF8);
            
            return filename.ToString();
        }

        public bool DeleteFile()
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            var file = Request.Form["filename"];
            var filePath = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            { filePath = webRootPath + "\\upload\\MysqlDB\\" + file; }
            else
            { filePath = webRootPath + "/upload/MysqlDB/" + file; }
            if (System.IO.File.Exists(filePath))
            {
                //删除文件
                System.IO.File.Delete(filePath);
            }
            return true;
        }

        public Task<bool> RemoveAll()
        {
            var returnBool = _mysqlDBService.RemoveAll();
            return returnBool;
        }

        public async Task<IActionResult> Download(string filename)
        {

            if (filename == null)
                return Content("filename not present");

            var path = "";
            string webRootPath = _hostingEnvironment.WebRootPath;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            { path = webRootPath + "\\upload\\MysqlDB\\" + filename; }
            else
            { path = webRootPath + "/upload/MysqlDB/" + filename; }

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, Util.File_Utils.GetContentType(path), Path.GetFileName(path));
        }

        public class Tables
        {
            public List<User> Users;
            public List<Assert_X86Server> assert_X86Servers;
            public List<Assert_X86ServerUserInformation> assert_X86ServerUserInformations;
            public List<VistorEntourage> vistorEntourages;
            public List<VistorRecord> vistorRecord;
            public List<VistorEntryRequest> VistorEntryRequests;
            public List<Failure> Falures;
        }

        public async Task<string> Backup_Upload(IFormCollection upload_files)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            var filePath = "";

            foreach (var item in upload_files.Files)
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                { filePath = webRootPath + "\\upload\\" + item.FileName; }
                else
                { filePath = webRootPath + "/upload/" + item.FileName; }


                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await item.CopyToAsync(stream);
                }
            }
            return filePath;
        }

        public async Task<bool> Restore(string filepath)
        {

            var json_str = System.IO.File.ReadAllText(filepath);
            var model = JsonConvert.DeserializeObject<Tables>(json_str);
            if (System.IO.File.Exists(filepath))
            {
                //删除临时文件
                System.IO.File.Delete(filepath);
            }
            foreach (var item in model.VistorEntryRequests)
            {
                await _vistorEntryRequestService.AddVistorEntryRequest(item);
            }
            bool retrunValue = await _mysqlDBService.AddAll(model.Falures, model.assert_X86Servers, model.assert_X86ServerUserInformations);

            return retrunValue;
        }

    }
}