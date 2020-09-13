using FootballManager.Data.Models;
using System.Collections.Generic;

namespace FootballManager.Data.Repositories
{
    public interface IHomeRepository
    {
        IEnumerable<Club> GetAllClubs();
    }
}