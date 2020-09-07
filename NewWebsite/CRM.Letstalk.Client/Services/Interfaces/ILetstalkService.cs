namespace CRM.Letstalk.Client.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CRM.Letstalk.Client.Models;
    using CRM.Letstalk.Client.Models.Results;

    public interface ILetstalkService
    {
        Task<PushMessageResult> PushAsync(IEnumerable<string> accountIds, Payload payload);

        Task<PushMessageResult> PushAsync(IEnumerable<string> accountIds, string message, DateTime? expiredTime = null);

        Task<PushMessageResult> PushAsync(string accountId, string message, DateTime? expiredTime = null);

        Task<PushMessageResult> PushAsync(IEnumerable<string> accountIds, Message message, DateTime? expiredTime = null);

        Task<BaseResult> PushRoomAsync(string chatroomToken, string message, DateTime? expiredTime = null);

        Task<(Contact, bool)> GetProfileAsync(string accountId);

        Task<RoomData> GetChatRoomData(string roomToken);

        Task<string> GetWebhookAsync();

        Task<BaseResult> SetWebhookAsync(string url);

        Task<BaseResult> SetAvatarAsync(string image);

        Task<BaseResult> SetWelcomeMessageAsync(string message);

        Task<BaseResult> ChangeDisplayNameAsync(string botName);

        Task<IList<string>> GetBotContactList();

        Task<IList<string>> GetChatRoomList();

        Task<BaseResult> SetPermissionAddBotToGroup(bool isAllowed);

        Task<BaseResult> LeaveChatRoom(string roomToken);

        Task LeaveAllRoom();
    }
}