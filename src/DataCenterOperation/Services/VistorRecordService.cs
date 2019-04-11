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
    public interface IVistorRecordService
    {
        Task<List<VistorRecord>> GetAllVistorRecords();
        Task<VistorRecord> AddVistorAsync(VistorEntryRequest request);
        Task<VistorRecord> GetVistorRecordById(Guid id);
        Task<VistorRecord> UpdateContactInfo(Guid id, string contactInfo);
        Task<VistorRecord> AddVistorAsync(string vistorName, int numberOfPeople, DateTime entryTime, string company, string matter, string contactInfo);
    }
    public class VistorRecordService : IVistorRecordService
    {
        private readonly DataCenterOperationDbContext _db;
        private readonly ILogger _logger;        

        public VistorRecordService(DataCenterOperationDbContext dbContext,ILoggerFactory loggerFactory)
        {
            _db = dbContext;
            _logger = loggerFactory.CreateLogger<VistorRecordService>();
        }

        public async Task<List<VistorRecord>> GetAllVistorRecords() {
            return await _db.VistorRecords.ToListAsync();
        }

        public async Task<VistorRecord> GetVistorRecordById(Guid id)
        {
            return await _db.VistorRecords.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<VistorRecord> UpdateContactInfo(Guid id, string contactInfo)
        {
            VistorRecord vistorRecord = await _db.VistorRecords.FirstOrDefaultAsync(a => a.Id == id);
            vistorRecord.ContactInfo = contactInfo;
            await _db.SaveChangesAsync();

            return vistorRecord;
        }

        public async Task<VistorRecord> AddVistorAsync(VistorEntryRequest request)
        {
            VistorRecord vistor = new VistorRecord
            {
                Id = Guid.NewGuid(),
                VistorName = request.RequestPeoopleName,

                EntryTime = request.RequestDate,
                Company = request.Company,
                Matter = request.Matter_Short,
                VistorEntryRequestGuid = request.Id,
            };

            if ( request.Entourage != null && request.Entourage.Count() > 0)
            {
                vistor.NumberOfPeople = request.Entourage.Count();
            }
            else
            {
                vistor.NumberOfPeople = 0;
            }

            _db.VistorRecords.Add(vistor);

            _db.SaveChanges();

            return await Task.FromResult(vistor);
        }

        public async Task<VistorRecord> AddVistorAsync(string vistorName, int numberOfPeople, DateTime entryTime, string company, string matter, string contactInfo )
        {
            var vistor = new VistorRecord
            {
                Id = Guid.NewGuid(),
                VistorName = vistorName,
                NumberOfPeople = numberOfPeople,
                EntryTime = entryTime,
                Company = company,
                Matter = matter,
                ContactInfo = contactInfo
            };                    

            _db.VistorRecords.Add(vistor);

            _db.SaveChanges();
            
            return await Task.FromResult(vistor);
        }

    }        
}
