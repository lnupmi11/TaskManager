using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManager.DAL.Models;

namespace TaskManager.DAL.EF
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(){}
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

            builder.Entity<TaskItem>()
                .HasMany(c => c.Changes)
                .WithOne(t => t.Task);
        }

        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        public virtual DbSet<TaskItem> Tasks { get; set; }
        public virtual DbSet<TaskChanges> TaskChanges { get; set; }
    }
}
