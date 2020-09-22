namespace Fanex.CRM.BackgroundJob.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Fanex.CRM.BackgroundJob.Models;
    using Fanex.CRM.BackgroundJob.Models.ChatBot;
    using global::CRM.Core.Models;
    using global::CRM.Core.Services;

    public interface IChatBotJobService
    {
        Task PullMessageAsync(AbnormalBetSystemSetting setting);
    }

    public class ChatBotJobService : IChatBotJobService
    {
        private readonly ICRMLogService logService;
        private readonly IChatBotTicketVoidingService ticketVoidingService;

        public ChatBotJobService(
            ICRMLogService logService,
            IChatBotTicketVoidingService ticketVoidingService)
        {
            this.logService = logService;
            this.ticketVoidingService = ticketVoidingService;
        }

        public async Task PullMessageAsync(AbnormalBetSystemSetting setting)
        {
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