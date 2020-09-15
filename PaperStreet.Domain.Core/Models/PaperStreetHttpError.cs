namespace PaperStreet.Domain.Core.Models
{
    public class PaperStreetHttpError
    {
        public int Code { get; set; }
        public int BadRequest = 400;
        public int Unauthorized = 401;
        public int NotFound = 404;
        public int InternalServerError = 400;
    }

    public class PaperStreetBadRequest : PaperStreetHttpError
    {
        public PaperStreetBadRequest()
        {
            Code = 400;
        }
    }

    public class PaperStreetUnauthorizedRequest : PaperStreetHttpError
    {
        public PaperStreetUnauthorizedRequest()
        {
            Code = 401;
        }
    }
    
    
}