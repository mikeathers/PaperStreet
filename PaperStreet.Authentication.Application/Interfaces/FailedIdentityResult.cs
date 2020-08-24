using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using PaperStreet.Authentication.Domain.Models;

namespace PaperStreet.Authentication.Application.Interfaces
{
    public interface IFailedIdentityResult
    {
        void Handle(AppUser user, IEnumerable<IdentityError> errors, string exceptionMessage);
    }
}