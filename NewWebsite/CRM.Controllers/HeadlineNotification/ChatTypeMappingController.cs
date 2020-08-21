namespace CRM.Controllers.HeadlineNotification
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using CRM.Core.Constants;
    using CRM.Core.Enumerables;
    using CRM.Criteria.ChatTypeMapping;
    using CRM.Criteria.Common;
    using CRM.Models;
    using CRM.Models.ChatTypeMapping;
    using CRM.Models.Setting;
    using CRM.Services;
    using CRM.Services.Interfaces;
    using CRM.ViewModels.ChatTypeMapping;
    using Fanex.Letstalk.Client.Services.Interfaces;
    using Fanex.NAP.Client.Configuration;
    using Fanex.NAP.Client.Helpers.Web;
    using Fanex.NAP.Client.Services;
    using Microsoft.AspNetCore.Mvc;

    public class ChatTypeMappingController : Controller
    {
        private readonly INapSession napSession;
        private readonly INapSettings napSettings;
        private readonly IUserService userService;
        private readonly ISettingService settingService;
        private readonly ICommonService commonService;
        private readonly IChatTypeMappingService chatTypeMappingService;
        private readonly Func<int, ILetstalkClientService> letsttalkClient;

        public ChatTypeMappingController(
            INapSession napSession,
            INapSettings napSettings,
            IUserService userService,
            ISettingService settingService,
            ICommonService commonService,
            IChatTypeMappingService chatTypeMappingService,
            Func<int, ILetstalkClientService> letsttalkClient)
        {
            this.napSession = napSession;
            this.napSettings = napSettings;
            this.userService = userService;
            this.settingService = settingService;
            this.commonService = commonService;
            this.chatTypeMappingService = chatTypeMappingService;
            this.letsttalkClient = letsttalkClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetSettings()
        {
            var bots = commonService.GetConversationBots();
            var lang = settingService.GetAsync(new GetSettingParam { SettingName = SettingName.NotificationTemplateLanguage, IsActive = 1 });
            var recipientTypes = commonService.GetRecipientTypes();

            await Task.WhenAll(bots, lang, recipientTypes);

            var result = new
            {
                ChatTypes = commonService.GetChatTypes(),
                MappingStatuses = commonService.GetMappingStatus(),
                Languages = (await lang).Select(l => new BaseModel { Id = l.SettingId, Name = l.SettingValue }),
                Bots = await bots,
                RecipientTypes = await recipientTypes,
            };
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetChatTypeMapping([FromBody]ChatTypeFilterViewModel filter)
        {
            var criteria = new GetChatTypeMappingCriteria
            {
                ChatTypeId = filter.ChatType,
                ConversationGroupName = filter.Name,
                Headline = filter.GroupHeadline,
                RecipientName = filter.RecipientIds,
                LetstalkId = filter.LetstalkId,
                LanguageId = filter.LanguageId,
                IsActive = filter.IsActive,
                ConversationBotIds = filter.BotIds,
                MappingStatus = filter.MappingStatuses,
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize
            };

            var data = await chatTypeMappingService.GetChatTypeMappingAsync(criteria);
            return Json(new
            {
                data = data.Item1,
                TotalItems = data.Item2
            });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateActiveStatus([FromBody]ActiveStatusModel groupInfo)
        {
            var commandResult = await chatTypeMappingService.UpdateActiveStatusAsync(groupInfo.ConversationGroupId, groupInfo.IsActive);
            return Json(commandResult);
        }

        [HttpPost]
        public async Task<IActionResult> InsertPersonalMapping([FromBody]PersonalMappingViewModel data)
        {
            var criteria = new InsertPersonalMappingCommand
            {
                ConversationBotId = data.BotId,
                ConversationGroupName = data.Name,
                ChatTypeId = (int)ChatType.Personal,
                RecipientTypeId = data.RecipientTypeId,
                HeadlineIds = data.GroupHeadlineIds,
                RecipientId = data.RecipientId,
                LetstalkId = data.LetstalkId,
                LanguageId = data.LanguageId,
                Remark = data.Remark,
                UserId = napSession.UserId
            };
            var result = await chatTypeMappingService.InsertPersonalMappingAsync(criteria);
            return Json(result);
        }

        public async Task<IActionResult> GetRecipientByType(int recipientTypeId)
        {
            var recipients = await commonService.GetRecipients(new GetRecipientsCriteria { RecipientType = recipientTypeId });
            return Json(recipients);
        }

        public async Task<IActionResult> GetGroupHeadlineByRecipient(string recipientIds)
        {
            var headlines = await chatTypeMappingService.GetHeadlineByRecipientAsync(recipientIds);
            return Json(headlines);
        }

        public async Task<IActionResult> ValidateLetstalkIDs(int botId, string letstalkId)
        {
            var botFriendList = await letsttalkClient(botId).GetBotContactList();
            var contactDetail = await letsttalkClient(botId).GetProfileAsync(letstalkId.Trim());
            var result = new LetsalkValidationModel
            {
                BotId = botId,
                IsFriendWithBot = botFriendList.Contains(letstalkId),
                DisplayName = contactDetail.Item2 ? contactDetail.Item1.DisplayName : string.Empty,
                IsInvalid = contactDetail.Item2,
                LetstalkId = contactDetail.Item1.UserId
            };
            return Json(result);
        }
    }
}