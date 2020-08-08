using FootballManager.Data.Models;
using System.Collections.Generic;

namespace FootballManager.API.DTOs.Response
{
    public class TournamentResponse
    {
        public string TournamentName { get; set; }
        public virtual ICollection<Club> ClubsPartycipatingInTournament { get; set; }
    }
}