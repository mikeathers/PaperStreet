using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Identity;
using PaperStreet.Authentication.Application.Interfaces;
using PaperStreet.Authentication.Domain.Models;
using PaperStreet.Domain.Core.Bus;
using PaperStreet.Domain.Core.Events.Errors;
using PaperStreet.Domain.Core.Formatters;
using PaperStreet.Domain.Core.Models;

namespace PaperStreet.Authentication.Application.Services
{
    public class FailedIdentityResult : IFailedIdentityResult
    {
        private readonly IEventBus _eventBus;

        public FailedIdentityResult(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void Handle(AppUser user, IEnumerable<IdentityError> errors, string exceptionMessage)
        {
            var identityErrors = errors as IdentityError[] ?? errors.ToArray();
            
            foreach (var error in identityErrors)
            {
                var errorMessage = ErrorFormatter.FormatIdentityError(error);
                _eventBus.Publish(new ErrorLogEvent(user.Id, errorMessage));
            }
            
            throw new RestException(HttpStatusCode.InternalServerError, identityErrors);
        }
    }
}