using JoggingTimeTracker.Core.Models;

namespace JoggingTimeTracker.Core.Interfaces
{
    public interface IAuthService
    {
        Task<string> Login(string username, string password);

        Task Logout();
    }
}
