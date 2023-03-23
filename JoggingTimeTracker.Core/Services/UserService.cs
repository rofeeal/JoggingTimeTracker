using JoggingTimeTracker.Core.Interfaces;
using JoggingTimeTracker.Core.Models;
using JoggingTimeTracker.Core.Models.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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

        public async Task<ResultWithData<IEnumerable<ApplicationUser>>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            if (users.Any())
            {
                return ResultWithData<IEnumerable<ApplicationUser>>.Success(users);
            }

            return ResultWithData<IEnumerable<ApplicationUser>>.Error("No users found");
        }

        public async Task<ResultWithData<ApplicationUser>> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                return ResultWithData<ApplicationUser>.Success(user);
            }

            return ResultWithData<ApplicationUser>.Error("User not found");
        }

        public async Task<ResultWithData<IList<ApplicationUser>>> GetUsersByRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
                return ResultWithData<IList<ApplicationUser>>.Success(usersInRole);
            }

            return ResultWithData<IList<ApplicationUser>>.Error("Role not found");
        }

        public async Task<ResultWithData<ApplicationUser>> CreateUserAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await AddUserToRole(user, user.Role);
                return ResultWithData<ApplicationUser>.Success(user);
            }

            var errorMessages = result.Errors.Select(e => e.Description).ToList();
            return ResultWithData<ApplicationUser>.Error(errorMessages.FirstOrDefault() ?? "An error occurred while creating the user");
        }

        public async Task<ResultWithData<ApplicationUser>> UpdateUserAsync(ApplicationUser user)
        {
            try
            {
                var result = await _userManager.UpdateAsync(user);

                return ResultWithData<ApplicationUser>.Success(user);
            }
            catch (Exception ex)
            {
                return ResultWithData<ApplicationUser>.Error("Failed to update user. " + ex.Message);
            }
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

        public async Task<Result> AddUserToRole(ApplicationUser user, string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                return new Result { Succeeded = true, Message = "User added to role successfully." };
            }

            var errorMessages = result.Errors.Select(e => e.Description).ToList();
            return new Result { Succeeded = false, Message = "Failed to add user to role.", Errors = errorMessages };
        }

        public async Task<Result> RemoveUserFromRole(ApplicationUser user, string roleName)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                return new Result { Succeeded = true, Message = "User removed from role successfully." };
            }

            var errorMessages = result.Errors.Select(e => e.Description).ToList();
            return new Result { Succeeded = false, Message = "Failed to remove user from role.", Errors = errorMessages };
        }


    }
}
