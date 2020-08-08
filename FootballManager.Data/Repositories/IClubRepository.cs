using FootballManager.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballManager.Data.Repositories
{
    public interface IClubRepository
    {
        Task<Club> GetSpecificClub(string clubName);
        Task<IEnumerable<Club>> GetAllClubs();
        Task PutExistingClub(string clubName, Club updatedClub);
        Task PostNewClub(Club newClub);
        Task DeleteSpecificClub(string clubName);
    }
}