using DataCenterOperation.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DataCenterOperation.Site.Extensions
{
    public static class AppExtensions
    {
        public static void InitDbContext(this IApplicationBuilder app, ILogger<Startup> logger)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataCenterOperationDbContext>();
                if (context == null) return;

                logger.LogInformation("Checking if the database already exist...");
                context.Database.EnsureCreated();

                logger.LogInformation("Trying to apply migrations...");
                context.Database.Migrate();

                logger.LogInformation("Init  metadata if they don't exist...");
                context.InitMetadata();
            }
        }
    }
}
