using MovieRentalAdminApi.CrossCutting.Dto;
using MovieRentalAdminApi.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieRentalAdminApi.Domain.Interfaces
{
    public interface IMovieRepository
    {
        Task CreateMovie(MovieEntity movie);
        Task UpdateMovie(MovieEntity movie);
        Task<MovieEntity> GetById(int id);
        Task DeleteMovie (MovieEntity movie);
        Task<MovieEntity> GetByTitle(string title);
        Task<IEnumerable<MovieEntity>> GetMoviesByFilters(PaginationDto pagination);
        Task<IEnumerable<MovieEntity>> SearchByTitle(string title);
    }
}