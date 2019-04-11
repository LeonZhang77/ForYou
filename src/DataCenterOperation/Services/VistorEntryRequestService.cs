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
    public interface IVistorEntryRequestService
    {
        Task<List<VistorEntryRequest>> GetAllVistorEntryRequests();

        Task<VistorEntryRequest> GetVistorEntryRequestsById(Guid id);

        Task<VistorEntryRequest> AddVistorEntryRequest(VistorEntryRequest request);
    }

    public class VistorEntryRequestService : IVistorEntryRequestService
    {
        private readonly DataCenterOperationDbContext _db;
        private readonly ILogger _logger;
        private readonly IVistorRecordService _vistorRecordService;
        private readonly IVistorEntourageService _vistorEntourageService;

        public VistorEntryRequestService(DataCenterOperationDbContext dbContext, 
            IVistorRecordService vistorRecordService,
            IVistorEntourageService vistorEntourageService,
            ILoggerFactory loggerFactory)
        {
            _db = dbContext;
            _vistorRecordService = vistorRecordService;
            _vistorEntourageService = vistorEntourageService;
            _logger = loggerFactory.CreateLogger<VistorEntryRequestService>();
            
        }

        public async Task<List<VistorEntryRequest>> GetAllVistorEntryRequests(){
            return await _db.VistorEntryRequests.ToListAsync();
        }

        public async Task<VistorEntryRequest> GetVistorEntryRequestsById(Guid id)
        {
            return await _db.VistorEntryRequests.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<VistorEntryRequest> AddVistorEntryRequest(VistorEntryRequest request)
        {                    
            request.Id = Guid.NewGuid();


            if (request.Entourage != null && request.Entourage.Count() > 0)
            {
                foreach (VistorEntourage item in request.Entourage)
                {
                    item.VistorEntryRequestGuid = request.Id;
                    await _vistorEntourageService.AddVistorEntourageAsync(item);
                }
            }

            await _vistorRecordService.AddVistorAsync(request);

            _db.VistorEntryRequests.Add(request);

            _db.SaveChanges();

            return request;
        }

    }        
}
