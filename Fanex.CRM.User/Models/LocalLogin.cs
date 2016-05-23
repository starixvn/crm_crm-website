namespace Fanex.CRM.User.Models
{
    public class LocalLogin
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string LoginToken { get; set; }

        public string Environment { get; set; }

        public int SiteId { get; set; }

        public string SessionId { get; set; }
    }
}
