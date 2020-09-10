namespace CRM.Letstalk.Client.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CRM.Letstalk.Client.Configurations;
    using CRM.Letstalk.Client.Models;
    using CRM.Letstalk.Client.Models.Results;
    using CRM.Letstalk.Client.Services.Interfaces;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class LetstalkService : ILetstalkService
    {
        private const int payloadId = 100;
        private const int defaultExpiredTimeInHour = 3;
        private readonly BotConfig botConfig;
        private readonly IRestClientService restClientService;
        private readonly ICryptographyService cryptographyService;

        private readonly JsonSerializerSettings DefaultJsonSerializeSetting = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public LetstalkService(ICryptographyService cryptographyService,
            IRestClientService restClientService,
            BotConfig botConfig)
        {
            this.cryptographyService = cryptographyService;
            this.restClientService = restClientService;
            this.botConfig = botConfig;
        }

        public async Task<BaseResult> ChangeDisplayNameAsync(string botName)
        {
            ValidateInputParam(nameof(botName), botName);

            return await restClientService.ExecutePutAsync<BaseResult>(
                $"bot/change_display_name/{botConfig.BotAccountId}",
                new Dictionary<string, object>
                {
                    { "display_name", botName }
                });
        }

        public async Task<IList<string>> GetBotContactList()
        {
            var result = await restClientService.ExecuteGetAsync<BotContactListResult>($"bot/v2/contact?botId={botConfig.BotAccountId}");
            if (result.IsSuccess)
            {
                return result.Contacts;
            }

            return new List<string>();
        }

        public async Task<RoomData> GetChatRoomData(string roomToken)
        {
            ValidateInputParam(nameof(roomToken), roomToken);

            var result = await restClientService.ExecuteGetAsync<RoomResult>($"bot/v2/chatroom/data?botId={botConfig.BotAccountId}&room={roomToken}");

            if (result.IsSuccess)
            {
                return result.Room;
            }

            return null;
        }

        public async Task<(Contact, bool)> GetProfileAsync(string accountId)
        {
            ValidateInputParam(nameof(accountId), accountId);

            var result = await restClientService.ExecuteGetAsync<ProfileResult>($"bot/get_profile/{botConfig.BotAccountId}/user/{accountId}");

            if (result != null && result.IsSuccess)
            {
                return (new Contact
                {
                    UserId = accountId,
                    AvatarUrl = result.AvatarUrl,
                    DisplayName = result.DisplayName
                }, true);
            }

            return (new Contact(), false);
        }

        public async Task<BaseResult> LeaveChatRoom(string roomToken)
        {
            ValidateInputParam(nameof(roomToken), roomToken);

            var result = await restClientService.ExecuteDeleteAsync<BaseResult>(
                $"bot/v2/leave_chatroom?botId={botConfig.BotAccountId}&token={roomToken}",
                new Dictionary<string, object>());

            return result;
        }

        public async Task<PushMessageResult> PushAsync(IEnumerable<string> accountIds, Payload payload)
        {
            ValidateInputParam(nameof(accountIds), accountIds);
            ValidateInputParam(nameof(payload), payload);

            var accountIdToLowerList = accountIds.Select(accountId => accountId.ToLowerInvariant());

            payload.UserId = botConfig.BotUserId;
            payload.Time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            payload.HashType = botConfig.EncryptionMode;
            payload.From = botConfig.BusinessAccountType;
            payload.Type = MessageType.Text;
            payload.Id = payloadId;
            payload.Receiver = accountIdToLowerList.ElementAt(0);
            payload.HashId = botConfig.ChatKeyVersion;

            var result = await restClientService.ExecutePostAsync<PushMessageResult>(
                $"bot/push/{botConfig.BotAccountId}",
                new Dictionary<string, object>
                {
                    { "receiver", accountIdToLowerList },
                    { "payload", JsonConvert.SerializeObject(payload, DefaultJsonSerializeSetting) }
                });

            return result;
        }

        public async Task<PushMessageResult> PushAsync(IEnumerable<string> accountIds, string message, DateTime? expiredTime = null)
                    => await PushAsync(accountIds, new Message { Content = message }, expiredTime);

        public async Task<PushMessageResult> PushAsync(string accountId, string message, DateTime? expiredTime = null)
                    => await PushAsync(new List<string> { accountId }, message, expiredTime);

        public async Task<PushMessageResult> PushAsync(IEnumerable<string> accountIds, Message message, DateTime? expiredTime = null)
        {
            ValidateInputParam(nameof(accountIds), accountIds);
            ValidateInputParam(nameof(message), message);

            var payload = new Payload();
            var formattedMessage = JsonConvert.SerializeObject(message, DefaultJsonSerializeSetting);
            payload.HashContentKey = cryptographyService.Hash(formattedMessage);
            payload.HashContent = cryptographyService.Encrypt(formattedMessage, botConfig.ChatKey, payload.HashContentKey);

            long? dueDate = expiredTime.HasValue
                        ? new DateTimeOffset(expiredTime.Value).ToUnixTimeMilliseconds()
                        : DateTimeOffset.Now.AddHours(defaultExpiredTimeInHour).ToUnixTimeMilliseconds();
            payload.DueDate = message.ExpiredTime.HasValue ? new DateTimeOffset(message.ExpiredTime.Value).ToUnixTimeMilliseconds() : dueDate;

            return await PushAsync(accountIds, payload);
        }

        public async Task<BaseResult> PushRoomAsync(string chatroomToken, string message, DateTime? expiredTime = null)
        {
            ValidateInputParam(nameof(chatroomToken), chatroomToken);
            ValidateInputParam(nameof(message), message);

            var result = await restClientService.ExecutePostAsync<BaseResult>(
               $"bot/push_chatroom/{botConfig.BotAccountId}",
               new Dictionary<string, object>
               {
                    { "receiver", chatroomToken },
                    { "msg", message }
               });

            return result ?? new BaseResult();
        }

        public async Task<BaseResult> SetAvatarAsync(string image)
        {
            ValidateInputParam(nameof(image), image);

            var result = await restClientService.ExecutePostAsync<BaseResult>(
                $"bot/upload_avatar_image/{botConfig.BotAccountId}",
                new Dictionary<string, object>
                {
                    { "image", image }
                });

            return result;
        }

        public async Task<BaseResult> SetPermissionAddBotToGroup(bool isAllowed)
        {
            var result = await restClientService.ExecutePostAsync<BaseResult>(
               $"bot/v2/chatroom_switch",
               new Dictionary<string, object>
               {
                    { "botId", botConfig.BotAccountId },
                    { "on", isAllowed ? 1 : 0 }
               });

            return result;
        }

        public async Task<BaseResult> SetWebhookAsync(string url)
        {
            ValidateInputParam(nameof(url), url);
            return await restClientService.ExecutePostAsync<BaseResult>(
                $"bot/set_webhook/{botConfig.BotAccountId}",
                new Dictionary<string, object>
                {
                    { "url", url }
                });
        }

        public async Task<BaseResult> SetWelcomeMessageAsync(string message)
        {
            ValidateInputParam(nameof(message), message);

            return await restClientService.ExecutePostAsync<BaseResult>(
                  $"bot/set_welcome_msg/{botConfig.BotAccountId}",
                  new Dictionary<string, object>
                {
                    { "message", message }
                });
        }

        private static void ValidateInputParam(string paramName, object paramValue)
        {
            if (paramValue == null
                || (paramValue is string && string.IsNullOrWhiteSpace(paramValue as string)))
            {
                throw new ArgumentNullException(paramName);
            }
        }

        public async Task<IList<string>> GetChatRoomList()
        {
            var result = await restClientService.ExecuteGetAsync<BotChatRoomListResult>($"bot/v2/chatroom?botId={botConfig.BotAccountId}");
            if (result.IsSuccess)
            {
                return result.Rooms;
            }

            return new List<string>();
        }

        public async Task<string> GetWebhookAsync()
        {
            var result = await restClientService.ExecuteGetAsync<WebhookResult>($"bot/get_webhook/{botConfig.BotAccountId}");

            if (result.IsSuccess)
            {
                return result.Url;
            }

            return string.Empty;
        }
    }
}