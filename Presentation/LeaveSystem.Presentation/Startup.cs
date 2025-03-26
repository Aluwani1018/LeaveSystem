using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LeaveSystem.Presentation.Services;
using LeaveSystem.Data.Model;
using LeaveSystem.Data;
using LeaveSystem.Business;
using LeaveSystem.Data.Uow;
using AutoMapper;
using NonFactors.Mvc.Grid;
using LeaveSystem.Business.Interfaces;
using AspNet.Security.OpenIdConnect.Primitives;
using LeaveSystem.Infrastructure;
using AppPermissions = LeaveSystem.Infrastructure.ApplicationPermissions;
using LeaveSystem.Presentation.Authorization;

namespace LeaveSystem.Presentation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// Configures the services for the application, including setting up
        /// the database context, identity, dependency injection for various
        /// managers and services, and initializing AutoMapper configuration.
        /// </summary>
        /// <param name="services">The IServiceCollection to add the services to.</param>
        /// <example>
        /// public void ConfigureServices(IServiceCollection services)
        /// {
        ///     // This will setup the database with SQL Server and configure
        ///     // the application to use identity with custom roles and employees.
        ///     ConfigureServices(services);
        /// }
        /// </example>
        public void ConfigureServices(IServiceCollection services)
        {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("LeaveSystem.Presentation")));

            services.AddIdentity<Employee, Role>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<IEmployeeManager, EmployeeManager>();
            services.AddScoped<ILeaveManager, LeaveManager>();
            services.AddScoped<IPublicHolidaysManager, PublicHolidaysManager>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IDatabaseInitializer, DatabaseInitializer>();
            Mapper.Initialize(cfg => cfg.AddProfile<AutoMapperProfile>());

            services.AddMvc();
            services.AddMvcGrid();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// Configures the application's request pipeline based on the hosting environment.
        /// </summary>
        /// <param name="app">An instance of <see cref="IApplicationBuilder"/> used to configure the application's request pipeline.</param>
        /// <param name="env">An instance of <see cref="IHostingEnvironment"/> that provides information about the web hosting environment.</param>
        /// <example>
        /// // Example usage in the Startup class:
        /// public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        /// {
        ///     Configure(app, env);
        /// }
        /// </example>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Leave/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Leave}/{action=Index}/{id?}");
            });
        }
    }
}
