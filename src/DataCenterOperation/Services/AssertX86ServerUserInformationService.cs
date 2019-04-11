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
    public interface IAssertX86ServerUserInformationService
    {
        Task<List<Assert_X86ServerUserInformation>> GetUsersByServerGuid(Guid guid);
        Task<List<Assert_X86ServerUserInformation>> GetUsersByServerFixedAssertNumber(string fixedAssertNumber);
        Task<Assert_X86ServerUserInformation> AddUserAsync(Assert_X86ServerUserInformation user);
        Task<Assert_X86ServerUserInformation> UpdateUserAsync(Assert_X86ServerUserInformation user);
        Task<bool> RemoveUserByGuidAsync(Guid id);
    }
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

        public async Task<List<Assert_X86ServerUserInformation>> GetUsersByServerFixedAssertNumber(string fixedAssertNumber)
        {

            List<Assert_X86ServerUserInformation> returnResult = await _db.Assert_X86ServerUserInformations.Where(f => f.FixedAssertNumber == fixedAssertNumber).ToListAsync();

            return (returnResult);
        }

        public async Task<Assert_X86ServerUserInformation> AddUserAsync(Assert_X86ServerUserInformation user) 
        {
            user.Id = Guid.NewGuid();
                
            _db.Assert_X86ServerUserInformations.Add(user);
            await _db.SaveChangesAsync();

            return user;
        }

        public async Task<Assert_X86ServerUserInformation> UpdateUserAsync(Assert_X86ServerUserInformation user) 
        {

            Assert_X86ServerUserInformation item = await _db.Assert_X86ServerUserInformations.FirstOrDefaultAsync( m => m.Id == user.Id );

            //item = user;
            item.UserName = user.UserName;
            item.UserDescription = user.UserDescription;
            item.PersonInCharge = user.PersonInCharge;

            await _db.SaveChangesAsync();

            return item;
        }

        public async Task<bool> RemoveUserByGuidAsync(Guid id) 
        {
            var item = await _db.Assert_X86ServerUserInformations.FirstOrDefaultAsync( m => m.Id == id );
                
            if (item == null)
            {
                return false;
            }

            try
            {
                _db.Assert_X86ServerUserInformations.Remove(item);
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
