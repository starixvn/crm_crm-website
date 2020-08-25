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

    public class ChatTypeMappingService : IChatTypeMappingService
    {
        private readonly IBaseService baseService;

        public ChatTypeMappingService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        public async Task<ValueTuple<IEnumerable<ChatTypeMappingModel>, int>> GetChatTypeMappingAsync(GetChatTypeMappingCriteria criteria)
        {
            var data = await baseService.FetchMultipleAsync<ChatTypeMappingModel, ConversationHeadlineModel, ConversationRecipient>(criteria);
            if (data.Item1.Any())
            {
                foreach (var item in data.Item1)
                {
                    item.Headlines = data.Item2.Where(headline => headline.ConversationGroupId == item.ConversationGroupId);
                    item.Recipients = data.Item3.Where(recipient => recipient.ConversationGroupId == item.ConversationGroupId);
                }
            }

            return new ValueTuple<IEnumerable<ChatTypeMappingModel>, int>(data.Item1, criteria.TotalRows);
        }

        public async Task<CommandResult> UpdateActiveStatusAsync(int conversationId, bool IsActive, int userId)
        {
            var criteria = new UpdateIsActiveStatusCommand
            {
                ConversationGroupId = conversationId,
                IsActive = IsActive,
                UserId = userId
            };

            return await baseService.ExecuteNonQueryAsync(criteria);
        }

        public async Task<IEnumerable<GroupHeadlineDirectAccountModel>> GetHeadlineByRecipientAsync(string recipientIds)
        {
            var criteria = new GetGroupHeadlineByRecipientCriteria
            {
                RecipientIds = recipientIds,
                IsGetDirectAccount = true
            };
            var tuple = await baseService.FetchMultipleAsync<GroupHeadlineDirectAccountModel, string>(criteria);
            foreach (var item in tuple.Item1)
            {
                if (item.Id == 0)
                {
                    item.DirectAccounts = tuple.Item2;
                }
            }
            return tuple.Item1;
        }

        public async Task<CommandResult> InsertPersonalMappingAsync(InsertPersonalMappingCommand criteria)
        {
            return await baseService.ExecuteNonQueryAsync(criteria);
        }

        public async Task<CommandResult> UpdateChatTypeMappingAsync(UpdateChatTypeMappingCommand criteria)
        {
            return await baseService.ExecuteNonQueryAsync(criteria);
        }
    }
}