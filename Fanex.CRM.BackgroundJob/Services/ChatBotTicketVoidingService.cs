namespace Fanex.CRM.BackgroundJob.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using Fanex.CRM.BackgroundJob.Criteria.ChatBot;
    using Fanex.CRM.BackgroundJob.Models;
    using Fanex.CRM.BackgroundJob.Models.ChatBot;
    using global::CRM.Core.Extensions.XML;
    using global::CRM.Core.Models;
    using global::CRM.Core.Services;

    public interface IChatBotTicketVoidingService
    {
        Task<IEnumerable<ChatBotVoidingMessage>> GetVoidingMessageAsync();

        Task<IEnumerable<ChatBotVoidingMessage>> ImportVoidingMessageAsync(IEnumerable<ChatBotVoidingMessage> voidingMessages, int batchSize);

        Task<CommandResult> EditVoidingMessageAsync(IEnumerable<ChatBotVoidingMessage> voidingMessages);

        Task<IEnumerable<Message>> GetMessageInfoAsync(IEnumerable<ChatBotVoidingMessage> voidingMessages);

        Task<IEnumerable<ChatBotVoidingTicketInfo>> GetTicketVoidingInfoAsync(IEnumerable<ChatBotVoidingMessage> voidingMessages);

        Task<CommandResult> ImportTicketVoidingAsync(
            IEnumerable<Message> messages,
            IEnumerable<ChatBotVoidingTicketInfo> ticketInfos,
            IEnumerable<ChatBotVoidingMessage> voidingMessages);
    }

    public class ChatBotTicketVoidingService : IChatBotTicketVoidingService
    {
        private readonly IBaseService baseService;

        public ChatBotTicketVoidingService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        public async Task<IEnumerable<ChatBotVoidingMessage>> GetVoidingMessageAsync()
        {
            var criteria = new GetChatBotVoidingMessageCriteria();
            return await baseService.GetDataAsync<ChatBotVoidingMessage>(criteria);
        }

        public async Task<IEnumerable<ChatBotVoidingMessage>> ImportVoidingMessageAsync(
            IEnumerable<ChatBotVoidingMessage> voidingMessages,
            int batchSize)
        {
            var criteria = new ImportChatBotVoidingMessageCriteria
            {
                Messages = voidingMessages.ToXML("root").ToString(),
                BatchSize = batchSize
            };
            return await baseService.GetDataAsync<ChatBotVoidingMessage>(criteria);
        }

        public async Task<CommandResult> EditVoidingMessageAsync(IEnumerable<ChatBotVoidingMessage> voidingMessages)
        {
            var criteria = new EditChatBotVoidingMessageCommand
            {
                Messages = voidingMessages.ToXML("root").ToString()
            };
            return await baseService.ExecuteNonQueryAsync(criteria);
        }

        public async Task<IEnumerable<Message>> GetMessageInfoAsync(IEnumerable<ChatBotVoidingMessage> voidingMessages)
        {
            var criteria = new GetChatBotMessageInfoCriteria
            {
                Messages = voidingMessages.ToXML("root").ToString()
            };
            var messages = await baseService.GetDataAsync<Message>(criteria);
            foreach (Message message in messages)
            {
                message.Content_en = message.Content_en?.Trim().Replace("@@", string.Empty);
                message.Content_cs = message.Content_cs?.Trim().Replace("@@", string.Empty);
                message.Content_th = message.Content_th?.Trim().Replace("@@", string.Empty);
                message.Content_vn = message.Content_vn?.Trim().Replace("@@", string.Empty);
            }

            return messages;
        }

        public async Task<IEnumerable<ChatBotVoidingTicketInfo>> GetTicketVoidingInfoAsync(IEnumerable<ChatBotVoidingMessage> voidingMessages)
        {
            var criteria = new GetChatBotTicketVoidingInfoCriteria
            {
                Messages = voidingMessages.ToXML("root").ToString()
            };
            return await baseService.GetDataAsync<ChatBotVoidingTicketInfo>(criteria);
        }

        public async Task<CommandResult> ImportTicketVoidingAsync(
            IEnumerable<Message> messages,
            IEnumerable<ChatBotVoidingTicketInfo> ticketInfos,
            IEnumerable<ChatBotVoidingMessage> voidingMessages)
        {
            const string XmlTemplate = "<r MessageId='{0}' Username='{1}' CustId='{2}' " +
                "Recommend='{3}' Mrecommend='{4}' Srecommend='{5}' IsTest='{6}' " +
                "Content_en='{7}' Content_cs='{8}' Content_th='{9}' Content_vn='{10}' />";
            var stringBuilder = new StringBuilder(Constants.Constants.BeginRootTag.ToLowerInvariant());
            foreach (ChatBotVoidingTicketInfo ticketInfo in ticketInfos)
            {
                Message message = messages.First(x => x.MessageId == ticketInfo.MessageId);
                stringBuilder.AppendFormat(XmlTemplate,
                    message.MessageId,
                    ticketInfo.Username,
                    ticketInfo.CustId,
                    ticketInfo.Recommend,
                    ticketInfo.Mrecommend,
                    ticketInfo.Srecommend,
                    ticketInfo.IsTest,
                    HttpUtility.HtmlEncode(message.Content_en),
                    HttpUtility.HtmlEncode(message.Content_cs),
                    HttpUtility.HtmlEncode(message.Content_th),
                    HttpUtility.HtmlEncode(message.Content_vn));
            }

            stringBuilder.Append(Constants.Constants.EndRootTag.ToLowerInvariant());
            var criteria = new ImportChatBotTicketVoidingInfoCommand
            {
                XmlData = stringBuilder.ToString(),
                Messages = voidingMessages.ToXML("root").ToString()
            };
            return await baseService.ExecuteNonQueryAsync(criteria);
        }
    }
}