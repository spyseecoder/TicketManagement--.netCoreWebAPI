using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TktMgmt.Common.DTOs.Ticket
{
    public class TicketUpdateDto
    {
        [Required]
        public string TicketGuid { get; set; } = string.Empty;
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        [Range(1, 3, ErrorMessage = "PriorityId must be 1, 2, or 3")]
        public int PriorityId { get; set; }
        [Required]
        [Range(1, 3, ErrorMessage = "StatusId must be 1, 2, or 3")]
        public int StatusId { get; set; }
        [Required]
        public int? AssignedToUserId { get; set; }
    }
}
