using System;
using System.Collections.Generic;

namespace FootballManager.Data.Models
{
    public class Club
    {
        public Club()
        {
            Footballers = new HashSet<Footballer>();
        }

        public int Id { get; set; }
        public string ClubName { get; set; }
        public string ClubImageUrl { get; set; }
        public string City { get; set; }
        public DateTime Founded { get; set; }
        public virtual Coach Coach { get; set; }
        public virtual Stadium Stadium { get; set; }
        public virtual ICollection<Footballer> Footballers { get; set; }
    }
}