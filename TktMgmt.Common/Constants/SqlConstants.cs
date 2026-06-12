using System;
using System.Collections.Generic;
using System.Text;

namespace TktMgmt.Common.Constants
{
    public static class SqlConstants
    {
        // Auth
        public const string Login = "usp_Login";

        // User
        public const string CreateUser = "usp_CreateUser";
        public const string UpdateUserRole = "usp_UpdateUserRole";
        public const string GetUserDetails = "usp_GetUserDetails";
        public const string GetUserByGuid = "usp_GetUserByGuid";
        public const string DeleteUser = "usp_DeleteUser";

        // Ticket
        public const string CreateTicket = "usp_CreateTicket";
        public const string UpdateTicket = "usp_UpdateTicket";
        public const string GetTickets = "usp_GetTickets";
        public const string GetTicketByGuid = "usp_GetTicketByGuid";
        public const string DeleteTicket = "usp_DeleteTicket";
        public const string GetTicketSummary = "usp_GetTicketSummary";
        public const string BulkInsertTicket = "usp_BulkInsertTicket";

        // Logging
        public const string LogError = "usp_LogError";
    }
}
