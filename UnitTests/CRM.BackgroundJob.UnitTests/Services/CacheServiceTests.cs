namespace CRM.BackgroundJob.UnitTests.Services
{
    using System.Threading.Tasks;
    using AutoFixture;
    using Fanex.CRM.BackgroundJob.Repositories;
    using Fanex.CRM.BackgroundJob.Services;
    using Newtonsoft.Json;
    using NSubstitute;
    using StackExchange.Redis;
    using Xunit;

    public class CacheServiceTests
    {
        private IRedisRepository redisRepository;

        [Fact]
        public async Task GetAsync_ValueIsNull_ReturnDefaultValue()
        {
            // Arrange
            var fixture = new Fixture();
            var key = fixture.Create<string>();
            var value = new RedisValue(null);
            var service = GetService();
            redisRepository.GetAsync(key).Returns(value);

            // Act
            var actual = await service.GetAsync<bool>(key);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public async Task GetAsync_DeserializeUnsuccessfully_ReturnDefaultValue()
        {
            // Arrange
            var fixture = new Fixture();
            var key = fixture.Create<string>();
            var value = fixture.Create<RedisValue>();
            var service = GetService();
            redisRepository.GetAsync(key).Returns(value);

            // Act
            var actual = await service.GetAsync<int>(key);

            // Assert
            Assert.Equal(0, actual);
        }

        [Fact]
        public async Task GetAsync_DeserializeSuccessfully_ReturnExpectedValue()
        {
            // Arrange
            var fixture = new Fixture();
            var key = fixture.Create<string>();
            var value = new RedisValue("1");
            var service = GetService();
            redisRepository.GetAsync(key).Returns(value);

            // Act
            var actual = await service.GetAsync<int>(key);

            // Assert
            Assert.Equal(1, actual);
        }

        [Fact]
        public async Task SetAsync_WhenCalled_ExpectedCallSetAsync()
        {
            // Arrange
            var fixture = new Fixture();
            var key = fixture.Create<string>();
            var value = fixture.Create<int>();
            var service = GetService();

            // Act
            await service.SetAsync(key, value);

            // Assert
            await redisRepository.Received().SetAsync(key, JsonConvert.SerializeObject(value));
        }

        #region Helpers

        private CacheService GetService()
        {
            redisRepository = Substitute.For<IRedisRepository>();
            return new CacheService(redisRepository);
        }

        #endregion Helpers
    }
}