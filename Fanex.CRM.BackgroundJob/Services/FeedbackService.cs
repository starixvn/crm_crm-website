namespace Fanex.CRM.BackgroundJob.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Fanex.CRM.BackgroundJob.Constants;
    using Fanex.CRM.BackgroundJob.Criteria;
    using Fanex.CRM.BackgroundJob.Models;
    using Fanex.CRM.BackgroundJob.Repositories;
    using Microsoft.Extensions.Options;

    public interface IFeedbackService
    {
        Task CreateFeedbackForVoidingAbnormalBetAsync(AbnormalBetSystemSetting setting);
    }

    public class FeedbackService : IFeedbackService
    {
        private readonly AppSettings appSettings;
        private readonly ICustomerRepository customerRepository;
        private readonly IBaseService baseService;

        public FeedbackService(
            IOptionsSnapshot<AppSettings> appSettings,
            ICustomerRepository customerRepository,
            IBaseService baseService)
        {
            this.appSettings = appSettings.Value;
            this.customerRepository = customerRepository;
            this.baseService = baseService;
        }

        public async Task CreateFeedbackForVoidingAbnormalBetAsync(AbnormalBetSystemSetting setting)
        {
            IEnumerable<Message> messages = await GetNewMessageAsync(setting.LastMsgId, setting.BatchSite);
            if (messages.Any())
            {
                var settingNames = new[] { SettingName.SubProduct, SettingName.Bettype };
                IEnumerable<SettingItem> settings = await GetAllSettingAsync();
                IEnumerable<SystemSettingMapping> settingMappings = await GetSystemSettingMappingAsync(settingNames);
                IEnumerable<SiteSettingMapping> siteSettingMappings = await GetSiteSettingMappingAsync();
                CustomerInfo licenseeInfo = await GetLicenseeInfoAsync();

                var messageIds = string.Join(",", messages.Select(x => x.MessageId));
                ValueTuple<IEnumerable<MatchInfo>, IEnumerable<TransactionInfo>> ticketInfo = await GetTicketInfoAsync(messageIds, setting);

                var custIds = string.Join(",", ticketInfo.Item2.Where(x => !x.IsDeposit).Select(x => x.CustId));
                IEnumerable<GroupHeadline> headlines = await GetGroupHeadlineAsync(custIds);

                foreach (MatchInfo matchInfo in ticketInfo.Item1)
                {
                    IEnumerable<TransactionInfo> credits = ticketInfo.Item2.Where(x => x.MessageId == matchInfo.MessageId && !x.IsDeposit);
                    IEnumerable<TransactionInfo> deposits = ticketInfo.Item2.Where(x => x.MessageId == matchInfo.MessageId && x.IsDeposit);

                    if (credits.Any())
                    {
                        await CreateFeedbackForCreditSiteAsync(
                            messages.First(x => x.MessageId == matchInfo.MessageId),
                            matchInfo,
                            credits,
                            headlines,
                            settings,
                            settingMappings);
                    }

                    if (deposits.Any())
                    {
                        await CreateFeedbackForDepositSiteAsync(
                            licenseeInfo,
                            messages.First(x => x.MessageId == matchInfo.MessageId),
                            matchInfo,
                            deposits,
                            settings,
                            settingMappings,
                            siteSettingMappings);
                    }
                }
            }
        }

        private async Task<IEnumerable<Message>> GetNewMessageAsync(int lastMessageId, int batchSize)
        {
            var criteria = new GetNewMessageCriteria
            {
                LastMessageId = lastMessageId,
                BatchSize = batchSize
            };

            var messages = await baseService.GetDataAsync<Message>(criteria);
            foreach (var message in messages)
            {
                message.Content_en = message.Content_en?.Trim().Replace("@@", string.Empty);
                message.Content_cs = message.Content_cs?.Trim().Replace("@@", string.Empty);
            }

            return messages;
        }

        private async Task<ValueTuple<IEnumerable<MatchInfo>, IEnumerable<TransactionInfo>>> GetTicketInfoAsync(
            string messageIds,
            AbnormalBetSystemSetting setting)
        {
            var criteria = new GetAbnormalTicketInfoCriteria
            {
                MessageIds = messageIds,
                ConfigUAT = setting.ConfigUAT,
                ConfigPRO = setting.ConfigPRO
            };

            return await baseService.FetchMultipleAsync<MatchInfo, TransactionInfo>(criteria);
        }

        private async Task<IEnumerable<GroupHeadline>> GetGroupHeadlineAsync(string custIds)
        {
            var criteria = new GetGroupHeadlineCriteria { CustIds = custIds };
            return await baseService.GetDataAsync<GroupHeadline>(criteria);
        }

        private async Task<IEnumerable<SystemSettingMapping>> GetSystemSettingMappingAsync(string[] settingNames)
        {
            var criteria = new GetSystemSettingMappingCriteria
            {
                Settings = string.Join(",", settingNames)
            };
            return await baseService.GetDataAsync<SystemSettingMapping>(criteria);
        }

        private async Task<IEnumerable<SiteSettingMapping>> GetSiteSettingMappingAsync()
        {
            var criteria = new GetSystemSettingMappingCriteria
            {
                Settings = "Site"
            };
            return await baseService.GetDataAsync<SiteSettingMapping>(criteria);
        }

        private async Task<CustomerInfo> GetLicenseeInfoAsync()
        {
            var criteria = new GetCustomerInfoByUsernameCriteria { Username = appSettings.LicenseeUsername, IsGuest = true };
            return await customerRepository.GetCustomerInfoByUsernameAsync(criteria);
        }

        private async Task<IEnumerable<SettingItem>> GetAllSettingAsync()
        {
            var criteria = new GetAllSettingCriteria();
            var settings = await baseService.GetDataAsync<SettingItem>(criteria);
            IEnumerable<SettingItem> relations = await GetAllSettingRelationAsync();
            foreach (SettingItem setting in settings)
            {
                var relation = relations.FirstOrDefault(x => x.SettingId == setting.SettingId);
                setting.ParentId = relation != null ? relation.ParentId : 0;
            }

            return settings;
        }

        private async Task<IEnumerable<SettingItem>> GetAllSettingRelationAsync()
        {
            var criteria = new GetAllSettingRelationCriteria();
            return await baseService.GetDataAsync<SettingItem>(criteria);
        }

        private async Task CreateFeedbackForCreditSiteAsync(
            Message message,
            MatchInfo matchInfo,
            IEnumerable<TransactionInfo> transactionInfos,
            IEnumerable<GroupHeadline> headlines,
            IEnumerable<SettingItem> settings,
            IEnumerable<SystemSettingMapping> settingMappings)
        {
            var timeSpan = new TimeSpan(appSettings.TimeSpan, 0, 0);
            var feedbackDate = DateTime.Now.Add(timeSpan);
            var status = settings.FirstOrDefault(x => x.SettingName.Equals(SettingName.Status, StringComparison.OrdinalIgnoreCase)
                && x.SettingValue.Equals("Closed", StringComparison.OrdinalIgnoreCase));
            var response = settings.FirstOrDefault(x => x.SettingName.Equals(SettingName.Response, StringComparison.OrdinalIgnoreCase)
                && x.SettingValue.Equals("Neutral", StringComparison.OrdinalIgnoreCase));
            var station = settings.FirstOrDefault(x => x.SettingName.Equals(SettingName.Extension, StringComparison.OrdinalIgnoreCase)
                && x.SettingId == Constants.ExtentionNA);
            var category = GetCategorySubCategory(settings);

            var criteria = new CreateMutipleFeedbackCriteria
            {
                CreatorId = appSettings.CreatorId,
                UserId = appSettings.CreatorId,
                UserRole = Constants.UserRole,
                FeedbackDate = feedbackDate.Subtract(timeSpan),
                ResolutionDate = feedbackDate.AddMinutes(1).Subtract(timeSpan),
                IsCheckedDefaultComm = true,
                StationId = station != null ? station.SettingId.ToString() : string.Empty,
                StatusId = status != null ? status.SettingId : 0,
                ResponseId = response != null ? response.SettingId : 0,
                CategoryXml = category.Item3,
                CategoryIds = category.Item1.ToString(),
                SubCategoryIds = category.Item2.ToString(),
                Subject = "Special Notification",
                AutoField = string.Empty,
                AutoField1 = string.Format("Date Time & Tool: {0}/{1}/", feedbackDate.ToString("yyyyMMdd"), feedbackDate.ToString("hh:mmtt")),
                AutoField2 = "System ID: ",
                AutoField3 = string.Empty,
                Description = GetDescription(matchInfo)
            };

            var headlineIds = headlines.Select(x => x.HeadlineId).Distinct();
            foreach (var headlineId in headlineIds)
            {
                var headline = headlines.First(x => x.HeadlineId == headlineId);
                var relatedCustIds = headlines.Where(x => x.HeadlineId == headline.HeadlineId).Select(x => x.CustId);
                var transactions = transactionInfos.Where(x => relatedCustIds.Any(y => y == x.CustId));

                criteria.ProductXml = GetProductXml(matchInfo, transactions, settings, settingMappings);
                criteria.Solution = GetSolution(message, transactions);
                criteria.CustomerXml = string.Format("<ROOT><Cust CustId='{0}' Username='{1}' /></ROOT>", headline.HeadlineId, headline.HeadlineName);
                criteria.RelatedCustomerXml = GetRelatedCustomerXml(headline.HeadlineId, transactions);

                await baseService.ExecuteNonQueryAsync(criteria);
            }
        }

        private async Task CreateFeedbackForDepositSiteAsync(
            CustomerInfo licenseeInfo,
            Message message,
            MatchInfo matchInfo,
            IEnumerable<TransactionInfo> transactionInfos,
            IEnumerable<SettingItem> settings,
            IEnumerable<SystemSettingMapping> settingMappings,
            IEnumerable<SiteSettingMapping> siteSettingMappings)
        {
            var timeSpan = new TimeSpan(appSettings.TimeSpan, 0, 0);
            var feedbackDate = DateTime.Now.Add(timeSpan);
            var communicationTool = GetCommunicationTool(settings);
            IEnumerable<SiteSettingMapping> licenseeManagers = siteSettingMappings
                .Where(x => transactionInfos.Any(y => x.SettingSource.Equals(y.Site, StringComparison.OrdinalIgnoreCase)));
            var status = settings.FirstOrDefault(x => x.SettingName.Equals(SettingName.Status, StringComparison.OrdinalIgnoreCase)
                && x.SettingValue.Equals("Closed", StringComparison.OrdinalIgnoreCase));
            var response = settings.FirstOrDefault(x => x.SettingName.Equals(SettingName.Response, StringComparison.OrdinalIgnoreCase)
                && x.SettingValue.Equals("Neutral", StringComparison.OrdinalIgnoreCase));
            var station = settings.FirstOrDefault(x => x.SettingName.Equals(SettingName.Extension, StringComparison.OrdinalIgnoreCase)
                && x.SettingId == Constants.ExtentionNA);
            var category = GetCategorySubCategory(settings);

            var criteria = new CreateFeedbackCriteria
            {
                CreatorId = appSettings.CreatorId,
                UserId = appSettings.CreatorId,
                FeedbackDate = feedbackDate.Subtract(timeSpan),
                ResolutionDate = feedbackDate.AddMinutes(1).Subtract(timeSpan),
                CustId = licenseeInfo.CustId,
                Username = licenseeInfo.Username,
                StationId = station != null ? station.SettingId.ToString() : string.Empty,
                CommunicationId = communicationTool,
                CategoryXml = category.Item3,
                ProductXml = GetProductXml(matchInfo, transactionInfos, settings, settingMappings),
                StatusId = status != null ? status.SettingId : 0,
                LanguageId = licenseeInfo.LanguageId,
                ResponseId = response != null ? response.SettingId : 0,
                RoleId = licenseeInfo.RoleId,
                CountryId = licenseeInfo.CountryId,
                RelatedCustId = string.Join(", ", licenseeManagers.Select(x => x.LicenseeManagerId).Distinct()),
                Subject = "Special Notification",
                AutoField = GetAutoField(feedbackDate, communicationTool, licenseeManagers),
                Description = GetDescription(),
                Solution = GetSolution(message),
                LicenseeType = licenseeInfo.LicenseeType,
                TitleId = licenseeInfo.TitleId
            };

            await baseService.ExecuteNonQueryAsync(criteria);
        }

        private static string GetCommunicationTool(IEnumerable<SettingItem> settings)
        {
            var communication = settings.FirstOrDefault(x => x.SettingName.Equals(SettingName.CommunicationTool, StringComparison.OrdinalIgnoreCase)
                && x.SettingValue.Equals("Skype", StringComparison.OrdinalIgnoreCase));

            return communication != null ? communication.SettingId.ToString() : Constants.CommunicationNA.ToString();
        }

        private static string GetProductXml(
            MatchInfo matchInfo,
            IEnumerable<TransactionInfo> transactionInfos,
            IEnumerable<SettingItem> settings,
            IEnumerable<SystemSettingMapping> settingMappings)
        {
            const string XmlTemplate = "<Product ProductID='{0}' SubProductID='{1}' BetTypeID='{2}' />";
            int sportsbook = GetSportsbookProduct(settings);
            if (sportsbook != Constants.ProductNA)
            {
                IEnumerable<int> subProducts = GetSubProduct(sportsbook, matchInfo, settings, settingMappings);
                if (subProducts.Any())
                {
                    IEnumerable<SettingItem> bettypes = GetBettype(subProducts, transactionInfos, settings, settingMappings);
                    var stringBuilder = new StringBuilder("<ROOT>");
                    foreach (var subProduct in subProducts)
                    {
                        if (bettypes.Any())
                        {
                            foreach (var bettype in bettypes.Where(x => x.ParentId == subProduct))
                            {
                                stringBuilder.AppendFormat(XmlTemplate, sportsbook, subProduct, bettype.SettingId);
                            }
                        }
                        else
                        {
                            stringBuilder.AppendFormat(XmlTemplate, sportsbook, subProduct, Constants.BettypeNA);
                        }
                    }

                    stringBuilder.Append("</ROOT>");
                    return stringBuilder.ToString();
                }

                return string.Format($"<ROOT>{XmlTemplate}</ROOT>", sportsbook, Constants.SubProductNA, Constants.BettypeNA);
            }

            return string.Format($"<ROOT>{XmlTemplate}</ROOT>", Constants.ProductNA, Constants.SubProductNA, Constants.BettypeNA);
        }

        private static int GetSportsbookProduct(IEnumerable<SettingItem> settings)
        {
            var product = settings.FirstOrDefault(x => x.SettingName.Equals(SettingName.Product, StringComparison.OrdinalIgnoreCase)
                && x.SettingValue.Equals("Sportsbook", StringComparison.OrdinalIgnoreCase));

            return product != null ? product.SettingId : Constants.ProductNA;
        }

        private static IEnumerable<int> GetSubProduct(
            int productId,
            MatchInfo matchInfo,
            IEnumerable<SettingItem> settings,
            IEnumerable<SystemSettingMapping> settingMappings)
        {
            var sport = matchInfo.SportName;
            var mappings = settingMappings.Where(x => x.SettingName.Equals(SettingName.SubProduct, StringComparison.OrdinalIgnoreCase)
                && x.SettingSource.Equals(sport, StringComparison.OrdinalIgnoreCase));

            return settings.Where(x => x.SettingName.Equals(SettingName.SubProduct, StringComparison.OrdinalIgnoreCase)
                && x.ParentId == productId
                && mappings.Any(y => y.SettingId == x.SettingId)).Select(x => x.SettingId).Distinct();
        }

        private static IEnumerable<SettingItem> GetBettype(
            IEnumerable<int> subProducts,
            IEnumerable<TransactionInfo> transactionInfos,
            IEnumerable<SettingItem> settings,
            IEnumerable<SystemSettingMapping> settingMappings)
        {
            var mappings = settingMappings.Where(x => x.SettingName.Equals(SettingName.Bettype, StringComparison.OrdinalIgnoreCase)
                && transactionInfos.Any(y => y.Bettype.Equals(x.SettingSource, StringComparison.OrdinalIgnoreCase)));

            return settings.Where(x => x.SettingName.Equals(SettingName.Bettype, StringComparison.OrdinalIgnoreCase)
                && subProducts.Any(y => y == x.ParentId)
                && mappings.Any(y => y.SettingId == x.SettingId));
        }

        private static Tuple<int, int, string> GetCategorySubCategory(IEnumerable<SettingItem> settings)
        {
            var category = settings.FirstOrDefault(x => x.SettingName.Equals(SettingName.MainCategory, StringComparison.OrdinalIgnoreCase)
                && x.SettingValue.Equals("Special Notification", StringComparison.OrdinalIgnoreCase));
            var subCategoryId = 0;

            if (category != null)
            {
                var subCategory = settings.FirstOrDefault(x => x.SettingName.Equals(SettingName.SubProduct, StringComparison.OrdinalIgnoreCase)
                    && x.ParentId == category.SettingId
                    && x.SettingValue.Equals("Abnormal Bets", StringComparison.OrdinalIgnoreCase));

                subCategoryId = subCategory != null ? subCategory.SettingId : 0;
            }

            return new Tuple<int, int, string>(
                category != null ? category.SettingId : 0,
                subCategoryId,
                string.Format("<ROOT><Category MainCategoryID='{0}' SubCategoryID='{1}' /></ROOT>",
                    category != null ? category.SettingId : 0,
                    subCategoryId));
        }

        private static string GetRelatedCustomerXml(int headlineId, IEnumerable<TransactionInfo> transactionInfos)
        {
            const string XmlTemplate = "<Row CustId='{0}' RelatedCustId='{1}' Username='{2}' />";
            var stringBuilder = new StringBuilder("<ROOT>");
            foreach (var transaction in transactionInfos)
            {
                stringBuilder.AppendFormat(XmlTemplate, headlineId, transaction.CustId, transaction.Username);
            }

            return stringBuilder.Append("</ROOT>").ToString();
        }

        private static string GetAutoField(
            DateTime feedbackDate,
            string communicationTool,
            IEnumerable<SiteSettingMapping> licenseeManagers)
        {
            var stringBuilder = new StringBuilder("Date Time & Tool: ");
            stringBuilder.AppendFormat("{0}/{1}/{2}", feedbackDate.ToString("yyyyMMdd"), feedbackDate.ToString("hh:mmtt"), communicationTool);
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.AppendFormat("System ID: {0}", string.Join(", ", licenseeManagers.Select(x => x.LicenseeManager).Distinct()));

            return stringBuilder.ToString();
        }

        private static string GetDescription()
        {
            var stringBuilder = new StringBuilder("<span style=\"color: rgb(255, 0, 0);\">This is an auto-generated entry.</span>");
            stringBuilder.Append("<br />Announcement sent to affected Licensees and informed our related department found abnormal tickets of members for the match.");
            stringBuilder.Append("<br /><br />Please refer to the system email subject \"Voided Bets Report\" for those affected customers &amp; tickets for reference.");
            return stringBuilder.ToString();
        }

        private static string GetDescription(MatchInfo matchInfo)
        {
            var stringBuilder = new StringBuilder("<span style=\"color: rgb(255, 0, 0);\">This is an auto-generated entry.</span>");
            stringBuilder.AppendFormat("<br />Date: {0}", matchInfo.EventDate.ToString("dd-MMM-yyyy"));
            stringBuilder.AppendFormat("<br />Sport: {0}", matchInfo.SportName);
            stringBuilder.AppendFormat("<br />Event Start Time: {0}", matchInfo.KickOffTime);
            stringBuilder.AppendFormat("<br />League: {0}", matchInfo.LeagueName);
            stringBuilder.AppendFormat("<br />Match: {0} - vs - {1}", matchInfo.HomeName, matchInfo.AwayName);
            return stringBuilder.ToString();
        }

        private static string GetSolution(Message message)
        {
            var stringBuilder = new StringBuilder(message.Content_en);
            stringBuilder.Append("<br /><br />").Append(message.Content_cs);
            return stringBuilder.ToString();
        }

        private static string GetSolution(Message message, IEnumerable<TransactionInfo> transactionInfos)
        {
            var stringBuilder = new StringBuilder(message.Content_en);
            stringBuilder.Append("<br /><br />").Append(message.Content_cs);
            stringBuilder.Append("<br /><br />Kindly refer to Agency site \"Cancelled Bets\" for Voided user.");
            stringBuilder.Append("<br /><br />Affted users and tickets:");
            foreach (var transaction in transactionInfos)
            {
                stringBuilder.AppendFormat("<br />{0} {1}", transaction.Username, transaction.TransId);
            }

            return stringBuilder.ToString();
        }
    }
}