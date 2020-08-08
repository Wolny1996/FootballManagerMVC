using FootballManager.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballManager.Data.Repositories
{
    public interface IFootballerRepository
    {
        Task<Footballer> GetSpecificFootballer(string surname);
        Task<IEnumerable<Footballer>> GetAllFootballers();
        Task PutExistingFootballer(string footballerSurname, string footballerCurrentClub, Footballer updatedFootballer);
        Task PostNewFootballer(string addedFootballerCurrentClub, Footballer newFootballer);
        Task DeleteSpecificFootballer(string surname);
    }
}