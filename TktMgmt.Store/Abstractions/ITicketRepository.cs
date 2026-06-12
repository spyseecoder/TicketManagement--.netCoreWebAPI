using System;
using System.Collections.Generic;
using System.Text;
using TktMgmt.Common.DTOs.Ticket;


namespace TktMgmt.Store.Abstractions
{
    public interface ITicketRepository
    {
        Task<(string TicketGuid, string Message)> CreateTicket(
            TicketCreateDto dto,
            int userId
        );
        Task<List<TicketListDto>> GetTickets(
            int pageNumber,
            int pageSize,
            int? statusId,
            int? priorityId
        );
        Task<(int Result, string Message)> UpdateTicket(
            TicketUpdateDto dto,
            int userId
        );
        Task<TicketSummaryDto> GetTicketSummary();

        Task<string> DeleteTicket(string ticketGuid);

        Task<TicketDetailsDto?> GetTicketByGuid(string ticketGuid);
    }


}
