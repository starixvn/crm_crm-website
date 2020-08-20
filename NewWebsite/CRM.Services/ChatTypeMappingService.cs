namespace CRM.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CRM.Core.Models;
    using CRM.Core.Services;
    using CRM.Criteria.ChatTypeMapping;
    using CRM.Models.ChatTypeMapping;
    using CRM.Services.Interfaces;
    using Fanex.NAP.Client.Helpers.Web;

    public class ChatTypeMappingService : IChatTypeMappingService
    {
        private readonly IBaseService baseService;
        private readonly INapSession napSession;

        public ChatTypeMappingService(IBaseService baseService,
            INapSession napSession)
        {
            this.baseService = baseService;
            this.napSession = napSession;
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

        public async Task<CommandResult> UpdateActiveStatusAsync(int conversationId, bool IsActive)
        {
            var criteria = new UpdateIsActiveStatusCommand
            {
                ConversationGroupId = conversationId,
                IsActive = IsActive,
                UserId = napSession.UserId
            };

            return await baseService.ExecuteNonQueryAsync(criteria);
        }

        public async Task<IEnumerable<GroupHeadlineDirectAccountModel>> GetHeadlineByRecipient(string recipientIds)
        {
            return await baseService.GetDataAsync<GroupHeadlineDirectAccountModel>(new GetGroupHeadlineByRecipientCriteria { RecipientIds = recipientIds });
        }
    }
}