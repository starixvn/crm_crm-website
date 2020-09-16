namespace CRM.Letstalk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CRM.Core.Enumerables;
    using CRM.Core.Extensions.XML;
    using CRM.Core.Services;
    using CRM.Letstalk.Client.Models.Results;
    using CRM.Letstalk.Client.Services.Interfaces;
    using CRM.Letstalk.Services.Constants;
    using CRM.Letstalk.Services.Criteria;
    using CRM.Letstalk.Services.Models.SendNotification;
    using CRM.Letstalk.Services.Services.Interfaces;
    using Microsoft.Extensions.Configuration;

    public class SendNotificationService : ISendNotificationService
    {
        private readonly IBaseService baseService;
        private readonly ICRMLogService logService;
        private readonly Func<int, ILetstalkService> letstalkService;
        private readonly int ItemEachBatch;
        private const int DefaultItemEacgBatch = 10;

        public SendNotificationService(IBaseService baseService,
            ICRMLogService logService,
            IConfiguration configuration,
            Func<int, ILetstalkService> letstalkService)
        {
            this.baseService = baseService;
            this.logService = logService;
            this.letstalkService = letstalkService;
            ItemEachBatch = configuration.GetValue<int?>("SendNotificationItemEachBatch") ?? DefaultItemEacgBatch;
        }

        public async Task SendNotificationToLetstalk()
        {
            var notifications = await GetNotificationToSend();

            if (notifications.Any())
            {
                await UpdateBotRemovedStatus(notifications.Where(noti => noti.BotId == 0 && noti.ChatType == ChatType.Group));

                await SendNotification(notifications.Where(noti => noti.BotId > 0));
            }
        }

        private async Task UpdateBotRemovedStatus(IEnumerable<NotificationMessage> notifications)
        {
            if (notifications.Any())
            {
                var results = new List<SendNotificationResult>();
                foreach (var notification in notifications)
                {
                    var result = new SendNotificationResult
                    {
                        NotificationId = notification.NotificationId,
                        ConversationGroupId = notification.ConversationGroupId,
                        Status = SendNotificationStatus.Error,
                        ErrMsg = SendNotificationErrMsg.BotRemovedMsg
                    };
                    results.Add(result);
                }

                await UpdateRecipientStatus(results);
            }
        }

        private async Task SendNotification(IEnumerable<NotificationMessage> notifications)
        {
            var batches = DivideBatchToSend(notifications);
            foreach (var batch in batches)
            {
                var result = await SendMessage(batch);

                await UpdateRecipientStatus(result);
            }
        }

        private async Task<IEnumerable<SendNotificationResult>> SendMessage(List<NotificationMessage> notifications)
        {
            var tasks = new List<Task<SendNotificationResult>>();

            foreach (var notification in notifications)
            {
                tasks.Add(Task.Run(() => SendToLestalk(notification)));
            }

            var results = (await Task.WhenAll(tasks)).ToList();

            return results;
        }

        private async Task<SendNotificationResult> SendToLestalk(NotificationMessage notification)
        {
            var sendResult = new SendNotificationResult
            {
                NotificationId = notification.NotificationId,
                ConversationGroupId = notification.ConversationGroupId,
            };
            try
            {
                if (notification.ChatType == ChatType.Personal)
                {
                    await SendToPersonal(notification, sendResult);
                }
                else
                {
                    await SendToGroup(notification, sendResult);
                }
            }
            catch (Exception exception)
            {
                sendResult.Status = SendNotificationStatus.Retrying;
                sendResult.ErrMsg = SendNotificationErrMsg.BotServiceErrorMsg;
                await logService.ErrorAsync(exception);
            }

            return sendResult;
        }

        private async Task SendToPersonal(NotificationMessage notification, SendNotificationResult sendResult)
        {
            var result = await letstalkService(notification.BotId).PushAsync(notification.ConversationId, notification.Content);

            if (result.IsSuccess && !result.BlockList.Any())
            {
                sendResult.Status = SendNotificationStatus.Sent;
                sendResult.ErrMsg = string.Empty;
            }

            if (result.BlockList.Any())
            {
                sendResult.Status = SendNotificationStatus.BotBlocked;
            }

            if (result.FailList.Any())
            {
                sendResult.Status = SendNotificationStatus.Error;
                sendResult.ErrMsg = SendNotificationErrMsg.LetstalkIdInvalidMsg;
            }

            await WriteLogMessage(notification, result, result.BlockList.Any());
        }

        private async Task SendToGroup(NotificationMessage notification, SendNotificationResult sendResult)
        {
            var result = await letstalkService(notification.BotId).PushRoomAsync(notification.ConversationId, notification.Content);
            sendResult.Status = result.IsSuccess ? SendNotificationStatus.Sent : SendNotificationStatus.Retrying;
            sendResult.ErrMsg = result.IsSuccess ? string.Empty : SendNotificationErrMsg.BotServiceErrorMsg;
            await WriteLogMessage(notification, result);
        }

        private async Task UpdateRecipientStatus(IEnumerable<SendNotificationResult> sentResults)
        {
            var command = new UpdateNotificationStatusCommand
            {
                XmlData = sentResults.ToXML().ToString()
            };

            await baseService.ExecuteNonQueryAsync(command);
        }

        private List<List<NotificationMessage>> DivideBatchToSend(IEnumerable<NotificationMessage> notifications)
        {
            var batches = new List<List<NotificationMessage>>();
            int totalBatch = (int)Math.Ceiling((double)notifications.Count() / ItemEachBatch);
            for (int i = 0; i < totalBatch; i++)
            {
                batches.Add(notifications.Skip(i * ItemEachBatch).Take(ItemEachBatch).ToList());
            }

            return batches;
        }

        private async Task<IEnumerable<NotificationMessage>> GetNotificationToSend()
        {
            return await baseService.GetDataAsync<NotificationMessage>(new GetNotificationToSendCriteria());
        }

        private async Task WriteLogMessage(NotificationMessage notification, BaseResult result, bool isBlocked = false)
        {
            var command = new WriteLogMessageCommand
            {
                NotificationId = notification.NotificationId,
                BotId = notification.BotId,
                Content = notification.Content,
                ConversationId = notification.ConversationId,
                IsSuccess = result.IsSuccess,
                IsBotBlocked = isBlocked,
                ErrMsg = result.Message,
            };

            await baseService.ExecuteNonQueryAsync(command);
        }
    }
}