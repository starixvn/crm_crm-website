namespace CRM.BackgroundJob.UnitTests.Testable
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CRM.Core.Models;
    using CRM.Core.Services;
    using Fanex.CRM.BackgroundJob.Models;
    using Fanex.CRM.BackgroundJob.Services;
    using Microsoft.Extensions.Options;

    public class FeedbackServiceTestable : FeedbackService
    {
        public FeedbackServiceTestable(IOptionsSnapshot<AppJobSettings> appSettings,
            IBaseService baseService,
            ISettingService settingService,
            ISystemSettingService systemSettingService,
            IMessageService messageService,
            ICustomerService customerService,
            ICRMLogService logService,
            IFeedbackTicketVoidingService ticketVoidingService)
            : base(appSettings,
                baseService,
                settingService,
                systemSettingService,
                messageService,
                customerService,
                logService,
                ticketVoidingService)
        {
        }

        public string Public_GetCommunicationTool(IEnumerable<SettingItem> settings)
        {
            return base.GetCommunicationTool(settings);
        }

        public SettingItem Public_GetSportsbookProduct(IEnumerable<SettingItem> settings)
        {
            return base.GetSportsbookProduct(settings);
        }

        public Tuple<int, int, string> Public_GetCategorySubCategory(IEnumerable<SettingItem> settings)
        {
            return base.GetCategorySubCategory(settings);
        }

        public string Public_GetRelatedCustomerXml(int headlineId, IEnumerable<TransactionInfo> transactionInfos)
        {
            return base.GetRelatedCustomerXml(headlineId, transactionInfos);
        }

        public string Public_GetAutoField(
            DateTime feedbackDate,
            string communicationTool,
            IEnumerable<string> licenseeManagers)
        {
            return base.GetAutoField(feedbackDate, communicationTool, licenseeManagers);
        }

        public string Public_GetDescription(int messageId, string requestIds)
        {
            return base.GetDescription(messageId, requestIds);
        }

        public string Public_GetDescription(int messageId, string requestIds, MatchInfo matchInfo)
        {
            return base.GetDescription(messageId, requestIds, matchInfo);
        }

        public string Public_GetSolution(Message message)
        {
            return base.GetSolution(message);
        }

        public string Public_GetSolution(Message message, IEnumerable<TransactionInfo> transactionInfos)
        {
            return base.GetSolution(message, transactionInfos);
        }

        public async Task<CommandResult> Public_EditSystemSettingAsync()
        {
            return await base.EditSystemSettingAsync();
        }
    }
}