using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TktMgmt.Common.DTOs.Ticket;
using TktMgmt.Service.Abstractions;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Logging;

namespace TktMgmt.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _service;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TicketController> _logger;

        public TicketController(
            ITicketService service,
            IConfiguration configuration,
            ILogger<TicketController> logger)
        {
            _service = service;
            _configuration = configuration;
            _logger = logger;
        }

        ///<summary>
        /// Create a new ticket. The user must be authenticated to create a ticket.
        ///</summary>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateTicket_Async([FromBody] TicketCreateDto dto)
        {
            int userId = int.Parse(
                User.FindFirst("UserId")?.Value!
            );

            var result = await _service.CreateTicket(dto, userId);

            return Ok(result);
        }

        ///<summary>
        /// Get ticket list with pagination and optional filtering by status and priority. Aceessible only to admin.
        ///</summary>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetTickets_async(
            int pageNumber = 1,
            int pageSize = 10,
            int? statusId = null,
            int? priorityId = null)
        {
            var result = await _service.GetTickets(
                pageNumber,
                pageSize,
                statusId,
                priorityId
            );

            return Ok(result);
        }

        ///<summary>
        /// Update an existing ticket. The user must be authenticated to update a ticket. 
        ///</summary>
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateTicket_async([FromBody] TicketUpdateDto dto)
        {
            int userId = int.Parse(
                User.FindFirst("UserId")?.Value!
            );

            var result = await _service.UpdateTicket(dto, userId);

            return Ok(result);
        }


        ///<summary>
        /// for Bulk ticket insertion. The user must be authenticated and must be Admin.
        ///</summary>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> BulkInsert_async(
        [FromBody] List<TicketBulkCreateDto> tickets)
        {
            int userId = int.Parse(
                User.FindFirst("UserId")!.Value
            );

            // Create DataTable for UDT
            DataTable table = new DataTable();
            table.Columns.Add("Title", typeof(string));
            table.Columns.Add("Description", typeof(string));
            table.Columns.Add("PriorityId", typeof(int));
            table.Columns.Add("TypeId", typeof(int));
            table.Columns.Add("CreatedByUserId", typeof(int));

            foreach (var t in tickets)
            {
                table.Rows.Add(
                    t.Title,
                    t.Description,
                    t.PriorityId,
                    t.TypeId,
                    userId
                );
            }

            using SqlConnection con = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")
            );

            using SqlCommand cmd = new SqlCommand("usp_BulkInsertTicket", con);
            cmd.CommandType = CommandType.StoredProcedure;

            var param = cmd.Parameters.AddWithValue("@TicketData", table);
            param.SqlDbType = SqlDbType.Structured;
            param.TypeName = "TicketBulkType";

            try
            {
                _logger.LogInformation("Bulk insert started");

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                _logger.LogInformation("Bulk insert completed successfully");

                return Ok("Bulk insert successful");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while bulk inserting tickets");

                return StatusCode(500, "Something went wrong");
            }
        }


        ///<summary>
        /// to get ticket summary for dashboard. The user must be authenticated and Must be Admin.
        ///</summary>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetSummary_Async()
        {
            var result = await _service.GetTicketSummary();
            return Ok(result);
        }


        ///<summary>
        /// Delete the ticket by ticket guid. The user must be authenticated and Must be Admin.
        ///</summary>
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteTicket_async(string ticketGuid)
        {
            var result = await _service.DeleteTicket(ticketGuid);
            return Ok(result);
        }


        ///<summary>
        /// Get ticket details by ticket guid. The user must be authenticated to access the ticket details.
        ///</summary>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetTicketByGuid_async(string ticketGuid)
        {
            var result = await _service.GetTicketByGuid(ticketGuid);

            if (result == null)
                return NotFound("Ticket not found");

            return Ok(result);
        }
    }
}
