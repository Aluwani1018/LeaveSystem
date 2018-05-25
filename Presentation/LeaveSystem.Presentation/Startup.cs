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
