using DataCenterOperation.Data.Entities;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace DataCenterOperation.Services
{
    public interface IUserService
    {
        Task<User> FindByUsernameAsync(string email);
        Task<User> GetUserAsync(Guid guid);
    }
}
