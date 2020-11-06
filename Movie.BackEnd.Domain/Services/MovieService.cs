using AutoMapper;
using Movie.BackEnd.Common.Utilities;
using Movie.BackEnd.Common.Utilities.Interfaces;
using Movie.BackEnd.Domain.Contract;
using Movie.BackEnd.Domain.DTO;
using Movie.BackEnd.Persistence.DBContext;
using Movie.BackEnd.Persistence.DBModel;
using Movie.BackEnd.Persistence.DBManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Movie.BackEnd.Domain.Services
{
    public class MovieService : IMovieRepository, IMessageResposeState
    {

        #region Variable Declatation & Initialization
        private MovieDBContext _movieContext = null;
        private Hashtable oHashTable = null;
        private IDBManager<dtoMovieDetails> _movieDetSqlMgr = null;
        private IDBManager<dtoMovieBooking> _movieBookSqlMgr = null;
        private IConfiguration _iconfig;
        private AppSettings appsettings = null;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _inMemoryCache;
        #endregion

        #region Constructor Initialization
        public MovieService(MovieDBContext movieContext, IDBManager<dtoMovieDetails> movieDetSqlMgr, IDBManager<dtoMovieBooking> movieBookSqlMgr, IConfiguration iconfig,
        IMapper mapper, IMemoryCache inMemoryCache)
        {
            this._movieContext = movieContext;
            this._movieDetSqlMgr = movieDetSqlMgr;
            this._movieBookSqlMgr = movieBookSqlMgr;
            this._iconfig = iconfig;
            this.appsettings = new AppSettings(_iconfig);
            this._mapper = mapper;
            this._inMemoryCache = inMemoryCache;
        }

        #endregion

        #region All Methods

        public async Task<object> GetMovieList()
        {
            List<dtoMovieDetails> movieList = null;
            object result = null;
            bool state = false;
            string message = String.Empty;

            try
            {
                using (_movieContext = new MovieDBContext())
                {
                    movieList = _mapper.Map<List<dtoMovieDetails>>(await _movieContext.MovieDetails.ToListAsync());

                    if (movieList.IsNotNull())
                    {
                        message = MessageConstants.Exist;
                        state = MessageConstants.SuccessState;
                    }
                    else
                    {
                        message = MessageConstants.NotExist;
                        state = MessageConstants.ErrorState;
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                state = MessageConstants.ErrorState;
            }
            return result = new
            {
                movieList,
                message,
                state
            };
        }

        public async Task<object> GetLanguageList()
        {
            List<dtoMovieDetails> languageList = null;
            object result = null;
            bool state = false;
            string message = String.Empty;

            try
            {
                using (_movieContext = new MovieDBContext())
                {
                    languageList = _mapper.Map<List<dtoMovieDetails>>(await _movieContext.MovieDetails.OrderByDescending(x=>x.Language).Distinct().ToListAsync());

                    if (languageList.IsNotNull())
                    {
                        message = MessageConstants.Exist;
                        state = MessageConstants.SuccessState;
                    }
                    else
                    {
                        message = MessageConstants.NotExist;
                        state = MessageConstants.ErrorState;
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                state = MessageConstants.ErrorState;
            }
            return result = new
            {
                languageList,
                message,
                state
            };
        }

        public async Task<object> GetLocationList()
        {
            List<dtoMovieDetails> locationList = null;
            object result = null;
            bool state = false;
            string message = String.Empty;

            try
            {
                using (_movieContext = new MovieDBContext())
                {
                    locationList = _mapper.Map<List<dtoMovieDetails>>(await _movieContext.MovieDetails.OrderByDescending(x => x.Language).Distinct().ToListAsync());

                    if (locationList.IsNotNull())
                    {
                        message = MessageConstants.Exist;
                        state = MessageConstants.SuccessState;
                    }
                    else
                    {
                        message = MessageConstants.NotExist;
                        state = MessageConstants.ErrorState;
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                state = MessageConstants.ErrorState;
            }
            return result = new
            {
                locationList,
                message,
                state
            };
        }

        public async Task<object> GetMoviesSearchByTitle(string title)
        {
            List<dtoMovieDetails> movieList = null;
            object result = null;
            bool state = false;
            string message = String.Empty;

            try
            {
                using (_movieContext = new MovieDBContext())
                {
                    movieList = _mapper.Map<List<dtoMovieDetails>>(await _movieContext.MovieDetails.Where(x => x.Title == title).ToListAsync());

                    if (movieList.IsNotNull())
                    {
                        message = MessageConstants.Exist;
                        state = MessageConstants.SuccessState;
                    }
                    else
                    {
                        message = MessageConstants.NotExist;
                        state = MessageConstants.ErrorState;
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                state = MessageConstants.ErrorState;
            }
            return result = new
            {
                movieList,
                message,
                state
            };
        }

        public async Task<object> GetMovieByCode(int code)
        {
            dtoMovieDetails oMovie = null;
            object result = null;
            bool state = false;
            string message = String.Empty;

            try
            {
                using (_movieContext = new MovieDBContext())
                {
                    oMovie = _mapper.Map<dtoMovieDetails>(await _movieContext.MovieDetails.FirstOrDefaultAsync(x => x.ImdbCode == code));
                    if (oMovie.IsNotNull())
                    {
                        message = MessageConstants.Exist;
                        state = MessageConstants.SuccessState;
                    }
                    else
                    {
                        message = MessageConstants.NotExist;
                        state = MessageConstants.ErrorState;
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                state = MessageConstants.ErrorState;
            }
            return result = new
            {
                oMovie,
                message,
                state
            };
        }

        public async Task<object> GetMoviesSearchByLanguage(string language)
        {
            List<dtoMovieDetails> movieList = null;
            object result = null;
            bool state = false;
            string message = String.Empty;

            try
            {
                using (_movieContext = new MovieDBContext())
                {
                    movieList = _mapper.Map<List<dtoMovieDetails>>(await _movieContext.MovieDetails.Where(x => x.Language == language).ToListAsync());

                    if (movieList.IsNotNull())
                    {
                        message = MessageConstants.Exist;
                        state = MessageConstants.SuccessState;
                    }
                    else
                    {
                        message = MessageConstants.NotExist;
                        state = MessageConstants.ErrorState;
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                state = MessageConstants.ErrorState;
            }
            return result = new
            {
                movieList,
                message,
                state
            };
        }

        public async Task<object> GetMoviesSearchByLocation(string location)
        {
            List<dtoMovieDetails> movieList = null;
            object result = null;
            bool state = false;
            string message = String.Empty;

            try
            {
                using (_movieContext = new MovieDBContext())
                {
                    movieList = _mapper.Map<List<dtoMovieDetails>>(await _movieContext.MovieDetails.Where(x => x.Location == location).ToListAsync());

                    if (movieList.IsNotNull())
                    {
                        message = MessageConstants.Exist;
                        state = MessageConstants.SuccessState;
                    }
                    else
                    {
                        message = MessageConstants.NotExist;
                        state = MessageConstants.ErrorState;
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                state = MessageConstants.ErrorState;
            }
            return result = new
            {
                movieList,
                message,
                state
            };
        }

        public async Task<object> BookedMovie(int userCode, int imdbCode)
        {
            object result = null; string message = string.Empty; bool state = false;
            Enum eventoperation = ExtensionMethods.EventOperation.New;
            try
            {
                using (_movieContext = new MovieDBContext())
                {
                    using (var _tran = _movieContext.Database.BeginTransaction())
                    {
                        try
                        {
                            if (imdbCode.MoreThanZero())
                            {
                                var MaxID = _movieContext.MovieBooking.DefaultIfEmpty().Max(x => x == null ? 0 : x.BookCode) + 1;
                                MovieBooking oMovieBooking = new MovieBooking();
                                oMovieBooking.BookCode = MaxID;
                                oMovieBooking.UserCode = userCode;
                                oMovieBooking.ImdbCode = imdbCode;
                                await _movieContext.AddAsync(oMovieBooking);

                            }

                            await _movieContext.SaveChangesAsync();

                            _tran.Commit();
                            message = MessageConstants.Booked;
                            state = MessageConstants.SuccessState;
                        }
                        catch (Exception ex)
                        {
                            _tran.Rollback();
                            message = ex.Message;
                            state = MessageConstants.ErrorState;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                state = MessageConstants.ErrorState;
            }
            return result = new
            {
                message,
                state
            };
        }

        

        public void SetMsgResState(out string message, out bool state, Enum eventoperation)
        {
            message = String.Empty; state = false;

            try
            {
                switch (eventoperation)
                {
                    case ExtensionMethods.EventOperation.New:
                        message = MessageConstants.Saved;
                        state = MessageConstants.SuccessState;
                        break;

                    case ExtensionMethods.EventOperation.Update:
                        message = MessageConstants.Updated;
                        state = MessageConstants.SuccessState;
                        break;
                }
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
        }


        #endregion

    }
}
