namespace CRM.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CRM.Core.Services;
    using CRM.Criteria.ChatTypeMapping;
    using CRM.Models.ChatTypeMapping;
    using CRM.Services.Interfaces;

    public class ChatTypeMappingService : IChatTypeMappingService
    {
        private readonly IBaseService baseService;

        public ChatTypeMappingService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        public async Task<ValueTuple<IEnumerable<ChatTypeMappingModel>, int>> GetChatTypeMappingAsync(GetChatTypeMappingCriteria criteria)
        {
            var data = await baseService.FetchMultipleAsync<ChatTypeMappingModel, ConversationHeadlineModel>(criteria);
            if (data.Item1.Any())
            {
                foreach (var item in data.Item1)
                {
                    item.Headlines = data.Item2.Where(headline => headline.ConversationGroupId == item.ConversationGroupId).ToList();
                }
            }

            return new ValueTuple<IEnumerable<ChatTypeMappingModel>, int>(data.Item1, criteria.TotalRows);
        }
    }
}