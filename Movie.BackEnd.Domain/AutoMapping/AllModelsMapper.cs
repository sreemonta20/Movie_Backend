using AutoMapper;
using Movie.BackEnd.Domain.DTO;
using Movie.BackEnd.Persistence.DBModel;
using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;

namespace Movie.BackEnd.Domain.AutoMapping
{
    public class AllModelsMapper: Profile
    {
        public AllModelsMapper()
        {
            CreateMap<LoginUser, dtoLoginUser>();
            CreateMap<dtoLoginUser, LoginUser>();
            CreateMap<MovieBooking, dtoMovieBooking>();
            CreateMap<dtoMovieBooking, MovieBooking>();
            CreateMap<MovieDetails, dtoMovieDetails>();
            CreateMap<dtoMovieDetails, MovieDetails>();

        }
    }
}
