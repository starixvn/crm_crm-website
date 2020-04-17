namespace Fanex.CRM.BackgroundJob.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.CRM.BackgroundJob.Criteria;
    using Fanex.CRM.BackgroundJob.Models;

    public interface IMessageService
    {
        Task<IEnumerable<Message>> GetNewMessageAsync(long lastMessageId, int batchSize);
    }

    public class MessageService : IMessageService
    {
        private readonly IBaseService baseService;

        public MessageService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        public async Task<IEnumerable<Message>> GetNewMessageAsync(long lastMessageId, int batchSize)
        {
            var criteria = new GetNewMessageCriteria
            {
                LastMessageId = lastMessageId,
                BatchSize = batchSize
            };
            return await baseService.GetDataAsync<Message>(criteria);
        }
    }
}