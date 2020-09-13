using System.Collections.Generic;
using AutoMapper;
using FootballManager.API.DTOs.Response;
using FootballManager.API.ViewModels;
using FootballManager.Data;
using FootballManager.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.Swagger.Annotations;

namespace FootballManager.API.Controllers
{
    /// <summary>
    ///  A controller for menaging clubs in database.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ClubsController : Controller
    {
        private readonly FootballManagerContext _context;
        private readonly IClubRepository _repository;
        private readonly ILogger<ClubsController> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        ///  Initializes a new istance of the <see cref="ClubsController"/> class.
        /// </summary>
        /// <param name="_context">Context instance</param>
        /// <param name="_logger">Logger instance</param>
        /// <param name="_mapper">Mapper instance</param>
        public ClubsController(FootballManagerContext context, IClubRepository repository, ILogger<ClubsController> logger, IMapper mapper)
        {
            _context = context;
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets a club by club's name for a requesting user.
        /// </summary>
        /// <response code="200">If a club exists returns OK.</response>
        /// <response code="400">If clubName claim is missing in the token return BadRequest.</response>
        /// <response code="404">If a club doesn't exist a response returns NotFound.</response>
        /// <param name="clubName">Name of a specific club to be retrieved.</param>
        /// <returns>Specific club for a requesting user.</returns>
        [HttpGet("{clubName}")]
        [SwaggerOperation(OperationId = "getSpecificClub")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetSpecificClub(string clubName)
        {
            try
            {
                var searchedClub = _repository.GetSpecificClub(clubName);
                var clubResponse = _mapper.Map<ClubResponse>(searchedClub);
                _logger.LogInformation($"Club has been downloaded.");
                Ok();
                return View(clubResponse);
            }
            catch (System.Exception ext)
            {
                _logger.LogError(ext, "Failed to download club.");
                // TODO return error object with proper error code.
                BadRequest();
                return View("Error");
            }
        }

        /// <summary>
        /// Gets clubs for a requesting user.
        /// </summary>
        /// <response code="200">If a list exists returns OK.</response>
        /// <response code="400">If failed to download clubs list in the token return BadRequest.</response>
        /// <response code="404">If a clubs don't exist a response returns NotFound.</response>
        /// <returns>Clubs list for requesting user.</returns>
        [HttpGet]
        [SwaggerOperation(OperationId = "getAllClubs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAllClubs()
        {
            try
            {
                var clubs = _repository.GetAllClubs();

                var clubsResponse = new List<ClubResponse>();

                foreach (var c in clubs)
                {
                    try
                    {
                        var clubResponse = _mapper.Map<ClubResponse>(c);
                        clubsResponse.Add(clubResponse);
                    }
                    catch (System.Exception)
                    {
                        throw new System.Exception();
                    }
                }

                var clubsListViewModel = new ClubsListViewModel
                {
                    Clubs = clubsResponse
                };

                _logger.LogInformation($"Clubs have been downloaded.");
                Ok();
                return View(clubsListViewModel);
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