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
    public class HomeRepository : IHomeRepository
    {
        private readonly FootballManagerContext _context;
        private readonly ILogger<HomeRepository> _logger;

        public HomeRepository(FootballManagerContext context, ILogger<HomeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Club> GetAllClubs()
        {
            var query = _context.Clubs;

            var policy = Policy.Handle<SqlException>().WaitAndRetry(new[]
            {
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10),
                TimeSpan.FromSeconds(15),
            }, (ext, timeSpan, retryCount, context) =>
            {
                _logger.LogError(ext, $"Error - try retry (count: {retryCount}, timeSpan: {timeSpan})");
            });

            var clubs = policy.Execute(() => query.ToList());

            if (clubs == null)
            {
                _logger.LogInformation($"No clubs in database.");
                // TODO return error object with proper error code.
                throw new Exception("Entity not founded.");
            }

            return clubs;
        }
    }
}