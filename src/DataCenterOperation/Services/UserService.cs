using DataCenterOperation.Data;
using DataCenterOperation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DataCenterOperation.Services
{
    public class UserService : IUserService
    {
        private readonly DataCenterOperationDbContext _db;
        private readonly ILogger _logger;

        public UserService(
            DataCenterOperationDbContext dbContext,
            ILoggerFactory loggerFactory)
        {
            _db = dbContext;
            _logger = loggerFactory.CreateLogger<UserService>();
        }

        public async Task<User> FindByUsernameAsync(string email)
        {
            return await _db.Users.SingleOrDefaultAsync(u => u.Username.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            return await _db.Users.SingleOrDefaultAsync(u => u.Id == userId);
        }
    }
}
