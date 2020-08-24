using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using PaperStreet.Domain.Core.Formatters;
using Xunit;

namespace PaperStreet.Tests.Domain.Formatters
{
    public class ErrorFormatterTests
    {
        [Fact]
        public void GivenErrorFormatter_WhenReceivesCollectionOfIdentityErrors_ThenShouldReturnFormattedString()
        {
            var error = new IdentityError
            {
                Code = "1",
                Description = "First Error"
            };

            var formattedString = ErrorFormatter.FormatIdentityError(error);
            const string expectedString = "Error Code: 1, Error Message: First Error.";
            
            Assert.Equal(formattedString, expectedString);
        }
    }
}