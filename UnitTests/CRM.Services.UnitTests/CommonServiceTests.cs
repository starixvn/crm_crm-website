namespace CRM.Services.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture;
    using CRM.Core.Enumerables;
    using CRM.Core.Extensions;
    using CRM.Core.Models;
    using CRM.Core.Services;
    using CRM.Criteria.Common;
    using CRM.Models;
    using CRM.Models.ChatBot;
    using CRM.Models.Common;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using NSubstitute;
    using Xunit;

    public class CommonServiceTests
    {
        private IBaseService baseService;
        private readonly IFixture fixture = new Fixture();

        [Fact]
        public async Task GetConversationBotAsync_WhenCalled_ReturnsExpectedResult()
        {
            // Arrange
            var service = GetService();
            var expected = fixture.Create<IEnumerable<ConversationBot>>();
            baseService.GetDataAsync<ConversationBot>(Arg.Any<GetConversationBotsCriteria>()).Returns(expected);

            // Action
            var result = await service.GetConversationBotAsync();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetRecipientAsync_WhenCalled_ReturnsExpectedResult()
        {
            // Arrange
            var service = GetService();
            var criteria = fixture.Create<GetRecipientsCriteria>();
            var expected = fixture.Create<IEnumerable<RecipientModel>>();
            baseService.GetDataAsync<RecipientModel>(criteria).Returns(expected);

            // Action
            var result = await service.GetRecipientAsync(criteria);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetRecipientTypeAsync_WhenCalled_ReturnsExpectedResult()
        {
            // Arrange
            var service = GetService();
            var expected = fixture.Create<IEnumerable<BaseModel>>();
            baseService.GetDataAsync<BaseModel>(Arg.Any<GetRecipientTypesCriteria>()).Returns(expected);

            // Action
            var result = await service.GetRecipientTypeAsync();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetSystemAsync_WhenCalled_ReturnsExpectedResult()
        {
            // Arrange
            var service = GetService();
            var expected = fixture.Create<IEnumerable<BaseModel>>();
            baseService.GetDataAsync<BaseModel>(Arg.Any<GetSystemsCriteria>()).Returns(expected);

            // Action
            var result = await service.GetSystemAsync();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetHeadlineAsync_WhenCalled_ReturnsExpectedResult()
        {
            // Arrange
            var service = GetService();
            var criteria = fixture.Create<GetHeadlinesCriteria>();
            var expected = fixture.Create<IEnumerable<BaseModel>>();
            baseService.GetDataAsync<BaseModel>(criteria).Returns(expected);

            // Action
            var result = await service.GetHeadlineAsync(criteria);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetChatTypes_WhenCalled_ReturnsExpectedResult()
        {
            // Arrange
            var service = GetService();
            var expected = Enum.GetValues(typeof(ChatType)).Cast<ChatType>()
                .Select(type => new BaseModel { Id = (int)type, Name = EnumHelper.GetDesciption(type) });

            // Action
            var result = service.GetChatTypes();

            // Assert
            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void GetMappingStatus_WhenCalled_ReturnsExpectedResult()
        {
            // Arrange
            var service = GetService();
            var expected = Enum.GetValues(typeof(MappingStatus)).Cast<MappingStatus>()
                .Select(type => new BaseModel { Id = (int)type, Name = EnumHelper.GetDesciption(type) });

            // Action
            var result = service.GetMappingStatus();

            // Assert
            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(result));
        }

        private CommonService GetService()
        {
            baseService = Substitute.For<IBaseService>();
            var settings = Substitute.For<IOptionsSnapshot<AppSettings>>();
            return new CommonService(baseService, settings);
        }
    }
}