using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TaskManager.BLL.Interfaces;
using TaskManager.BLL.Services;
using TaskManager.DAL.EF;
using TaskManager.DAL.Models;
using TaskManager.DAL.Models.Enums;

namespace TaskManager
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
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<UserProfile, IdentityRole>(
                config => config.SignIn.RequireConfirmedEmail = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITaskService, TaskService>();

            CreateRoles(services.BuildServiceProvider()).Wait();
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<UserProfile>>();
            var adminRole = Roles.Admin.ToString();
            var userRole = Roles.User.ToString();
            var userSettings = Configuration.GetSection("UserSettings");

            if (!(await roleManager.RoleExistsAsync(adminRole)))
            {
                var role = new IdentityRole
                {
                    Name = adminRole
                };
                await roleManager.CreateAsync(role);

                var user = new UserProfile
                {
                    UserName = userSettings["UserEmail"],
                    Email = userSettings["UserEmail"],
                    EmailConfirmed = true,
                };

                string userPassword = userSettings["UserPassword"];

                var result = await userManager.CreateAsync(user, userPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, adminRole);
                }
            }

            if (!(await roleManager.RoleExistsAsync(userRole)))
            {
                var role = new IdentityRole
                {
                    Name = userRole
                };
                await roleManager.CreateAsync(role);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
