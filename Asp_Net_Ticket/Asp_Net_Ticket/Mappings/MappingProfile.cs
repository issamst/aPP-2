using AutoMapper;
using Asp_Net_Ticket.Entity;
using Asp_Net_Ticket.Dto.Ticket;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Asp_Net_Ticket.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TickeDb, Ticket_DTO>()
                .ReverseMap(); 
        }
    }
}
