using DataCenterOperation.Data;
using DataCenterOperation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataCenterOperation.Services
{
    public interface IVistorEntourageService
    {
        Task<List<VistorEntourage>> GetAllVistorEntourage();
        Task<List<VistorEntourage>> GetVistorEntourageByRequestGUID(Guid guid);
        Task<VistorEntourage> AddVistorEntourageAsync(VistorEntourage vistorEntourage);
    }
    public class VistorEntourageService : IVistorEntourageService
    {        
        private readonly DataCenterOperationDbContext _db;
        private readonly ILogger _logger;        

        public VistorEntourageService(DataCenterOperationDbContext dbContext,ILoggerFactory loggerFactory)
        {
            _db = dbContext;
            _logger = loggerFactory.CreateLogger<VistorEntourageService>();
        }

        public async Task<List<VistorEntourage>> GetAllVistorEntourage() {
            List<VistorEntourage>  returnResult = await _db.VistorEntourages.ToListAsync();
            return (returnResult);
        }
        
        public async Task<List<VistorEntourage>> GetVistorEntourageByRequestGUID(Guid guid) {
        //Search according to VistorEntryGUID

            List<VistorEntourage> returnResult = await _db.VistorEntourages.Where(f => f.VistorEntryRequestGuid == guid).ToListAsync();
                        
            return (returnResult);
        }

        public async Task<VistorEntourage> AddVistorEntourageAsync(VistorEntourage vistorEntourage) {

            vistorEntourage.Id = Guid.NewGuid();

            _db.VistorEntourages.Add(vistorEntourage);
            _db.SaveChanges();

            return await Task.FromResult(vistorEntourage);
        }    

    }        
}
