using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tasky.Models;

namespace Tasky
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
   public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<Category>  Categories { get; set; }    

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TaskItem>()
                .HasOne(t=>t.AppUser)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.AppUserId)
                .OnDelete(DeleteBehavior.Cascade); 


            builder.Entity<TaskItem>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Tasks)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.NoAction); 
        }
    }
}
