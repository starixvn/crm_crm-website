namespace CRM.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CRM.Criteria.Common;
    using CRM.Models;
    using CRM.Models.ChatBot;
    using CRM.Models.Common;

    public interface ICommonService
    {
        Task<IEnumerable<ConversationBot>> GetConversationBotAsync();

        Task<IEnumerable<RecipientModel>> GetRecipientAsync(GetRecipientsCriteria criteria);

        Task<IEnumerable<BaseModel>> GetRecipientTypeAsync();

        Task<IEnumerable<BaseModel>> GetSystemAsync();

        IEnumerable<BaseModel> GetChatTypes();

        IEnumerable<BaseModel> GetMappingStatus();

        Task<IEnumerable<BaseModel>> GetHeadlineAsync(GetHeadlinesCriteria criteria);
    }
}