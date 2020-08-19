namespace CRM.Controllers.HeadlineNotification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CRM.Core.Constants;
    using CRM.Core.Enumerables;
    using CRM.Core.Extensions;
    using CRM.Core.Models;
    using CRM.Core.Services;
    using CRM.Models;
    using CRM.Models.Setting;
    using CRM.Services;
    using CRM.ViewModels.ChatTypeMapping;
    using Microsoft.AspNetCore.Mvc;

    public class ChatTypeMappingController : Controller
    {
        private readonly IBaseService baseService;
        private readonly ISettingService settingService;
        private readonly IChatBotService chatbotService;

        public ChatTypeMappingController(IBaseService baseService, ISettingService settingService, IChatBotService chatbotService)
        {
            this.baseService = baseService;
            this.settingService = settingService;
            this.chatbotService = chatbotService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetSettings()
        {
            var chatType = Enum.GetValues(typeof(ChatType)).Cast<ChatType>().Select(type => new BaseModel { Id = (int)type, Name = EnumHelper.GetDesciption(type) });
            var status = Enum.GetValues(typeof(MappingStatus)).Cast<MappingStatus>().Select(type => new BaseModel { Id = (int)type, Name = EnumHelper.GetDesciption(type) });

            var bots = chatbotService.GetConversationBots();
            var lang = settingService.GetAsync(new GetSettingParam { SettingName = SettingName.NotificationTemplateLanguage, IsActive = 1 });
            var recipientTypes = chatbotService.GetRecipientTypes();

            await Task.WhenAll(bots, lang, recipientTypes);

            var result = new
            {
                ChatTypes = chatType,
                MappingStatuses = status,
                Languages = lang,
                Bots = bots,
                RecipientTypes = recipientTypes
            };
            return Json(result);
        }

        [HttpPost]
        public IActionResult GetChatTypeMapping(ChatTypeFilterViewModel filter)
        {
            return Json(null);
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