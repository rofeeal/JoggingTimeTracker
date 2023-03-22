using JoggingTimeTracker.Core.Models;
using JoggingTimeTracker.Core.Models.Report;
using JoggingTimeTracker.Core.Models.Results;

namespace JoggingTimeTracker.Core.Interfaces
{
    public interface IJoggingTimeRepository
    {
        Task<ResultWithData<IEnumerable<JoggingTime>>> GetAllJoggingTimes(string userId);
        Task<ResultWithData<IEnumerable<JoggingTime>>> GetJoggingTimesFilterDate(string userId, DateTime? fromDate, DateTime? toDate);
        Task<ResultWithData<IEnumerable<WeeklyJoggingStats>>> GetJoggingTimesFilterWeeek(string userId);
        Task<ResultWithData<JoggingTime>> GetJoggingTime(int joggingTimeId);
        Task<ResultWithData<JoggingTime>> AddJoggingTime(JoggingTime joggingTime);
        Task<ResultWithData<JoggingTime>> UpdateJoggingTime(JoggingTime joggingTime);
        Task<Result> DeleteJoggingTimeAsync(int JoggingTimeId);

    }
}
