using System;
using System.Collections.Generic;
using System.Text;

namespace Movie.BackEnd.Common.Utilities
{
    public static class AppUser
    {
        private static int? appUserCode;
        private static string appUserId;
        private static string appUserName;

        public static int? AppUserCode { get => appUserCode; set => appUserCode = value; }
        public static string AppUserId { get => appUserId; set => appUserId = value; }
        public static string AppUserName { get => appUserName; set => appUserName = value; }
    }
}
