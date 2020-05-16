using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieRentalAdminApi.CrossCutting.Dto;
using MovieRentalAdminApi.Domain.Entities;
using MovieRentalAdminApi.Domain.Interfaces;
using MovieRentalAdminApi.Dto;
using MovieRentalAdminApi.Utils;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRentalAdminApi.Controllers
{
    [Route("movies")]
    public class MovieController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IRentalSettingsRepository _rentalSettingsRepository;
        private readonly ITokenFactory _tokenFactory;
        public MovieController(IMovieRepository movieRepository, IRentalSettingsRepository rentalSettingsRepository, ITokenFactory tokenFactory, IUserAccountRepository userAccountRepository)
        {
            _movieRepository = movieRepository;
            _rentalSettingsRepository = rentalSettingsRepository;
            _tokenFactory = tokenFactory;
            _userAccountRepository = userAccountRepository;;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody]AddMovieDto movie)
        {
            var _movie = await _movieRepository.GetByTitle(movie.Title);
            if (_movie != null)
                return BadRequest($"There is already a movie with the name: {movie.Title}");

            _movie = new MovieEntity().Create(movie.Title, movie.Description, movie.Stock, movie.RentalPrice, movie.SalePrice, movie.Availability);
            if (_movie == null)
                return BadRequest("Invalid data.");

            await _movieRepository.CreateMovie(_movie);
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("uploadimages")]
        public async Task<IActionResult> UploadImages(int id, [FromForm]IFormFile[] files)
        {
            if (files.Count() < 1)
                return BadRequest("No images to upload.");

            var movie = await _movieRepository.GetById(id);
            if (movie == null)
                return NotFound("Movie not found.");

            foreach (var file in files)
            {
                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                movie.UploadImage(ms.ToArray(), file.FileName);

                ms.Close();
                ms.Dispose();
            }
            await _movieRepository.UpdateMovie(movie);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("{id}/update")]
        public async Task<IActionResult> Update(int id, [FromBody]UpdateMovieDto movie)
        {
            var _movie = await _movieRepository.GetByTitle(movie.Title);
            if (_movie != null)
                return BadRequest($"There is already a movie with the name: {movie.Title}");

            _movie = await _movieRepository.GetById(id);
            if (_movie == null)
                return NotFound("Movie not found.");

            if (!_movie.Update(movie.Title, movie.Description, movie.Stock, movie.RentalPrice, movie.SalePrice, movie.Availability))
                return BadRequest("Invalid data.");

            await _movieRepository.UpdateMovie(_movie);
            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _movieRepository.GetById(id);
            if (movie == null)
                return NotFound("Movie not found.");

            await _movieRepository.DeleteMovie(movie);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Customer")]
        [Route("{id}/rent")]
        public async Task<IActionResult> Rent(int id, [FromQuery]int quantity)
        {
            var movie = await _movieRepository.GetById(id);
            var rentalSetting = await _rentalSettingsRepository.GetActiveSetting();

            if (movie == null)
                return NotFound("Movie not found.");

            if(rentalSetting == null)
                return BadRequest("There is no active setting to rent movies");

            var result = movie.Rent(quantity, rentalSetting.DaysForRent, rentalSetting.Penalty);

            if (!result)
                return BadRequest("There is not enough movies in stock or is not available.");

            await _movieRepository.UpdateMovie(movie);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Customer")]
        [Route("{id}/buy")]
        public async Task<IActionResult> Buy (int id, [FromQuery]int quantity)
        {
            var movie = await _movieRepository.GetById(id);

            if (movie == null)
                return NotFound("Movie not found.");

            var result = movie.Buy(quantity);
            if (!result) 
                return BadRequest("There is not enough movies in stock or is not available.");

            await _movieRepository.UpdateMovie(movie);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Customer")]
        [Route("{id}/like")]
        public async Task<IActionResult> Like(int id)
        {
            var movie = await _movieRepository.GetById(id);
            if (movie == null)
                return NotFound("Movie not found.");

            var userNameClaim = _tokenFactory.GetUser();
            if (userNameClaim == null)
                return Unauthorized();

            var user = await _userAccountRepository.GetByUserName(userNameClaim);
            if (user == null)
                return BadRequest("User account not found.");

            movie.Like(user);
            await _movieRepository.UpdateMovie(movie);

            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("movie")]
        public async Task<IActionResult> SearchByTitle([FromQuery]string title)
        {
            var result = await _movieRepository.SearchByTitle(title);
            if (result == null)
                return NotFound("Movie not found.");
            return Ok(result.Select(a => new GetMoviesResponseDto
            {
                Id = a.Id,
                Availability = a.Availability,
                Description = a.Description,
                Likes = a.Likes,
                RentalPrice = a.RentalPrice,
                SalePrice = a.SalePrice,
                Stock = a.Stock,
                Title = a.Title
            }));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get([FromQuery]Filter filter = Filter.All, GetMoviesDto sortable = null)
        {
            sortable = sortable ?? new GetMoviesDto();
            var result = await _movieRepository.GetMoviesByFilters(new PaginationDto
            {
                PageNumber = sortable.PageNumber,
                PageSize = sortable.PageSize,
                SortBy = sortable.SortBy.ToString(),
                Order = sortable.Order.ToString(),
                Filter = (int)filter
            });
            return Ok(result.Select(a => new GetMoviesResponseDto
            {
                Id = a.Id,
                Availability = a.Availability,
                Description = a.Description,
                Likes = a.Likes,
                RentalPrice = a.RentalPrice,
                SalePrice = a.SalePrice,
                Stock = a.Stock,
                Title = a.Title
            }));
        }
         

        [HttpGet]
        [AllowAnonymous]
        [Route("availables")]
        public async Task<IActionResult> Availables([FromQuery]GetMoviesDto sortable = null)
        {
            sortable = sortable ?? new GetMoviesDto();
            var result = await _movieRepository.GetMoviesByFilters(new PaginationDto
            {
                PageNumber = sortable.PageNumber,
                PageSize = sortable.PageSize,
                SortBy = sortable.SortBy.ToString(),
                Order = sortable.Order.ToString(),
                Filter = 1
            });
            return Ok(result.Select(a => new GetMoviesResponseDto
            {
                Id = a.Id,
                Availability = a.Availability,
                Description = a.Description,
                Likes = a.Likes,
                RentalPrice = a.RentalPrice,
                SalePrice = a.SalePrice,
                Stock = a.Stock,
                Title = a.Title
            }));
        }
    }
}