using System;
using System.Collections.Generic;

namespace Movie.BackEnd.Domain.DTO
{
    public partial class dtoLoginUser
    {
        public int UserCode { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string Token { get; set; }

    }
}
