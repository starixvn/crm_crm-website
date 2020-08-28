namespace CRM.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CRM.Core.Enumerables;
    using CRM.Core.Extensions;
    using CRM.Core.Models;
    using CRM.Core.Services;
    using CRM.Criteria.Common;
    using CRM.Models;
    using CRM.Models.ChatBot;
    using CRM.Models.Common;
    using Microsoft.Extensions.Options;

    public class CommonService : ICommonService
    {
        private readonly IBaseService baseService;
        private readonly AppSettings appSettings;

        public CommonService(
            IBaseService baseService,
            IOptionsSnapshot<AppSettings> appSettings)
        {
            this.baseService = baseService;
            this.appSettings = appSettings.Value;
        }

        public async Task<IEnumerable<ConversationBot>> GetConversationBotAsync()
        {
            var criteria = new GetConversationBotsCriteria();
            return await baseService.GetDataAsync<ConversationBot>(criteria);
        }

        public async Task<IEnumerable<RecipientModel>> GetRecipientAsync(GetRecipientsCriteria criteria)
        {
            return await baseService.GetDataAsync<RecipientModel>(criteria);
        }

        public async Task<IEnumerable<BaseModel>> GetRecipientTypeAsync()
        {
            var criteria = new GetRecipientTypesCriteria();
            return await baseService.GetDataAsync<BaseModel>(criteria);
        }

        public async Task<IEnumerable<BaseModel>> GetSystemAsync()
        {
            var criteria = new GetSystemsCriteria();
            return await baseService.GetDataAsync<BaseModel>(criteria);
        }

        public async Task<IEnumerable<BaseModel>> GetHeadlineAsync(GetHeadlinesCriteria criteria)
        {
            return await baseService.GetDataAsync<BaseModel>(criteria);
        }

        public IEnumerable<BaseModel> GetChatTypes()
        {
            return Enum.GetValues(typeof(ChatType)).Cast<ChatType>()
                .Select(type => new BaseModel { Id = (int)type, Name = EnumHelper.GetDesciption(type) });
        }

        public IEnumerable<BaseModel> GetMappingStatus()
        {
            return Enum.GetValues(typeof(MappingStatus)).Cast<MappingStatus>()
                .Select(type => new BaseModel { Id = (int)type, Name = EnumHelper.GetDesciption(type) });
        }

        public DateTime ConvertToClientDateTime(DateTime date)
        {
            var timeSpan = new TimeSpan(appSettings.ClientTimezone, 0, 0);
            return date.ToUniversalTime().Add(timeSpan);
        }

        public DateTime ConvertToServerDateTime(DateTime date)
        {
            var timeSpan = new TimeSpan(appSettings.ServerTimezone, 0, 0);
            return date.ToUniversalTime().Add(timeSpan);
        }
    }
}