using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Movie.BackEnd.Common.Utilities;
using Movie.BackEnd.Domain.Contract;
using Movie.BackEnd.Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace Movie.BackEnd.API.Controllers.MovieManagement
{
    [Authorize]
    [Route("api/[controller]"), Produces("application/json"), EnableCors("AllowOrigin")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        #region Variable Declatation & Initialization

        IMovieRepository _movieService;

        #endregion


        #region Constructor Initialization

        public MovieController(IMovieRepository movieService)
        {
            this._movieService = movieService;
        }

        #endregion


        #region Api Controller Methods or Verbs
        // GET: api/Movie/GetMovies
        [HttpGet("GetMovies")]
        public async Task<object> GetMovies()
        {
            object result = null;
            string message;
            bool state;
            try
            {
                result = await _movieService.GetMovieList();
                message = MessageConstants.Exist;
                state = MessageConstants.SuccessState;
            }
            catch (Exception Ex)
            {
                message = Ex.Message;
                state = MessageConstants.ErrorState;
            }
            return new
            {
                result,
                message,
                state
            };
        }

        // GET: api/Movie/GetLanguage
        [HttpGet("GetLanguage")]
        public async Task<object> GetLanguage()
        {
            object result = null;
            string message;
            bool state;
            try
            {
                result = await _movieService.GetLanguageList();
                message = MessageConstants.Exist;
                state = MessageConstants.SuccessState;
            }
            catch (Exception Ex)
            {
                message = Ex.Message;
                state = MessageConstants.ErrorState;
            }
            return new
            {
                result,
                message,
                state
            };
        }

        // GET: api/Movie/GetLocation
        [HttpGet("GetLocation")]
        public async Task<object> GetLocation()
        {
            object result = null;
            string message;
            bool state;
            try
            {
                result = await _movieService.GetLocationList();
                message = MessageConstants.Exist;
                state = MessageConstants.SuccessState;
            }
            catch (Exception Ex)
            {
                message = Ex.Message;
                state = MessageConstants.ErrorState;
            }
            return new
            {
                result,
                message,
                state
            };
        }

        // GET: api/Movie/GetMoviesByTitle
        [HttpGet("GetMoviesByTitle")]
        public async Task<object> GetMoviesByTitle([FromQuery] string param)
        {
            object result = null;
            string message;
            bool state;
            try
            {
                dynamic data = JsonConvert.DeserializeObject(param);
                ModelCommParam cmnParam = JsonConvert.DeserializeObject<ModelCommParam>(data[0].ToString());
                result = await _movieService.GetMoviesSearchByTitle((string)cmnParam.searchValue);
                message = MessageConstants.Exist;
                state = MessageConstants.SuccessState;
            }
            catch (Exception Ex)
            {
                message = Ex.Message;
                state = MessageConstants.ErrorState;
            }
            return new
            {
                result,
                message,
                state
            };
        }

        // GET: api/Movie/GetMovieByCode
        [HttpGet("GetMovieByCode")]
        public async Task<object> GetMovieByCode([FromQuery] string param)
        {
            object result = null;
            string message;
            bool state;
            try
            {
                dynamic data = JsonConvert.DeserializeObject(param);
                ModelCommParam cmnParam = JsonConvert.DeserializeObject<ModelCommParam>(data[0].ToString());
                result = await _movieService.GetMovieByCode((int)cmnParam.imdbCode);
                message = MessageConstants.Exist;
                state = MessageConstants.SuccessState;
            }
            catch (Exception Ex)
            {
                message = Ex.Message;
                state = MessageConstants.ErrorState;
            }
            return new
            {
                result,
                message,
                state
            };
        }

        // GET: api/Movie/GetMoviesByLanguage
        [HttpGet("GetMoviesByLanguage")]
        public async Task<object> GetMoviesByLanguage([FromQuery] string param)
        {
            object result = null;
            string message;
            bool state;
            try
            {
                dynamic data = JsonConvert.DeserializeObject(param);
                ModelCommParam cmnParam = JsonConvert.DeserializeObject<ModelCommParam>(data[0].ToString());
                result = await _movieService.GetMoviesSearchByLanguage((string)cmnParam.searchValue);
                message = MessageConstants.Exist;
                state = MessageConstants.SuccessState;
            }
            catch (Exception Ex)
            {
                message = Ex.Message;
                state = MessageConstants.ErrorState;
            }
            return new
            {
                result,
                message,
                state
            };
        }
        // GET: api/Movie/GetMoviesByLocation
        [HttpGet("GetMoviesByLocation")]
        public async Task<object> GetMoviesByLocation([FromQuery] string param)
        {
            object result = null;
            string message;
            bool state;
            try
            {
                dynamic data = JsonConvert.DeserializeObject(param);
                ModelCommParam cmnParam = JsonConvert.DeserializeObject<ModelCommParam>(data[0].ToString());
                result = await _movieService.GetMoviesSearchByLocation((string)cmnParam.searchValue);
                message = MessageConstants.Exist;
                state = MessageConstants.SuccessState;
            }
            catch (Exception Ex)
            {
                message = Ex.Message;
                state = MessageConstants.ErrorState;
            }
            return new
            {
                result,
                message,
                state
            };
        }

        // POST: api/Movie/MovieBook
        [HttpPost("MovieBook")]
        public async Task<object> MovieBook([FromQuery] string param)
        {
            object result = null; object resData = null; string message = String.Empty;
            try
            {
                dynamic data = JsonConvert.DeserializeObject(param);
                ModelCommParam cmnParam = JsonConvert.DeserializeObject<ModelCommParam>(data[0].ToString());
                resData = await _movieService.BookedMovie((int)cmnParam.userCode, (int)cmnParam.imdbCode);

            }
            catch (Exception Ex)
            {
                message = Ex.Message;
            }
            return result = new
            {
                resData,
                message
            };
        }
        

        #endregion

    }
}