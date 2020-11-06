using System;
using System.Collections.Generic;

namespace Movie.BackEnd.Domain.DTO
{
    public partial class dtoMovieBooking
    {
        public int BookCode { get; set; }
        public int? UserCode { get; set; }
        public int? ImdbCode { get; set; }
    }
}
