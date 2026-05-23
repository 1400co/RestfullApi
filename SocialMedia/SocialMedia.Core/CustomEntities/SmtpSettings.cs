namespace SocialMedia.Core.CustomEntities
{
    public class SmtpSettings
    {
        public string Server { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool UseSSL { get; set; }
        public string Url { get; set; } = string.Empty;
    }
}
