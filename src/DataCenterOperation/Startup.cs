using DataCenterOperation.Data;
using DataCenterOperation.Services;
using DataCenterOperation.Site.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
            // Add framework services.
            services.AddDbContext<DataCenterOperationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc()
                .AddRazorOptions(options =>
                {
                    options.ViewLocationFormats.Clear();
                    options.ViewLocationFormats.Add("/Site/Views/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Site/Views/Shared/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Site/Views/{1}/{0}.cshtml");
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireClaim("IsAdmin", true.ToString()));
            });

            //services.Configure<SmtpOptions>(Configuration.GetSection("Smtp"));

            // Add application services.
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IUserService, UserService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

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

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "DataCenterOperation.CookieScheme",
                LoginPath = new PathString("/Account/Login/"),
                LogoutPath = new PathString("/Account/Logout"),
                AccessDeniedPath = new PathString("/Account/AccessDenied/"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                Events = new CookieAuthenticationEvents
                {
                    OnValidatePrincipal = ValidatePrincipal
                }
            });

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
                await context.HttpContext.Authentication.SignOutAsync("DataCenterOperation.CookieScheme");
            }
        }
    }
}
