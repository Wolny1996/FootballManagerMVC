using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballManager.Data.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;

namespace FootballManager.Data.Repositories.Concrete
{
    public class CoachRepository : ICoachRepository
    {
        private readonly FootballManagerContext _context;
        private readonly ILogger<CoachRepository> _logger;

        public CoachRepository(FootballManagerContext context, ILogger<CoachRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Coach> GetSpecificCoach(string coachSurname)
        {
            var query = _context.Coaches
                    .Include(c => c.Club);

            var policy = Policy.Handle<SqlException>().WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10),
                TimeSpan.FromSeconds(15),
            },(ext, timeSpan, retryCount, context) =>
                {
                    _logger.LogError(ext, $"Error - try retry (count: {retryCount}, timeSpan: {timeSpan})");
                });

            var searchedCoach = await policy.ExecuteAsync(() => query.FirstOrDefaultAsync(c => c.Surname == coachSurname));

            if (searchedCoach == null)
            {
                _logger.LogInformation($"coach with {coachSurname} doesn't exists.");
                // todo return error object with proper error code.
                throw new Exception("entity not founded.");
            }

            return searchedCoach;
        }

        public async Task<IEnumerable<Coach>> GetAllCoaches()
        {
            var query = _context.Coaches
                .Include(c => c.Club)
                .AsNoTracking();

            var policy = Policy.Handle<SqlException>().WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10),
                TimeSpan.FromSeconds(15),
            }, (ext, timeSpan, retryCount, context) =>
                {
                    _logger.LogError(ext, $"Error - try retry (count: {retryCount}, timeSpan: {timeSpan})");
                });

            var coaches = await policy.ExecuteAsync(() => query.ToListAsync());

            if (coaches == null)
            {
                _logger.LogInformation($"No coaches in database.");
                // TODO return error object with proper error code.
                throw new Exception("Entity not founded.");
            }

            return coaches;
        }

        public async Task PutExistingCoach(string coachSurname, string coachCurrentClub, Coach updatedCoach)
        {
            var policy = Policy.Handle<SqlException>().WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10),
                TimeSpan.FromSeconds(15),
            }, (ext, timeSpan, retryCount, context) =>
                {
                    _logger.LogError(ext, $"Error - try retry (count: {retryCount}, timeSpan: {timeSpan})");
                });

            var existingCoach = await policy.ExecuteAsync(() =>
                _context.Coaches.FirstOrDefaultAsync(c => c.Surname == coachSurname));

            if (existingCoach == null)
            {
                _logger.LogInformation($"Coach with name {coachSurname} doesn't exists.");
                // TODO return error object with proper error code.
                throw new Exception("Entity not founded.");
            }

            var existingClub = await _context.Clubs.FirstOrDefaultAsync(c => c.ClubName == coachCurrentClub);

            if (existingClub == null)
            {
                _logger.LogInformation($"Club with name {updatedCoach.Club.ClubName} doesn't exists.");
                // TODO return error object with proper error code.
                throw new Exception("Entity not founded.");
            }

            existingCoach.Name = updatedCoach.Name;
            existingCoach.Surname = updatedCoach.Surname;
            existingCoach.Club = existingClub;

            _context.Update(existingCoach);
            await _context.SaveChangesAsync();
        }

        public async Task PostNewCoach(string addedCoachCurrentClub, Coach newCoach)
        {
            var policy = Policy.Handle<SqlException>().WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10),
                TimeSpan.FromSeconds(15),
            }, (ext, timeSpan, retryCount, context) =>
                {
                    _logger.LogError(ext, $"Error - try retry (count: {retryCount}, timeSpan: {timeSpan})");
                });

            var existingClub = await policy.ExecuteAsync(() =>
                _context.Clubs.FirstOrDefaultAsync(c => c.ClubName == addedCoachCurrentClub));

            if (existingClub == null)
            {
                _logger.LogInformation($"Club with name {addedCoachCurrentClub} doesn't exists.");
                // TODO return error object with proper error code.
                throw new Exception("Entity not founded.");
            }

            newCoach.Club = existingClub;
            _context.Coaches.Add(newCoach);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSpecificCoach(string coachSurname)
        {
            var policy = Policy.Handle<SqlException>().WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10),
                TimeSpan.FromSeconds(15),
            }, (ext, timeSpan, retryCount, context) =>
                {
                    _logger.LogError(ext, $"Error - try retry (count: {retryCount}, timeSpan: {timeSpan})");
                });

            var existingCoach = await policy.ExecuteAsync(() =>
                _context.Coaches.FirstOrDefaultAsync(c => c.Surname == coachSurname));

            if (existingCoach == null)
            {
                _logger.LogInformation($"Coach with name {coachSurname} doesn't exists.");
                // TODO return error object with proper error code.
                throw new Exception("Entity not founded.");
            }

            _context.Coaches.Remove(existingCoach);
            await _context.SaveChangesAsync();
        }
    }
}