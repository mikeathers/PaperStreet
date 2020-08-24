using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace PaperStreet.Domain.Core.Formatters
{
    public static class ErrorFormatter
    {
        public static string FormatIdentityError(IdentityError error)
        {
            return $"Error Code: {error.Code}, Error Message: {error.Description}.";
        }
    }
}