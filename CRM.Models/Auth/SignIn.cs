namespace CRM.Models.Auth
{
    public class SignIn
    {
        public string OnUserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}