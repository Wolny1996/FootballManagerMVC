using FootballManager.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballManager.Data.Repositories
{
    public interface ICoachRepository
    {
        Task<Coach> GetSpecificCoach(string surname);
        Task<IEnumerable<Coach>> GetAllCoaches();
        Task PutExistingCoach(string surname, string coachCurrentClub, Coach updatedCoach);
        Task PostNewCoach(string addedCoachCurrentClub, Coach newCoach);
        Task DeleteSpecificCoach(string surname);
    }
}