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
        public DbSet<Failure> Failures { get; set; }
        public DbSet<VistorRecord> VistorRecords { get; set; }
        public DbSet<VistorEntryRequest> VistorEntryRequests { get; set; }
        public DbSet<VistorEntourage> VistorEntourages { get; set; }
    }
}
