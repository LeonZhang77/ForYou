using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataCenterOperation.Data.Entities;

namespace DataCenterOperation.Services
{
    public interface IVistorEntourageService
    {
        Task<List<VistorEntourage>> GetAllVistorEntourage();
        Task<List<VistorEntourage>> GetVistorEntourageByRequestGUID(Guid guid);
        Task<VistorEntourage> AddVistorEntourageAsync(VistorEntourage vistorEntourage);
    }
}
