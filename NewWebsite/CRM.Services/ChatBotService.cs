namespace CRM.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CRM.Core.Services;
    using CRM.Criteria.ChatBot;
    using CRM.Models;
    using CRM.Models.ChatBot;
    using CRM.Services.Interfaces;

    public class ChatBotService : IChatBotService
    {
        private readonly IBaseService baseService;

        public ChatBotService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        public async Task<IEnumerable<ConversationBot>> GetConversationBots()
        {
            var criteria = new GetConversationBotsCriteria();
            var result = await baseService.GetDataAsync<ConversationBot>(criteria);
            return result;
        }

        public async Task<IEnumerable<BaseModel>> GetChatTypes()
        {
            var criteria = new GetChatTypesCriteria();
            var result = await baseService.GetDataAsync<BaseModel>(criteria);
            return result;
        }

        public async Task<IEnumerable<BaseModel>> GetRecipientsByType(int recipientType)
        {
            var criteria = new GetRecipientsByTypeCriteria { RecipientType = recipientType };
            var result = await baseService.GetDataAsync<BaseModel>(criteria);
            return result;
        }

        public async Task<IEnumerable<BaseModel>> GetRecipientTypes()
        {
            var criteria = new GetRecipientTypesCriteria();
            var result = await baseService.GetDataAsync<BaseModel>(criteria);
            return result;
        }

        public async Task<IEnumerable<BaseModel>> GetSystems()
        {
            var criteria = new GetSystemsCriteria();
            var result = await baseService.GetDataAsync<BaseModel>(criteria);
            return result;
        }
    }
}