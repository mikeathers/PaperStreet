using System;
using FluentValidation;

namespace PaperStreet.Reservations.Application.Validators
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilder<T, DateTime> WithinOpeningHours<T>(this IRuleBuilder<T, DateTime> ruleBuilder)
        {
            var options = ruleBuilder
                .NotEmpty()
                .NotNull()
                .Must(BeWithinOpeningHours);

            return options;
        }

        private static bool BeWithinOpeningHours(DateTime dateTime)
        {
            var reservationTime = dateTime.TimeOfDay;
            var openingHour = new TimeSpan(12, 0, 0);
            var closingHour = new TimeSpan(21, 0, 0);

            return (reservationTime > openingHour) && (reservationTime < closingHour);
        }
    }
}