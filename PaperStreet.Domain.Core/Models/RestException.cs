using System;
using System.Collections;
using System.Net;

namespace PaperStreet.Domain.Core.Models
{
    public class RestException : Exception
    {
        public RestException(HttpStatusCode errorCode, IEnumerable errors = null)
        {
            ErrorCode = errorCode;
            Errors = errors;
        }

        public HttpStatusCode ErrorCode { get; }
        public object Errors { get; }
    }
}