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
    public interface IMysqlDBService
    {
        Task<List<User>> GetAllUsers();
        Task<List<Assert_X86Server>> GetAllAssertX86Server();
        Task<List<Assert_X86ServerUserInformation>> GetAllAssert_X86ServerUserInformation();
        Task<List<Failure>> GetAllFailure();
        Task<List<VistorEntryRequest>> GetAllVistorEntryRequest();
        Task<List<VistorRecord>> GetAllVistorRecord();
        Task<List<VistorEntourage>> GetAllVistorEntourage();
        Task<bool> RemoveAll();
        Task<bool> AddAll(
            List<Failure> failures,
            List<Assert_X86Server> assert_X86Servers,
            List<Assert_X86ServerUserInformation> assert_X86ServerUserInformations
            );
    }
    public class MysqlDBService : IMysqlDBService
    {
        private readonly DataCenterOperationDbContext _db;
        private readonly ILogger _logger;

        public MysqlDBService(
            DataCenterOperationDbContext dbContext,
            ILoggerFactory loggerFactory)
        {
            _db = dbContext;
            _logger = loggerFactory.CreateLogger<AccountService>();
        }

        public async Task<List<User>> GetAllUsers()
        {            
            return await _db.Users.ToListAsync(); 
        }

        public async Task<List<Assert_X86Server>> GetAllAssertX86Server()
        {
            return await _db.Assert_X86Servers.ToListAsync();
        }

        public async Task<List<Assert_X86ServerUserInformation>> GetAllAssert_X86ServerUserInformation()
        {
            return await _db.Assert_X86ServerUserInformations.ToListAsync();
        }

        public async Task<List<Failure>> GetAllFailure()
        {
            return await _db.Failures.ToListAsync();
        }

        public async Task<List<VistorRecord>> GetAllVistorRecord()
        {
            return await _db.VistorRecords.ToListAsync();
        }

        public async Task<List<VistorEntryRequest>> GetAllVistorEntryRequest()
        {
            return await _db.VistorEntryRequests.ToListAsync();
        }
        public async Task<List<VistorEntourage>> GetAllVistorEntourage()
        {
            return await _db.VistorEntourages.ToListAsync();
        }

        public async Task<bool> RemoveAll()
        {


            List<VistorEntourage> vistorEntourages = await _db.VistorEntourages.ToListAsync();
            _db.VistorEntourages.RemoveRange(vistorEntourages);
            List<VistorEntryRequest> vistorEntrysRequests = await _db.VistorEntryRequests.ToListAsync();
            _db.VistorEntryRequests.RemoveRange(vistorEntrysRequests);
            List<VistorRecord> vistorRecords = await _db.VistorRecords.ToListAsync();
            _db.VistorRecords.RemoveRange(vistorRecords);
            List<Failure> failures = await _db.Failures.ToListAsync();
            _db.Failures.RemoveRange(failures);
            List<Assert_X86Server> assert_X86Servers = await _db.Assert_X86Servers.ToListAsync();
            _db.Assert_X86Servers.RemoveRange(assert_X86Servers);
            List<Assert_X86ServerUserInformation> assert_X86ServerUserInformations = await _db.Assert_X86ServerUserInformations.ToListAsync();
            _db.Assert_X86ServerUserInformations.RemoveRange(assert_X86ServerUserInformations);
            
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddAll(
            List<Failure> failures,
            List<Assert_X86Server> assert_X86Servers,
            List<Assert_X86ServerUserInformation> assert_X86ServerUserInformations
            )
        {

            _db.Failures.AddRange(failures);
            _db.Assert_X86Servers.AddRange(assert_X86Servers);
            _db.Assert_X86ServerUserInformations.AddRange(assert_X86ServerUserInformations);

            await _db.SaveChangesAsync();

            return true;
        }
    }
}
