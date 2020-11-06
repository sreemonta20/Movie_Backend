using System;

namespace Movie.BackEnd.Common.Core
{

    [Flags]
    public enum ModelState
    {
        Added = 1,
        Modified = 2,
        Deleted = 3,
        Unchanged = 4,
        Detached = 5
    }

    //public const string Unchange = "Unchange";
    //public const string Save = "Save";
    //public const string Update = "Update";
    //public const string Delete = "Delete";
}
