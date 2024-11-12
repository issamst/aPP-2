using Asp_Net_Ticket.Entity;
using Asp_Net_Ticket.Dto.Ticket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asp_Net_Ticket.Context;
using AutoMapper;

namespace Asp_Net_Ticket.Service.Ticket
{
    public class Ticket_Services
    {
        private readonly AppDbContext _context;
        private readonly ILogger<Ticket_Services> _logger;
        private readonly IMapper _mapper;

        public Ticket_Services(AppDbContext context, ILogger<Ticket_Services> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<TickeDb>> GetAllTickets()
        {
            return await _context.Tickets.ToListAsync();
        }

        public async Task<TickeDb> GetTicketById(int id)
        {
            return await _context.Tickets.FindAsync(id);
        }

        public async Task<bool> CreateTicket(Ticket_DTO ticketDto)
        {
            try
            {
                var ticket = _mapper.Map<TickeDb>(ticketDto); // Map from DTO to Entity
                ticket.DateCreated = DateTime.Now;

                _context.Tickets.Add(ticket);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating ticket: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateTicket(int id, Ticket_DTO ticketDto)
        {
            try
            {
                var existingTicket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);
                if (existingTicket != null)
                {
                    _mapper.Map(ticketDto, existingTicket);
                    existingTicket.DateChanged = DateTime.Now;

                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating ticket: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteTicket(int id)
        {
            try
            {
                var ticket = await _context.Tickets.FindAsync(id);
                if (ticket != null)
                {
                    _context.Tickets.Remove(ticket);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting ticket: {ex.Message}");
                return false;
            }
        }
    }
}
