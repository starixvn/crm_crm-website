namespace CRM.Controllers.HeadlineNotification
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CRM.Core.Constants;
    using CRM.Core.Models;
    using CRM.Criteria.ChatTypeMapping;
    using CRM.Criteria.Common;
    using CRM.Models;
    using CRM.Models.Setting;
    using CRM.Services;
    using CRM.Services.Interfaces;
    using CRM.ViewModels.ChatTypeMapping;
    using Microsoft.AspNetCore.Mvc;

    public class ChatTypeMappingController : Controller
    {
        private readonly ISettingService settingService;
        private readonly ICommonService commonService;
        private readonly IChatTypeMappingService chatTypeMappingService;

        public ChatTypeMappingController(ISettingService settingService,
            ICommonService commonService,
            IChatTypeMappingService chatTypeMappingService)
        {
            this.settingService = settingService;
            this.commonService = commonService;
            this.chatTypeMappingService = chatTypeMappingService;
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
            var headlines = commonService.GetHeadlines(new GetHeadlinesCriteria { Username = string.Empty });

            await Task.WhenAll(bots, lang, recipientTypes, headlines);

            var result = new
            {
                ChatTypes = commonService.GetChatTypes(),
                MappingStatuses = commonService.GetMappingStatus(),
                Languages = lang,
                Bots = bots,
                RecipientTypes = recipientTypes,
                GroupHeadline = headlines
            };
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetChatTypeMapping(ChatTypeFilterViewModel filter)
        {
            var criteria = new GetChatTypeMappingCriteria
            {
                ChatTypeId = filter.ChatType,
                ConversationGroupName = filter.Name,
                Headline = filter.GroupHeadline,
                RecipientName = filter.RecipientIds,
                LetstalkId = filter.LetstalkId,
                LanguageIds = filter.LanguageIds,
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
        public IActionResult UpdateActiveStatus(int ConversationId, bool IsActive)
        {
            var result = new CommandResult { Code = 0, Message = "Success" };
            return Json(result);
        }

        [HttpPost]
        public IActionResult GetMappingByConversationId(int conversationId)
        {
            var result = new CommandResult { Code = 0, Message = "Success" };
            return Json(result);
        }

        [HttpPost]
        public IActionResult InsertPersonalMapping(object data)
        {
            var result = new CommandResult { Code = 0, Message = "Success" };
            return Json(result);
        }

        public IActionResult GetRecipientByGroupHeadline(string headlineIds)
        {
            var recipients = new List<BaseModel>
            {
                new BaseModel { Id = 1, Name = "Recipient1" },
                new BaseModel { Id = 2, Name = "Recipient1" }
            };

            return Json(recipients);
        }
    }
}