namespace CRM.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CRM.Models;
    using CRM.Models.ChatBot;

    public interface IChatBotService
    {
        Task<IEnumerable<ConversationBot>> GetConversationBots();

        Task<IEnumerable<BaseModel>> GetChatTypes();

        Task<IEnumerable<BaseModel>> GetRecipientsByType(int recipientType);

        Task<IEnumerable<BaseModel>> GetRecipientTypes();

        Task<IEnumerable<BaseModel>> GetSystems();
    }
}