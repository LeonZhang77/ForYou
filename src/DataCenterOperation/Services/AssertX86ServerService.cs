using DataCenterOperation.Data;
using DataCenterOperation.Data.Entities;
using DataCenterOperation.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace DataCenterOperation.Services
{
    public class AssertX86ServerService : IAssertX86ServerService
    {
        private readonly DataCenterOperationDbContext _db;
        private readonly ILogger _logger;

        public AssertX86ServerService(DataCenterOperationDbContext dbContext, ILoggerFactory loggerFactory)
        {
            _db = dbContext;
            _logger = loggerFactory.CreateLogger<AssertX86ServerService>();
        }

        public async Task<List<Assert_X86Server>> GetAllAssertX86Server()
        {
            return await _db.Assert_X86Servers.ToListAsync();
        }

        public async Task<Assert_X86Server> AddAssertX86Server(Assert_X86Server request)
        {
            request.Id = Guid.NewGuid();

            _db.Assert_X86Servers.Add(request);

            await _db.SaveChangesAsync();

            return request;
        }

        public async Task<Assert_X86Server> UpdateAssertX86Server(Assert_X86Server entity)
        {
            var currentEntity = _db.Assert_X86Servers.FirstOrDefault(f => f.Id == entity.Id);
            
            // Id and FixAssertNumber can't be modified.
            currentEntity.Name = entity.Name;
            currentEntity.SerialNumber = entity.SerialNumber;
            currentEntity.HD = entity.HD;
            currentEntity.OS = entity.OS;
            currentEntity.EngineNumber = entity.EngineNumber;
            currentEntity.RackLocation = entity.RackLocation;
            currentEntity.BeginU = entity.BeginU;
            currentEntity.EndU = entity.EndU;
            currentEntity.VirtualizedResourcePool = entity.VirtualizedResourcePool;
            currentEntity.BusinessSystem = entity.BusinessSystem;
            currentEntity.IP = entity.IP;
            currentEntity.NetcardNumber = entity.NetcardNumber;
            currentEntity.HBANumber = entity.HBANumber;
            currentEntity.StorageSize = entity.StorageSize;
            currentEntity.MaintenanceInformation = entity.MaintenanceInformation;
            currentEntity.InstallDate = entity.InstallDate;
            currentEntity.Band = entity.Band;
            currentEntity.CPU = entity.CPU;
            currentEntity.Memory = entity.Memory;

            await _db.SaveChangesAsync();
            return currentEntity;
        }

        public async Task<Assert_X86Server> GetAssertX86ServerById(Guid id)
        {
            return await _db.Assert_X86Servers.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Assert_X86Server> GetAssertX86ServerByColumn(String value, Util.ENUMS.Type type)
        {
            Assert_X86Server returnServer = null;
            switch(type)
            {

                case ENUMS.Type.FixedAssertNumber:
                    returnServer = await _db.Assert_X86Servers.FirstOrDefaultAsync(f => f.FixedAssertNumber == value);
                    break;

                default:
                    returnServer = null;
                    break;
            }
            return returnServer;
        }

        public async Task<bool> RemoveAssertX86ServerByGuid(Guid id)
        { 
            var x86Server = await _db.Assert_X86Servers.FirstOrDefaultAsync(f => f.Id == id);
            if (x86Server == null)
            {
                return false;
            }

            try
            {
                _db.Assert_X86Servers.Remove(x86Server);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return false;
            }
        }

    }
}