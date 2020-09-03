namespace Fanex.CRM.BackgroundJob.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Fanex.CRM.BackgroundJob.Criteria;
    using Fanex.CRM.BackgroundJob.Models;

    public interface IMessageService
    {
        Task<IEnumerable<VoidingMessage>> GetVoidingMessageAsync();

        Task<IEnumerable<VoidingMessage>> StoreVoidingMessageAsync(IEnumerable<VoidingMessage> voidingMessages, int batchSize);

        Task<IEnumerable<VoidingMessage>> GetFailMessageAsync();

        Task<IEnumerable<Message>> GetMessageInfoAsync(IEnumerable<int> messageIds);

        Task<CommandResult> UpdateStatusVoidingMessageAsync(IEnumerable<VoidingMessage> voidingMessages);
    }

    public class MessageService : IMessageService
    {
        private readonly IBaseService baseService;

        public MessageService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        public async Task<IEnumerable<VoidingMessage>> GetVoidingMessageAsync()
        {
            var criteria = new GetVoidingMessageCriteria();
            return await baseService.GetDataAsync<VoidingMessage>(criteria);
        }

        public async Task<IEnumerable<VoidingMessage>> StoreVoidingMessageAsync(
            IEnumerable<VoidingMessage> voidingMessages,
            int batchSize)
        {
            const string XmlTemplate = "<r MsgID='{0}' ReqID='{1}' />";
            var stringBuilder = new StringBuilder(Constants.Constants.BeginRootTag);
            foreach(var message in voidingMessages)
            {
                stringBuilder.AppendFormat(XmlTemplate, message.MessageId, message.RequestId);
            }
            stringBuilder.Append(Constants.Constants.EndRootTag);

            var criteria = new StoreVoidingMessageCriteria
            {
                MessageIds = stringBuilder.ToString(),
                BatchSize = batchSize
            };
            return await baseService.GetDataAsync<VoidingMessage>(criteria);
        }

        public async Task<IEnumerable<VoidingMessage>> GetFailMessageAsync()
        {
            var criteria = new GetFailMessageCriteria();
            return await baseService.GetDataAsync<VoidingMessage>(criteria);
        }

        public async Task<IEnumerable<Message>> GetMessageInfoAsync(IEnumerable<int> messageIds)
        {
            var criteria = new GetMessageInfoCriteria
            {
                MessageIds = string.Join(",", messageIds)
            };

            var messages = (await baseService.GetDataAsync<Message>(criteria)).ToList();
            foreach (Message message in messages)
            {
                message.Content_en = message.Content_en?.Trim().Replace("@@", string.Empty);
                message.Content_cs = message.Content_cs?.Trim().Replace("@@", string.Empty);
            }

            return messages;
        }

        public async Task<CommandResult> UpdateStatusVoidingMessageAsync(IEnumerable<VoidingMessage> voidingMessages)
        {
            const string XmlTemplate = "<r MsgID='{0}' ReqID='{1}' />";
            var stringBuilder = new StringBuilder(Constants.Constants.BeginRootTag);
            foreach (var message in voidingMessages)
            {
                stringBuilder.AppendFormat(XmlTemplate, message.MessageId, message.RequestId);
            }
            stringBuilder.Append(Constants.Constants.EndRootTag);
            var criteria = new UpdateStatusVoidingMessageCriteria
            {
                MessageIds = stringBuilder.ToString()
            };
            return await baseService.ExecuteNonQueryAsync(criteria);
        }
    }
}