using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "a1ef085b-3f08-4d8a-b032-341fdfb38a86";
            var writerRoleId = "0920cbea-c7c6-48ee-b454-5cd264408648";

            //Create reader and writer role
            var roles = new List<IdentityRole> 
            {
                new IdentityRole()
                {
                    Id = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = readerRoleId
                },

                new IdentityRole()
                {
                    Id = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp = writerRoleId
                }
            };

            //seed the roles
            builder.Entity<IdentityRole>().HasData(roles);

            //Create an Admin User
            var adminUserId = "5f356dcc-5f62-4f08-af17-d29cbf2e14fb";
            var admin = new IdentityUser()
            {
                Id = adminUserId,
                UserName = "admin@blognet.com",
                Email = "admin@blognet.com",
                NormalizedEmail = "admin@blognet.com".ToUpper(),
                NormalizedUserName = "admin@blognet.com".ToUpper(),
            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");

            builder.Entity<IdentityUser>().HasData(admin);

            //Give Roles To Admin
            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                { 
                    UserId = adminUserId,
                    RoleId = readerRoleId
                },
                new()
                { 
                    UserId = adminUserId,
                    RoleId = writerRoleId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
        }

    }
}
