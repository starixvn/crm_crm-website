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
            appSettings = new AppJobSettings { LicenseeUsername = "starixIT" };
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