using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataCenterOperation.Data;
using DataCenterOperation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataCenterOperation.Services
{
    public interface IVistorRecordService
    {
        Task<List<VistorRecord>> GetVistorRecordsAsync(string keyword, int pageIndex, int pageSize);
        Task<int> CountAsync(string keyword);
        Task<VistorRecord> AddVistorAsync(VistorEntryRequest request);
        Task<VistorRecord> GetVistorRecordById(Guid id);
        Task<VistorRecord> UpdateContactInfo(Guid id, string contactInfo);
        Task<VistorRecord> AddVistorAsync(string vistorName, int numberOfPeople, DateTime entryTime, string company, string matter, string contactInfo);
    }
    public class VistorRecordService : IVistorRecordService
    {
        private readonly DataCenterOperationDbContext _db;
        private readonly ILogger _logger;

        public VistorRecordService(DataCenterOperationDbContext dbContext, ILoggerFactory loggerFactory)
        {
            _db = dbContext;
            _logger = loggerFactory.CreateLogger<VistorRecordService>();
        }

        public async Task<List<VistorRecord>> GetVistorRecordsAsync(string keyword, int pageIndex, int pageSize)
        {
            var query = PrepareQuery(keyword);

            return await query.OrderByDescending(f => f.EntryTime)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> CountAsync(string keyword)
        {
            return await PrepareQuery(keyword).CountAsync();
        }

        private IQueryable<VistorRecord> PrepareQuery(string keyword)
        {
            return string.IsNullOrWhiteSpace(keyword)
                ? from f in _db.VistorRecords select f
                : from f in _db.VistorRecords
                  where f.Company.Contains(keyword)
                  || f.ContactInfo.Contains(keyword)
                  || f.VistorName.Contains(keyword)
                  || f.Matter.Contains(keyword)
                  select f;
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

            if (request.Entourage != null && request.Entourage.Count() > 0)
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

        public async Task<VistorRecord> AddVistorAsync(string vistorName, int numberOfPeople, DateTime entryTime, string company, string matter, string contactInfo)
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
