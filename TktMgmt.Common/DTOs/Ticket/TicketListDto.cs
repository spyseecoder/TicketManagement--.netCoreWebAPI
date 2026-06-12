using System;
using System.Collections.Generic;
using System.Text;

namespace TktMgmt.Common.DTOs.Ticket
{
    public class TicketListDto
    {
        public string TicketGuid { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string StatusName { get; set; } = string.Empty;
        public string PriorityName { get; set; } = string.Empty;
        public string TypeName { get; set; } = string.Empty;
        public int TotalCount { get; set; }
    }
}
