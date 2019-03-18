using DataCenterOperation.Data;
using DataCenterOperation.Services;
using DataCenterOperation.Site.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DataCenterOperation
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        private ILogger<Startup> _logger;

        public Startup(IHostingEnvironment env, ILogger<Startup> logger)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

            _logger = logger;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();

            // Add framework services.
            services.AddDbContext<DataCenterOperationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddRazorOptions(options =>
                {
                    options.ViewLocationFormats.Clear();
                    options.ViewLocationFormats.Add("/Site/Views/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Site/Views/Shared/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Site/Views/{1}/{0}.cshtml");
                });

            services.AddAuthentication("DataCenterOperation.CookieScheme")
                .AddCookie("DataCenterOperation.CookieScheme", o =>
                {
                    o.LoginPath = new PathString("/Account/Login/");
                    o.LogoutPath = new PathString("/Account/Logout");
                    o.AccessDeniedPath = new PathString("/Account/AccessDenied/");
                    o.Events = new CookieAuthenticationEvents
                    {
                        OnValidatePrincipal = ValidatePrincipal
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireClaim("IsAdmin", true.ToString()));
            });

            // Add application services.
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IUserService, UserService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.InitDbContext(_logger);
        }

        async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var accountService = context.HttpContext.RequestServices.GetRequiredService<IAccountService>();
            var username = context.Principal.GetUsername();
            var updatedTime = context.Principal.GetUpdatedTime();

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(updatedTime) ||
                !await accountService.ValidatePrincipal(username, updatedTime))
            {
                context.RejectPrincipal();
                await context.HttpContext.SignOutAsync("DataCenterOperation.CookieScheme");
            }
        }
    }
}
