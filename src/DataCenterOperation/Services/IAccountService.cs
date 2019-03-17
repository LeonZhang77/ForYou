using DataCenterOperation.Data.Entities;
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
}
