using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TaskManager.DAL.EF;
using TaskManager.DAL.Models;

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

            // Add application services.
            services.AddMvc();

           CreateRolesAndUsersAsync(services.BuildServiceProvider()).Wait();
        }

        private async Task CreateRolesAndUsersAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<UserProfile>>();

            if (!(await roleManager.RoleExistsAsync("Admin")))
            {
                var role = new IdentityRole
                {
                    Name = "Admin"
                };
                await roleManager.CreateAsync(role);

                var user = new UserProfile
                {
                    UserName = Configuration.GetSection("UserSettings")["UserEmail"],
                    Email = Configuration.GetSection("UserSettings")["UserEmail"],
                    EmailConfirmed = true,
                };

                string userPassword = Configuration.GetSection("UserSettings")["UserPassword"];

                var result = await userManager.CreateAsync(user, userPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

            if (!(await roleManager.RoleExistsAsync("User")))
            {
                var role = new IdentityRole
                {
                    Name = "User"
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
