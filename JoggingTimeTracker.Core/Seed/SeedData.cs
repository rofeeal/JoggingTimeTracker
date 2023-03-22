using JoggingTimeTracker.Core.Models;
using JoggingTimeTracker.Core.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace JoggingTimeTracker.Core.Seed
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync(UserRoles.Admin).Result)
            {
                var role = new IdentityRole { Name = UserRoles.Admin };
                var result = roleManager.CreateAsync(role).Result;
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to create admin role");
                }
            }

            if (!roleManager.RoleExistsAsync(UserRoles.UserManager).Result)
            {
                var role = new IdentityRole { Name = UserRoles.UserManager };
                var result = roleManager.CreateAsync(role).Result;
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to create user manager role");
                }
            }

            if (!roleManager.RoleExistsAsync(UserRoles.RegularUser).Result)
            {
                var role = new IdentityRole { Name = UserRoles.RegularUser };
                var result = roleManager.CreateAsync(role).Result;
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to create regular user role");
                }
            }
        }

        private static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            if (userManager.FindByNameAsync("admin").Result == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = "admin",
                    FirstName = "Rofaeil",
                    LastName = "Samir",
                    Email = "admin@example.com",
                    EmailConfirmed = true
                };
                var result = userManager.CreateAsync(admin, "P@ssw0rd").Result;
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to create admin user");
                }

                result = userManager.AddToRoleAsync(admin, UserRoles.Admin).Result;
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to add admin user to admin role");
                }
            }

            if (userManager.FindByNameAsync("usermanager").Result == null)
            {
                var userManagerUser = new ApplicationUser
                {
                    UserName = "usermanager",
                    FirstName = "User",
                    LastName = "Manager",
                    Email = "usermanager@example.com",
                    EmailConfirmed = true
                };
                var result = userManager.CreateAsync(userManagerUser, "P@ssw0rd").Result;
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to create user manager user");
                }

                result = userManager.AddToRoleAsync(userManagerUser, UserRoles.UserManager).Result;
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to add user manager user to user manager role");
                }
            }

            if (userManager.FindByNameAsync("regularuser").Result == null)
            {
                var regularUser = new ApplicationUser
                {
                    UserName = "regularuser",
                    FirstName = "Regular",
                    LastName = "User",
                    Email = "regularuser@example.com",
                    EmailConfirmed = true
                };
                var result = userManager.CreateAsync(regularUser, "P@ssw0rd").Result;
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to create regular user");

                }
                result = userManager.AddToRoleAsync(regularUser, UserRoles.RegularUser).Result;
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to add regular user to RegularUser role");
                }
            }
        }
    }
}
