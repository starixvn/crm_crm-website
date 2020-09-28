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

        [Fact]
        public async Task CreateFeedbackForDepositSiteAsync_WhenCalled_ExpectedCallCreateFeedbackByBatchAsync()
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
            Assert.True(true);
        }

        #region CreateFeedbackByBatchAsync

        [Fact]
        public async Task CreateFeedbackByBatchAsync_CreateSuccessful_ExpectedWriteLog()
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
            await service.Public_CreateFeedbackByBatchAsync(licenseeInfo, message, transactionInfos, feedbackDate, communicationTool, criteria);

            // Assert
            Assert.Equal("1", criteria.RelatedCustId);
            Assert.Equal("auto field", criteria.AutoField);
            Assert.Equal(message.MessageId, criteria.MessageId);
            Assert.Equal("1", criteria.ListOrigCustIds);
            Assert.Equal("1", criteria.RequestIds);
            logService.Received(2).LogInformation(Arg.Any<string>());
        }

        [Fact]
        public async Task CreateFeedbackByBatchAsync_CreateUnsuccessful_ExpectedWriteLogAndUpdateStatus()
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
            await service.Public_CreateFeedbackByBatchAsync(licenseeInfo, message, transactionInfos, feedbackDate, communicationTool, criteria);

            // Assert
            logService.Received(2).LogInformation(Arg.Any<string>());
            await ticketVoidingService.EditFeedbackInfoAsync(message.MessageId, Arg.Any<IEnumerable<int>>(), Arg.Any<IEnumerable<int>>(), CreateFeedbackJobStatus.Fail, commandResult.Message);
            await logService.ErrorAsync(commandResult, criteria);
        }

        [Fact]
        public async Task CreateFeedbackByBatchAsync_Exception_ExpectedWriteLogAndUpdateStatus()
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
            await service.Public_CreateFeedbackByBatchAsync(licenseeInfo, message, transactionInfos, feedbackDate, communicationTool, criteria);

            // Assert
            logService.Received(2).LogInformation(Arg.Any<string>());
            await ticketVoidingService.EditFeedbackInfoAsync(message.MessageId, Arg.Any<IEnumerable<int>>(), Arg.Any<IEnumerable<int>>(), CreateFeedbackJobStatus.Fail, exception.Message);
            await logService.ErrorAsync(exception);
        }

        #endregion CreateFeedbackByBatchAsync

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
        public void GetDescriptionForDeposit_WhenCalled_ReturnExpectedDescription()
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
            var actual = service.Public_GetDescription(messageId, requestIds);

            // Assert
            Assert.Equal(description.ToString(), actual);
        }

        [Fact]
        public void GetDescriptionForCredit_WhenCalled_ReturnExpectedDescription()
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
            var actual = service.Public_GetDescription(messageId, requestIds, matchInfo);

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
        public void GetSolutionForCredit_WhenCalled_ReturnExpectedSolution()
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
            var actual = service.Public_GetSolution(message, transactions);

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