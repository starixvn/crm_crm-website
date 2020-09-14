namespace CRM.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CRM.Core.Constants;
    using CRM.Core.Enumerables;
    using CRM.Core.Extensions.XML;
    using CRM.Core.Services;
    using CRM.Criteria.SendNotification;
    using CRM.Letstalk.Client.Models.Results;
    using CRM.Letstalk.Client.Services.Interfaces;
    using CRM.Models.SendNotification;
    using CRM.Services.Interfaces;

    public class SendNotificationService : ISendNotificationService
    {
        private readonly IBaseService baseService;
        private readonly IWriteLogService writeLogService;
        private readonly Func<int, ILetstalkService> letstalkService;
        private const int ItemEachBatch = 10;

        public SendNotificationService(IBaseService baseService,
            IWriteLogService writeLogService,
            Func<int, ILetstalkService> letstalkService)
        {
            this.baseService = baseService;
            this.writeLogService = writeLogService;
            this.letstalkService = letstalkService;
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
            var results = new List<SendNotificationResult>();
            foreach (var notification in notifications)
            {
                var result = new SendNotificationResult
                {
                    NotificationId = notification.NotificationId,
                    ConversationGroupId = notification.ConversationGroupId,
                    Status = NotificationRecipientStatus.Error,
                    ErrMsg = NotificationRecipientErrorMessage.BotRemovedMsg
                };
                results.Add(result);
            }

            await UpdateRecipientStatus(results);
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
                await writeLogService.WriteErrorLogAsync(exception);
                sendResult.Status = NotificationRecipientStatus.Retrying;
                sendResult.ErrMsg = NotificationRecipientErrorMessage.BotServiceErrorMsg;
            }

            return sendResult;
        }

        private async Task SendToPersonal(NotificationMessage notification, SendNotificationResult sendResult)
        {
            var result = await letstalkService(notification.BotId).PushAsync(notification.ConversationId, notification.Content);

            if (result.IsSuccess && !result.BlockList.Any())
            {
                sendResult.Status = NotificationRecipientStatus.Sent;
                sendResult.ErrMsg = string.Empty;
            }

            if (result.BlockList.Any())
            {
                sendResult.Status = NotificationRecipientStatus.BotBlocked;
            }

            if (result.FailList.Any())
            {
                sendResult.Status = NotificationRecipientStatus.Error;
                sendResult.ErrMsg = NotificationRecipientErrorMessage.LetstalkIdInvalidMsg;
            }

            await WriteLogMessage(notification, result, result.BlockList.Any());
        }

        private async Task SendToGroup(NotificationMessage notification, SendNotificationResult sendResult)
        {
            var result = await letstalkService(notification.BotId).PushRoomAsync(notification.ConversationId, notification.Content);
            sendResult.Status = result.IsSuccess ? NotificationRecipientStatus.Sent : NotificationRecipientStatus.Retrying;
            sendResult.ErrMsg = result.IsSuccess ? string.Empty : NotificationRecipientErrorMessage.BotServiceErrorMsg;
            await WriteLogMessage(notification, result);
        }

        private async Task UpdateRecipientStatus(IEnumerable<SendNotificationResult> sentResults)
        {
            if (sentResults.Any())
            {
                var command = new UpdateNotificationStatusCommand
                {
                    XmlData = sentResults.ToXML().ToString()
                };

                await baseService.ExecuteNonQueryAsync(command);
            }
        }

        private static List<List<NotificationMessage>> DivideBatchToSend(IEnumerable<NotificationMessage> notifications)
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