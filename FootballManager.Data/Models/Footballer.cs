namespace FootballManager.Data.Models
{
    public class Footballer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int ClubId { get; set; }
        public virtual Club Club { get; set; }
    }
}