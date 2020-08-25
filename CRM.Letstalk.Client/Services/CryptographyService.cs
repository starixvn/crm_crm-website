namespace CRM.Letstalk.Client.Services
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using CRM.Letstalk.Client.Services.Interfaces;

    public class CryptographyService : ICryptographyService
    {
        public string Decrypt(string encryptedMessage, string key, string salt)
        {
            if (string.IsNullOrWhiteSpace(encryptedMessage))
            {
                throw new ArgumentNullException(nameof(encryptedMessage));
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (string.IsNullOrWhiteSpace(salt))
            {
                throw new ArgumentNullException(nameof(salt));
            }

            string plaintext = null;

            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = ConvertHexStringToByte(key);
                rijAlg.IV = ComputeSalt(salt);

                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedMessage)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        var srDecrypt = new StreamReader(csDecrypt);
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }

            return plaintext;
        }

        public string Encrypt(string message, string key, string salt)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (string.IsNullOrWhiteSpace(salt))
            {
                throw new ArgumentNullException(nameof(salt));
            }

            byte[] encrypted;

            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = ConvertHexStringToByte(key);
                rijAlg.IV = ComputeSalt(salt);

                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(message);
                        }

                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encrypted);
        }

        public string Hash(string message)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);
            var sha = new SHA1CryptoServiceProvider();
            var result = sha.ComputeHash(messageBytes);

            return ConvertHexByteToString(result);
        }

        private static string ConvertHexByteToString(byte[] data)
            => BitConverter.ToString(data).Replace("-", "");

        private static byte[] ConvertHexStringToByte(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }

        private static byte[] ComputeSalt(string salt)
        {
            MD5 md5 = MD5.Create();
            byte[] source = Encoding.Default.GetBytes(salt);
            return md5.ComputeHash(source);
        }
    }
}