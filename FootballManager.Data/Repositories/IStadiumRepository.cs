using FootballManager.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballManager.Data.Repositories
{
    public interface IStadiumRepository
    {
        Task<Stadium> GetSpecificStadium(string stadiumName);
        Task<IEnumerable<Stadium>> GetAllStadiums();
        Task PutExistingStadium(string stadiumName, string stadiumClub, Stadium updatedStadium);
        Task PostNewStadium(string addedStadiumClub, Stadium newStadium);
        Task DeleteSpecificStadium(string stadiumName);
    }
}