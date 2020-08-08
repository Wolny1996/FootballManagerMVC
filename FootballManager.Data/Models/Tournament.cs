using System.Collections.Generic;

namespace FootballManager.Data.Models
{
    public partial class Tournament
    {
        public Tournament()
        {
            ClubTournament = new HashSet<ClubTournament>();
        }

        public int Id { get; set; }
        public string TournamentName { get; set; }
        public virtual ICollection<ClubTournament> ClubTournament { get; set; }
    }
}