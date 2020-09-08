namespace CRM.Letstalk.Client.Tests.Services
{
    using System;
    using CRM.Letstalk.Client.Services;
    using Xunit;

    public class CryptographyServiceTests
    {
        private readonly CryptographyService cryptographyService;

        public CryptographyServiceTests()
        {
            cryptographyService = new CryptographyService();
        }

        [Fact]
        public void Decrypt_EncryptedMessageIsNullOrEmpty_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => cryptographyService.Decrypt(string.Empty, "abc", "abc"));
        }

        [Fact]
        public void Decrypt_KeyIsNullOrEmpty_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => cryptographyService.Decrypt("encryptedMessage", string.Empty, "salt"));
        }

        [Fact]
        public void Decrypt_SaltIsNullOrEmpty_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => cryptographyService.Decrypt("encryptedMessage", "key", string.Empty));
        }

        [Fact]
        public void Encrypt_MessageIsNullOrEmpty_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => cryptographyService.Encrypt(string.Empty, "key", "salt"));
        }

        [Fact]
        public void Encrypt_KeyIsNullOrEmpty_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => cryptographyService.Encrypt("msg", string.Empty, "salt"));
        }

        [Fact]
        public void Encrypt_SaltIsNullOrEmpty_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => cryptographyService.Encrypt("msg", "key", string.Empty));
        }

        [Fact]
        public void Hash_InputMessage_ReturnExpectedHashString()
        {
            var msg = "This is test message";
            var expected = "6158b73165ae6160704e19ad7d6a8bddd050651e";
            var service = new CryptographyService();
            var actual = service.Hash(msg);

            Assert.Equal(expected.ToUpperInvariant(), actual);
        }
    }
}