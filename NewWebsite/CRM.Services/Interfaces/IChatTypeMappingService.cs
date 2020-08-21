namespace CRM.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CRM.Core.Models;
    using CRM.Criteria.ChatTypeMapping;
    using CRM.Models.ChatTypeMapping;

    public interface IChatTypeMappingService
    {
        Task<ValueTuple<IEnumerable<ChatTypeMappingModel>, int>> GetChatTypeMappingAsync(GetChatTypeMappingCriteria criteria);

        Task<CommandResult> UpdateActiveStatusAsync(int conversationId, bool IsActive);

        Task<IEnumerable<GroupHeadlineDirectAccountModel>> InsertPersonalMappingAsync(string recipientIds);

        Task<CommandResult> InsertPersonalMappingAsync(InsertPersonalMappingCommand criteria);
    }
}