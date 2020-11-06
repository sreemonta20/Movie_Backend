using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Movie.BackEnd.Common.Utilities
{
    public static class QueryDeposit
    {
        public static string GetQueryForID(int queryNo, Hashtable oHt)
        {
            string vResult = string.Empty;
            switch (queryNo)
            {
                case 0:
                    vResult = String.Format(@"Select Top 1 " + Convert.ToString(oHt["ColumnName"]) + 
                                            " From " + Convert.ToString(oHt["TableName"]) + 
                                            " Where " + Convert.ToString(oHt["ColumnName"]) + 
                                            " Like '" + Convert.ToString(oHt["Value"]) + "%' " +
                                            "Order By " + Convert.ToString(oHt["ColumnName"]) + " Desc");
                    break;

                case 1:
                    vResult = String.Format(@"Select Top 1 {0}
                                            From {1}
                                            Where {2} Like '{3}%'
                                            Order By {4} Desc", Convert.ToString(oHt["ColumnName"]),
                                                                Convert.ToString(oHt["TableName"]), 
                                                                Convert.ToString(oHt["ColumnName"]), 
                                                                Convert.ToString(oHt["Value"]),
                                                                Convert.ToString(oHt["ColumnName"]));
                    break;
            }
            
            return vResult;
        }
    }
}
