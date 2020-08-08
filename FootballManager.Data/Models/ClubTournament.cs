namespace FootballManager.Data.Models
{
    public class ClubTournament
    {
        public int ClubId { get; set; }
        public int TournamentId { get; set; }
        public virtual Club Clubs { get; set; }
        public virtual Tournament Tournaments { get; set; }
    }
}