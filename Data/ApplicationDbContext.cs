using Microsoft.EntityFrameworkCore;
using Web_Learning.Model;

namespace Web_Learning.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Define DbSets
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<User_Role> UserRole { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<Cart> Cart { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ensure unique email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Configure many-to-many relationship
            modelBuilder.Entity<User_Role>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<User_Role>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRole)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<User_Role>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRole)
                .HasForeignKey(ur => ur.RoleId);

            // Seed roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin", Description = "Administrator with full privileges" },
                new Role { Id = 2, Name = "User", Description = "Regular user with limited privileges" },
                new Role { Id = 3, Name = "Tutor", Description = "Tutor with Manage courses privilages" }
            );

            // Seed default Admin user with plain text password (insecure)
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Email = "admin@gmail.com",
                    Password = "admin" // Plain text password
                });

            // Seed Admin role assignment
            modelBuilder.Entity<User_Role>().HasData(
                new User_Role
                {
                    UserId = 1,
                    RoleId = 1 // Admin role
                });
        }
    }
}
