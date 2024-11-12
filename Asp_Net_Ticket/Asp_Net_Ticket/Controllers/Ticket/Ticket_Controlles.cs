using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asp_Net_Ticket.Service.Ticket;
using Asp_Net_Ticket.Dto.Ticket;
using Microsoft.Extensions.Logging;
using Asp_Net_Ticket.Entity;
using FluentValidation;
using AutoMapper;

namespace Asp_Net_Ticket.Controllers.Ticket
{
    [Route("api/Ticket")]
    [ApiController]
    public class Ticket_Controllers : ControllerBase
    {
        private readonly Ticket_Services _ticketService;
        private readonly ILogger<Ticket_Controllers> _logger;
        private readonly IValidator<Ticket_DTO> _createTicketValidator;
        private readonly IValidator<Ticket_DTO> _updateTicketValidator;
        private readonly IMapper _mapper;

        public Ticket_Controllers(Ticket_Services ticketService, ILogger<Ticket_Controllers> logger,
                                  IValidator<Ticket_DTO> createTicketValidator,
                                  IValidator<Ticket_DTO> updateTicketValidator,
                                  IMapper mapper)
        {
            _ticketService = ticketService;
            _logger = logger;
            _createTicketValidator = createTicketValidator;
            _updateTicketValidator = updateTicketValidator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TickeDb>>> GetAllTickets()
        {
            try
            {
                var tickets = await _ticketService.GetAllTickets();
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving tickets: {ex.Message}");
                return BadRequest(new { ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TickeDb>> GetTicketById(int id)
        {
            try
            {
                var ticket = await _ticketService.GetTicketById(id);
                if (ticket != null)
                {
                    return Ok(ticket);
                }
                return NotFound("Ticket not found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving ticket by ID: {ex.Message}");
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateTicket(Ticket_DTO ticket)
        {
            var validationResult = await _createTicketValidator.ValidateAsync(ticket);
            if (!validationResult.IsValid)
            {
                // Extract and return all error messages as a single string
                var errorMessages = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(errorMessages);
            }

            try
            {
                var success = await _ticketService.CreateTicket(ticket);
                if (success)
                {
                    return Ok(new { Message = "Ticket created successfully" });
                }
                return BadRequest("Error creating ticket");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating ticket: {ex.Message}");
                return BadRequest(new { ex.Message });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTicket(int id, Ticket_DTO ticket)
        {
            var validationResult = await _updateTicketValidator.ValidateAsync(ticket);
            if (!validationResult.IsValid)
            {
                var errorMessage = validationResult.Errors.First().ErrorMessage;
                return BadRequest(errorMessage);

            }

            try
            {
                var success = await _ticketService.UpdateTicket(id, ticket);
                if (success)
                {
                    return Ok(new { Message = "Ticket updated successfully" });
                }
                return NotFound("Ticket not found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating ticket: {ex.Message}");
                return BadRequest(new { ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            try
            {
                var success = await _ticketService.DeleteTicket(id);
                if (success)
                {
                    return Ok(new { Message = "Ticket deleted successfully" });
                }
                return NotFound("Ticket not found");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting ticket: {ex.Message}");
                return BadRequest(new { ex.Message });
            }
        }
    }
}
