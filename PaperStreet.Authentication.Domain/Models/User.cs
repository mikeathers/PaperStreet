namespace PaperStreet.Authentication.Domain.Models
{
    public class User
    {
        public string DisplayName { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
    }
}