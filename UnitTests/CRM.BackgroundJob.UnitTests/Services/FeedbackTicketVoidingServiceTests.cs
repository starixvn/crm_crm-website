namespace CRM.BackgroundJob.UnitTests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture;
    using CRM.Core.Extensions.XML;
    using CRM.Core.Models;
    using CRM.Core.Services;
    using Fanex.CRM.BackgroundJob.Criteria;
    using Fanex.CRM.BackgroundJob.Models;
    using Fanex.CRM.BackgroundJob.Services;
    using NSubstitute;
    using Xunit;

    public class FeedbackTicketVoidingServiceTests
    {
        private IBaseService baseService;
        private IMessageService messageService;
        private ICRMLogService logService;
        private readonly IFixture fixture = new Fixture();

        [Fact]
        public async Task GetTicketVoidingInfoAsync_MessageDoNotHaveTransactionInfo_ExpectedUpdateStatusAndWriteLog()
        {
            // Arrange
            fixture.Customize<VoidingMessage>(x => x.With(b => b.MessageId, 1));
            fixture.Customize<MatchInfo>(x => x.With(b => b.MessageId, 1));
            var setting = fixture.Create<AbnormalBetSystemSetting>();
            var voidingMessages = fixture.Create<IEnumerable<VoidingMessage>>();
            var xml = voidingMessages.ToXML("ROOT").ToString();
            var matchInfos = fixture.Create<IEnumerable<MatchInfo>>();
            var transactionInfos = new List<TransactionInfo>();
            var ticketInfo = new ValueTuple<IEnumerable<MatchInfo>, IEnumerable<TransactionInfo>>(matchInfos, transactionInfos);
            var criteria = new GetAbnormalTicketInfoCriteria();
            var service = GetService();
            baseService.FetchMultipleAsync<MatchInfo, TransactionInfo>(Arg.Do<GetAbnormalTicketInfoCriteria>(x => criteria = x)).Returns(ticketInfo);

            // Act
            var actual = await service.GetTicketVoidingInfoAsync(voidingMessages, setting);

            // Assert
            logService.Received().LogInformation(string.Format("Don't have ticket info: {0}", string.Join(",", matchInfos.Select(x => x.MessageId))));
            await messageService.Received().EditStatusVoidingMessageAsync(Arg.Any<IEnumerable<VoidingMessage>>());

            Assert.Empty(actual.Item1);
            Assert.Equal(transactionInfos, actual.Item2);
            Assert.Equal(xml, criteria.MessageIds);
            Assert.Equal(setting.ConfigUAT, criteria.ConfigUAT);
            Assert.Equal(setting.ConfigPRO, criteria.ConfigPRO);
        }

        [Fact]
        public async Task GetTicketVoidingInfoAsync_MessageHaveTransactionInfo_ReturnExpectedTicketInfos()
        {
            // Arrange
            fixture.Customize<MatchInfo>(x => x.With(b => b.MessageId, 1));
            fixture.Customize<TransactionInfo>(x => x.With(b => b.MessageId, 1));
            var setting = fixture.Create<AbnormalBetSystemSetting>();
            var voidingMessages = fixture.Create<IEnumerable<VoidingMessage>>();
            var xml = voidingMessages.ToXML("ROOT").ToString();
            var matchInfos = fixture.Create<IEnumerable<MatchInfo>>();
            var transactionInfos = fixture.Create<IEnumerable<TransactionInfo>>();
            var expectedMatchInfos = matchInfos.Where(x => transactionInfos.Any(y => x.MessageId == y.MessageId)).ToList();
            var ticketInfo = new ValueTuple<IEnumerable<MatchInfo>, IEnumerable<TransactionInfo>>(matchInfos, transactionInfos);
            var criteria = new GetAbnormalTicketInfoCriteria();
            var service = GetService();
            baseService.FetchMultipleAsync<MatchInfo, TransactionInfo>(Arg.Do<GetAbnormalTicketInfoCriteria>(x => criteria = x)).Returns(ticketInfo);

            // Act
            var actual = await service.GetTicketVoidingInfoAsync(voidingMessages, setting);

            // Assert
            logService.DidNotReceive().LogInformation(Arg.Any<string>());
            await messageService.DidNotReceive().EditStatusVoidingMessageAsync(Arg.Any<IEnumerable<VoidingMessage>>());

            Assert.Equal(expectedMatchInfos, actual.Item1);
            Assert.Equal(transactionInfos, actual.Item2);
            Assert.Equal(xml, criteria.MessageIds);
            Assert.Equal(setting.ConfigUAT, criteria.ConfigUAT);
            Assert.Equal(setting.ConfigPRO, criteria.ConfigPRO);
        }

        [Fact]
        public async Task ImportTicketVoidingInfoAsync_FeedbackCreated_ExpectedUpdateStatusAndWriteLog()
        {
            // Arrange
            fixture.Customize<VoidingMessage>(x => x.With(b => b.MessageId, 1));
            fixture.Customize<MatchInfo>(x => x.With(b => b.MessageId, 1));
            var voidingMessages = fixture.Create<IEnumerable<VoidingMessage>>();
            var matchInfos = fixture.Create<IEnumerable<MatchInfo>>();
            var transactionInfos = new List<TransactionInfo>();
            var siteSettingMappings = fixture.Create<IEnumerable<SiteSettingMapping>>();
            var headlines = fixture.Create<IEnumerable<GroupHeadline>>();
            var licenseeInfo = fixture.Create<CustomerInfo>();
            var feedbackInfos = new List<AutoFeedbackInfo>();
            var criteria = new ImportFeedbackInfoCriteria();
            var service = GetService();
            baseService.GetDataAsync<AutoFeedbackInfo>(Arg.Do<ImportFeedbackInfoCriteria>(x => criteria = x)).Returns(feedbackInfos);

            // Act
            var actual = await service.ImportTicketVoidingInfoAsync(
                voidingMessages,
                matchInfos,
                transactionInfos,
                siteSettingMappings,
                headlines,
                licenseeInfo);

            // Assert
            logService.Received().LogInformation(string.Format("Don't have ticket info or created: {0}", string.Join(",", matchInfos.Select(x => x.MessageId))));
            await messageService.Received().EditStatusVoidingMessageAsync(Arg.Any<IEnumerable<VoidingMessage>>());

            Assert.Empty(actual.Item1);
            Assert.Empty(actual.Item2);
            Assert.Equal("<ROOT></ROOT>", criteria.Xml);
        }

        [Fact]
        public async Task ImportTicketVoidingInfoAsync_TicketVoidingForCredit_ReturnExpectedInfo()
        {
            // Arrange
            var voidingMessages = fixture.Create<IEnumerable<VoidingMessage>>();
            var matchInfos = new List<MatchInfo>
            {
                new MatchInfo { MessageId = 1, HomeName = "home", AwayName = "away" }
            };
            var transactionInfos = new List<TransactionInfo>
            {
                new TransactionInfo { MessageId = 1, RequestId = 1, CustId = 1 }
            };
            var siteSettingMappings = fixture.Create<IEnumerable<SiteSettingMapping>>();
            var headlines = new List<GroupHeadline>
            {
                new GroupHeadline { CustId = 1, HeadlineId = 1 }
            };
            var licenseeInfo = fixture.Create<CustomerInfo>();
            var feedbackInfos = new List<AutoFeedbackInfo>
            {
                new AutoFeedbackInfo { MessageId = 1, CustId = 1 }
            };
            var xml = "<ROOT><r msgid='1' reqid='1' custid='1' hl='1' related='1' event='home -vs- away' /></ROOT>";
            var criteria = new ImportFeedbackInfoCriteria();
            var service = GetService();
            baseService.GetDataAsync<AutoFeedbackInfo>(Arg.Do<ImportFeedbackInfoCriteria>(x => criteria = x)).Returns(feedbackInfos);

            // Act
            var actual = await service.ImportTicketVoidingInfoAsync(
                voidingMessages,
                matchInfos,
                transactionInfos,
                siteSettingMappings,
                headlines,
                licenseeInfo);

            // Assert
            logService.DidNotReceive().LogInformation(Arg.Any<string>());
            await messageService.DidNotReceive().EditStatusVoidingMessageAsync(Arg.Any<IEnumerable<VoidingMessage>>());

            Assert.Equal(matchInfos, actual.Item1);
            Assert.Equal(transactionInfos, actual.Item2);
            Assert.Equal(xml, criteria.Xml);
        }

        [Fact]
        public async Task ImportTicketVoidingInfoAsync_TicketVoidingForDeposit_ReturnExpectedInfo()
        {
            // Arrange
            var voidingMessages = fixture.Create<IEnumerable<VoidingMessage>>();
            var matchInfos = new List<MatchInfo>
            {
                new MatchInfo { MessageId = 1, HomeName = "home", AwayName = "away" }
            };
            var transactionInfos = new List<TransactionInfo>
            {
                new TransactionInfo { MessageId = 1, RequestId = 1, CustId = 1, Site = "Site", IsDeposit = true }
            };
            var siteSettingMappings = new List<SiteSettingMapping>
            {
                new SiteSettingMapping { SettingSource = "Site", LicenseeManagerId = 1 }
            };
            var headlines = fixture.Create< IEnumerable<GroupHeadline>>();
            var licenseeInfo = new CustomerInfo { CustId = 1 };
            var feedbackInfos = new List<AutoFeedbackInfo>
            {
                new AutoFeedbackInfo { MessageId = 1, CustId = 1 }
            };
            var xml = "<ROOT><r msgid='1' reqid='1' custid='1' hl='1' related='1' event='home -vs- away' /></ROOT>";
            var criteria = new ImportFeedbackInfoCriteria();
            var service = GetService();
            baseService.GetDataAsync<AutoFeedbackInfo>(Arg.Do<ImportFeedbackInfoCriteria>(x => criteria = x)).Returns(feedbackInfos);

            // Act
            var actual = await service.ImportTicketVoidingInfoAsync(
                voidingMessages,
                matchInfos,
                transactionInfos,
                siteSettingMappings,
                headlines,
                licenseeInfo);

            // Assert
            logService.DidNotReceive().LogInformation(Arg.Any<string>());
            await messageService.DidNotReceive().EditStatusVoidingMessageAsync(Arg.Any<IEnumerable<VoidingMessage>>());

            Assert.Equal(matchInfos, actual.Item1);
            Assert.Equal(transactionInfos, actual.Item2);
            Assert.Equal(xml, criteria.Xml);
        }

        [Fact]
        public async Task EditFeedbackInfoAsync_WhenCalled_ReturnExpectedCommandResult()
        {
            // Arrange
            var expected = fixture.Create<CommandResult>();
            var messageId = fixture.Create<int>();
            var requestIds = fixture.Create<IEnumerable<int>>();
            var custIds = fixture.Create<IEnumerable<int>>();
            var status = fixture.Create<string>();
            var description = fixture.Create<string>();
            var criteria = new EditFeedbackInfoCommand();
            var service = GetService();
            baseService.ExecuteNonQueryAsync(Arg.Do<EditFeedbackInfoCommand>(x => criteria = x)).Returns(expected);

            // Act
            var actual = await service.EditFeedbackInfoAsync(messageId, requestIds, custIds, status, description);

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(messageId, criteria.MessageId);
            Assert.Equal(string.Join(",", requestIds), criteria.RequestIds);
            Assert.Equal(string.Join(",", custIds), criteria.CustIds);
            Assert.Equal(string.Join(",", status), criteria.Status);
            Assert.Equal(string.Join(",", description), criteria.Description);
        }

        #region Helpers

        private FeedbackTicketVoidingService GetService()
        {
            baseService = Substitute.For<IBaseService>();
            messageService = Substitute.For<IMessageService>();
            logService = Substitute.For<ICRMLogService>();
            return new FeedbackTicketVoidingService(baseService, messageService, logService);
        }

        #endregion Helpers
    }
}