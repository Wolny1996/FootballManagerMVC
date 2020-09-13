namespace FootballManager.Data.Models
{
    public class Stadium
    {
        public int Id { get; set; }
        public string StadiumName { get; set; }
        public string StadiumImageUrl { get; set; }
        public int Capacity { get; set; }
        public int ClubId { get; set; }
        public virtual Club Club { get; set; }
    }
}