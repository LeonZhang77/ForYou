using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataCenterOperation.Data.Entities;

namespace DataCenterOperation.Services
{
    public interface IVistorRecordService
    {
        Task<List<VistorRecord>> GetAllVistorRecords();
        Task<VistorRecord> AddVistorAsync(VistorEntryRequest request);
        Task<VistorRecord> AddVistorAsync(string vistorName, int numberOfPeople, DateTime entryTime, string company, string matter, string contactInfo);
    }
}
