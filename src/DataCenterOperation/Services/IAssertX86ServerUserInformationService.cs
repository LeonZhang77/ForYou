using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataCenterOperation.Data.Entities;

namespace DataCenterOperation.Services
{
    public interface IAssertX86ServerUserInformationService
    {
        Task<List<Assert_X86ServerUserInformation>> GetUsersByServerGuid(Guid guid);
        Task<List<Assert_X86ServerUserInformation>> GetUsersByServerFixedAssertNumber(string fixedAssertNumber);
        Task<Assert_X86ServerUserInformation> AddUserAsync(Assert_X86ServerUserInformation user);
        Task<Assert_X86ServerUserInformation> UpdateUserAsync(Assert_X86ServerUserInformation user);
        Task<bool> RemoveUserByGuidAsync(Guid id);
    }
}
