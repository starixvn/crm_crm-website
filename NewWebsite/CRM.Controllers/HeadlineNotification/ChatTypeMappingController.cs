namespace CRM.Controllers.HeadlineNotification
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using CRM.Core.Constants;
    using CRM.Core.Enumerables;
    using CRM.Core.Filters;
    using CRM.Core.Models;
    using CRM.Criteria.ChatTypeMapping;
    using CRM.Criteria.Common;
    using CRM.Letstalk.Client.Services.Interfaces;
    using CRM.Models;
    using CRM.Models.Setting;
    using CRM.Services;
    using CRM.Services.Interfaces;
    using CRM.ViewModels.ChatTypeMapping;
    using Fanex.NAP.Client.Configuration;
    using Fanex.NAP.Client.Helpers.Web;
    using Fanex.NAP.Client.Services;
    using Microsoft.AspNetCore.Mvc;
    using static Fanex.NAP.Core.Business.Constant;

    [CrmAuthorize]
    [CrmPermission(FunctionKey = FunctionKey.NotificationList, Policy = Policy.Read)]
    public class ChatTypeMappingController : Controller
    {
        private readonly INapSession napSession;
        private readonly INapSettings napSettings;
        private readonly IUserService userService;
        private readonly ISettingService settingService;
        private readonly ICommonService commonService;
        private readonly IChatTypeMappingService chatTypeMappingService;
        private readonly Func<int, ILetstalkService> letstalkService;

        public ChatTypeMappingController(
            INapSession napSession,
            INapSettings napSettings,
            IUserService userService,
            ISettingService settingService,
            ICommonService commonService,
            IChatTypeMappingService chatTypeMappingService,
            Func<int, ILetstalkService> letstalkService)
        {
            this.napSession = napSession;
            this.napSettings = napSettings;
            this.userService = userService;
            this.settingService = settingService;
            this.commonService = commonService;
            this.chatTypeMappingService = chatTypeMappingService;
            this.letstalkService = letstalkService;
        }

        public IActionResult Index()
        {
            var isHost = userService.IsHavingPermission(napSession.UserId, napSettings.SiteId, FunctionKey.NotificationTemplate, Policy.Host);
            return View(isHost);
        }

        public async Task<IActionResult> GetSettings()
        {
            var bots = commonService.GetConversationBotAsync();
            var lang = settingService.GetAsync(new GetSettingParam { SettingName = SettingName.NotificationTemplateLanguage, IsActive = 1 });
            var recipientTypes = commonService.GetRecipientTypeAsync();

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
                BotIds = filter.BotIds,
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
            try
            {
                var commandResult = await chatTypeMappingService.UpdateActiveStatusAsync(groupInfo.ConversationGroupId, groupInfo.IsActive, 8);
                return Json(commandResult);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertPersonalMapping([FromBody]PersonalMappingViewModel data)
        {
            var result = new CommandResult();
            var contactDetail = await letstalkService(data.BotId).GetProfileAsync(data.LetstalkId.Trim());
            if (contactDetail.Item2)
            {
                var criteria = new InsertPersonalMappingCommand
                {
                    BotId = data.BotId,
                    ConversationGroupName = contactDetail.Item1.DisplayName,
                    ChatTypeId = (int)ChatType.Personal,
                    RecipientTypeId = data.RecipientTypeId,
                    HeadlineIds = data.GroupHeadlineIds,
                    RecipientId = data.RecipientId,
                    LetstalkId = data.LetstalkId,
                    LanguageId = data.LanguageId,
                    Remark = data.Remark,
                    UserId = 8
                };
                result = await chatTypeMappingService.InsertPersonalMappingAsync(criteria);
            }
            else
            {
                result.Message = "The selected Letstalk ID does not existed";
            }

            return Json(result);
        }

        public async Task<IActionResult> GetRecipientByType(int recipientTypeId)
        {
            var recipients = await commonService.GetRecipientAsync(new GetRecipientsCriteria { RecipientType = recipientTypeId });
            return Json(recipients);
        }

        public async Task<IActionResult> GetGroupHeadlineByRecipient(string recipientIds)
        {
            try
            {
                var headlines = await chatTypeMappingService.GetHeadlineByRecipientAsync(recipientIds);
                return Json(headlines);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePersonalMapping([FromBody]PersonalMappingViewModel data)
        {
            var result = new CommandResult();
            var contactDetail = await letstalkService(data.BotId).GetProfileAsync(data.LetstalkId.Trim());
            if (contactDetail.Item2)
            {
                var criteria = new UpdateChatTypeMappingCommand
                {
                    ConversationGroupId = data.ConversationGroupId,
                    BotId = data.BotId,
                    ConversationGroupName = contactDetail.Item1.DisplayName,
                    RecipientTypeId = data.RecipientTypeId,
                    HeadlineIds = data.GroupHeadlineIds,
                    RecipientIds = data.RecipientId.ToString(),
                    LetstalkId = data.LetstalkId,
                    LanguageId = data.LanguageId,
                    Remark = data.Remark,
                    UserId = 8
                };
                result = await chatTypeMappingService.UpdateChatTypeMappingAsync(criteria);
            }
            else
            {
                result.Message = "The selected Letstalk ID does not existed";
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateGroupMapping([FromBody]PersonalMappingViewModel data)
        {
            var criteria = new UpdateChatTypeMappingCommand
            {
                ConversationGroupId = data.ConversationGroupId,
                BotId = data.BotId,
                ConversationGroupName = data.Name,
                RecipientTypeId = data.RecipientTypeId,
                HeadlineIds = data.GroupHeadlineIds,
                RecipientIds = data.RecipientIds,
                LetstalkId = data.LetstalkId,
                LanguageId = data.LanguageId,
                Remark = data.Remark,
                UserId = 8
            };
            var result = await chatTypeMappingService.UpdateChatTypeMappingAsync(criteria);

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyLetstalkInGroup([FromBody]LetstalkVerification data)
        {
            data.GroupConversationId = "KQrsF8lVV1vrcLDPHJKYQ_Eq8Zh5kd0A7x9Gb4nsC1yZeCnMDkfq9IPKobj3UQmK";
            if (data.BotId > 0 && !string.IsNullOrWhiteSpace(data.GroupConversationId))
            {
                var botFriends = await letstalkService(data.BotId).GetBotContactList();
                var roomData = await letstalkService(data.BotId).GetChatRoomData(data.GroupConversationId);
                foreach (var recipient in data.Recipients)
                {
                    foreach (var letstalk in recipient.Letstalks)
                    {
                        letstalk.IsGroupMember = roomData.Members.Contains(letstalk.LetstalkId.Trim());
                        letstalk.IsBotFriend = botFriends.Contains(letstalk.LetstalkId.Trim());
                    }
                }

                return Json(data.Recipients);
            }

            return Json(null);
        }
    }
}