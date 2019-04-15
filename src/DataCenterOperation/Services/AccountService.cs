using DataCenterOperation.Data;
using DataCenterOperation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DataCenterOperation.Services
{
    public interface IAccountService
    {
        Task<User> LoginAsync(string username, string password);
        Task ResetSystemAdmin();
        Task<bool> ValidatePrincipal(string username, string updatedTime);
        Task<string> GenerateNewPasswordAsync(Guid userId);
    }
    public class AccountService : IAccountService
    {
        private readonly DataCenterOperationDbContext _db;
        private readonly ILogger _logger;

        public AccountService(
            DataCenterOperationDbContext dbContext,
            ILoggerFactory loggerFactory)
        {
            _db = dbContext;
            _logger = loggerFactory.CreateLogger<AccountService>();
        }

        public async Task<string> GenerateNewPasswordAsync(Guid userId)
        {
            var user = await _db.Users.SingleOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new Exception($"Failed to find the user with the guid: {userId}.");
            }

            var ticket = DateTime.Now.Ticks.ToString();
            user.Password = ticket.Substring(ticket.Length - 6);
            await _db.SaveChangesAsync();

            return user.Password;
        }

        public async Task<User> LoginAsync(string username, string password)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u =>
                        u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)
                        && u.Password.Equals(password, StringComparison.Ordinal)
            );

            return user;
        }

        public async Task ResetSystemAdmin()
        {
            var saUsername = User.Default_Admin_Username;
            var saPassword = User.Default_Admin_Password;
            var sa = await _db.Users.FirstOrDefaultAsync(u => saUsername.Equals(u.Username, StringComparison.OrdinalIgnoreCase));

            if (sa == null)
            {
                sa = new User
                {
                    Username = saUsername,
                    Password = saPassword,
                    IsAdmin = true,
                    Disabled = false
                };
                sa.UpdatedTime = sa.CreatedTime = DateTime.Now;
                sa.UpdatedBy = sa.CreatedBy = "System";
                _db.Users.Add(sa);
            }
            else
            {
                sa.Password = saPassword;
                sa.IsAdmin = true;
                sa.Disabled = false;
                sa.UpdatedTime = DateTime.Now;
            }

            await _db.SaveChangesAsync();
        }

        public async Task<bool> ValidatePrincipal(string username, string updatedTime)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            if (user == null)
            {
                return false;
            }

            _logger.LogInformation($"Validating UpdatedTime in principal '{updatedTime}' with UpdatedTime in database '{user.UpdatedTime}' ... ");

            if (!user.UpdatedTime.HasValue)
            {
                return false;
            }

            return user.UpdatedTime.Value.Ticks.ToString().Equals(updatedTime, StringComparison.OrdinalIgnoreCase);
        }
    }
}
