using System.Net;

namespace PaperStreet.Domain.Core.Models
{
    public class RestResponse 
    {
        public RestResponse(HttpStatusCode code, object body = null)
        {
            Code = code;
            Body = body;
        }
        public HttpStatusCode Code { get; set; }
        public object Body { get; }
    }
}