using FootballManager.API.ViewModels;
using FootballManager.Data;
using FootballManager.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FootballManager.API.Controllers
{
    public class HomeController : Controller
    {
        private readonly FootballManagerContext _context;
        private readonly IHomeRepository _repository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(FootballManagerContext context, IHomeRepository repository, ILogger<HomeController> logger)
        {
            _context = context;
            _repository = repository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var homeViewModel = new HomeViewModel
                {
                    Clubs = _repository.GetAllClubs()
                };

                _logger.LogInformation($"Clubs have been downloaded.");
                Ok();
                return View(homeViewModel);
            }
            catch (System.Exception ext)
            {
                _logger.LogError(ext, "Failed to download clubs list.");
                // TODO return error object with proper error code.
                BadRequest();
                return View("Error");
            }
        }
    }
}