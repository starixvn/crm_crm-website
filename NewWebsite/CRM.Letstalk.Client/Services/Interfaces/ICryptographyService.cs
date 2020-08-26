namespace CRM.Letstalk.Client.Services.Interfaces
{
    public interface ICryptographyService
    {
        string Decrypt(string encryptedMessage, string key, string salt);

        string Encrypt(string message, string key, string salt);

        string Hash(string message);
    }
}