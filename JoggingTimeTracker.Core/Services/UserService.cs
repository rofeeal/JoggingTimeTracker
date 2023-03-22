using JoggingTimeTracker.Core.Interfaces;
using JoggingTimeTracker.Core.Models;
using JoggingTimeTracker.Core.Models.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JoggingTimeTracker.Core.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersByRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
                return usersInRole;
            }

            return Enumerable.Empty<ApplicationUser>();
        }

        public async Task<Result> CreateUserAsync(ApplicationUser user, string password)
        {
            var resultDto = new Result();
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await AddUserToRole(user, user.Role);
                resultDto.Succeeded = true;
                resultDto.Message = "User created successfully.";
            }
            else
            {
                resultDto.Succeeded = false;
                resultDto.Message = "Failed to create user.";
                resultDto.Errors = result.Errors.Select(e => e.Description);
            }

            return resultDto;
        }


        public async Task<Result> UpdateUserAsync(ApplicationUser user)
        {
            var resultDto = new Result();
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                resultDto.Succeeded = true;
                resultDto.Message = "User updated successfully.";
            }
            else
            {
                resultDto.Succeeded = false;
                resultDto.Message = "Failed to update user.";
                resultDto.Errors = result.Errors.Select(e => e.Description);
            }

            return resultDto;
        }


        public async Task<Result> DeleteUserAsync(string id)
        {
            var resultDto = new Result();
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    resultDto.Succeeded = true;
                    resultDto.Message = "User deleted successfully.";
                }
                else
                {
                    resultDto.Succeeded = false;
                    resultDto.Message = "Failed to delete user.";
                    resultDto.Errors = result.Errors.Select(e => e.Description);
                }
            }
            else
            {
                resultDto.Succeeded = false;
                resultDto.Message = "User not found.";
            }

            return resultDto;
        }


        public async Task AddUserToRole(ApplicationUser user, string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task RemoveUserFromRole(ApplicationUser user, string roleName)
        {
            await _userManager.RemoveFromRoleAsync(user, roleName);
        }

    }
}
