using System;
using FluentValidation;
using MediatR;
using PaperStreet.Reservations.Application.Validators;

namespace PaperStreet.Reservations.Application.Events
{
    public class CreateReservationEvent : IRequest<bool>
    {
        public DateTime DateTime { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public int NumberInParty { get; set; }
    }

    public class CreateReservationCommandValidator : AbstractValidator<CreateReservationEvent>
    {
        public CreateReservationCommandValidator()
        {
            RuleFor(x => x.DateTime).WithinOpeningHours();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.NumberInParty).NotEqual(0);
        }
    }
}