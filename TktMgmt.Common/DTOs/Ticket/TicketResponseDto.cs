using System;
using System.Collections.Generic;
using System.Text;

namespace TktMgmt.Common.DTOs.Ticket
{
    public class TicketResponseDto
    {
        public string TicketGuid { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
