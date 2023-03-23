using JoggingTimeTracker.Core.Models;
using JoggingTimeTracker.Core.Models.Results;

namespace JoggingTimeTracker.Core.Interfaces
{
    public interface IUserService
    {
        Task<ResultWithData<IEnumerable<ApplicationUser>>> GetAllUsersAsync();
        Task<ResultWithData<ApplicationUser>> GetUserByIdAsync(string id);
        Task<ResultWithData<IList<ApplicationUser>>> GetUsersByRole(string roleName);
        Task<ResultWithData<ApplicationUser>> CreateUserAsync(ApplicationUser user, string password);
        Task<ResultWithData<ApplicationUser>> UpdateUserAsync(ApplicationUser user);
        Task<Result> DeleteUserAsync(string id);
        Task<Result> AddUserToRole(ApplicationUser user, string roleName);
        Task<Result> RemoveUserFromRole(ApplicationUser user, string roleName);
    }
}
