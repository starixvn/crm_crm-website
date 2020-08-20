namespace CRM.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CRM.Criteria.Common;
    using CRM.Models;
    using CRM.Models.ChatBot;

    public interface ICommonService
    {
        Task<IEnumerable<ConversationBot>> GetConversationBots();

        Task<IEnumerable<BaseModel>> GetRecipients(GetRecipientsCriteria criteria);

        Task<IEnumerable<BaseModel>> GetRecipientTypes();

        Task<IEnumerable<BaseModel>> GetSystems();

        IEnumerable<BaseModel> GetChatTypes();

        IEnumerable<BaseModel> GetMappingStatus();

        Task<IEnumerable<BaseModel>> GetHeadlines(GetHeadlinesCriteria criteria);
    }
}