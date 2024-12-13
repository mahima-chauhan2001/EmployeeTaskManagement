using EmployeeTaskManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTaskManagement.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options) 
        { 

        }

        public DbSet<TasksModel> Tasks { get; set; }

        public DbSet<Roles> Roles { get; set; }

        public DbSet<UserModel> UserModels { get; set; } 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Explicitly define the primary key for the Task entity
            modelBuilder.Entity<TasksModel>().HasKey(t => t.Id);  // Make sure 'Id' is the primary key

            modelBuilder.Entity<TasksModel>()
               .HasOne(t => t.AssignedFrom)
               .WithMany()  // assuming no navigation property back to tasks in IdentityUser
               .HasForeignKey(t => t.AssignedFromId)
               .OnDelete(DeleteBehavior.SetNull);  // Or you can use `DeleteBehavior.Cascade` depending on your needs

            modelBuilder.Entity<TasksModel>()
                .HasOne(t => t.AssignedTo)
                .WithMany()  // assuming no navigation property back to tasks in IdentityUser
                .HasForeignKey(t => t.AssignedToId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Roles>().HasKey(t => t.RoleId);

            modelBuilder.Entity<UserModel>().HasKey(t => t.UserId); 
            
        }
         
    }
}
    