using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using TaskManager.DAL.Models;

namespace TaskManager.DAL.EF
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserProfile>()
                .HasMany(t => t.Tasks)
                .WithOne(u => u.User);

            builder.Entity<UserProfile>()
                .HasMany(t => t.Categories)
                .WithOne(u => u.User);

            builder.Entity<UserProfile>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<TaskItem>()
                .HasMany(c => c.Changes)
                .WithOne(t => t.Task);

            builder.Entity<TaskItem>()
                .HasMany(c => c.Categories)
                .WithOne(t => t.Task);

            builder.Entity<TaskItem>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<TaskCategories>()
                .HasOne(t => t.Category);

            builder.Entity<TaskCategories>()
                .HasOne(t => t.Task);

            builder.Entity<TaskCategories>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            //builder.Entity<CategoryItem>()
            //    .HasOne(c => c.User);

            builder.Entity<CategoryItem>()
                .HasMany(c => c.TaskCategories)
                .WithOne(t => t.Category);

            builder.Entity<CategoryItem>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
        }

        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        public virtual DbSet<TaskItem> Tasks { get; set; }
        public virtual DbSet<TaskChanges> TaskChanges { get; set; }
        public virtual DbSet<CategoryItem> Categories { get; set; }
        public virtual DbSet<TaskCategories> TaskCategories { get; set; }
    }

    public class DbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var dbContextBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            dbContextBuilder.UseSqlServer(connectionString);

            return new ApplicationDbContext(dbContextBuilder.Options);
        }
    }
}
