namespace Fanex.CRM.BackgroundJob.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Fanex.CRM.BackgroundJob.Models;
    using Fanex.CRM.BackgroundJob.Models.ChatBot;
    using global::CRM.Core.Models;
    using global::CRM.Core.Services;
    using global::CRM.Letstalk.Services.Services.Interfaces;

    public interface IChatBotJobService
    {
        Task PullMessageAndSendNotificationAsync();
    }

    public class ChatBotJobService : IChatBotJobService
    {
        private readonly ICRMLogService logService;
        private readonly ISystemSettingService systemSettingService;
        private readonly IChatBotTicketVoidingService ticketVoidingService;
        private readonly ISendNotificationService sendNotificationService;

        public ChatBotJobService(
            ICRMLogService logService,
            ISystemSettingService systemSettingService,
            IChatBotTicketVoidingService ticketVoidingService,
            ISendNotificationService sendNotificationService)
        {
            this.logService = logService;
            this.systemSettingService = systemSettingService;
            this.ticketVoidingService = ticketVoidingService;
            this.sendNotificationService = sendNotificationService;
        }

        public async Task PullMessageAndSendNotificationAsync()
        {
            AbnormalBetSystemSetting setting = await systemSettingService.GetChatBotSystemSettingAsync();
            if (!setting.ServiceFlag)
            {
                logService.LogInformation("Chat Bot Service is shutdown.");
                return;
            }

            IEnumerable<ChatBotVoidingMessage> voidingMessages = await ticketVoidingService.GetVoidingMessageAsync();
            voidingMessages = await ticketVoidingService.ImportVoidingMessageAsync(voidingMessages, setting.BatchSize);
            IEnumerable<Message> messages = await ticketVoidingService.GetMessageInfoAsync(voidingMessages);
            IEnumerable<ChatBotVoidingTicketInfo> ticketInfos = await ticketVoidingService.GetTicketVoidingInfoAsync(voidingMessages);
            if (ticketInfos.Any())
            {
                CommandResult commandResult = await ticketVoidingService.ImportTicketVoidingAsync(messages, ticketInfos);
                if (commandResult.IsSuccessful)
                {
                    await ticketVoidingService.EditVoidingMessageAsync(voidingMessages);
                    await sendNotificationService.SendNotificationToLetstalk();
                }
                else
                {
                    logService.LogInformation(commandResult.Message);
                }
            }
            else
            {
                await ticketVoidingService.EditVoidingMessageAsync(voidingMessages);
            }
        }
    }
}