namespace CRM.Letstalk.Client.Configurations
{
    public class BotConfig
    {
        public string ApiEndpoint { get; set; }

        public string ApiKey { get; set; }

        public string SecureKey { get; set; }

        public string ChatKey { get; set; }

        public int BotAccountId { get; set; }

        public string BotUserId { get; set; }

        public int ChatKeyVersion { get; set; }

        public int BusinessAccountType { get; set; }

        public int EncryptionMode { get; set; }

        public string ProxyAddress { get; set; }

        public bool ByPassProxyLocal { get; set; }

        public bool IsUseProxy { get; set; }
    }
}