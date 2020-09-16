namespace CRM.Controllers.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture;
    using CRM.Core.Constants;
    using CRM.Core.Models;
    using CRM.Criteria.ChatTypeMapping;
    using CRM.Criteria.Common;
    using CRM.Letstalk.Client.Models;
    using CRM.Letstalk.Client.Models.Results;
    using CRM.Letstalk.Client.Services.Interfaces;
    using CRM.Models;
    using CRM.Models.ChatTypeMapping;
    using CRM.Models.Common;
    using CRM.Models.Setting;
    using CRM.Resources;
    using CRM.Services;
    using CRM.Services.Interfaces;
    using CRM.ViewModels.ChatTypeMapping;
    using Fanex.NAP.Client.Configuration;
    using Fanex.NAP.Client.Helpers.Web;
    using Fanex.NAP.Client.Services;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using NSubstitute;
    using Xunit;
    using static Fanex.NAP.Core.Business.Constant;

    public class ChatTypeMappingControllerTests
    {
        private INapSession napSession;
        private INapSettings napSettings;
        private IUserService userService;
        private ISettingService settingService;
        private ICommonService commonService;
        private IChatTypeMappingService chatTypeMappingService;
        private Func<int, ILetstalkService> letstalkService;

        [Fact]
        public void Index_WhenCalled_ReturnExpectedViewResult()
        {
            // Arrange
            var controller = GetController();
            userService.IsHavingPermission(napSession.UserId, napSettings.SiteId, FunctionKey.ChatTypeMapping, Policy.Host).Returns(true);

            // Act
            var actual = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(actual);
            var viewModel = Assert.IsType<bool>(viewResult.Model);
            Assert.True(viewModel);
        }

        [Fact]
        public async Task GetSettings_WhenCalled_ReturnExpectedJsonResult()
        {
            var fixture = new Fixture();
            var bots = fixture.Create<IEnumerable<BaseModel>>();
            var langs = fixture.Create<IEnumerable<Setting>>();
            var recipientTypes = fixture.Create<IEnumerable<BaseModel>>();
            var chatTypes = fixture.Create<IEnumerable<BaseModel>>();
            var mappingStatues = fixture.Create<IEnumerable<BaseModel>>();
            var languages = langs.Select(l => new BaseModel { Id = l.SettingId, Name = l.SettingValue });

            var expected = new
            {
                ChatTypes = chatTypes,
                MappingStatuses = mappingStatues,
                Languages = languages,
                Bots = bots,
                RecipientTypes = recipientTypes,
            };

            // Arrange
            var controller = GetController();
            commonService.GetConversationBotAsync().Returns(bots);
            settingService.GetAsync(Arg.Any<GetSettingParam>()).Returns(langs);
            commonService.GetRecipientTypeAsync().Returns(recipientTypes);
            commonService.GetChatTypes().Returns(chatTypes);
            commonService.GetMappingStatus().Returns(mappingStatues);

            // Act
            var actual = await controller.GetSettings();

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(actual);
            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(jsonResult.Value));
        }

        [Fact]
        public async Task GetChatTypeMapping_WhenCalled_ReturnExpectedJsonResult()
        {
            var fixture = new Fixture();
            var mappingData = fixture.Create<IEnumerable<ChatTypeMappingModel>>();
            var totalRows = fixture.Create<int>();

            var controler = GetController();

            chatTypeMappingService.GetChatTypeMappingAsync(Arg.Any<GetChatTypeMappingCriteria>())
                .Returns(new ValueTuple<IEnumerable<ChatTypeMappingModel>, int>(mappingData, totalRows));

            var actual = await controler.GetChatTypeMapping(fixture.Create<ChatTypeFilterViewModel>());
            // Assert
            var jsonResult = Assert.IsType<JsonResult>(actual);
            var mappings = (IEnumerable<ChatTypeMappingModel>)jsonResult.Value.GetType().GetProperty("data").GetValue(jsonResult.Value);
            var totalItems = (int)jsonResult.Value.GetType().GetProperty("TotalItems").GetValue(jsonResult.Value);
            Assert.Equal(mappings, mappingData);
            Assert.Equal(totalItems, totalRows);
        }

        [Fact]
        public async Task UpdateActiveStatus_WhenCalled_ReturnExpectedJsonCommandResult()
        {
            var expected = new CommandResult { Code = 0 };
            var controller = GetController();

            chatTypeMappingService.EditActiveStatusAsync(Arg.Any<int>(), Arg.Any<bool>(), Arg.Any<int>()).Returns(expected);

            var actual = await controller.UpdateActiveStatus(new ActiveStatusModel { ConversationGroupId = 1, IsActive = true });

            var jsonResult = Assert.IsType<JsonResult>(actual);
            var result = (CommandResult)jsonResult.Value;
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task InsertPersonalMapping_WhenInvalidLetstalkId_ReturnExpectedCommandResult()
        {
            var fixture = new Fixture();
            var letstalkResult = new ValueTuple<Contact, bool>(new Contact(), false);
            var controller = GetController();
            var invalidLetstalk = fixture.Create<string>();
            letstalkService(Arg.Any<int>()).GetProfileAsync(Arg.Any<string>()).Returns(letstalkResult);

            var actual = await controller.InsertPersonalMapping(new PersonalMappingViewModel { BotId = 100020, LetstalkId = invalidLetstalk });

            var jsonResult = Assert.IsType<JsonResult>(actual);
            var commandResult = (CommandResult)jsonResult.Value;
            Assert.False(commandResult.IsSuccessful);
            Assert.Equal(string.Format(ViewResources.LetstalkInvalidMessage, invalidLetstalk), commandResult.Message);
        }

        [Fact]
        public async Task InsertPersonalMapping_WhenValidLetstalkId_ReturnExpectedCommandResult()
        {
            var fixture = new Fixture();
            var expected = fixture.Create<CommandResult>();
            var letstalkResult = new ValueTuple<Contact, bool>(new Contact(), true);
            var viewModal = fixture.Create<PersonalMappingViewModel>();
            viewModal.RecipientIds = "1";

            var controller = GetController();

            letstalkService(Arg.Any<int>()).GetProfileAsync(Arg.Any<string>()).Returns(letstalkResult);
            chatTypeMappingService.CreatePersonalMappingAsync(Arg.Any<InsertPersonalMappingCommand>()).Returns(expected);

            var actual = await controller.InsertPersonalMapping(viewModal);

            var jsonResult = Assert.IsType<JsonResult>(actual);
            var commandResult = (CommandResult)jsonResult.Value;
            Assert.False(commandResult.IsSuccessful);
        }

        [Fact]
        public async Task UpdatePersonalMapping_WhenInvalidLetstalkId_ReturnExpectedCommandResult()
        {
            var fixture = new Fixture();
            var letstalkResult = new ValueTuple<Contact, bool>(new Contact(), false);
            var controller = GetController();
            var invalidLetstalk = fixture.Create<string>();
            letstalkService(Arg.Any<int>()).GetProfileAsync(Arg.Any<string>()).Returns(letstalkResult);

            var actual = await controller.UpdatePersonalMapping(new PersonalMappingViewModel { BotId = 100020, LetstalkId = invalidLetstalk });

            var jsonResult = Assert.IsType<JsonResult>(actual);
            var commandResult = (CommandResult)jsonResult.Value;
            Assert.False(commandResult.IsSuccessful);
            Assert.Equal(string.Format(ViewResources.LetstalkInvalidMessage, invalidLetstalk), commandResult.Message);
        }

        [Fact]
        public async Task UpdatePersonalMapping_WhenValidLetstalkId_ReturnExpectedCommandResult()
        {
            var fixture = new Fixture();
            var expected = fixture.Create<CommandResult>();
            var letstalkResult = new ValueTuple<Contact, bool>(new Contact(), true);
            var viewModal = fixture.Create<PersonalMappingViewModel>();
            viewModal.RecipientIds = "1";

            var controller = GetController();

            letstalkService(Arg.Any<int>()).GetProfileAsync(Arg.Any<string>()).Returns(letstalkResult);
            chatTypeMappingService.EditChatTypeMappingAsync(Arg.Any<UpdateChatTypeMappingCommand>()).Returns(expected);

            var actual = await controller.UpdatePersonalMapping(viewModal);

            var jsonResult = Assert.IsType<JsonResult>(actual);
            var commandResult = (CommandResult)jsonResult.Value;
            Assert.Equal(expected, commandResult);
        }

        [Fact]
        public async Task GetRecipientByType_WhenCalled_ReturnExpectedRecipients()
        {
            var fixture = new Fixture();

            var expected = fixture.Create<IEnumerable<RecipientModel>>();

            var controller = GetController();
            commonService.GetRecipientAsync(Arg.Any<GetRecipientsCriteria>()).Returns(expected);

            var actual = await controller.GetRecipientByType(1);

            var jsonResult = Assert.IsType<JsonResult>(actual);
            var result = (IEnumerable<RecipientModel>)jsonResult.Value;
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetGroupHeadlineByRecipient_WhenCalled_ReturnExpectedHeadlineList()
        {
            var fixture = new Fixture();

            var expected = fixture.Create<IEnumerable<GroupHeadlineDirectAccountModel>>();

            var controller = GetController();
            chatTypeMappingService.GetHeadlineByRecipientAsync(Arg.Any<string>()).Returns(expected);

            var actual = await controller.GetGroupHeadlineByRecipient("1,2,3,4,5");

            var jsonResult = Assert.IsType<JsonResult>(actual);
            var result = (IEnumerable<GroupHeadlineDirectAccountModel>)jsonResult.Value;
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task UpdateGroupMapping_WhenValidLetstalkId_ReturnExpectedCommandResult()
        {
            var fixture = new Fixture();
            var expected = fixture.Create<CommandResult>();
            var viewModal = fixture.Create<PersonalMappingViewModel>();

            var controller = GetController();

            chatTypeMappingService.EditChatTypeMappingAsync(Arg.Any<UpdateChatTypeMappingCommand>()).Returns(expected);

            var actual = await controller.UpdateGroupMapping(viewModal);

            var jsonResult = Assert.IsType<JsonResult>(actual);
            var commandResult = (CommandResult)jsonResult.Value;
            Assert.Equal(expected, commandResult);
        }

        [Fact]
        public async Task VerifyLetstalkInGroup_WhenNoBotId_ReturnNullJsonResult()
        {
            var controller = GetController();
            var actual = await controller.VerifyLetstalkInGroup(new LetstalkVerification { BotId = 0 });

            var jsonResult = Assert.IsType<JsonResult>(actual);
            Assert.Null(jsonResult.Value);
        }

        [Fact]
        public async Task VerifyLetstalkInGroup_WhenNoGroupId_ReturnNullJsonResult()
        {
            var controller = GetController();
            var actual = await controller.VerifyLetstalkInGroup(new LetstalkVerification { BotId = 10, GroupConversationId = string.Empty });

            var jsonResult = Assert.IsType<JsonResult>(actual);
            Assert.Null(jsonResult.Value);
        }

        [Fact]
        public async Task VerifyLetstalkInGroup_WhenValidBotIdAndGroupId_ReturnExpectedJsonResult()
        {
            var fixture = new Fixture();
            var botFriends = fixture.Create<IList<string>>();
            var roomData = fixture.Create<RoomData>();
            LetstalkVerification inputData = new LetstalkVerification
            {
                BotId = 100020,
                GroupConversationId = "TestingGroupID",
                Recipients = new List<RecipientLetstalk>
                {
                    new RecipientLetstalk
                    {
                        RecipientId =1,
                        RecipientName = "TestName",
                        Letstalks = new List<LetstalkInGroup>
                        {
                            new LetstalkInGroup { LetstalkId = roomData.Members[0]},
                            new LetstalkInGroup { LetstalkId = botFriends[1]},
                            new LetstalkInGroup { LetstalkId = "test"},
                        }
                    }
                }
            };

            var expected = JsonConvert.DeserializeObject<List<RecipientLetstalk>>(JsonConvert.SerializeObject(inputData.Recipients));
            expected.FirstOrDefault().Letstalks[0].IsGroupMember = true;
            expected.FirstOrDefault().Letstalks[1].IsBotFriend = true;
            var controller = GetController();

            letstalkService(Arg.Any<int>()).GetBotContactList().Returns(botFriends);
            letstalkService(Arg.Any<int>()).GetChatRoomData(Arg.Any<string>()).Returns(roomData);

            var actual = await controller.VerifyLetstalkInGroup(inputData);

            var jsonResult = Assert.IsType<JsonResult>(actual);
            var result = (List<RecipientLetstalk>)jsonResult.Value;
            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public async Task GetLetstalkDisplayName_WhenInvalidLetstalkId_ReturnEmptyString()
        {
            var contactDetail = new ValueTuple<Contact, bool>(new Contact(), false);
            var controller = GetController();
            letstalkService(Arg.Any<int>()).GetProfileAsync(Arg.Any<string>()).Returns(contactDetail);
            var actual = await controller.GetLetstalkDisplayName(1, "abc");
            Assert.Equal(string.Empty, actual);
        }

        [Fact]
        public async Task GetLetstalkDisplayName_WhenValidLetstalkId_ReturnExpectedName()
        {
            var fixture = new Fixture();
            var expectedName = fixture.Create<string>();
            var contactDetail = new ValueTuple<Contact, bool>(new Contact { DisplayName = expectedName }, true);

            var controller = GetController();
            letstalkService(Arg.Any<int>()).GetProfileAsync(Arg.Any<string>()).Returns(contactDetail);

            var actual = await controller.GetLetstalkDisplayName(1, "abc");

            Assert.Equal(expectedName, actual);
        }

        #region helper

        private ChatTypeMappingController GetController()
        {
            userService = Substitute.For<IUserService>();
            napSession = Substitute.For<INapSession>();
            napSettings = Substitute.For<INapSettings>();
            settingService = Substitute.For<ISettingService>();
            commonService = Substitute.For<ICommonService>();
            chatTypeMappingService = Substitute.For<IChatTypeMappingService>();
            letstalkService = Substitute.For<Func<int, ILetstalkService>>();

            return new ChatTypeMappingController(napSession, napSettings, userService, settingService, commonService, chatTypeMappingService, letstalkService);
        }

        #endregion helper
    }
}