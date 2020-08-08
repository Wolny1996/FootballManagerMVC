using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballManager.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FootballManager.Data.Repositories.Concrete
{
    public class ClubRepository : IClubRepository
    {
        private readonly FootballManagerContext _context;
        private readonly ILogger<ClubRepository> _logger;

        public ClubRepository(FootballManagerContext context, ILogger<ClubRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Club> GetSpecificClub(string clubName)
        {
            var searchedClub = await _context.Clubs
                .Include(c => c.Stadium)
                .Include(c => c.Coach)
                .Include(c => c.Footballers)
                .Include(c => c.ClubTournaments)
                .ThenInclude(ct => ct.Tournaments)
                .FirstOrDefaultAsync(c => c.ClubName == clubName);

            if (searchedClub == null)
            {
                _logger.LogInformation($"Club with name {clubName} doesn't exists.");
                // TODO return error object with proper error code.
                throw new Exception("Entity not founded.");
            }

            return searchedClub;
        }

        public async Task<IEnumerable<Club>> GetAllClubs()
        {
            var clubs = await _context.Clubs
                .Include(c => c.Stadium)
                .Include(c => c.Coach)
                .Include(c => c.Footballers)
                .Include(c => c.ClubTournaments)
                .ThenInclude(ct => ct.Tournaments)
                .ToListAsync();

            if (clubs == null)
            {
                _logger.LogInformation($"No clubs in database.");
                // TODO return error object with proper error code.
                throw new Exception("Entity not founded.");
            }

            return clubs;
        }

        public async Task PutExistingClub(string clubName, Club updatedClub)
        {
            var existingClub = await _context.Clubs.FirstOrDefaultAsync(c => c.ClubName == clubName);

            if (existingClub == null)
            {
                _logger.LogInformation($"Club with name {clubName} doesn't exists.");
                // TODO return error object with proper error code.
                throw new Exception("Entity not founded.");
            }

            existingClub.ClubName = updatedClub.ClubName;
            existingClub.City = updatedClub.City;
            existingClub.Founded = updatedClub.Founded;
            _context.Update(existingClub);
            await _context.SaveChangesAsync();
        }

        public async Task PostNewClub(Club newClub)
        {
            _context.Clubs.Add(newClub);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSpecificClub(string clubName)
        {
            var existingClub = await _context.Clubs.FirstOrDefaultAsync(c => c.ClubName == clubName);

            if (existingClub == null)
            {
                _logger.LogInformation($"Club with name {clubName} doesn't exists.");
                // TODO return error object with proper error code.
                throw new Exception("Entity not founded.");
            }

            _context.Clubs.Remove(existingClub);
            await _context.SaveChangesAsync();
        }
    }
}