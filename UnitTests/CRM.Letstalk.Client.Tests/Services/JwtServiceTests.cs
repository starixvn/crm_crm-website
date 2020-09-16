namespace CRM.Letstalk.Client.Tests.Services
{
    using System.Collections.Generic;
    using CRM.Letstalk.Client.Services;
    using Jose;
    using Newtonsoft.Json;
    using Xunit;

    public class JwtServiceTests
    {
        [Fact]
        public void Decode_WhenValidSecrectKey_ReturnExpectedPayload()
        {
            var token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.DxScZwXTZcb8X7Kn9uuW4dgSALCScwN3YaomzI_FLJb7bDmWwGmhV_zygtA9e__BWvKOyCbq35AgBiTLniShyA";
            var secretkey = "abcd";
            var payload = new
            {
                sub = "1234567890",
                name = "John Doe",
                admin = true,
                iat = 1516239022
            };

            var expectedPayload = JsonConvert.SerializeObject(payload);

            var service = new JwtService();

            var actual = service.Decode(secretkey, token);

            Assert.Equal(expectedPayload, actual);
        }

        [Fact]
        public void Decode_WhenInvalidSecrectKey_ReturnExpectedPayload()
        {
            var token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.DxScZwXTZcb8X7Kn9uuW4dgSALCScwN3YaomzI_FLJb7bDmWwGmhV_zygtA9e__BWvKOyCbq35AgBiTLniShyA";
            var secretkey = "1234aa";

            var service = new JwtService();

            Assert.Throws<IntegrityException>(() => service.Decode(secretkey, token));
        }

        [Fact]
        public void Encode_WhenValidSecrectKey_ReturnExpectedPayload()
        {
            var expectedPayload = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.DxScZwXTZcb8X7Kn9uuW4dgSALCScwN3YaomzI_FLJb7bDmWwGmhV_zygtA9e__BWvKOyCbq35AgBiTLniShyA";
            var secretkey = "abcd";
            IDictionary<string, object> input = new Dictionary<string, object>
            {
                {"sub", "1234567890" },
                { "name", "John Doe" },
                { "admin", true },
                { "iat", 1516239022 }
            };

            var service = new JwtService();

            var actual = service.Encode(secretkey, input);

            Assert.Equal(expectedPayload, actual);
        }

        [Fact]
        public void Encode_WhenInvalidSecrectKey_ReturnWrongPayload()
        {
            var token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.DxScZwXTZcb8X7Kn9uuW4dgSALCScwN3YaomzI_FLJb7bDmWwGmhV_zygtA9e__BWvKOyCbq35AgBiTLniShyA";
            var secretkey = "1234aa";

            IDictionary<string, object> input = new Dictionary<string, object>
            {
                {"sub", "1234567890" },
                { "name", "John Doe" },
                { "admin", true },
                { "iat", 1516239022 }
            };

            var service = new JwtService();

            var actual = service.Encode(secretkey, input);

            Assert.NotEqual(token, actual);
        }
    }
}