using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballManager.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FootballManager.Data.Repositories.Concrete
{
    public class FootballerRepository : IFootballerRepository
    {
        private readonly FootballManagerContext _context;
        private readonly ILogger<FootballerRepository> _logger;

        public FootballerRepository(FootballManagerContext context, ILogger<FootballerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Footballer> GetSpecificFootballer(string footballerSurname)
        {
            var searchedFootballer = await _context.Footballers
                    .Include(f => f.Club)
                    .FirstOrDefaultAsync(f => f.Surname == footballerSurname);

            if (searchedFootballer == null)
            {
                _logger.LogInformation($"Footballer with {footballerSurname} doesn't exists.");
                // TODO return error object with proper error code.
                throw new Exception("Entity not founded.");
            }

            return searchedFootballer;
        }

        public async Task<IEnumerable<Footballer>> GetAllFootballers()
        {
            var footballers = await _context.Footballers
                .Include(f => f.Club)
                .AsNoTracking()
                .ToListAsync();

            if (footballers == null)
            {
                _logger.LogInformation($"No footballers in database.");
                // TODO return error object with proper error code.
                throw new Exception("Entity not founded.");
            }

            return footballers;
        }

        public async Task PutExistingFootballer(string footballerSurname, string footballerCurrentClub, Footballer updatedFootballer)
        {
            var existingFootballer = await _context.Footballers.FirstOrDefaultAsync(f => f.Surname == footballerSurname);

            if (existingFootballer == null)
            {
                _logger.LogInformation($"Footballer with name {footballerSurname} doesn't exists.");
                // TODO return error object with proper error code.
                throw new Exception("Entity not founded.");
            }

            var existingClub = await _context.Clubs.FirstOrDefaultAsync(c => c.ClubName == footballerCurrentClub);

            if (existingClub == null)
            {
                _logger.LogInformation($"Club with name {footballerCurrentClub} doesn't exists.");
                // TODO return error object with proper error code.
                throw new Exception("Entity not founded.");
            }

            existingFootballer.Name = updatedFootballer.Name;
            existingFootballer.Surname = updatedFootballer.Surname;
            existingFootballer.Club = existingClub;

            _context.Update(existingFootballer);
            await _context.SaveChangesAsync();
        }

        public async Task PostNewFootballer(string addedFootballerCurrentClub, Footballer newFootballer)
        {
            var existingClub = await _context.Clubs.FirstOrDefaultAsync(c => c.ClubName == addedFootballerCurrentClub);

            if (existingClub == null)
            {
                _logger.LogInformation($"Club with name {addedFootballerCurrentClub} doesn't exists.");
                // TODO return error object with proper error code.
                throw new Exception("Entity not founded.");
            }

            newFootballer.Club = existingClub;
            _context.Footballers.Add(newFootballer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSpecificFootballer(string footballerSurname)
        {
            var existingFootballer = await _context.Footballers.FirstOrDefaultAsync(f => f.Surname == footballerSurname);

            if (existingFootballer == null)
            {
                _logger.LogInformation($"Footballer with name {footballerSurname} doesn't exists.");
                // TODO return error object with proper error code.
                throw new Exception("Entity not founded.");
            }

            _context.Footballers.Remove(existingFootballer);
            await _context.SaveChangesAsync();
        }
    }
}