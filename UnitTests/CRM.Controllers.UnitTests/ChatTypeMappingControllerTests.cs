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
    using CRM.Letstalk.Client.Models;
    using CRM.Letstalk.Client.Services.Interfaces;
    using CRM.Models;
    using CRM.Models.ChatTypeMapping;
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

            chatTypeMappingService.UpdateActiveStatusAsync(Arg.Any<int>(), Arg.Any<bool>(), Arg.Any<int>()).Returns(expected);

            var actual = await controller.UpdateActiveStatus(new ActiveStatusModel { ConversationGroupId = 1, IsActive = true });

            var jsonResult = Assert.IsType<JsonResult>(actual);
            var message = (string)jsonResult.Value.GetType().GetProperty("Message").GetValue(jsonResult.Value);
            Assert.Equal(ViewResources.UpdateActiveStatusSuccess, message);
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
            chatTypeMappingService.InsertPersonalMappingAsync(Arg.Any<InsertPersonalMappingCommand>()).Returns(expected);

            var actual = await controller.InsertPersonalMapping(viewModal);

            var jsonResult = Assert.IsType<JsonResult>(actual);
            var commandResult = (CommandResult)jsonResult.Value;
            Assert.False(commandResult.IsSuccessful);
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