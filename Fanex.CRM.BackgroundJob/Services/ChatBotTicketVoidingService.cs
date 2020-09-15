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

    public interface IChatBotTicketVoidingService
    {
        Task ExecuteJobAsync(AbnormalBetSystemSetting setting);

        Task<IEnumerable<VoidingMessage>> GetVoidingMessageAsync();

        Task<IEnumerable<VoidingMessage>> ImportVoidingMessageAsync(IEnumerable<VoidingMessage> voidingMessages, int batchSize);

        Task<CommandResult> UpdateVoidingMessageAsync(IEnumerable<VoidingMessage> voidingMessages);

        Task<IEnumerable<Message>> GetMessageInfoAsync(IEnumerable<VoidingMessage> voidingMessages);

        Task<IEnumerable<ChatBotVoidingTicketInfo>> GetTicketVoidingInfoAsync(IEnumerable<VoidingMessage> voidingMessages);

        Task<CommandResult> ImportTicketVoidingAsync(IEnumerable<Message> messages, IEnumerable<ChatBotVoidingTicketInfo> ticketInfos);
    }

    public class ChatBotTicketVoidingService : IChatBotTicketVoidingService
    {
        private readonly IBaseService baseService;
        private readonly ICRMLogService logService;

        public ChatBotTicketVoidingService(
            IBaseService baseService,
            ICRMLogService logService)
        {
            this.baseService = baseService;
            this.logService = logService;
        }

        public async Task ExecuteJobAsync(AbnormalBetSystemSetting setting)
        {
            IEnumerable<VoidingMessage> voidingMessages = await GetVoidingMessageAsync();
            voidingMessages = await ImportVoidingMessageAsync(voidingMessages, setting.BatchSize);
            if (voidingMessages.Any())
            {
                IEnumerable<Message> messages = await GetMessageInfoAsync(voidingMessages);
                IEnumerable<ChatBotVoidingTicketInfo> ticketInfos = await GetTicketVoidingInfoAsync(voidingMessages);
                CommandResult commandResult = await ImportTicketVoidingAsync(messages, ticketInfos);
                if (commandResult.IsSuccessful)
                {
                    await UpdateVoidingMessageAsync(voidingMessages);
                }
                else
                {
                    logService.LogInformation(commandResult.Message);
                }
            }
        }

        public async Task<IEnumerable<VoidingMessage>> GetVoidingMessageAsync()
        {
            var criteria = new GetChatBotVoidingMessageCriteria();
            return await baseService.GetDataAsync<VoidingMessage>(criteria);
        }

        public async Task<IEnumerable<VoidingMessage>> ImportVoidingMessageAsync(
            IEnumerable<VoidingMessage> voidingMessages,
            int batchSize)
        {
            var criteria = new ImportChatBotVoidingMessageCriteria
            {
                Messages = ParseToXml(voidingMessages),
                BatchSize = batchSize
            };
            return await baseService.GetDataAsync<VoidingMessage>(criteria);
        }

        public async Task<CommandResult> UpdateVoidingMessageAsync(IEnumerable<VoidingMessage> voidingMessages)
        {
            var criteria = new UpdateChatBotVoidingMessageCommand
            {
                Messages = ParseToXml(voidingMessages)
            };
            return await baseService.ExecuteNonQueryAsync(criteria);
        }

        public async Task<IEnumerable<Message>> GetMessageInfoAsync(IEnumerable<VoidingMessage> voidingMessages)
        {
            var criteria = new GetChatBotMessageInfoCriteria
            {
                Messages = ParseToXml(voidingMessages)
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

        public async Task<IEnumerable<ChatBotVoidingTicketInfo>> GetTicketVoidingInfoAsync(IEnumerable<VoidingMessage> voidingMessages)
        {
            var criteria = new GetChatBotTicketVoidingInfoCriteria
            {
                Messages = ParseToXml(voidingMessages)
            };
            return await baseService.GetDataAsync<ChatBotVoidingTicketInfo>(criteria);
        }

        public async Task<CommandResult> ImportTicketVoidingAsync(IEnumerable<Message> messages, IEnumerable<ChatBotVoidingTicketInfo> ticketInfos)
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
                XmlData = stringBuilder.ToString()
            };
            return await baseService.ExecuteNonQueryAsync(criteria);
        }

        private static string ParseToXml(IEnumerable<VoidingMessage> voidingMessages)
        {
            const string XmlTemplate = "<r MessageID='{0}' RequestID='{1}' />";
            var stringBuilder = new StringBuilder(Constants.Constants.BeginRootTag.ToLowerInvariant());
            foreach (var message in voidingMessages)
            {
                stringBuilder.AppendFormat(XmlTemplate, message.MessageId, message.RequestId);
            }

            stringBuilder.Append(Constants.Constants.EndRootTag.ToLowerInvariant());
            return stringBuilder.ToString();
        }
    }
}