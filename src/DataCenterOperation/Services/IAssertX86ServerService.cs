using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataCenterOperation.Data.Entities;

namespace DataCenterOperation.Services
{
    public interface IAssertX86ServerService
    {
        Task<List<Assert_X86Server>> GetAllAssertX86Server();

        Task<Assert_X86Server> GetAssertX86ServerById(Guid id);

        Task<Assert_X86Server> AddAssertX86Server(Assert_X86Server request);

        Task<Assert_X86Server> UpdateAssertX86Server(Assert_X86Server entity);

        Task<Assert_X86Server> GetAssertX86ServerByColumn(String value, Util.ENUMS.Type type);

        Task<bool> RemoveAssertX86ServerByGuid(Guid id);
    }
}
