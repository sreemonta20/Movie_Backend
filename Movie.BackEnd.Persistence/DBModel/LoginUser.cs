using System;
using System.Collections.Generic;

namespace Movie.BackEnd.Persistence.DBModel
{
    public partial class LoginUser
    {
        public int UserCode { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
    }
}
