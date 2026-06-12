using System;
using System.Collections.Generic;
using System.Text;
using TktMgmt.Common.DTOs.Ticket;

namespace TktMgmt.Service.Abstractions
{
    public interface ITicketService
    {
        Task<TicketResponseDto> CreateTicket(TicketCreateDto dto, int userId);
        Task<List<TicketListDto>> GetTickets(int pageNumber,int pageSize,int? statusId,int? priorityId);
        Task<(int Result, string Message)> UpdateTicket(TicketUpdateDto dto,int userId);
        Task<TicketSummaryDto> GetTicketSummary();
        Task<string> DeleteTicket(string ticketGuid);
        Task<TicketDetailsDto?> GetTicketByGuid(string ticketGuid);
    }

}
