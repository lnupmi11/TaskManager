﻿using System.IO;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using TaskManager.DAL.Models;

namespace TaskManager.DAL.EF
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<UserProfile>()
                .HasMany(t => t.Tasks)
                .WithOne(u => u.User);

            builder.Entity<TaskItem>()
                .HasMany(c => c.Changes)
                .WithOne(t => t.Task);
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<TaskChanges> TaskChanges { get; set; }
    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>     {         public ApplicationDbContext CreateDbContext(string[] args)         {             IConfigurationRoot configuration = new ConfigurationBuilder()                 .SetBasePath(Directory.GetCurrentDirectory())                 .AddJsonFile("appsettings.json")                 .Build();             var builder = new DbContextOptionsBuilder<ApplicationDbContext>();             var connectionString = configuration.GetConnectionString("DefaultConnection");             builder.UseSqlServer(connectionString);             return new ApplicationDbContext(builder.Options);         }     } 

}
