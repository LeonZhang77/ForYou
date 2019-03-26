using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataCenterOperation.Data.Entities;

namespace DataCenterOperation.Services
{
    public interface IVistorEntryRequestService
    {
        Task<List<VistorEntryRequest>> GetAllVistorEntryRequests();

        Task<VistorEntryRequest> GetVistorEntryRequestsById(Guid id);

        Task<VistorEntryRequest> AddVistorEntryRequest(VistorEntryRequest request);
    }
}
