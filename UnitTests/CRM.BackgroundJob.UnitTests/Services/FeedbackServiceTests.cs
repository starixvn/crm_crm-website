namespace CRM.BackgroundJob.UnitTests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AutoFixture;
    using CRM.BackgroundJob.UnitTests.Testable;
    using CRM.Core.Models;
    using CRM.Core.Services;
    using Fanex.CRM.BackgroundJob.Constants;
    using Fanex.CRM.BackgroundJob.Criteria;
    using Fanex.CRM.BackgroundJob.Models;
    using Fanex.CRM.BackgroundJob.Services;
    using Microsoft.Extensions.Options;
    using NSubstitute;
    using Xunit;

    public class FeedbackServiceTests
    {
        private AppJobSettings appSettings;
        private IBaseService baseService;
        private ISettingService settingService;
        private ISystemSettingService systemSettingService;
        private IMessageService messageService;
        private ICustomerService customerService;
        private ICRMLogService logService;
        private IFeedbackTicketVoidingService ticketVoidingService;
        private readonly IFixture fixture = new Fixture();

        #region CreateFeedbackForVoidingAbnormalBetAsync

        [Fact]
        public async Task CreateFeedbackForVoidingAbnormalBetAsync_NotExistedVoidingMessage_ExpectedUpdateSystemSetting()
        {
            // Arrange
            var setting = fixture.Create<AbnormalBetSystemSetting>();
            var voidingMessages = new List<VoidingMessage>();
            var service = MockService();
            messageService.GetVoidingMessageAsync().Returns(voidingMessages);
            messageService.ImportVoidingMessageAsync(voidingMessages, setting.BatchSize).Returns(voidingMessages);
            messageService.GetFailMessageAsync().Returns(voidingMessages);

            // Act
            await service.CreateFeedbackForVoidingAbnormalBetAsync(setting);

            // Assert
            service.Received().Protected("EditSystemSettingAsync");
            service.DidNotReceive().Protected(
                "CreateFeedbackForCreditSiteAsync",
                Arg.Any<Message>(),
                Arg.Any<MatchInfo>(),
                Arg.Any<IEnumerable<TransactionInfo>>(),
                Arg.Any<IEnumerable<GroupHeadline>>(),
                Arg.Any<IEnumerable<SettingItem>>(),
                Arg.Any<IEnumerable<SystemSettingMapping>>());
            service.DidNotReceive().Protected(
                "CreateFeedbackForDepositSiteAsync",
                Arg.Any<CustomerInfo>(),
                Arg.Any<Message>(),
                Arg.Any<MatchInfo>(),
                Arg.Any<IEnumerable<TransactionInfo>>(),
                Arg.Any<IEnumerable<SettingItem>>(),
                Arg.Any<IEnumerable<SystemSettingMapping>>());
        }

        [Fact]
        public async Task CreateFeedbackForVoidingAbnormalBetAsync_NotExistedTicketVoiding_ExpectedUpdateSystemSetting()
        {
            // Arrange
            var setting = fixture.Create<AbnormalBetSystemSetting>();
            var voidingMessages = fixture.Create<IEnumerable<VoidingMessage>>();
            var matchInfos = new List<MatchInfo>();
            var transactionInfos = fixture.Create<IEnumerable<TransactionInfo>>();
            var ticketInfo = new ValueTuple<IEnumerable<MatchInfo>, IEnumerable<TransactionInfo>>(matchInfos, transactionInfos);
            var service = MockService();
            messageService.GetVoidingMessageAsync().Returns(voidingMessages);
            messageService.ImportVoidingMessageAsync(voidingMessages, setting.BatchSize).Returns(voidingMessages);
            messageService.GetFailMessageAsync().Returns(voidingMessages);
            ticketVoidingService.GetTicketVoidingInfoAsync(Arg.Any<IEnumerable<VoidingMessage>>(), setting).Returns(ticketInfo);

            // Act
            await service.CreateFeedbackForVoidingAbnormalBetAsync(setting);

            // Assert
            service.Received().Protected("EditSystemSettingAsync");
            service.DidNotReceive().Protected(
                "CreateFeedbackForCreditSiteAsync",
                Arg.Any<Message>(),
                Arg.Any<MatchInfo>(),
                Arg.Any<IEnumerable<TransactionInfo>>(),
                Arg.Any<IEnumerable<GroupHeadline>>(),
                Arg.Any<IEnumerable<SettingItem>>(),
                Arg.Any<IEnumerable<SystemSettingMapping>>());
            service.DidNotReceive().Protected(
                "CreateFeedbackForDepositSiteAsync",
                Arg.Any<CustomerInfo>(),
                Arg.Any<Message>(),
                Arg.Any<MatchInfo>(),
                Arg.Any<IEnumerable<TransactionInfo>>(),
                Arg.Any<IEnumerable<SettingItem>>(),
                Arg.Any<IEnumerable<SystemSettingMapping>>());
        }

        [Fact]
        public async Task CreateFeedbackForVoidingAbnormalBetAsync_ExistedTicketVoiding_ExpectedCreateFeedbackForCreditAndDeposit()
        {
            // Arrange
            var setting = fixture.Create<AbnormalBetSystemSetting>();
            var voidingMessages = fixture.Create<IEnumerable<VoidingMessage>>();
            var messages = new List<Message>
            {
                new Message { MessageId = 1 }
            };
            var matchInfos = new List<MatchInfo>
            {
                new MatchInfo { MessageId = 1 }
            };
            var transactionInfos = new List<TransactionInfo>
            {
                new TransactionInfo { MessageId = 1, CustId = 1 },
                new TransactionInfo { MessageId = 1, IsDeposit = true }
            };
            var ticketInfo = new ValueTuple<IEnumerable<MatchInfo>, IEnumerable<TransactionInfo>>(matchInfos, transactionInfos);
            var settings = fixture.Create<IEnumerable<SettingItem>>();
            var settingMappings = fixture.Create<IEnumerable<SystemSettingMapping>>();
            var siteSettingMappings = fixture.Create<IEnumerable<SiteSettingMapping>>();
            var licenseeInfo = fixture.Create<CustomerInfo>();
            var headlines = new List<GroupHeadline>
            {
                new GroupHeadline { CustId = 1 }
            };
            var service = MockService();
            messageService.GetVoidingMessageAsync().Returns(voidingMessages);
            messageService.ImportVoidingMessageAsync(voidingMessages, setting.BatchSize).Returns(voidingMessages);
            messageService.GetFailMessageAsync().Returns(voidingMessages);
            messageService.GetMessageInfoAsync(Arg.Any<IEnumerable<int>>()).Returns(messages);
            settingService.GetAllSettingAsync().Returns(settings);
            settingService.GetSystemSettingMappingAsync(Arg.Any<string[]>()).Returns(settingMappings);
            settingService.GetSiteSettingMappingAsync().Returns(siteSettingMappings);
            customerService.GetCustomerInfoAsync(appSettings.LicenseeUsername, true).Returns(licenseeInfo);
            customerService.GetGroupHeadlineAsync(Arg.Any<IEnumerable<int>>()).Returns(headlines);

            ticketVoidingService.GetTicketVoidingInfoAsync(Arg.Any<IEnumerable<VoidingMessage>>(), setting).Returns(ticketInfo);
            ticketVoidingService.ImportTicketVoidingInfoAsync(
                Arg.Any<IEnumerable<VoidingMessage>>(),
                ticketInfo.Item1,
                ticketInfo.Item2,
                siteSettingMappings,
                headlines,
                licenseeInfo).Returns(ticketInfo);

            // Act
            await service.CreateFeedbackForVoidingAbnormalBetAsync(setting);

            // Assert
            logService.Received().LogInformation(Arg.Any<string>());
            service.Received().Protected("EditSystemSettingAsync");
            service.Received().Protected(
                "CreateFeedbackForCreditSiteAsync",
                Arg.Any<Message>(),
                Arg.Any<MatchInfo>(),
                Arg.Any<IEnumerable<TransactionInfo>>(),
                Arg.Any<IEnumerable<GroupHeadline>>(),
                settings,
                settingMappings);
            service.Received().Protected(
                "CreateFeedbackForDepositSiteAsync",
                licenseeInfo,
                Arg.Any<Message>(),
                Arg.Any<MatchInfo>(),
                Arg.Any<IEnumerable<TransactionInfo>>(),
                settings,
                settingMappings);
        }

        #endregion CreateFeedbackForVoidingAbnormalBetAsync

        [Fact]
        public async Task CreateFeedbackForCreditSiteAsync_WhenCalled_ExpectedCallCreateFeedbackForCreditSiteByBatchAsync()
        {
            // Arrange
            var message = fixture.Create<Message>();
            var matchInfo = fixture.Create<MatchInfo>();
            var transactionInfos = fixture.Create<IEnumerable<TransactionInfo>>();
            var headlines = new List<GroupHeadline>
            {
                new GroupHeadline { HeadlineId = 1, CustId = 1 }
            };
            var settings = new List<SettingItem>
            {
                new SettingItem { SettingId = 1, SettingName = "Status", SettingValue = "Closed" },
                new SettingItem { SettingId = 2, SettingName = "Response", SettingValue = "Neutral" },
                new SettingItem { SettingId = 2022, SettingName = "Station" }
            };
            var settingMappings = fixture.Create<IEnumerable<SystemSettingMapping>>();
            var service = MockService();
            service.Protected("GetCategorySubCategory", settings).Returns(new Tuple<int, int, string>(1, 1, "xml"));

            // Act
            await service.Public_CreateFeedbackForCreditSiteAsync(
                message,
                matchInfo,
                transactionInfos,
                headlines,
                settings,
                settingMappings);

            // Assert
            logService.Received(2).LogInformation(Arg.Any<string>());
            service.Received().Protected(
                "CreateFeedbackForCreditSiteByBatchAsync",
                Arg.Any<GroupHeadline>(),
                Arg.Any<IEnumerable<int>>(),
                message,
                matchInfo,
                transactionInfos,
                settings,
                settingMappings,
                Arg.Any<CreateMutipleFeedbackCommand>());
        }

        #region CreateFeedbackForCreditSiteByBatchAsync

        [Fact]
        public async Task CreateFeedbackForCreditSiteByBatchAsync_CreateSuccessful_ExpectedWriteLog()
        {
            // Arrange
            var headline = new GroupHeadline { HeadlineId = 1 };
            var relatedCustomerIds = new List<int> { 1 };
            var message = new Message { MessageId = 1, RequestId = 1 };
            var matchInfo = fixture.Create<MatchInfo>();
            var transactionInfos = new List<TransactionInfo>
            {
                new TransactionInfo { RequestId = 1, CustId = 1 }
            };
            var settings = fixture.Create<IEnumerable<SettingItem>>();
            var settingMappings = fixture.Create<IEnumerable<SystemSettingMapping>>();
            var criteria = fixture.Create<CreateMutipleFeedbackCommand>();
            var commandResult = new CommandResult { Code = 0 };
            var service = MockService();
            baseService.ExecuteNonQueryAsync(criteria).Returns(commandResult);
            service.Protected("GetProductXml", matchInfo, Arg.Any<IEnumerable<TransactionInfo>>(), settings, settingMappings).Returns("product xml");
            service.Protected("GetSolutionForCreditSite", message, Arg.Any<IEnumerable<TransactionInfo>>()).Returns("solution");
            service.Protected("GetRelatedCustomerXml", headline.HeadlineId, Arg.Any<IEnumerable<TransactionInfo>>()).Returns("related customer xml");
            service.Protected("GetDescriptionForCreditSite", message.MessageId, "1", matchInfo).Returns("description");

            // Act
            await service.Public_CreateFeedbackForCreditSiteByBatchAsync(
                headline,
                relatedCustomerIds,
                message,
                matchInfo,
                transactionInfos,
                settings,
                settingMappings,
                criteria);

            // Assert
            Assert.Equal("product xml", criteria.ProductXml);
            Assert.Equal("solution", criteria.Solution);
            Assert.Equal("related customer xml", criteria.RelatedCustomerXml);
            Assert.Equal(message.MessageId, criteria.MessageId);
            Assert.Equal("1", criteria.ListOrigCustIds);
            Assert.Equal("1", criteria.RequestIds);
            Assert.Equal("description", criteria.Description);
            logService.Received(4).LogInformation(Arg.Any<string>());
        }

        [Fact]
        public async Task CreateFeedbackForCreditSiteByBatchAsync_CreateUnsuccessful_ExpectedWriteLogAndUpdateStatus()
        {
            // Arrange
            var headline = new GroupHeadline { HeadlineId = 1 };
            var relatedCustomerIds = new List<int> { 1 };
            var message = new Message { MessageId = 1, RequestId = 1 };
            var matchInfo = fixture.Create<MatchInfo>();
            var transactionInfos = new List<TransactionInfo>
            {
                new TransactionInfo { RequestId = 1, CustId = 1 }
            };
            var settings = fixture.Create<IEnumerable<SettingItem>>();
            var settingMappings = fixture.Create<IEnumerable<SystemSettingMapping>>();
            var criteria = fixture.Create<CreateMutipleFeedbackCommand>();
            var commandResult = new CommandResult();
            var service = MockService();
            baseService.ExecuteNonQueryAsync(criteria).Returns(commandResult);

            // Act
            await service.Public_CreateFeedbackForCreditSiteByBatchAsync(
                headline,
                relatedCustomerIds,
                message,
                matchInfo,
                transactionInfos,
                settings,
                settingMappings,
                criteria);

            // Assert
            logService.Received(4).LogInformation(Arg.Any<string>());
            await ticketVoidingService.EditFeedbackInfoAsync(message.MessageId, Arg.Any<IEnumerable<int>>(), Arg.Any<IEnumerable<int>>(), CreateFeedbackJobStatus.Fail, commandResult.Message);
            await logService.ErrorAsync(commandResult, criteria);
        }

        [Fact]
        public async Task CreateFeedbackForCreditSiteByBatchAsync_Exception_ExpectedWriteLogAndUpdateStatus()
        {
            // Arrange
            var exception = fixture.Create<Exception>();
            var headline = new GroupHeadline { HeadlineId = 1 };
            var relatedCustomerIds = new List<int> { 1 };
            var message = new Message { MessageId = 1, RequestId = 1 };
            var matchInfo = fixture.Create<MatchInfo>();
            var transactionInfos = new List<TransactionInfo>
            {
                new TransactionInfo { RequestId = 1, CustId = 1 }
            };
            var settings = fixture.Create<IEnumerable<SettingItem>>();
            var settingMappings = fixture.Create<IEnumerable<SystemSettingMapping>>();
            var criteria = fixture.Create<CreateMutipleFeedbackCommand>();
            var service = MockService();
            baseService.When(x => x.ExecuteNonQueryAsync(criteria)).Throw(exception);

            // Act
            await service.Public_CreateFeedbackForCreditSiteByBatchAsync(
                headline,
                relatedCustomerIds,
                message,
                matchInfo,
                transactionInfos,
                settings,
                settingMappings,
                criteria);

            // Assert
            logService.Received(4).LogInformation(Arg.Any<string>());
            await ticketVoidingService.EditFeedbackInfoAsync(message.MessageId, Arg.Any<IEnumerable<int>>(), Arg.Any<IEnumerable<int>>(), CreateFeedbackJobStatus.Fail, exception.Message);
            await logService.ErrorAsync(exception);
        }

        #endregion CreateFeedbackForCreditSiteByBatchAsync

        [Fact]
        public async Task CreateFeedbackForDepositSiteAsync_WhenCalled_ExpectedCallCreateFeedbackForDepositByBatchAsync()
        {
            // Arrange
            var licenseeInfo = fixture.Create<CustomerInfo>();
            var message = fixture.Create<Message>();
            var matchInfo = fixture.Create<MatchInfo>();
            var transactionInfos = fixture.Create<IEnumerable<TransactionInfo>>();
            var settings = new List<SettingItem>
            {
                new SettingItem { SettingId = 1, SettingName = "Status", SettingValue = "Closed" },
                new SettingItem { SettingId = 2, SettingName = "Response", SettingValue = "Neutral" },
                new SettingItem { SettingId = 2022, SettingName = "Station" }
            };
            var settingMappings = fixture.Create<IEnumerable<SystemSettingMapping>>();
            var service = MockService();

            // Act
            await service.Public_CreateFeedbackForDepositSiteAsync(
                licenseeInfo,
                message,
                matchInfo,
                transactionInfos,
                settings,
                settingMappings);

            // Assert
            logService.Received(2).LogInformation(Arg.Any<string>());
            service.Received().Protected("GetCommunicationTool", settings);
            service.Received().Protected("GetCategorySubCategory", settings);
            service.Received().Protected("GetProductXml", matchInfo, transactionInfos, settings, settingMappings);
            service.Received().Protected("CreateFeedbackForDepositByBatchAsync", licenseeInfo, message, transactionInfos, Arg.Any<DateTime>(), Arg.Any<string>(), Arg.Any<CreateFeedbackCommand>());
        }

        #region CreateFeedbackForDepositByBatchAsync

        [Fact]
        public async Task CreateFeedbackForDepositByBatchAsync_CreateSuccessful_ExpectedWriteLog()
        {
            // Arrange
            var licenseeInfo = new CustomerInfo { CustId = 1 };
            var message = new Message { RequestId = 1 };
            var feedbackDate = fixture.Create<DateTime>();
            var communicationTool = fixture.Create<string>();
            var criteria = fixture.Create<CreateFeedbackCommand>();
            var transactionInfos = new List<TransactionInfo>
            {
                new TransactionInfo { RequestId = 1, CustId = 1, LicenseeManagerId = 1, LicenseeManager = "starixIT" }
            };
            var commandResult = new CommandResult { Code = 0 };
            var service = MockService();
            baseService.ExecuteNonQueryAsync(criteria).Returns(commandResult);
            service.Protected("GetAutoField", feedbackDate, communicationTool, Arg.Any<IList<string>>()).Returns("auto field");

            // Act
            await service.Public_CreateFeedbackForDepositByBatchAsync(licenseeInfo, message, transactionInfos, feedbackDate, communicationTool, criteria);

            // Assert
            Assert.Equal("1", criteria.RelatedCustId);
            Assert.Equal("auto field", criteria.AutoField);
            Assert.Equal(message.MessageId, criteria.MessageId);
            Assert.Equal("1", criteria.ListOrigCustIds);
            Assert.Equal("1", criteria.RequestIds);
            logService.Received(2).LogInformation(Arg.Any<string>());
        }

        [Fact]
        public async Task CreateFeedbackForDepositByBatchAsync_CreateUnsuccessful_ExpectedWriteLogAndUpdateStatus()
        {
            // Arrange
            var licenseeInfo = new CustomerInfo { CustId = 1 };
            var message = new Message { RequestId = 1 };
            var feedbackDate = fixture.Create<DateTime>();
            var communicationTool = fixture.Create<string>();
            var criteria = fixture.Create<CreateFeedbackCommand>();
            var transactionInfos = new List<TransactionInfo>
            {
                new TransactionInfo { RequestId = 1, CustId = 1, LicenseeManagerId = 1, LicenseeManager = "starixIT" }
            };
            var commandResult = new CommandResult();
            var service = MockService();
            baseService.ExecuteNonQueryAsync(criteria).Returns(commandResult);
            service.Protected("GetAutoField", feedbackDate, communicationTool, Arg.Any<IList<string>>()).Returns("auto field");

            // Act
            await service.Public_CreateFeedbackForDepositByBatchAsync(licenseeInfo, message, transactionInfos, feedbackDate, communicationTool, criteria);

            // Assert
            logService.Received(2).LogInformation(Arg.Any<string>());
            await ticketVoidingService.EditFeedbackInfoAsync(message.MessageId, Arg.Any<IEnumerable<int>>(), Arg.Any<IEnumerable<int>>(), CreateFeedbackJobStatus.Fail, commandResult.Message);
            await logService.ErrorAsync(commandResult, criteria);
        }

        [Fact]
        public async Task CreateFeedbackForDepositByBatchAsync_Exception_ExpectedWriteLogAndUpdateStatus()
        {
            // Arrange
            var exception = fixture.Create<Exception>();
            var licenseeInfo = new CustomerInfo { CustId = 1 };
            var message = new Message { RequestId = 1 };
            var feedbackDate = fixture.Create<DateTime>();
            var communicationTool = fixture.Create<string>();
            var criteria = fixture.Create<CreateFeedbackCommand>();
            var transactionInfos = new List<TransactionInfo>
            {
                new TransactionInfo { RequestId = 1, CustId = 1, LicenseeManagerId = 1, LicenseeManager = "starixIT" }
            };
            var service = MockService();
            baseService.When(x => x.ExecuteNonQueryAsync(criteria)).Throw(exception);
            service.Protected("GetAutoField", feedbackDate, communicationTool, Arg.Any<IList<string>>()).Returns("auto field");

            // Act
            await service.Public_CreateFeedbackForDepositByBatchAsync(licenseeInfo, message, transactionInfos, feedbackDate, communicationTool, criteria);

            // Assert
            logService.Received(2).LogInformation(Arg.Any<string>());
            await ticketVoidingService.EditFeedbackInfoAsync(message.MessageId, Arg.Any<IEnumerable<int>>(), Arg.Any<IEnumerable<int>>(), CreateFeedbackJobStatus.Fail, exception.Message);
            await logService.ErrorAsync(exception);
        }

        #endregion CreateFeedbackForDepositByBatchAsync

        #region GetLicenseeManager

        [Fact]
        public void GetLicenseeManager_ExistedMapping_ReturnExpectedLicenseeManager()
        {
            // Arrange
            var transactionInfos = new List<TransactionInfo>
            {
                new TransactionInfo { Site = "Site" }
            };
            var siteSettingMappings = new List<SiteSettingMapping>
            {
                new SiteSettingMapping { SettingSource = "Site", LicenseeManagerId = 1, LicenseeManager = "starixIT" }
            };
            var service = MockService();

            // Act
            service.Public_GetLicenseeManager(transactionInfos, siteSettingMappings);

            // Assert
            Assert.Equal(1, transactionInfos.First().LicenseeManagerId);
            Assert.Equal("starixIT", transactionInfos.First().LicenseeManager);
        }

        [Fact]
        public void GetLicenseeManager_NotExistedMapping_ReturnExpectedLicenseeManager()
        {
            // Arrange
            var transactionInfos = new List<TransactionInfo>
            {
                new TransactionInfo()
            };
            var siteSettingMappings = new List<SiteSettingMapping>();
            var service = MockService();

            // Act
            service.Public_GetLicenseeManager(transactionInfos, siteSettingMappings);

            // Assert
            Assert.Equal(0, transactionInfos.First().LicenseeManagerId);
            Assert.Null(transactionInfos.First().LicenseeManager);
        }

        #endregion GetLicenseeManager

        #region GetCommunicationTool

        [Fact]
        public void GetCommunicationTool_NotExistedSetting_ReturnDefaultCommunicationTool()
        {
            // Arrange
            var service = MockService();
            var settings = new List<SettingItem>();

            // Act
            var actual = service.Public_GetCommunicationTool(settings);

            // Assert
            Assert.Equal(Constants.CommunicationNA.ToString(), actual);
        }

        [Fact]
        public void GetCommunicationTool_ExistedSetting_ReturnExpectedCommunicationTool()
        {
            // Arrange
            var service = MockService();
            var settings = new List<SettingItem>
            {
                new SettingItem { SettingId = 1, SettingName = "Communication", SettingValue = "Skype" }
            };

            // Act
            var actual = service.Public_GetCommunicationTool(settings);

            // Assert
            Assert.Equal(settings.First().SettingId.ToString(), actual);
        }

        #endregion GetCommunicationTool

        #region GetProductXml

        [Fact]
        public void GetProductXml_NotExistedSubProduct_ReturnExpectedXml()
        {
            // Arrange
            var matchInfo = fixture.Create<MatchInfo>();
            var transactionInfos = fixture.Create<IEnumerable<TransactionInfo>>();
            var settings = fixture.Create<IEnumerable<SettingItem>>();
            var settingMappings = fixture.Create<IEnumerable<SystemSettingMapping>>();
            var sportsbook = fixture.Create<SettingItem>();
            var subProducts = new List<SettingItem>();
            var service = MockService();
            service.Protected("GetSportsbookProduct", settings).Returns(sportsbook);
            service.Protected("GetSubProduct", sportsbook.SettingId, matchInfo, settings, settingMappings).Returns(subProducts);

            // Act
            var actual = service.Public_GetProductXml(matchInfo, transactionInfos, settings, settingMappings);

            // Assert
            Assert.Equal("<ROOT></ROOT>", actual);
            Received.InOrder(() =>
            {
                logService.LogInformation("Begin mapping product, sub-product, bet type.");
                logService.LogInformation("End mapping product, sub-product, bet type.");
            });
        }

        [Fact]
        public void GetProductXml_NotExistedBettype_ReturnExpectedXml()
        {
            // Arrange
            var matchInfo = fixture.Create<MatchInfo>();
            var transactionInfos = fixture.Create<IEnumerable<TransactionInfo>>();
            var settings = fixture.Create<IEnumerable<SettingItem>>();
            var settingMappings = fixture.Create<IEnumerable<SystemSettingMapping>>();
            var sportsbook = new SettingItem { SettingId = 1 };
            var subProducts = new List<SettingItem>
            {
                new SettingItem { SettingId = 1 }
            };
            var bettypes = new List<SettingItem>();
            var service = MockService();
            service.Protected("GetSportsbookProduct", settings).Returns(sportsbook);
            service.Protected("GetSubProduct", sportsbook.SettingId, matchInfo, settings, settingMappings).Returns(subProducts);
            service.Protected("GetBettype", Arg.Any<IEnumerable<int>>(), transactionInfos, settings, settingMappings).Returns(bettypes);

            // Act
            var actual = service.Public_GetProductXml(matchInfo, transactionInfos, settings, settingMappings);

            // Assert
            Assert.Equal("<ROOT><Product ProductID='1' SubProductID='1' BetTypeID='9144' /></ROOT>", actual);
        }

        [Fact]
        public void GetProductXml_ExistedBettype_ReturnExpectedXml()
        {
            // Arrange
            var matchInfo = fixture.Create<MatchInfo>();
            var transactionInfos = fixture.Create<IEnumerable<TransactionInfo>>();
            var settings = fixture.Create<IEnumerable<SettingItem>>();
            var settingMappings = fixture.Create<IEnumerable<SystemSettingMapping>>();
            var sportsbook = new SettingItem { SettingId = 1 };
            var subProducts = new List<SettingItem>
            {
                new SettingItem { SettingId = 1 }
            };
            var bettypes = new List<SettingItem>
            {
                new SettingItem { SettingId = 1, ParentIds = new List<int> { 1 } }
            };
            var service = MockService();
            service.Protected("GetSportsbookProduct", settings).Returns(sportsbook);
            service.Protected("GetSubProduct", sportsbook.SettingId, matchInfo, settings, settingMappings).Returns(subProducts);
            service.Protected("GetBettype", Arg.Any<IEnumerable<int>>(), transactionInfos, settings, settingMappings).Returns(bettypes);

            // Act
            var actual = service.Public_GetProductXml(matchInfo, transactionInfos, settings, settingMappings);

            // Assert
            Assert.Equal("<ROOT><Product ProductID='1' SubProductID='1' BetTypeID='1' /></ROOT>", actual);
        }

        #endregion GetProductXml

        #region GetSportsbookProduct

        [Fact]
        public void GetSportsbookProduct_NotExistedSetting_ReturnDefaultProduct()
        {
            // Arrange
            var service = MockService();
            var settings = new List<SettingItem>();

            // Act
            var actual = service.Public_GetSportsbookProduct(settings);

            // Assert
            Assert.Equal(Constants.ProductNA, actual.SettingId);
        }

        [Fact]
        public void GetSportsbookProduct_ExistedSetting_ReturnExpectedProduct()
        {
            // Arrange
            var service = MockService();
            var settings = new List<SettingItem>
            {
                new SettingItem { SettingId = 1, SettingName = "Product", SettingValue = "Sportsbook" }
            };

            // Act
            var actual = service.Public_GetSportsbookProduct(settings);

            // Assert
            Assert.Equal(settings.First(), actual);
        }

        #endregion GetSportsbookProduct

        #region GetSubProduct

        [Fact]
        public void GetSubProduct_NotExistedMapping_ReturnDefaultSubProduct()
        {
            // Arrange
            var productId = 1;
            var matchInfo = fixture.Create<MatchInfo>();
            var settings = fixture.Create<IEnumerable<SettingItem>>();
            var settingMappings = fixture.Create<IEnumerable<SystemSettingMapping>>();
            var service = MockService();

            // Act
            var actual = service.Public_GetSubProduct(productId, matchInfo, settings, settingMappings);

            // Assert
            Assert.Single(actual);
            Assert.Equal(Constants.SubProductNA, actual.First().SettingId);

            Assert.Single(actual.First().ParentIds);
            Assert.Equal(Constants.ProductNA, actual.First().ParentIds.First());
        }

        [Fact]
        public void GetBettype_ExistedMapping_ReturnExpectedSubProducts()
        {
            // Arrange
            var productId = 1;
            var matchInfo = new MatchInfo { SportName = "sport" };
            var settings = new List<SettingItem>
            {
                new SettingItem { SettingId = 1, SettingName = "SubProduct", ParentIds = new List<int> { 1 } }
            };
            var settingMappings = new List<SystemSettingMapping>
            {
                new SystemSettingMapping { SettingId = 1, SettingName = "SubProduct", SettingSource = "sport" }
            };
            var service = MockService();

            // Act
            var actual = service.Public_GetSubProduct(productId, matchInfo, settings, settingMappings);

            // Assert
            Assert.Single(actual);
            Assert.Equal(settings.First(), actual.First());
        }

        #endregion GetSubProduct

        #region GetBettype

        [Fact]
        public void GetBettype_NotExistedMapping_ReturnDefaultBettype()
        {
            // Arrange
            var subProducts = fixture.Create<IEnumerable<int>>();
            var transactionInfos = fixture.Create<IEnumerable<TransactionInfo>>();
            var settings = fixture.Create<IEnumerable<SettingItem>>();
            var settingMappings = fixture.Create<IEnumerable<SystemSettingMapping>>();
            var service = MockService();

            // Act
            var actual = service.Public_GetBettype(subProducts, transactionInfos, settings, settingMappings);

            // Assert
            Assert.Single(actual);
            Assert.Equal(Constants.BettypeNA, actual.First().SettingId);

            Assert.Single(actual.First().ParentIds);
            Assert.Equal(Constants.SubProductNA, actual.First().ParentIds.First());
        }

        [Fact]
        public void GetBettype_ExistedMapping_ReturnExpectedBettypes()
        {
            // Arrange
            var subProducts = new List<int> { 1 };
            var settings = new List<SettingItem>
            {
                new SettingItem { SettingId = 1, SettingName = "Bettype", ParentIds = new List<int> { 1 } }
            };
            var settingMappings = new List<SystemSettingMapping>
            {
                new SystemSettingMapping { SettingId = 1, SettingName = "Bettype", SettingSource = "Bettype" }
            };
            var transactionInfos = new List<TransactionInfo>
            {
                new TransactionInfo { Bettype = "Bettype" }
            };
            var service = MockService();

            // Act
            var actual = service.Public_GetBettype(subProducts, transactionInfos, settings, settingMappings);

            // Assert
            Assert.Single(actual);
            Assert.Equal(settings.First(), actual.First());
        }

        #endregion GetBettype

        #region GetCategorySubCategory

        [Fact]
        public void GetCategorySubCategory_NotExistedCategory_ReturnExpectedTuple()
        {
            // Arrange
            var settings = new List<SettingItem>();
            var service = MockService();

            // Act
            var actual = service.Public_GetCategorySubCategory(settings);

            // Assert
            Assert.Equal(0, actual.Item1);
            Assert.Equal(0, actual.Item2);
            Assert.Equal("<ROOT><Category MainCategoryID='0' SubCategoryID='0' /></ROOT>", actual.Item3);
        }

        [Fact]
        public void GetCategorySubCategory_NotExistedSubCategory_ReturnExpectedTuple()
        {
            // Arrange
            var settings = new List<SettingItem>
            {
                new SettingItem { SettingId = 1, SettingName = "MainCategory", SettingValue = "Special Notification" }
            };
            var service = MockService();

            // Act
            var actual = service.Public_GetCategorySubCategory(settings);

            // Assert
            Assert.Equal(1, actual.Item1);
            Assert.Equal(0, actual.Item2);
            Assert.Equal("<ROOT><Category MainCategoryID='1' SubCategoryID='0' /></ROOT>", actual.Item3);
        }

        [Fact]
        public void GetCategorySubCategory_ExistedMainAndSubCategory_ReturnExpectedTuple()
        {
            // Arrange
            var settings = new List<SettingItem>
            {
                new SettingItem { SettingId = 1, SettingName = "MainCategory", SettingValue = "Special Notification" },
                new SettingItem { SettingId = 2, SettingName = "SubCategory", SettingValue = "Abnormal Bets", ParentIds = new List<int>{ 1 } }
            };
            var service = MockService();

            // Act
            var actual = service.Public_GetCategorySubCategory(settings);

            // Assert
            Assert.Equal(1, actual.Item1);
            Assert.Equal(2, actual.Item2);
            Assert.Equal("<ROOT><Category MainCategoryID='1' SubCategoryID='2' /></ROOT>", actual.Item3);
        }

        #endregion GetCategorySubCategory

        [Fact]
        public void GetRelatedCustomerXml_WhenCalled_ReturnExpectedXml()
        {
            // Arrange
            var headlineId = 1;
            var transactions = new List<TransactionInfo>
            {
                new TransactionInfo { CustId = 1, Username = "starixIT" }
            };
            var xml = "<ROOT><Row CustId='1' RelatedCustId='1' Username='starixIT' /></ROOT>";
            var service = MockService();

            // Act
            var actual = service.Public_GetRelatedCustomerXml(headlineId, transactions);

            // Assert
            Assert.Equal(xml, actual);
        }

        [Fact]
        public void GetAutoField_WhenCalled_ReturnExpectedAutoField()
        {
            // Arrange
            var feedbackDate = fixture.Create<DateTime>();
            var communicationTool = fixture.Create<string>();
            var licenseeManagers = fixture.Create<IEnumerable<string>>();
            var autoField = new StringBuilder("Date Time & Tool: ");
            autoField.AppendFormat("{0}/{1}/{2}", feedbackDate.ToString("yyyyMMdd"), feedbackDate.ToString("hh:mmtt"), communicationTool);
            autoField.Append(Environment.NewLine);
            autoField.AppendFormat("System ID: {0}", string.Join(", ", licenseeManagers));
            var service = MockService();

            // Act
            var actual = service.Public_GetAutoField(feedbackDate, communicationTool, licenseeManagers);

            // Assert
            Assert.Equal(autoField.ToString(), actual);
        }

        #region GetDescription

        [Fact]
        public void GetDescriptionForDepositeSite_WhenCalled_ReturnExpectedDescription()
        {
            // Arrange
            var messageId = fixture.Create<int>();
            var requestIds = fixture.Create<string>();
            var description = new StringBuilder("<span style=\"color: rgb(255, 0, 0);\">This is an auto-generated entry.</span>");
            description.AppendFormat("<br /><span style=\"color: rgb(255, 0, 0);\">Msg ID: {0}, Request No.: {1}</span>", messageId, requestIds);
            description.Append("<br />Announcement sent to affected Licensees and informed our related department found abnormal tickets of members for the match.");
            description.Append("<br /><br />Please refer to the system email subject \"Voided Bets Report\" for those affected customers &amp; tickets for reference.");
            var service = MockService();

            // Act
            var actual = service.Public_GetDescriptionForDepositeSite(messageId, requestIds);

            // Assert
            Assert.Equal(description.ToString(), actual);
        }

        [Fact]
        public void GetDescriptionForCreditSite_WhenCalled_ReturnExpectedDescription()
        {
            // Arrange
            var messageId = fixture.Create<int>();
            var requestIds = fixture.Create<string>();
            var matchInfo = fixture.Create<MatchInfo>();
            var description = new StringBuilder("<span style=\"color: rgb(255, 0, 0);\">This is an auto-generated entry.</span>");
            description.AppendFormat("<br /><span style=\"color: rgb(255, 0, 0);\">Msg ID: {0}, Request No.: {1}</span>", messageId, requestIds);
            description.AppendFormat("<br />Date: {0}", matchInfo.EventDate.ToString("dd-MMM-yyyy"));
            description.AppendFormat("<br />Sport: {0}", matchInfo.SportName);
            description.AppendFormat("<br />Event Start Time: {0}", matchInfo.KickOffTime);
            description.AppendFormat("<br />League: {0}", matchInfo.LeagueName);
            description.AppendFormat("<br />Match: {0} - vs - {1}", matchInfo.HomeName, matchInfo.AwayName);
            var service = MockService();

            // Act
            var actual = service.Public_GetDescriptionForCreditSite(messageId, requestIds, matchInfo);

            // Assert
            Assert.Equal(description.ToString(), actual);
        }

        #endregion GetDescription

        #region GetSolution

        [Fact]
        public void GetSolutionForDeposit_WhenCalled_ReturnExpectedSolution()
        {
            // Arrange
            var message = fixture.Create<Message>();
            var solution = new StringBuilder(message.Content_en);
            solution.Append("<br /><br />").Append(message.Content_cs);
            var service = MockService();

            // Act
            var actual = service.Public_GetSolution(message);

            // Assert
            Assert.Equal(solution.ToString(), actual);
        }

        [Fact]
        public void GetSolutionForCreditSite_WhenCalled_ReturnExpectedSolution()
        {
            // Arrange
            var message = fixture.Create<Message>();
            var transactions = new List<TransactionInfo>
            {
                new TransactionInfo {TransId = 1, Username = "starixIT"}
            };
            var solution = new StringBuilder(message.Content_en);
            solution.Append("<br /><br />").Append(message.Content_cs);
            solution.Append("<br /><br />Kindly refer to Agency site \"Cancelled Bets\" for Voided user.");
            solution.Append("<br /><br />Affected users and tickets:");
            solution.Append("<br />starixIT 1");
            var service = MockService();

            // Act
            var actual = service.Public_GetSolutionForCreditSite(message, transactions);

            // Assert
            Assert.Equal(solution.ToString(), actual);
        }

        #endregion GetSolution

        [Fact]
        public async Task EditSystemSettingAsync_WhenCalled_ReturnExpectedCommandResult()
        {
            // Arrange
            var commandResult = fixture.Create<CommandResult>();
            var service = MockService();
            systemSettingService.EditSystemSettingAsync(Arg.Any<IEnumerable<SystemSettingItem>>()).Returns(commandResult);

            // Act
            var actual = await service.Public_EditSystemSettingAsync();

            // Assert
            Assert.Equal(commandResult, actual);
        }

        #region Helpers

        private FeedbackServiceTestable MockService()
        {
            appSettings = new AppJobSettings
            {
                CreatorId = 8,
                LicenseeUsername = "starixIT",
                NoOfRelatedCustomerPerFeedback = 2,
                TimeSpan = 12
            };
            var appJobSettings = Substitute.For<IOptionsSnapshot<AppJobSettings>>();
            appJobSettings.Value.Returns(appSettings);
            baseService = Substitute.For<IBaseService>();
            settingService = Substitute.For<ISettingService>();
            systemSettingService = Substitute.For<ISystemSettingService>();
            messageService = Substitute.For<IMessageService>();
            customerService = Substitute.For<ICustomerService>();
            logService = Substitute.For<ICRMLogService>();
            ticketVoidingService = Substitute.For<IFeedbackTicketVoidingService>();

            return Substitute.For<FeedbackServiceTestable>(
                appJobSettings,
                baseService,
                settingService,
                systemSettingService,
                messageService,
                customerService,
                logService,
                ticketVoidingService);
        }

        #endregion Helpers
    }
}