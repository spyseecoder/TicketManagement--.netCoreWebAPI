using System;
using System.Collections.Generic;
using System.Text;
using TktMgmt.Common.Constants;
using TktMgmt.Common.DTOs.Ticket;
using TktMgmt.Service.Abstractions;
using TktMgmt.Store.Abstractions;

namespace TktMgmt.Service.Implementations
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _repo;
        //private readonly ITicketRepository _repository;

        public TicketService(ITicketRepository repo)
        {
            _repo = repo;
        }

        public async Task<TicketResponseDto> CreateTicket(TicketCreateDto dto, int userId)
        {
            try
            {
                var result = await _repo.CreateTicket(dto, userId);

                return new TicketResponseDto
                {
                    TicketGuid = result.TicketGuid,
                    Message = result.Message
                };
            }
            catch
            {
                return new TicketResponseDto
                {
                    Message = AppConstants.Error
                };
            }
        }

        public async Task<List<TicketListDto>> GetTickets(
            int pageNumber,
            int pageSize,
            int? statusId,
            int? priorityId)
        {
            try
            {
                return await _repo.GetTickets(pageNumber, pageSize, statusId, priorityId);
            }
            catch
            {
                return new List<TicketListDto>();
            }
        }
        public async Task<(int Result, string Message)> UpdateTicket(TicketUpdateDto dto,int userId)
        {
            try
            {
                return await _repo.UpdateTicket(dto, userId);
            }
            catch
            {
                return (-1, AppConstants.Error);
            }
        }

        public async Task<TicketSummaryDto> GetTicketSummary()
        {
            try
            {
                return await _repo.GetTicketSummary();
            }
            catch
            {
                return new TicketSummaryDto();
            }
        }

        public async Task<string> DeleteTicket(string ticketGuid)
        {
            try
            {
                return await _repo.DeleteTicket(ticketGuid);
            }
            catch
            {
                return AppConstants.Error;
            }
        }
        public async Task<TicketDetailsDto?> GetTicketByGuid(string ticketGuid)
        {
            try
            {
                return await _repo.GetTicketByGuid(ticketGuid);
            }
            catch
            {
                return null;
            }
        }
    }
}
