using DataCenterOperation.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataCenterOperation.Data
{
    public partial class DataCenterOperationDbContext : DbContext
    {
        public DataCenterOperationDbContext(DbContextOptions<DataCenterOperationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<User> Users { get; set; }
    }
}
