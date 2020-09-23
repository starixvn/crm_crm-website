namespace Fanex.CRM.BackgroundJob.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using Fanex.CRM.BackgroundJob.Constants;
    using Fanex.CRM.BackgroundJob.Criteria;
    using Fanex.CRM.BackgroundJob.Models;
    using global::CRM.Core.Extensions.XML;
    using global::CRM.Core.Models;
    using global::CRM.Core.Services;

    public interface IFeedbackTicketVoidingService
    {
        Task<ValueTuple<IEnumerable<MatchInfo>, IEnumerable<TransactionInfo>>> GetTicketVoidingInfoAsync(
               IEnumerable<VoidingMessage> voidingMessages,
               AbnormalBetSystemSetting setting);

        Task<ValueTuple<IEnumerable<MatchInfo>, IEnumerable<TransactionInfo>>> ImportTicketVoidingInfoAsync(
            IEnumerable<VoidingMessage> voidingMessages,
            IEnumerable<MatchInfo> matchInfos,
            IEnumerable<TransactionInfo> transactionInfos,
            IEnumerable<SiteSettingMapping> siteSettingMappings,
            IEnumerable<GroupHeadline> headlines,
            CustomerInfo licenseeInfo);

        Task<CommandResult> EditFeedbackInfoAsync(
            int messageId,
            IEnumerable<int> requestIds,
            IEnumerable<int> custIds,
            string status,
            string description);
    }

    public class FeedbackTicketVoidingService : IFeedbackTicketVoidingService
    {
        private readonly IBaseService baseService;
        private readonly IMessageService messageService;
        private readonly ICRMLogService logService;

        public FeedbackTicketVoidingService(
            IBaseService baseService,
            IMessageService messageService,
            ICRMLogService logService)
        {
            this.baseService = baseService;
            this.messageService = messageService;
            this.logService = logService;
        }

        public async Task<ValueTuple<IEnumerable<MatchInfo>, IEnumerable<TransactionInfo>>> GetTicketVoidingInfoAsync(
               IEnumerable<VoidingMessage> voidingMessages,
               AbnormalBetSystemSetting setting)
        {
            var criteria = new GetAbnormalTicketInfoCriteria
            {
                MessageIds = voidingMessages.ToXML("ROOT").ToString(),
                ConfigUAT = setting.ConfigUAT,
                ConfigPRO = setting.ConfigPRO
            };
            var ticketInfo = await baseService.FetchMultipleAsync<MatchInfo, TransactionInfo>(criteria);

            var messageIds = ticketInfo.Item1.Where(x => !ticketInfo.Item2.Any(y => x.MessageId == y.MessageId))
                .Select(x => x.MessageId).ToList();
            if (messageIds.Any())
            {
                IEnumerable<VoidingMessage> messages = voidingMessages.Where(x => messageIds.Any(y => x.MessageId == y));
                await messageService.EditStatusVoidingMessageAsync(messages);
                logService.LogInformation(string.Format("Don't have ticket info: {0}", string.Join(",", messageIds)));
            }

            IEnumerable<MatchInfo> matchInfos = ticketInfo.Item1.Where(x => ticketInfo.Item2.Any(y => x.MessageId == y.MessageId)).ToList();
            return new ValueTuple<IEnumerable<MatchInfo>, IEnumerable<TransactionInfo>>(matchInfos, ticketInfo.Item2);
        }

        public async Task<ValueTuple<IEnumerable<MatchInfo>, IEnumerable<TransactionInfo>>> ImportTicketVoidingInfoAsync(
            IEnumerable<VoidingMessage> voidingMessages,
            IEnumerable<MatchInfo> matchInfos,
            IEnumerable<TransactionInfo> transactionInfos,
            IEnumerable<SiteSettingMapping> siteSettingMappings,
            IEnumerable<GroupHeadline> headlines,
            CustomerInfo licenseeInfo)
        {
            const string XmlTemplate = "<r msgid='{0}' reqid='{1}' custid='{2}' hl='{3}' related='{4}' event='{5}' />";
            var stringBuilder = new StringBuilder(Constants.BeginRootTag);
            foreach (var transaction in transactionInfos)
            {
                var matchInfo = matchInfos.FirstOrDefault(x => x.MessageId == transaction.MessageId);
                var headlineId = 0;
                var relatedCustId = 0;

                if (transaction.IsDeposit)
                {
                    SiteSettingMapping licenseeManager = siteSettingMappings.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.SettingSource)
                        && x.SettingSource.Equals(transaction.Site, StringComparison.OrdinalIgnoreCase));
                    headlineId = licenseeInfo.CustId;
                    relatedCustId = licenseeManager != null ? licenseeManager.LicenseeManagerId : 0;
                }
                else
                {
                    GroupHeadline headline = headlines.FirstOrDefault(x => x.CustId == transaction.CustId);
                    headlineId = headline != null ? headline.HeadlineId : 0;
                    relatedCustId = transaction.CustId;
                }

                stringBuilder.AppendFormat(XmlTemplate,
                    transaction.MessageId,
                    transaction.RequestId,
                    transaction.CustId,
                    headlineId,
                    relatedCustId,
                    string.Format("{0} -vs- {1}", HttpUtility.HtmlEncode(matchInfo?.HomeName), HttpUtility.HtmlEncode(matchInfo?.AwayName)));
            }

            stringBuilder.Append(Constants.EndRootTag);

            var criteria = new ImportFeedbackInfoCriteria
            {
                Xml = stringBuilder.ToString()
            };
            var feedbackInfos = await baseService.GetDataAsync<AutoFeedbackInfo>(criteria);

            var messageIds = matchInfos.Where(x => !feedbackInfos.Any(y => y.MessageId == x.MessageId)).Select(x => x.MessageId).ToList();
            if (messageIds.Any())
            {
                IEnumerable<VoidingMessage> messages = voidingMessages.Where(x => messageIds.Any(y => x.MessageId == y));
                await messageService.EditStatusVoidingMessageAsync(messages);
                logService.LogInformation(string.Format("Don't have ticket info or created: {0}", string.Join(",", messageIds)));
            }

            matchInfos = matchInfos.Where(x => feedbackInfos.Any(y => y.MessageId == x.MessageId));
            transactionInfos = transactionInfos.Where(x => feedbackInfos.Any(y => y.MessageId == x.MessageId && y.CustId == x.CustId));
            return new ValueTuple<IEnumerable<MatchInfo>, IEnumerable<TransactionInfo>>(matchInfos, transactionInfos);
        }

        public async Task<CommandResult> EditFeedbackInfoAsync(
            int messageId,
            IEnumerable<int> requestIds,
            IEnumerable<int> custIds,
            string status,
            string description)
        {
            var criteria = new EditFeedbackInfoCommand
            {
                MessageId = messageId,
                RequestIds = string.Join(",", requestIds),
                CustIds = string.Join(",", custIds),
                Status = status,
                Description = description
            };
            return await baseService.ExecuteNonQueryAsync(criteria);
        }
    }
}