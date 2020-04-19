using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ReverseMyBudget.Application;
using ReverseMyBudget.Data;
using ReverseMyBudget.MLCategorisation;
using ReverseMyBudget.Models;
using ReverseMyBudget.Persistence;
using ReverseMyBudget.Persistence.Sql;
using Serilog;

namespace ReverseMyBudget
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private ILoggerFactory _sqlLoggerFactory;
        private bool _useTestUser = false;

        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            Configuration = configuration;

            if (env.IsDevelopment())
            {
                _sqlLoggerFactory = LoggerFactory.Create(builder =>
                {
                    builder.AddFilter((category, level) =>
                                category == DbLoggerCategory.Database.Command.Name
                                && level == LogLevel.Information)
                            .AddConsole()   // when running from command prompt
                            .AddDebug();    // when debugging from VS
                });

                _useTestUser = Configuration.GetValue<bool>("UseTestUser");
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Log.Logger);

            services.AddDbContext<ApplicationUserDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            // https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/providers?tabs=dotnet-core-cli
            services.AddDbContext<ReverseMyBudgetDbContext>(options =>
            {
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("ReverseMyBudget"));
                options.UseLoggerFactory(_sqlLoggerFactory);
            });

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationUserDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationUserDbContext>();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<LoggingExceptionFilter>();
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;   // Can receive camel case or Pascal case
                options.JsonSerializerOptions.PropertyNamingPolicy = null;  // Don't change from Pascal case
            });

            services.AddRazorPages();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            if (!_useTestUser)
            {
                services.AddOidcUserProvider();
            }
            else
            {
                services.AddTestUserProvider();
            }

            // Application Services
            services.AddMediatR(typeof(ImportTransactionsRequest).Assembly);

            services.TryAddSingleton<ITransactionConverter, NabCsvToTransactionConverter>();
            services.TryAddSingleton<ITransactionCategoriser, MLTransactionCategoriser>();

            // Scoped
            services.TryAddScoped<ITransactionStore, SqlTransactionStore>();
            services.TryAddScoped<IAccountStore, SqlAccountStore>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<RequestLogger>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

                app.UseHttpsRedirection();
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    //spa.UseReactDevelopmentServer(npmScript: "start");

                    // Speed up debugging by running SPA independantly
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
                }
            });
        }
    }
}