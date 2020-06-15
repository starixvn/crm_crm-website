namespace Fanex.CRM.BackgroundJob.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Fanex.CRM.BackgroundJob.Criteria;
    using Fanex.CRM.BackgroundJob.Models;

    public interface IMessageService
    {
        Task<IEnumerable<int>> GetVoidingMessageAsync();

        Task<IEnumerable<int>> StoreVoidingMessageAsync(IEnumerable<int> messageIds, int batchSize);

        Task<IEnumerable<int>> GetFailMessageAsync();

        Task<IEnumerable<Message>> GetMessageInfoAsync(IEnumerable<int> messageIds);

        Task<CommandResult> UpdateStatusVoidingMessageAsync(IEnumerable<int> messageIds);
    }

    public class MessageService : IMessageService
    {
        private readonly IBaseService baseService;

        public MessageService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        public async Task<IEnumerable<int>> GetVoidingMessageAsync()
        {
            var criteria = new GetVoidingMessageCriteria();
            return await baseService.GetDataAsync<int>(criteria);
        }

        public async Task<IEnumerable<int>> StoreVoidingMessageAsync(IEnumerable<int> messageIds, int batchSize)
        {
            var criteria = new StoreVoidingMessageCriteria
            {
                MessageIds = string.Join(",", messageIds),
                BatchSize = batchSize
            };
            return await baseService.GetDataAsync<int>(criteria);
        }

        public async Task<IEnumerable<int>> GetFailMessageAsync()
        {
            var criteria = new GetFailMessageCriteria();
            return await baseService.GetDataAsync<int>(criteria);
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

        public async Task<CommandResult> UpdateStatusVoidingMessageAsync(IEnumerable<int> messageIds)
        {
            var criteria = new UpdateStatusVoidingMessageCriteria
            {
                MessageIds = string.Join(",", messageIds)
            };
            return await baseService.ExecuteNonQueryAsync(criteria);
        }
    }
}