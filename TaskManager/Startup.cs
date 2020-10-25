using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.BLL.Interfaces;
using TaskManager.BLL.Services;
using TaskManager.DAL.EF;
using TaskManager.DAL.Interfaces;
using TaskManager.DAL.Models;
using TaskManager.DAL.Models.Enums;
using TaskManager.DAL.Repositories;
using TaskManager.Extensions.Email;

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
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
               x => x.MigrationsAssembly("TaskManager")));

            services.AddIdentity<UserProfile, IdentityRole>(
                config => config.SignIn.RequireConfirmedEmail = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAutoMapper(typeof(Startup));

            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddScoped(typeof(IRepository<TaskItem>), typeof(TaskRepository));
            services.AddScoped(typeof(IRepository<UserProfile>), typeof(UserRepository));
            services.AddScoped(typeof(IRepository<CategoryItem>), typeof(CategoryRepository));
            services.AddScoped(typeof(IRepository<TaskCategories>), typeof(TaskCategoryRepository));
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITaskService, TaskService>();
            services.AddTransient<ICategoryService, CategoryService>();

            CreateRoles(services).Wait();
            CreateCategories(services);
        }

        private async Task CreateRoles(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
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

        private void CreateCategories(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var categoriesManager = serviceProvider.GetRequiredService<IRepository<CategoryItem>>();
            var categoryValues = new List<string>
            {
                        "Sport Activity",
                        "Phone Call",
                        "Mettings",
                        "Family",
                        "Friends",
                        "Weekend",
                        "Work",
                        "Homework"
            };

            foreach (var value in categoryValues)
            {
                if (!categoriesManager.Any(p => p.Name == value))
                {
                    var category = new CategoryItem { Name = value };
                    categoriesManager.Create(category);
                }
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
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
