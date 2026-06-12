using System;
using System.Collections.Generic;
using System.Text;

namespace TktMgmt.Common.DTOs.Ticket
{
    public class TicketSummaryDto
    {
        public int TotalActiveTickets { get; set; }
        public List<PriorityCountDto> PriorityBreakdown { get; set; } = new();
    }

    public class PriorityCountDto
    {
        public string Priority { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
