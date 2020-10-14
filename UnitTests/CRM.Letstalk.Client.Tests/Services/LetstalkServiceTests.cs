namespace CRM.Letstalk.Client.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CRM.Letstalk.Client.Configurations;
    using CRM.Letstalk.Client.Models;
    using CRM.Letstalk.Client.Models.Results;
    using CRM.Letstalk.Client.Services;
    using CRM.Letstalk.Client.Services.Interfaces;
    using Newtonsoft.Json;
    using NSubstitute;
    using Xunit;

    public class LetstalkServiceTests
    {
        private readonly LetstalkService letstalkService;
        private readonly BotConfig botConfig;
        private readonly ICryptographyService cryptographyService;
        private readonly IRestClientService restClientService;
        private readonly IJwtService jwtService;

        public LetstalkServiceTests()
        {
            botConfig = new BotConfig
            {
                ApiEndpoint = "http://test.com",
                ApiKey = "Key",
                BotAccountId = 1000,
                BotUserId = "@test",
                BusinessAccountType = 1,
                ByPassProxyLocal = true,
                ChatKey = "ABC",
                ProxyAddress = "10.0.0.0",
                ChatKeyVersion = 5,
                EncryptionMode = 2,
                IsUseProxy = true,
                SecureKey = "Secure"
            };
            cryptographyService = Substitute.For<ICryptographyService>();
            jwtService = Substitute.For<IJwtService>();
            restClientService = Substitute.For<IRestClientService>();
            letstalkService = new LetstalkService(cryptographyService, restClientService, botConfig);
        }

        [Fact]
        public void GetProfileAsync_AccountIdIsNullOrWhiteSpace_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => letstalkService.GetProfileAsync(string.Empty));
        }

        [Fact]
        public async Task GetProfileAsync_ResultIsOk_ReturnsCorrectResult()
        {
            // Arrange
            var accountId = "AccountId";
            var profileResult = new ProfileResult
            {
                Status = 1,
                AvatarUrl = "AvatarUrl",
                DisplayName = "DisplayName",
            };
            restClientService.ExecuteGetAsync<ProfileResult>(Arg.Any<string>())
                .Returns(profileResult);

            var expected = (new Contact
            {
                UserId = accountId,
                AvatarUrl = profileResult.AvatarUrl,
                DisplayName = profileResult.DisplayName
            }, true);

            // Act
            var actual = await letstalkService.GetProfileAsync(accountId);

            // Assert
            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }

        [Fact]
        public async Task GetProfileAsync_AccountIsNotExisted_ReturnsCorrectResult()
        {
            // Arrange
            var accountId = "AccountId";
            var profileResult = new ProfileResult { Status = 21, };
            restClientService.ExecuteGetAsync<ProfileResult>(Arg.Is(accountId))
                .Returns(profileResult);

            var expected = (new Contact(), false);

            // Act
            var actual = await letstalkService.GetProfileAsync(accountId);

            // Assert
            Assert.False(actual.Item2);
            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }

        [Fact]
        public async Task GetBotContactList_WhenCalledSuccess_ReturnExpectedListContact()
        {
            var listContact = new List<string> { "contact1", "contact2" };
            var botContactResult = new BotContactListResult
            {
                Contacts = listContact,
                Status = 1
            };
            restClientService.ExecuteGetAsync<BotContactListResult>(Arg.Any<string>())
                .Returns(botContactResult);

            var actual = await letstalkService.GetBotContactList();

            Assert.Equal(listContact, actual);
        }

        [Fact]
        public async Task GetBotContactList_WhenCalledSuccess_ReturnEmptyL()
        {
            var listContact = new List<string> { "contact1", "contact2" };
            var botContactResult = new BotContactListResult
            {
                Contacts = listContact,
                Status = 10
            };
            restClientService.ExecuteGetAsync<BotContactListResult>(Arg.Any<string>())
                .Returns(botContactResult);

            var actual = await letstalkService.GetBotContactList();

            Assert.Equal(new List<string>(), actual);
        }

        [Fact]
        public void GetChatRoomData_WhenNullRoomToken_ThrowsArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => letstalkService.GetChatRoomData(string.Empty));
        }

        [Fact]
        public async Task GetChatRoomData_WhenGetSuccess_ReturnExpectedRoomResult()
        {
            var roomToken = "roomToken";
            var expectRoomData = new RoomData
            {
                Title = "Room1",
                Members = new List<string> { "mem1", "mem2" }
            };

            var roomResult = new RoomResult
            {
                Status = 1,
                Room = expectRoomData
            };
            restClientService.ExecuteGetAsync<RoomResult>(Arg.Is($"bot/v2/chatroom/data?botId={botConfig.BotAccountId}&room={roomToken}"))
                .Returns(roomResult);

            var actual = await letstalkService.GetChatRoomData(roomToken);

            Assert.Equal(expectRoomData, actual);
        }

        [Fact]
        public async Task GetChatRoomData_WhenGetFail_ReturnNull()
        {
            var roomToken = "roomToken";

            var roomResult = new RoomResult
            {
                Status = -1,
                Room = new RoomData()
            };
            restClientService.ExecuteGetAsync<RoomResult>(Arg.Is($"bot/v2/chatroom/data?botId={botConfig.BotAccountId}&room={roomToken}"))
                .Returns(roomResult);

            var actual = await letstalkService.GetChatRoomData(roomToken);

            Assert.Null(actual);
        }

        [Fact]
        public void LeaveChatRoom_WhenNullRoomToken_ThrowsArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => letstalkService.LeaveChatRoom(string.Empty));
        }

        [Fact]
        public async Task LeaveChatRoom_WhenCalled_ReturnExpectBaseResult()
        {
            var roomToken = "roomToken";
            var result = new BaseResult { Status = 1 };
            restClientService.ExecuteDeleteAsync<BaseResult>(Arg.Is($"bot/v2/leave_chatroom?botId={botConfig.BotAccountId}&token={roomToken}"), Arg.Any<Dictionary<string, object>>())
                .Returns(result);
            var actual = await letstalkService.LeaveChatRoom(roomToken);
            Assert.Equal(result, actual);
        }

        [Fact]
        public void PushAsync_AccountIdsIsNull_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => letstalkService.PushAsync((IEnumerable<string>)null, string.Empty));
        }

        [Fact]
        public void PushAsync_AccountIdsIsEmptyList_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => letstalkService.PushAsync(Enumerable.Empty<string>(), string.Empty));
        }

        [Fact]
        public void PushAsync_MessageIsNullOrWhiteSpace_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => letstalkService.PushAsync(new List<string> { "account" }, string.Empty));
        }

        [Fact]
        public async Task PushAsync_ValidParams_CallsCryptographyHelperHashMessage()
        {
            // Arrange
            var message = "message";
            var formattedMessage = JsonConvert.SerializeObject(new { msg = message });

            // Act
            await letstalkService.PushAsync(new List<string> { "account" }, message, DateTime.Now);

            //Assert
            cryptographyService.Received().Hash(Arg.Is(formattedMessage));
        }

        [Fact]
        public async Task PushAsync_ValidParams_CallsCryptographyHelperEncrypt()
        {
            // Arrange
            var message = "message";
            var formattedMessage = JsonConvert.SerializeObject(new { msg = message });

            var hashContentKey = "HashContentKey";
            cryptographyService.Hash(Arg.Any<string>()).Returns(hashContentKey);

            // Act
            await letstalkService.PushAsync(new List<string> { "account" }, message);

            //Assert
            cryptographyService.Received().Encrypt(
                Arg.Is(formattedMessage),
                Arg.Is(botConfig.ChatKey),
                Arg.Is(hashContentKey));
        }

        [Fact]
        public async Task PushAsync_ValidParamsSingleAccountId_CallsCryptographyHelperEncrypt()
        {
            // Arrange
            var message = "message";
            var formattedMessage = JsonConvert.SerializeObject(new { msg = message });

            var hashContentKey = "HashContentKey";
            cryptographyService.Hash(Arg.Any<string>()).Returns(hashContentKey);

            // Act
            await letstalkService.PushAsync("account", message);

            //Assert
            cryptographyService.Received().Encrypt(
                Arg.Is(formattedMessage),
                Arg.Is(botConfig.ChatKey),
                Arg.Is(hashContentKey));
        }

        [Fact]
        public async Task PushAsync_WithMessageDueDate_WillCallsExpected()
        {
            // Arrange
            var message = "message";
            var formattedMessage = JsonConvert.SerializeObject(new { msg = message });
            var msg = new Message
            {
                Content = message,
                ExpiredTime = DateTime.Now
            };
            var hashContentKey = "HashContentKey";
            cryptographyService.Hash(Arg.Any<string>()).Returns(hashContentKey);
            cryptographyService.Encrypt(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns("ABC");

            // Act
            await letstalkService.PushAsync(new List<string> { "account" }, msg);

            //Assert
            cryptographyService.Received().Encrypt(
                Arg.Is(formattedMessage),
                Arg.Is(botConfig.ChatKey),
                Arg.Is(hashContentKey));
        }

        [Fact]
        public void PushRoomAsync_WhenNullOrEmptyChatRoomToken_ThrowsArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => letstalkService.PushRoomAsync(string.Empty, "message"));
        }

        [Fact]
        public void PushRoomAsync_WhenNullOrEmptyMessage_ThrowsArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => letstalkService.PushRoomAsync("token", string.Empty));
        }

        [Fact]
        public async Task PushRoomAsync_ValidParams_ReturnExpectedBaseResult()
        {
            var result = new BaseResult { Status = 1, Message = "Test" };
            var chatRoomToken = "token";
            var msg = "message";
            restClientService.ExecutePostAsync<BaseResult>(Arg.Is($"bot/push_chatroom/{botConfig.BotAccountId}"),
                Arg.Any<Dictionary<string, object>>()).Returns(result);

            var actual = await letstalkService.PushRoomAsync(chatRoomToken, msg);

            Assert.Equal(result, actual);
        }

        [Fact]
        public void SetWebhookAsync_UrlIsNullOrWhiteSpace_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => letstalkService.SetWebhookAsync(string.Empty));
        }

        [Fact]
        public async Task SetWebhookAsync_ValidUrl_CallsExecutePostAsync()
        {
            // Arrange
            var url = "url";

            // Act
            await letstalkService.SetWebhookAsync(url);

            // Assert
            await restClientService.Received().ExecutePostAsync<BaseResult>(
                Arg.Is($"bot/set_webhook/{botConfig.BotAccountId}"),
                Arg.Is<Dictionary<string, object>>(d => d["url"].Equals(url)));
        }

        [Fact]
        public void SetWelcomeMessageAsync_MessageIsNullOrWhiteSpace_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => letstalkService.SetWelcomeMessageAsync(string.Empty));
        }

        [Fact]
        public async Task SetWelcomeMessageAsync_ValidMessage_CallsExecutePostAsync()
        {
            // Arrange
            var message = "message";

            // Act
            await letstalkService.SetWelcomeMessageAsync(message);

            // Assert
            await restClientService.Received().ExecutePostAsync<BaseResult>(
                Arg.Is($"bot/set_welcome_msg/{botConfig.BotAccountId}"),
                Arg.Is<Dictionary<string, object>>(d => d["message"].Equals(message)));
        }

        [Fact]
        public void ChangeDisplayNameAsync_BotNameIsNullOrWhiteSpace_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => letstalkService.ChangeDisplayNameAsync(string.Empty));
        }

        [Fact]
        public async Task ChangeDisplayNameAsync_ValidBotName_CallsExecutePutAsync()
        {
            // Arrange
            var botName = "BotName";

            // Act
            await letstalkService.ChangeDisplayNameAsync(botName);

            // Assert
            await restClientService.Received().ExecutePutAsync<BaseResult>(
                Arg.Is($"bot/change_display_name/{botConfig.BotAccountId}"),
                Arg.Is<Dictionary<string, object>>(d => d["display_name"].Equals(botName)));
        }

        [Fact]
        public void SetAvatarAsync_ImageIsNullOrWhiteSpace_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => letstalkService.SetAvatarAsync(string.Empty));
        }

        [Fact]
        public async Task SetAvatarAsync_ValidImage_CallsExecutePostAsync()
        {
            // Arrange
            var image = "Image";

            // Act
            await letstalkService.SetAvatarAsync(image);

            // Assert
            await restClientService.Received().ExecutePostAsync<BaseResult>(
               Arg.Is($"bot/upload_avatar_image/{botConfig.BotAccountId}"),
               Arg.Is<Dictionary<string, object>>(d => d["image"].Equals(image)));
        }

        [Fact]
        public async Task SetPermissionAddBotToGroup_WhenCalled_CallExecutePostAndReturnExpectedResult()
        {
            var result = new BaseResult { Status = 1 };
            restClientService.ExecutePostAsync<BaseResult>(Arg.Any<string>(), Arg.Any<Dictionary<string, object>>()).Returns(result);

            var actual = await letstalkService.SetPermissionAddBotToGroup(true);
            await restClientService.ExecutePostAsync<BaseResult>(
                Arg.Is("bot/v2/chatroom_switch"),
                Arg.Is<Dictionary<string, object>>(d => d["botId"].Equals(botConfig.BotAccountId) && d["on"].Equals(1))
                );
            Assert.Equal(result, actual);
        }

        [Fact]
        public async Task GetChatRoomList_WhenCalledSuccess_ReturnExpectedListRoom()
        {
            var listRoomResult = new BotChatRoomListResult
            {
                Status = 1,
                Count = 10,
                Rooms = new List<string> { "room1", "room2" }
            };
            restClientService.ExecuteGetAsync<BotChatRoomListResult>(Arg.Any<string>()).Returns(listRoomResult);

            var actual = await letstalkService.GetChatRoomList();

            await restClientService.ExecuteGetAsync<BotChatRoomListResult>(Arg.Is($"bot/v2/chatroom?botId={botConfig.BotAccountId}"));
            Assert.Equal(listRoomResult.Rooms, actual);
        }

        [Fact]
        public async Task GetChatRoomList_WhenCalledFailed_ReturnEmptyList()
        {
            var listRoomResult = new BotChatRoomListResult
            {
                Status = 100
            };
            restClientService.ExecuteGetAsync<BotChatRoomListResult>(Arg.Any<string>()).Returns(listRoomResult);

            var actual = await letstalkService.GetChatRoomList();

            Assert.Equal(new List<string>(), actual);
        }

        [Fact]
        public async Task GetWebhookAsync_WhenCalledSuccess_ReturnExpectUrl()
        {
            var listRoomResult = new WebhookResult
            {
                Status = 1,
                Url = "WebhookURL"
            };
            restClientService.ExecuteGetAsync<WebhookResult>(Arg.Any<string>()).Returns(listRoomResult);

            var actual = await letstalkService.GetWebhookAsync();

            await restClientService.ExecuteGetAsync<WebhookResult>(Arg.Is($"bot/get_webhook/{botConfig.BotAccountId}"));
            Assert.Equal("WebhookURL", actual);
        }

        [Fact]
        public async Task GetWebhookAsync_WhenCalledFailed_ReturnEmptyList()
        {
            var listRoomResult = new WebhookResult
            {
                Status = 100
            };
            restClientService.ExecuteGetAsync<WebhookResult>(Arg.Any<string>()).Returns(listRoomResult);

            var actual = await letstalkService.GetWebhookAsync();

            Assert.Empty(actual);
        }
    }
}