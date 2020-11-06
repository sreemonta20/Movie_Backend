
using Movie.BackEnd.Common.Utilities;
using Movie.BackEnd.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Movie.BackEnd.Domain.Contract
{
    public interface IMovieRepository
    {
        Task<object> GetMovieList();
        Task<object> GetLanguageList();
        Task<object> GetLocationList();
        Task<object> BookedMovie(int userCode, int imdbCode);
        Task<object> GetMoviesSearchByTitle(string title);
        Task<object> GetMovieByCode(int code);
        Task<object> GetMoviesSearchByLanguage(string language);
        Task<object> GetMoviesSearchByLocation(string location);
    }
}
