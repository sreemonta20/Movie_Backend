using System;
using System.Collections.Generic;
using System.Text;

namespace Movie.BackEnd.Common.Utilities
{
    public class StoredProcedureDeposit
    {
        // Security Services (User)
        public const string SpGet_SecurityUsersWithPagination = "[dbo].[Get_SecurityUsersWithPagination]";
        public const string SpGet_AuthenticateSecurityUser = "[dbo].[Get_AuthenticateSecurityUser]";
        public const string SpGet_SecurityUserByID = "[dbo].[Get_SecurityUserByID]";
        public const string SpSet_SecurityUser = "[dbo].[Set_SecurityUser]";
        public const string SpUpdate_SecurityUser = "[dbo].[Update_SecurityUser]";
        public const string SpDel_SecurityUser = "[dbo].[Del_SecurityUser]";

        // Security Services (Menu )
        public const string SpGet_RoleUserMenu  = "[dbo].[Get_RoleUserMenu]";
        public const string SpGet_GetAllModules = "[dbo].[Get_AllModulesByID]";

        // Security Services (Team)
        public const string SpGet_LSTeamsWithPagination = "[dbo].[Get_LSTeamsWithPagination]";
        public const string SpGet_LSTeamByID = "[dbo].[Get_LSTeamByID]";
        public const string SpSet_LSTeam = "[dbo].[Set_LSTeam]";
        public const string SpUpdate_LSTeam = "[dbo].[Update_LSTeam]";
        public const string SpDel_LSTeam = "[dbo].[Del_LSTeam]";
        public const string SpGet_uspCodeFile = "[dbo].[uspCodeFile]";

        // ---------------------------- Added by Sree for MFR starts---------------------------------------
        // Production Formula Services (MFRMaster)
        public const string SpGet_WithPagination = "[dbo].[Get_AllFormulaDetailsWithPaging]";
        public const string SpGet_WithPaginationSearch = "[dbo].[Get_AllFormulaDetailsWithPagingSearch]";
        public const string SpGet_ExtnWithPagination = "[dbo].[Get_ExtnAllFormulaDetailsWithPagingSearch]";
        // ---------------------------- Added by Sree for MFR ends---------------------------------------
        public const string SpGet_BatchIssueWithPaginationSearch = "[dbo].[Get_AllBatchIssueWithPagingSearch]";
        public const string SpGet_AllBatchItemsByBatchCode = "[dbo].[Get_AllBatchItemsByBatchCode]";
        public const string SpGet_AllBatchItemsByBatchCodeWithPaging = "[dbo].[Get_AllBatchItemsByBatchCodeWithPaging]";
        public const string SpGet_BatchMasterAndDetailsByCode = "[dbo].[Get_BatchMasterAndDetailsByBatchCode]";
        // ---------------------------- Added by Sree for Batch Issue ends---------------------------------------
        // Production Formulation Order

        public const string SpGet_FrmlOrderWithPagingSearch = "[dbo].[Get_AllFrmlOrderWithPagingSearch]";

        //Production Plan
        public const string SpGet_GetAllPlansWithSearch = "[dbo].[Get_AllPdmPlanWithPagingSearch]";

        //UI Configulation
        public const string SpGet_UIConfiguration = "[dbo].[uspGetUIConfiguration]";

    }
}
