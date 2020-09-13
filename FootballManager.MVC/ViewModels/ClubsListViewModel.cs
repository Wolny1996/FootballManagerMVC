using FootballManager.API.DTOs.Response;
using System.Collections.Generic;

namespace FootballManager.API.ViewModels
{
    public class ClubsListViewModel
    {
        public IEnumerable<ClubResponse> Clubs { get; set; }
    }
}