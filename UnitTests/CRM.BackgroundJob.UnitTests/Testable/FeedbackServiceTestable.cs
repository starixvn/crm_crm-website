namespace CRM.BackgroundJob.UnitTests.Testable
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using CRM.Core.Models;
    using CRM.Core.Services;
    using Fanex.CRM.BackgroundJob.Criteria;
    using Fanex.CRM.BackgroundJob.Models;
    using Fanex.CRM.BackgroundJob.Services;
    using Microsoft.Extensions.Options;

    public static class Reflect
    {
        public static object Protected(this object target, string name, params object[] args)
        {
            var type = target.GetType();
            var method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                             .Single(x => x.Name == name);
            return method.Invoke(target, args);
        }
    }

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

        public async Task Public_CreateFeedbackForCreditSiteAsync(
            Message message,
            MatchInfo matchInfo,
            IEnumerable<TransactionInfo> transactionInfos,
            IEnumerable<GroupHeadline> headlines,
            IEnumerable<SettingItem> settings,
            IEnumerable<SystemSettingMapping> settingMappings)
        {
            await base.CreateFeedbackForCreditSiteAsync(message, matchInfo, transactionInfos, headlines, settings, settingMappings);
        }

        public async Task Public_CreateFeedbackForCreditSiteByBatchAsync(
            GroupHeadline headline,
            IEnumerable<int> relatedCustomerIds,
            Message message,
            MatchInfo matchInfo,
            IEnumerable<TransactionInfo> transactionInfos,
            IEnumerable<SettingItem> settings,
            IEnumerable<SystemSettingMapping> settingMappings,
            CreateMutipleFeedbackCommand criteria)
        {
            await base.CreateFeedbackForCreditSiteByBatchAsync(
                headline,
                relatedCustomerIds,
                message,
                matchInfo,
                transactionInfos,
                settings,
                settingMappings,
                criteria);
        }

        public async Task Public_CreateFeedbackForDepositSiteAsync(
            CustomerInfo licenseeInfo,
            Message message,
            MatchInfo matchInfo,
            IEnumerable<TransactionInfo> transactionInfos,
            IEnumerable<SettingItem> settings,
            IEnumerable<SystemSettingMapping> settingMappings)
        {
            await base.CreateFeedbackForDepositSiteAsync(licenseeInfo, message, matchInfo, transactionInfos, settings, settingMappings);
        }

        public async Task Public_CreateFeedbackForDepositByBatchAsync(
            CustomerInfo licenseeInfo,
            Message message,
            IEnumerable<TransactionInfo> transactionInfos,
            DateTime feedbackDate,
            string communicationTool,
            CreateFeedbackCommand criteria)
        {
            await base.CreateFeedbackForDepositByBatchAsync(licenseeInfo, message, transactionInfos, feedbackDate, communicationTool, criteria);
        }

        public void Public_GetLicenseeManager(
            IEnumerable<TransactionInfo> transactionInfos,
            IEnumerable<SiteSettingMapping> siteSettingMappings)
        {
            base.GetLicenseeManager(transactionInfos, siteSettingMappings);
        }

        public string Public_GetCommunicationTool(IEnumerable<SettingItem> settings)
        {
            return base.GetCommunicationTool(settings);
        }

        public string Public_GetProductXml(
            MatchInfo matchInfo,
            IEnumerable<TransactionInfo> transactionInfos,
            IEnumerable<SettingItem> settings,
            IEnumerable<SystemSettingMapping> settingMappings)
        {
            return base.GetProductXml(matchInfo, transactionInfos, settings, settingMappings);
        }

        public SettingItem Public_GetSportsbookProduct(IEnumerable<SettingItem> settings)
        {
            return base.GetSportsbookProduct(settings);
        }

        public IList<SettingItem> Public_GetSubProduct(
            int productId,
            MatchInfo matchInfo,
            IEnumerable<SettingItem> settings,
            IEnumerable<SystemSettingMapping> settingMappings)
        {
            return base.GetSubProduct(productId, matchInfo, settings, settingMappings);
        }

        public IList<SettingItem> Public_GetBettype(
            IEnumerable<int> subProducts,
            IEnumerable<TransactionInfo> transactionInfos,
            IEnumerable<SettingItem> settings,
            IEnumerable<SystemSettingMapping> settingMappings)
        {
            return base.GetBettype(subProducts, transactionInfos, settings, settingMappings);
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

        public string Public_GetDescriptionForDepositeSite(int messageId, string requestIds)
        {
            return base.GetDescriptionForDepositeSite(messageId, requestIds);
        }

        public string Public_GetDescriptionForCreditSite(int messageId, string requestIds, MatchInfo matchInfo)
        {
            return base.GetDescriptionForCreditSite(messageId, requestIds, matchInfo);
        }

        public string Public_GetSolution(Message message)
        {
            return base.GetSolution(message);
        }

        public string Public_GetSolutionForCreditSite(Message message, IEnumerable<TransactionInfo> transactionInfos)
        {
            return base.GetSolutionForCreditSite(message, transactionInfos);
        }

        public async Task<CommandResult> Public_EditSystemSettingAsync()
        {
            return await base.EditSystemSettingAsync();
        }
    }
}