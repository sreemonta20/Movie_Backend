using System;
using System.Collections.Generic;

namespace Movie.BackEnd.Persistence.DBModel
{
    public partial class MovieBooking
    {
        public int BookCode { get; set; }
        public int? UserCode { get; set; }
        public int? ImdbCode { get; set; }
    }
}
