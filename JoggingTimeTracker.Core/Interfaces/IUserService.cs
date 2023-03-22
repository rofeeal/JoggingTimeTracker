using JoggingTimeTracker.Core.Models;
using JoggingTimeTracker.Core.Models.Results;

namespace JoggingTimeTracker.Core.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser> GetUserByIdAsync(string id);
        Task<IEnumerable<ApplicationUser>> GetUsersByRole(string roleName);
        Task<Result> CreateUserAsync(ApplicationUser user, string password);
        Task<Result> UpdateUserAsync(ApplicationUser user);
        Task<Result> DeleteUserAsync(string id);
        Task AddUserToRole(ApplicationUser user, string roleName);
        Task RemoveUserFromRole(ApplicationUser user, string roleName);
    }
}
