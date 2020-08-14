namespace CRM.Controllers.HeadlineNotification
{
    using System.Collections.Generic;
    using CRM.Core.Models;
    using CRM.Models;
    using CRM.Models.ChatTypeMapping;
    using CRM.ViewModels.ChatTypeMapping;
    using Microsoft.AspNetCore.Mvc;

    public class ChatTypeMappingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetSettings()
        {
            var chatType = new List<BaseModel>
            {
                new BaseModel { Id = 1, Name = "Group" },
                new BaseModel { Id = 2, Name = "Personal" }
            };

            var bots = new List<BaseModel>
            {
                new BaseModel { Id = 100020, Name = "247_Notification" },
            };

            var lang = new List<BaseModel>
            {
                new BaseModel { Id = 1, Name = "EN" },
                new BaseModel { Id = 2, Name = "SC" },
                new BaseModel { Id = 3, Name = "EN & SC" },
            };

            var status = new List<BaseModel>
            {
                new BaseModel { Id = 1, Name = "Not Mapped" },
                new BaseModel { Id = 2, Name = "Mapped" },
                new BaseModel { Id = 3, Name = "Bot Removed" },
            };

            var recipientTypes = new List<BaseModel>
            {
                new BaseModel { Id = 1, Name = "Pic Of Headline" }
            };

            var groupHeadline = new List<BaseModel>
            {
                new BaseModel { Id = 1, Name = "Headline 01" },
                new BaseModel { Id = 2, Name = "Headline 02" }
            };

            var result = new
            {
                ChatTypes = chatType,
                MappingStatuses = status,
                Languages = lang,
                Bots = bots,
                RecipientTypes = recipientTypes,
                GroupHeadline = groupHeadline
            };
            return Json(result);
        }

        [HttpPost]
        public IActionResult GetChatTypeMapping(ChatTypeFilterViewModel filter)
        {
            var fakeData = new List<ChatTypeMappingModel>
            {
                new ChatTypeMappingModel { ChatTypeId = 0, ChatType = "Personal", BotName = "247_Notification", ConversationId = 1, Name = "Headline1",
                    RecipientType = "PIC of Headline", GroupHeadline = "HL01", Recipient = "Boss(Boss)", LetstalkId = "letstalk0", Language = "SC",
                    IsActive = true, Remark = "ABC"},

                new ChatTypeMappingModel { ChatTypeId = 1,ChatType = "Group", BotName = "247_Notification", ConversationId = 2, Name = "Headline2",
                    RecipientType = "PIC of Headline", GroupHeadline = "HL02", Recipient = "Boss1(Boss)", LetstalkId = "letstalk3", Language = "SC",
                    MappingStatusId = 1, MappingStatus = "Not Mapped", IsActive = true, Remark = "ABC"},

                new ChatTypeMappingModel {ChatTypeId = 1, ChatType = "Group", BotName = "247_Notification", ConversationId = 2, Name = "Headline2",
                    RecipientType = "PIC of Headline", GroupHeadline = "HL02", Recipient = "Boss2(Staff)", LetstalkId = "letstalk1",
                    Language = "EN", MappingStatusId = 1, MappingStatus = "Not Mapped", IsActive = true, Remark = "ABC"},

                new ChatTypeMappingModel {ChatTypeId = 1, ChatType = "Group", BotName = "247_Notification", ConversationId = 2, Name = "Headline2",
                    RecipientType = "PIC of Headline", GroupHeadline = "HL02", Recipient = "Boss3(PIC)", LetstalkId = "letstalk2", Language = "EN & SC",
                    MappingStatusId = 1, MappingStatus = "Not Mapped", IsActive = true, Remark = "ABC"},

                new ChatTypeMappingModel {ChatTypeId = 1, ChatType = "Group", BotName = "247_Notification", ConversationId = 3, Name = "Headline3",
                    RecipientType = "PIC of Headline", GroupHeadline = "HL03", Recipient = "Boss4(Boss)", LetstalkId = "letstalk3", Language = "SC",
                    MappingStatusId = 0, MappingStatus = "Bot Removed", IsActive = false, Remark = "ABC"},

                new ChatTypeMappingModel {ChatTypeId = 1, ChatType = "Group", BotName = "247_Notification", ConversationId = 3, Name = "Headline3",
                    RecipientType = "PIC of Headline", GroupHeadline = "HL01", Recipient = "Boss5(Staff)", LetstalkId = "letstalk4", Language = "EN",
                    MappingStatusId = 0, MappingStatus = "Bot Removed", IsActive = false, Remark = "ABC"},
                new ChatTypeMappingModel {ChatTypeId = 1, ChatType = "Group", BotName = "247_Notification", ConversationId = 4, Name = "HeadlineN",
                    RecipientType = "PIC of Headline", GroupHeadline = "HL007", Recipient = "Boss9(Staff)", LetstalkId = "letstalk7", Language = "EN",
                    MappingStatusId = 2, MappingStatus = "Mapped", IsActive = false, Remark = "ABC"},
            };

            return Json(fakeData);
        }

        [HttpPost]
        public IActionResult UpdateActiveStatus(int ConversationId, bool IsActive)
        {
            var result = new CommandResult { Code = 0, Message = "Success" };
            return Json(result);
        }

        [HttpPost]
        public IActionResult UpdateMapping(object data)
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