using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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

            builder.Entity<TaskItem>()
                .HasMany(c => c.Changes)
                .WithOne(t => t.Task);

            builder.Entity<TaskItem>()
                .HasMany(c => c.Categories)
                .WithOne(t => t.Task);

            builder.Entity<TaskCategories>()
                .HasOne(t => t.Category);

            builder.Entity<TaskCategories>()
                .HasOne(t => t.Task);

            //builder.Entity<CategoryItem>()
            //    .HasOne(c => c.User);

            builder.Entity<CategoryItem>()
                .HasMany(c => c.TaskCategories)
                .WithOne(t => t.Category);
        }

        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        public virtual DbSet<TaskItem> Tasks { get; set; }
        public virtual DbSet<TaskChanges> TaskChanges { get; set; }
        public virtual DbSet<CategoryItem> Categories { get; set; }
        public virtual DbSet<TaskCategories> TaskCategories { get; set; }
    }
}
