using FootballManager.Data.Models;
using System.Collections.Generic;

namespace FootballManager.API.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Club> Clubs { get; set; }
    }
}