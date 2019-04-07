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
    public class AssertX86ServerUserInformationService : IAssertX86ServerUserInformationService
    {
        private readonly DataCenterOperationDbContext _db;
        private readonly ILogger _logger;        

        public AssertX86ServerUserInformationService(DataCenterOperationDbContext dbContext,ILoggerFactory loggerFactory)
        {
            _db = dbContext;
            _logger = loggerFactory.CreateLogger<AssertX86ServerUserInformationService>();
        }

     
        public async Task<List<Assert_X86ServerUserInformation>> GetUsersByServerGuid(Guid guid)
        {

            Assert_X86Server relateServer = await _db.Assert_X86Servers.FirstOrDefaultAsync(m => m.Id == guid);
            List<Assert_X86ServerUserInformation> returnResult = await _db.Assert_X86ServerUserInformations.Where(f => f.FixedAssertNumber == relateServer.FixedAssertNumber).ToListAsync();

            return (returnResult);
        }

        public async Task<Assert_X86ServerUserInformation> AddUserAsync(Assert_X86ServerUserInformation user) 
        {

            user.Id = Guid.NewGuid();
            
            _db.Assert_X86ServerUserInformations.Add(user);
            _db.SaveChanges();

            return await Task.FromResult(user);
        }

        public async Task<Assert_X86ServerUserInformation> UpdateUserAsync(Assert_X86ServerUserInformation user) 
        {

            Assert_X86ServerUserInformation item = await _db.Assert_X86ServerUserInformations.FirstOrDefaultAsync( m => m.Id == user.Id );

            item = user;

            _db.SaveChanges();

            return await Task.FromResult(item);
        }  

    }        
}
