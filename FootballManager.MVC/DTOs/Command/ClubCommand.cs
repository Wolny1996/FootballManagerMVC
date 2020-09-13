using System;

namespace FootballManager.API.DTOs.Command
{
    public class ClubCommand
    {
        public string ClubName { get; set; }
        public string City { get; set; }
        public DateTime Founded { get; set; }
    }
}