namespace CRM.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CRM.Core.Services;
    using CRM.Criteria.ChatBot;
    using CRM.Criteria.Common;
    using CRM.Models;
    using CRM.Models.ChatBot;

    public class CommonService : ICommonService
    {
        private readonly IBaseService baseService;

        public CommonService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        public async Task<IEnumerable<ConversationBot>> GetConversationBots()
        {
            var criteria = new GetConversationBotsCriteria();
            var result = await baseService.GetDataAsync<ConversationBot>(criteria);
            return result;
        }

        public async Task<IEnumerable<BaseModel>> GetRecipients(GetRecipientsCriteria criteria)
        {
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

        public async Task<IEnumerable<BaseModel>> GetHeadlines(GetHeadlinesCriteria criteria)
        {
            var result = await baseService.GetDataAsync<BaseModel>(criteria);
            return result;
        }
    }
}