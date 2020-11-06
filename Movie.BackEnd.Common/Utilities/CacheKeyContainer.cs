using System;
using System.Collections.Generic;
using System.Text;

namespace Movie.BackEnd.Common.Utilities
{
    public  class CacheKeyContainer
    {
        public static string SaltHashPass = "SaltHashPass";
        public static string AllUserDataExPaging = "AllUserDataExPaging";
        public static string UserMenu = "UserMenu";
        public static string UserModule = "UserModule";
        public static string AllTeamDataExPaging = "AllTeamDataExPaging";
        public static string AllProductDataExPaging = "AllProductDataExPaging";
        public static string AllFormulaDataExPaging = "AllFormulaDataExPaging";
        public static string AllWorkingUnitDataExPaging = "AllWorkingUnitDataExPaging";
        //---------------added by Sree for batch issue-------------
        public static string AllBatchesExPaging = "AllBatchesExPaging";
        public static string AllFormulaOrdersExPaging = "AllFormulaOrdersExPaging";
        public static string AllProductionPlan = "AllProductionPlan";

        //----------------------------------------------------------
        public static string Uikey(long uiId)
        {
            return "uiconfig-" + uiId.ToString();
        }
    }
}
