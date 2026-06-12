using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TktMgmt.Common;
using TktMgmt.Common.DTOs.Ticket;
using TktMgmt.Store.Abstractions;
using TktMgmt.Common.Constants;


namespace TktMgmt.Store.Implementations
{
    public class TicketRepository : ITicketRepository
    {
        private readonly string _connectionString;

        public TicketRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<(string TicketGuid, string Message)> CreateTicket(
            TicketCreateDto dto, int userId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand(SqlConstants.CreateTicket, con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Title", dto.Title);
                cmd.Parameters.AddWithValue("@Description", dto.Description);
                cmd.Parameters.AddWithValue("@PriorityId", dto.PriorityId);
                cmd.Parameters.AddWithValue("@TypeId", dto.TypeId);
                cmd.Parameters.AddWithValue("@CreatedByUserId", userId);

                await con.OpenAsync();

                using SqlDataReader reader = await cmd.ExecuteReaderAsync();

                string ticketGuid = string.Empty;
                string message = string.Empty;

                if (await reader.ReadAsync())
                {
                    ticketGuid = reader["TicketGuid"]?.ToString() ?? "";
                    message = reader["Message"]?.ToString() ?? "";
                }

                return (ticketGuid, message);
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "CreateTicket");
                throw;
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
                List<TicketListDto> tickets = new();

                using SqlConnection con = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand(SqlConstants.GetTickets, con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);
                cmd.Parameters.AddWithValue("@StatusId", (object?)statusId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PriorityId", (object?)priorityId ?? DBNull.Value);

                await con.OpenAsync();

                using SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    tickets.Add(new TicketListDto
                    {
                        TicketGuid = reader["TicketGuid"].ToString() ?? "",
                        Title = reader["Title"].ToString() ?? "",
                        Description = reader["Description"].ToString() ?? "",
                        StatusName = reader["StatusName"].ToString() ?? "",
                        PriorityName = reader["PriorityName"].ToString() ?? "",
                        TypeName = reader["TypeName"].ToString() ?? "",
                        TotalCount = Convert.ToInt32(reader["TotalCount"])
                    });
                }

                return tickets;
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "GetTickets");
                throw;
            }
        }

        public async Task<(int Result, string Message)> UpdateTicket(TicketUpdateDto dto, int userId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand(SqlConstants.UpdateTicket, con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TicketGuid", dto.TicketGuid);
                cmd.Parameters.AddWithValue("@Title", dto.Title);
                cmd.Parameters.AddWithValue("@Description", dto.Description);
                cmd.Parameters.AddWithValue("@PriorityId", dto.PriorityId);
                cmd.Parameters.AddWithValue("@StatusId", dto.StatusId);
                cmd.Parameters.AddWithValue("@AssignedToUserId",
                    (object?)dto.AssignedToUserId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@UpdatedBy", userId);

                await con.OpenAsync();

                using SqlDataReader reader = await cmd.ExecuteReaderAsync();

                int result = -1;
                string message = string.Empty;

                if (await reader.ReadAsync())
                {
                    result = Convert.ToInt32(reader["Result"]);
                    message = reader["Message"]?.ToString() ?? string.Empty;
                }

                return (result, message);
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "UpdateTicket");
                throw;
            }
        }

        public async Task<TicketSummaryDto> GetTicketSummary()
        {
            try
            {
                var result = new TicketSummaryDto();

                using SqlConnection con = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand(SqlConstants.GetTicketSummary, con);

                cmd.CommandType = CommandType.StoredProcedure;

                await con.OpenAsync();

                using SqlDataReader reader = await cmd.ExecuteReaderAsync();

                // First result → total count
                if (await reader.ReadAsync())
                {
                    result.TotalActiveTickets = Convert.ToInt32(reader["TotalActiveTickets"]);
                }

                // Move to next result set
                await reader.NextResultAsync();

                // Second result → priority breakdown
                while (await reader.ReadAsync())
                {
                    result.PriorityBreakdown.Add(new PriorityCountDto
                    {
                        Priority = reader["PriorityName"].ToString()!,
                        Count = Convert.ToInt32(reader["TicketCount"])
                    });
                }

                return result;
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "GetTicketSummary");
                throw;
            }
        }

        public async Task<string> DeleteTicket(string ticketGuid)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand(SqlConstants.DeleteTicket, con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TicketGuid", ticketGuid);

                await con.OpenAsync();

                var result = await cmd.ExecuteScalarAsync();

                return result?.ToString() ?? "Error deleting ticket";
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "DeleteTicket");
                throw;
            }
        }

        public async Task<TicketDetailsDto?> GetTicketByGuid(string ticketGuid)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand(SqlConstants.GetTicketByGuid, con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TicketGuid", ticketGuid);

                await con.OpenAsync();

                using SqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new TicketDetailsDto
                    {
                        TicketGuid = reader["TicketGuid"].ToString()!,
                        Title = reader["Title"].ToString()!,
                        Description = reader["Description"].ToString()!,
                        StatusName = reader["StatusName"].ToString()!,
                        PriorityName = reader["PriorityName"].ToString()!,
                        TypeName = reader["TypeName"].ToString()!,
                        CreatedOn = Convert.ToDateTime(reader["CreatedOn"])
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "GetTicketByGuid");
                throw;
            }
        }
        private async Task LogErrorAsync(Exception ex, string source)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(SqlConstants.LogError, con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Message", ex.Message);
            cmd.Parameters.AddWithValue("@StackTrace", ex.StackTrace ?? "");
            cmd.Parameters.AddWithValue("@Source", source);

            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

    }
}



