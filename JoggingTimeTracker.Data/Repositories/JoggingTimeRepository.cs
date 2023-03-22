using JoggingTimeTracker.Core.Interfaces;
using JoggingTimeTracker.Core.Models;
using JoggingTimeTracker.Core.Models.Report;
using JoggingTimeTracker.Core.Models.Results;
using JoggingTimeTracker.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace JoggingTimeTracker.Data.Repositories
{
    public class JoggingTimeRepository : IJoggingTimeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public JoggingTimeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultWithData<JoggingTime>> AddJoggingTime(JoggingTime joggingTime)
        {
            try
            {
                var result = await _dbContext.JoggingRecords.AddAsync(joggingTime);
                await _dbContext.SaveChangesAsync();
                return ResultWithData<JoggingTime>.Success(result.Entity);
            }
            catch (Exception ex)
            {
                return ResultWithData<JoggingTime>.Error("An error occurred while adding the jogging time record.");
            }
        }

        public async Task<Result> DeleteJoggingTimeAsync(int JoggingTimeId)
        {
            var result = await _dbContext.JoggingRecords.FirstOrDefaultAsync(e => e.JoggingTimeId == JoggingTimeId);

            if (result != null)
            {
                _dbContext.JoggingRecords.Remove(result);
                await _dbContext.SaveChangesAsync();
                return new Result { Succeeded = true, Message = "Jogging time has been deleted successfully." };
            }
            else
            {
                return new Result { Succeeded = false, Message = "No jogging time was found with the given Id." };
            }
        }

        public async Task<ResultWithData<IEnumerable<JoggingTime>>> GetAllJoggingTimes(string userId)
        {
            try
            {
                var joggingTimes = await _dbContext.JoggingRecords.Where(j => j.UserId == userId).ToListAsync();

                if (joggingTimes == null)
                {
                    return ResultWithData<IEnumerable<JoggingTime>>.Error($"Jogging time with user id {userId} not found");
                }
                return ResultWithData<IEnumerable<JoggingTime>>.Success(joggingTimes);
            }
            catch (Exception ex)
            {
                return ResultWithData<IEnumerable<JoggingTime>>.Error($"Error: {ex.Message}");
            }
        }

        public async Task<ResultWithData<JoggingTime>> GetJoggingTime(int joggingTimeId)
        {
            try
            {
                var joggingTime = await _dbContext.JoggingRecords.FirstOrDefaultAsync(e => e.JoggingTimeId == joggingTimeId);

                if (joggingTime == null)
                {
                    return ResultWithData<JoggingTime>.Error($"Jogging time with id {joggingTimeId} not found");
                }

                return ResultWithData<JoggingTime>.Success(joggingTime);
            }
            catch (Exception ex)
            {
                return ResultWithData<JoggingTime>.Error($"Error occurred while getting jogging time with id {joggingTimeId}: {ex.Message}");
            }
        }

        public async Task<ResultWithData<IEnumerable<JoggingTime>>> GetJoggingTimesFilterDate(string userId, DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                var data = await _dbContext.JoggingRecords
                    .Where(j => j.UserId == userId && (!fromDate.HasValue || j.Date >= fromDate.Value) && (!toDate.HasValue || j.Date <= toDate.Value))
                    .ToListAsync();

                return ResultWithData<IEnumerable<JoggingTime>>.Success(data);
            }
            catch (Exception ex)
            {
                return ResultWithData<IEnumerable<JoggingTime>>.Error($"Error: {ex.Message}");
            }
        }

        public async Task<ResultWithData<IEnumerable<WeeklyJoggingStats>>> GetJoggingTimesFilterWeeek(string userId)
        {
            try
            {
                var allJoggingRecords = await _dbContext.JoggingRecords
                    .Where(j => j.UserId == userId)
                    .ToListAsync();

                var weeklyStats = allJoggingRecords
                    .GroupBy(j => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(j.Date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday))
                    .Select(g => new WeeklyJoggingStats
                    {
                        WeekNumber = g.Key,
                        AverageSpeed = g.Average(j => j.Speed),
                        AverageDistance = g.Average(j => j.DistanceInMeter)
                    })
                    .OrderBy(w => w.WeekNumber)
                    .ToList();

                return ResultWithData<IEnumerable<WeeklyJoggingStats>>.Success(weeklyStats);
            }
            catch (Exception ex)
            {
                return ResultWithData<IEnumerable<WeeklyJoggingStats>>.Error($"Error: {ex.Message}");
            }
        }

        public async Task<ResultWithData<JoggingTime>> UpdateJoggingTime(JoggingTime joggingTime)
        {
            JoggingTime old = await _dbContext.JoggingRecords.FirstOrDefaultAsync(e => e.JoggingTimeId == joggingTime.JoggingTimeId);
            if (old == null)
            {
                return ResultWithData<JoggingTime>.Error($"JoggingTime with id {joggingTime.JoggingTimeId} not found.");
            }

            joggingTime.UserId = old.UserId;
            try
            {
                _dbContext.ChangeTracker.Clear();
                _dbContext.JoggingRecords.Update(joggingTime);
                await _dbContext.SaveChangesAsync();
                return ResultWithData<JoggingTime>.Success(joggingTime);
            }
            catch (Exception ex)
            {
                return ResultWithData<JoggingTime>.Error(ex.Message);
            }
        }

    }
}
