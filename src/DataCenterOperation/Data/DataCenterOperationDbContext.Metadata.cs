using DataCenterOperation.Data.Entities;
using System;
using System.Linq;

namespace DataCenterOperation.Data
{
    public partial class DataCenterOperationDbContext
    {
        internal void InitMetadata()
        {
            var now = DateTime.Now;

            if (!Users.Any())
            {
                Users.AddRange(new User[]
                {
                    new User {
                        Id = Guid.NewGuid(),
                        IsAdmin = true,
                        Username = User.Default_Admin_Username,
                        Password = User.Default_Admin_Password,
                        Email = string.Empty,
                        Disabled = false,
                        CreatedBy = "System",
                        UpdatedBy = "System",
                        CreatedTime = now,
                        UpdatedTime = now }
                 });
            }

            SaveChanges();
        }
    }
}
