namespace CRM.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CRM.Core.Enumerables;
    using CRM.Core.Extensions.XML;
    using CRM.Core.Services;
    using CRM.Criteria.SendNotification;
    using CRM.Letstalk.Client.Services.Interfaces;
    using CRM.Models.SendNotification;
    using CRM.Services.Interfaces;

    public class SendNotificationService : ISendNotificationService
    {
        private readonly IBaseService baseService;
        private readonly IWriteLogService writeLogService;
        private readonly Func<int, ILetstalkService> letstalkService;
        private const int ItemEachBatch = 2;

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
                await SendNotification(notifications);
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
                NotificationId = notification.NotifcationId,
                RecipientId = notification.RecipientId,
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
                sendResult.ErrCode = (int)NotificationStatus.Error;
                sendResult.ErrMsg = exception.Message;
            }

            return sendResult;
        }

        private async Task SendToPersonal(NotificationMessage notification, SendNotificationResult sendResult)
        {
            var result = await letstalkService(notification.BotId).PushAsync(notification.LetstalkConversationId, notification.Content);
            sendResult.IsSuccess = result.IsSuccess;
            if (result.BlockList.Any())
            {
                sendResult.ErrCode = (int)NotificationStatus.BotBlocked;
                sendResult.ErrMsg = "The User has been block the bot";
            }

            if (result.FailList.Any())
            {
                sendResult.ErrCode = (int)NotificationStatus.Error;
                sendResult.ErrMsg = "Error occured on Chatbot service";
            }

            await WriteLogMessage(notification, sendResult);
        }

        private async Task SendToGroup(NotificationMessage notification, SendNotificationResult sendResult)
        {
            var result = await letstalkService(notification.BotId).PushRoomAsync(notification.LetstalkConversationId, notification.Content);
            sendResult.IsSuccess = result.IsSuccess;
            sendResult.ErrCode = result.IsSuccess ? 0 : (int)NotificationStatus.Error;
            sendResult.ErrMsg = result.IsSuccess ? string.Empty : "Error occured on Chatbot service";
            await WriteLogMessage(notification, sendResult);
        }

        private async Task UpdateRecipientStatus(IEnumerable<SendNotificationResult> sentResults)
        {
            var command = new UpdateNotificationStatusCommand
            {
                SentStatus = sentResults.ToXML().ToString()
            };

            await baseService.ExecuteNonQueryAsync(command);
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

        private async Task WriteLogMessage(NotificationMessage notification, SendNotificationResult result)
        {
            var command = new WriteLogMessageCommand
            {
                ReferenceId = notification.NotifcationId,
                BotId = notification.BotId,
                Content = notification.Content,
                LetstalkId = notification.LetstalkConversationId,
                IsSuccess = result.IsSuccess,
                ErrorMessage = result.ErrMsg
            };

            await baseService.ExecuteNonQueryAsync(command);
        }
    }
}