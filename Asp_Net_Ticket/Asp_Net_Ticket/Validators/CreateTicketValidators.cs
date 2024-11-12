using Asp_Net_Ticket.Dto.Ticket;
using FluentValidation;

namespace Asp_Net_Ticket.Validators
{
    public class CreateTicketValidators : AbstractValidator<Ticket_DTO>
    {
        public CreateTicketValidators()
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description cannot be longer than 500 characters.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.")
                .Must(status => status == "Open" || status == "Closed" )
                .WithMessage("Status must be either 'Open', 'Closed'.");
        }
    }
}
