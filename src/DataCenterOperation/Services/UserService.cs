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
    public interface IUserService
    {
        Task<User> FindByUsernameAsync(string email);
        Task<User> GetUserAsync(Guid guid);
        Task<List<User>> GetAllUsers();
        Task AddOrUpdateUser(User entity);

        Task RemoveUser(Guid guid);
    }
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

        public async Task<List<User>> GetAllUsers()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task AddOrUpdateUser(User entity)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => entity.Username.Equals(u.Username, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                user = new User
                {
                    Username = entity.Username,
                    Password = entity.Password,
                    IsAdmin = entity.IsAdmin,
                    Disabled = false
                };
                user.UpdatedTime = user.CreatedTime = DateTime.Now;
                user.UpdatedBy = user.CreatedBy = "System";
                _db.Users.Add(user);
            }
            else
            {
                user.Username = entity.Username;
                user.Password = entity.Password;
                user.IsAdmin = entity.IsAdmin;
                user.Disabled = false;
                user.UpdatedTime = DateTime.Now;
            }

            await _db.SaveChangesAsync();
        }

        public async Task RemoveUser(Guid guid)
        {
            var entity = await _db.Users.SingleOrDefaultAsync(u => u.Id == guid);

            _db.Users.Remove(entity);

            await _db.SaveChangesAsync();             
        }

    }
}
