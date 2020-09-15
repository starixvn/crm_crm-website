namespace Fanex.CRM.BackgroundJob.Services
{
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Fanex.CRM.BackgroundJob.Criteria.ChatBot;
    using Fanex.CRM.BackgroundJob.Models;
    using Fanex.CRM.BackgroundJob.Models.ChatBot;

    public interface ITicketVoidingService
    {
        Task<IEnumerable<ChatBotVoidingTicketInfo>> GetTicketVoidingInfoForChatBot(IEnumerable<VoidingMessage> voidingMessages);
    }

    public class TicketVoidingService : ITicketVoidingService
    {
        private readonly IBaseService baseService;

        public TicketVoidingService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        public async Task<IEnumerable<ChatBotVoidingTicketInfo>> GetTicketVoidingInfoForChatBot(IEnumerable<VoidingMessage> voidingMessages)
        {
            const string XmlTemplate = "<r MessageID='{0}' RequestID='{1}' />";
            var stringBuilder = new StringBuilder(Constants.Constants.BeginRootTag.ToLowerInvariant());
            foreach (var message in voidingMessages)
            {
                stringBuilder.AppendFormat(XmlTemplate, message.MessageId, message.RequestId);
            }
            stringBuilder.Append(Constants.Constants.EndRootTag.ToLowerInvariant());
            var criteria = new GetVoidingMessageForChatBotCriteria
            {
                Messages = stringBuilder.ToString()
            };
            return await baseService.GetDataAsync<ChatBotVoidingTicketInfo>(criteria);
        }
    }
}