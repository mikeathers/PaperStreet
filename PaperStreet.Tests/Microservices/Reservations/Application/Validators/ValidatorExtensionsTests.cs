using System;
using System.Collections.Generic;
using FluentValidation;
using PaperStreet.Reservations.Application.Validators;
using Xunit;

namespace PaperStreet.Tests.Microservices.Reservations.Application.Validators
{
    public class ValidatorExtensionsTests
    {
        private class TestValidator : AbstractValidator<TestValidations>
        {
            public TestValidator()
            {
                RuleFor(x => x.DateTime).WithinOpeningHours();
            }
        }
        private class TestValidations
        {
            public DateTime DateTime { get; set; }
        }

        public static IEnumerable<object[]> DateTimeData => new List<object[]>
        {
            new object[] {new DateTime(2020, 7, 15, 19, 00, 0), true},
            new object[] {new DateTime(2020, 7, 15, 12, 01, 0), true},
            new object[] {new DateTime(2020, 7, 15, 20, 59, 0), true},
            new object[] {new DateTime(2020, 7, 15, 11, 59, 0), false},
            new object[] {new DateTime(2020, 7, 15, 07, 00, 0), false},
            new object[] {new DateTime(2020, 7, 15, 21, 01, 0), false},
        };
        
        [Theory]
        [MemberData(nameof(DateTimeData))]
        public void GivenValidDateTimeExtensionMethod_WhenDateTimeProvided_ThenShouldReturnExpectedResult(
            DateTime reservationDate, bool expectedResult)
        {
            var sut = new TestValidations
            {
                DateTime = reservationDate
            };
            
            var validator = new TestValidator();
            var result = validator.Validate(sut);
            
            Assert.Equal(result.IsValid, expectedResult);

        }
    }
}