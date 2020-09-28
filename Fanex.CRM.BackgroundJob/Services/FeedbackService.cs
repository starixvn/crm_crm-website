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
    using global::CRM.Core.Models;
    using global::CRM.Core.Services;
    using Microsoft.Extensions.Options;

    public interface IFeedbackService
    {
        Task CreateFeedbackForVoidingAbnormalBetAsync(AbnormalBetSystemSetting setting);
    }

    public class FeedbackService : IFeedbackService
    {
        private readonly AppJobSettings appSettings;
        private readonly IBaseService baseService;
        private readonly ISettingService settingService;
        private readonly ISystemSettingService systemSettingService;
        private readonly IMessageService messageService;
        private readonly ICustomerService customerService;
        private readonly ICRMLogService logService;
        private readonly IFeedbackTicketVoidingService ticketVoidingService;

#pragma warning disable S107 // Methods should not have too many parameters
        public FeedbackService(
            IOptionsSnapshot<AppJobSettings> appSettings,
            IBaseService baseService,
            ISettingService settingService,
            ISystemSettingService systemSettingService,
            IMessageService messageService,
            ICustomerService customerService,
            ICRMLogService logService,
            IFeedbackTicketVoidingService ticketVoidingService)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            this.appSettings = appSettings.Value;
            this.baseService = baseService;
            this.settingService = settingService;
            this.systemSettingService = systemSettingService;
            this.messageService = messageService;
            this.customerService = customerService;
            this.logService = logService;
            this.ticketVoidingService = ticketVoidingService;
        }

        public async Task CreateFeedbackForVoidingAbnormalBetAsync(AbnormalBetSystemSetting setting)
        {
            var settingJobStatus = new List<SystemSettingItem>
            {
                new SystemSettingItem { SettingName = SystemSettingName.JobStatus, Value = JobStatus.Running },
                new SystemSettingItem { SettingName = SystemSettingName.LastRuntime, Value = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff tt") }
            };
            await systemSettingService.EditSystemSettingAsync(settingJobStatus);

            IEnumerable<VoidingMessage> voidingMessages = await messageService.GetVoidingMessageAsync();
            voidingMessages = await messageService.ImportVoidingMessageAsync(voidingMessages, setting.BatchSize);
            IEnumerable<VoidingMessage> failMessages = await messageService.GetFailMessageAsync();
            voidingMessages = voidingMessages.Concat(failMessages).Distinct().ToList();
            if (!voidingMessages.Any())
            {
                await EditSystemSettingAsync();
                return;
            }

            IEnumerable<int> messageIds = voidingMessages.Select(x => x.MessageId).Distinct().ToList();
            IEnumerable<Message> messages = await messageService.GetMessageInfoAsync(messageIds);
            ValueTuple<IEnumerable<MatchInfo>, IEnumerable<TransactionInfo>> ticketInfo = await ticketVoidingService.GetTicketVoidingInfoAsync(voidingMessages, setting);
            if (!ticketInfo.Item1.Any())
            {
                await EditSystemSettingAsync();
                return;
            }

            logService.LogInformation(string.Format("Create feedback for messages: {0}", string.Join(",", ticketInfo.Item1.Select(x => x.MessageId))));

            var settingNames = new[] { SettingName.SubProduct, SettingName.Bettype };
            IEnumerable<SettingItem> settings = await settingService.GetAllSettingAsync();
            IEnumerable<SystemSettingMapping> settingMappings = await settingService.GetSystemSettingMappingAsync(settingNames);
            IEnumerable<SiteSettingMapping> siteSettingMappings = await settingService.GetSiteSettingMappingAsync();
            CustomerInfo licenseeInfo = await customerService.GetCustomerInfoAsync(appSettings.LicenseeUsername, true);
            var custIds = ticketInfo.Item2.Where(x => !x.IsDeposit).Select(x => x.CustId).Distinct();
            IEnumerable<GroupHeadline> headlines = await customerService.GetGroupHeadlineAsync(custIds);

            ticketInfo = await ticketVoidingService.ImportTicketVoidingInfoAsync(
                voidingMessages,
                ticketInfo.Item1,
                ticketInfo.Item2,
                siteSettingMappings,
                headlines,
                licenseeInfo);

            foreach (MatchInfo matchInfo in ticketInfo.Item1)
            {
                Message message = messages.First(x => x.MessageId == matchInfo.MessageId);
                IEnumerable<TransactionInfo> credits = ticketInfo.Item2.Where(x => x.MessageId == matchInfo.MessageId && !x.IsDeposit);
                IEnumerable<TransactionInfo> deposits = ticketInfo.Item2.Where(x => x.MessageId == matchInfo.MessageId && x.IsDeposit);

                if (credits.Any())
                {
                    await CreateFeedbackForCreditSiteAsync(
                        message,
                        matchInfo,
                        credits,
                        headlines.Where(x => credits.Any(y => x.CustId == y.CustId)),
                        settings,
                        settingMappings);
                }

                if (deposits.Any())
                {
                    GetLicenseeManager(deposits, siteSettingMappings);
                    await CreateFeedbackForDepositSiteAsync(
                        licenseeInfo,
                        message,
                        matchInfo,
                        deposits,
                        settings,
                        settingMappings);
                }
            }

            await EditSystemSettingAsync();
        }

        protected internal virtual async Task CreateFeedbackForCreditSiteAsync(
            Message message,
            MatchInfo matchInfo,
            IEnumerable<TransactionInfo> transactionInfos,
            IEnumerable<GroupHeadline> headlines,
            IEnumerable<SettingItem> settings,
            IEnumerable<SystemSettingMapping> settingMappings)
        {
            logService.LogInformation("Begin mapping common setting for MessageId: " + message.MessageId);
            var timeSpan = new TimeSpan(appSettings.TimeSpan, 0, 0);
            var feedbackDate = DateTime.Now.Add(timeSpan);
            var status = settings.FirstOrDefault(x => x.SettingName.Equals(SettingName.Status, StringComparison.OrdinalIgnoreCase)
                && x.SettingValue.Equals("Closed", StringComparison.OrdinalIgnoreCase));
            var response = settings.FirstOrDefault(x => x.SettingName.Equals(SettingName.Response, StringComparison.OrdinalIgnoreCase)
                && x.SettingValue.Equals("Neutral", StringComparison.OrdinalIgnoreCase));
            var station = settings.FirstOrDefault(x => x.SettingName.Equals(SettingName.Extension, StringComparison.OrdinalIgnoreCase)
                && x.SettingId == Constants.ExtentionNA);
            var category = GetCategorySubCategory(settings);

            var criteria = new CreateMutipleFeedbackCommand
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
                AutoField1 = string.Empty,
                AutoField2 = string.Format("Date Time & Tool: {0}/{1}/", feedbackDate.ToString("yyyyMMdd"), feedbackDate.ToString("hh:mmtt")),
                AutoField3 = $"{Environment.NewLine}System ID: "
            };
            logService.LogInformation("End mapping common setting for MessageId: " + message.MessageId);

            var headlineIds = headlines.Select(x => x.HeadlineId).Distinct();
            foreach (var headlineId in headlineIds)
            {
                GroupHeadline headline = headlines.First(x => x.HeadlineId == headlineId);
                IEnumerable<int> relatedCustomerIds = headlines.Where(x => x.HeadlineId == headline.HeadlineId).Select(x => x.CustId);
                criteria.CustomerXml = string.Format("<ROOT><Cust CustId='{0}' Username='{1}' /></ROOT>", headline.HeadlineId, headline.HeadlineName);

                await CreateFeedbackForCreditSiteByBatchAsync(
                    headline,
                    relatedCustomerIds,
                    message,
                    matchInfo,
                    transactionInfos,
                    settings,
                    settingMappings,
                    criteria);
            }
        }

#pragma warning disable S107 // Methods should not have too many parameters
        protected internal virtual async Task CreateFeedbackForCreditSiteByBatchAsync(
            GroupHeadline headline,
            IEnumerable<int> relatedCustomerIds,
            Message message,
            MatchInfo matchInfo,
            IEnumerable<TransactionInfo> transactionInfos,
            IEnumerable<SettingItem> settings,
            IEnumerable<SystemSettingMapping> settingMappings,
            CreateMutipleFeedbackCommand criteria)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            int itemEachBatch = appSettings.NoOfRelatedCustomerPerFeedback;
            int totalBatch = (int)Math.Ceiling((double)relatedCustomerIds.Count() / itemEachBatch);
            for (int i = 0; i < totalBatch; i++)
            {
                List<int> batch = relatedCustomerIds.Skip(i * itemEachBatch).Take(itemEachBatch).ToList();

                logService.LogInformation(string.Format("Begin mapping setting for MessageId: {0}\n\rHeadlineId: {1}\n\rRelatedCustIds: {2}",
                    message.MessageId,
                    headline.HeadlineId,
                    string.Join(",", batch)));

                IEnumerable<TransactionInfo> transactions = transactionInfos.Where(x => batch.Any(y => y == x.CustId));
                criteria.ProductXml = GetProductXml(matchInfo, transactions, settings, settingMappings);
                criteria.Solution = GetSolutionForCreditSite(message, transactions);
                criteria.RelatedCustomerXml = GetRelatedCustomerXml(headline.HeadlineId, transactions);
                criteria.MessageId = message.MessageId;
                criteria.ListOrigCustIds = string.Join(",", batch);

                logService.LogInformation(string.Format("End mapping setting for MessageId: {0}\n\rHeadlineId: {1}\n\rRelatedCustIds: {2}",
                    message.MessageId,
                    headline.HeadlineId,
                    string.Join(",", batch)));
                IEnumerable<int> requestIds = transactions.Select(x => x.RequestId).Distinct().ToList();
                criteria.RequestIds = string.Join(",", requestIds);
                criteria.Description = GetDescriptionForCreditSite(message.MessageId, criteria.RequestIds, matchInfo);
                try
                {
                    logService.LogInformation(string.Format("Begin create feedback for MessageId: {0}\n\rHeadlineId: {1}\n\rRelatedCustIds: {2}",
                        message.MessageId,
                        headline.HeadlineId,
                        string.Join(",", batch)));
                    CommandResult commandResult = await baseService.ExecuteNonQueryAsync(criteria);
                    if (commandResult.IsSuccessful)
                    {
                        logService.LogInformation(string.Format("Create feedback successfully. MessageId: {0}\n\rHeadlineId: {1}\n\rRelatedCustIds: {2}",
                            message.MessageId,
                            headline.HeadlineId,
                            string.Join(",", batch)));
                    }
                    else
                    {
                        logService.LogInformation(string.Format("{0}. MessageId: {1}\n\rHeadlineId: {2}\n\rRelatedCustIds: {3}",
                            commandResult.Message,
                            message.MessageId,
                            headline.HeadlineId,
                            string.Join(",", batch)));
                        await ticketVoidingService.EditFeedbackInfoAsync(message.MessageId, requestIds, batch, CreateFeedbackJobStatus.Fail, commandResult.Message);
                        await logService.ErrorAsync(commandResult, criteria);
                    }
                }
                catch (Exception ex)
                {
                    logService.LogInformation(string.Format("{0}. MessageId: {1}\n\rHeadlineId: {2}\n\rRelatedCustIds: {3}",
                        ex.Message,
                        message.MessageId,
                        headline.HeadlineId,
                        string.Join(",", batch)));
                    await ticketVoidingService.EditFeedbackInfoAsync(message.MessageId, requestIds, batch, CreateFeedbackJobStatus.Fail, ex.Message);
                    await logService.ErrorAsync(ex);
                }
            }
        }

        protected internal virtual async Task CreateFeedbackForDepositSiteAsync(
            CustomerInfo licenseeInfo,
            Message message,
            MatchInfo matchInfo,
            IEnumerable<TransactionInfo> transactionInfos,
            IEnumerable<SettingItem> settings,
            IEnumerable<SystemSettingMapping> settingMappings)
        {
            logService.LogInformation("Begin mapping setting for MessageId: " + message.MessageId);
            var timeSpan = new TimeSpan(appSettings.TimeSpan, 0, 0);
            var feedbackDate = DateTime.Now.Add(timeSpan);
            var communicationTool = GetCommunicationTool(settings);
            var status = settings.FirstOrDefault(x => x.SettingName.Equals(SettingName.Status, StringComparison.OrdinalIgnoreCase)
                && x.SettingValue.Equals("Closed", StringComparison.OrdinalIgnoreCase));
            var response = settings.FirstOrDefault(x => x.SettingName.Equals(SettingName.Response, StringComparison.OrdinalIgnoreCase)
                && x.SettingValue.Equals("Neutral", StringComparison.OrdinalIgnoreCase));
            var station = settings.FirstOrDefault(x => x.SettingName.Equals(SettingName.Extension, StringComparison.OrdinalIgnoreCase)
                && x.SettingId == Constants.ExtentionNA);
            var category = GetCategorySubCategory(settings);

            var criteria = new CreateFeedbackCommand
            {
                CreatorId = appSettings.CreatorId,
                UserId = appSettings.CreatorId,
                FeedbackDate = feedbackDate.Subtract(timeSpan),
                ResolutionDate = feedbackDate.AddMinutes(1).Subtract(timeSpan),
                CustId = licenseeInfo.CustId,
                Username = licenseeInfo.Username,
                StationId = station != null ? station.SettingId.ToString() : string.Empty,
                CommunicationId = communicationTool,
                CategoryXml = category != null ? category.Item3 : string.Empty,
                ProductXml = GetProductXml(matchInfo, transactionInfos, settings, settingMappings),
                StatusId = status != null ? status.SettingId : 0,
                LanguageId = licenseeInfo.LanguageId,
                ResponseId = response != null ? response.SettingId : 0,
                RoleId = licenseeInfo.RoleId,
                CountryId = licenseeInfo.CountryId,
                Subject = "Special Notification",
                Solution = GetSolution(message),
                LicenseeType = licenseeInfo.LicenseeType,
                TitleId = licenseeInfo.TitleId,
                SiteId = licenseeInfo.SiteId
            };
            logService.LogInformation("End mapping setting for MessageId: " + message.MessageId);

            await CreateFeedbackForDepositByBatchAsync(
                licenseeInfo,
                message,
                transactionInfos,
                feedbackDate,
                communicationTool,
                criteria);
        }

        protected internal virtual async Task CreateFeedbackForDepositByBatchAsync(
            CustomerInfo licenseeInfo,
            Message message,
            IEnumerable<TransactionInfo> transactionInfos,
            DateTime feedbackDate,
            string communicationTool,
            CreateFeedbackCommand criteria)
        {
            IList<int> licenseeManagerIds = transactionInfos.Select(x => x.LicenseeManagerId).Distinct().ToList();
            int itemEachBatch = appSettings.NoOfRelatedCustomerPerFeedback;
            int totalBatch = (int)Math.Ceiling((double)licenseeManagerIds.Count() / itemEachBatch);
            for (int i = 0; i < totalBatch; i++)
            {
                IList<int> batch = licenseeManagerIds.Skip(i * itemEachBatch).Take(itemEachBatch).ToList();
                IList<TransactionInfo> transactions = transactionInfos.Where(x => batch.Any(y => y == x.LicenseeManagerId)).ToList();
                IList<string> licenseeManagers = transactions.Where(x => x.LicenseeManagerId != 0).Select(x => x.LicenseeManager).Distinct().ToList();
                IList<int> custIds = transactions.Select(x => x.CustId).Distinct().ToList();

                criteria.RelatedCustId = string.Join(", ", batch.Where(x => x != 0).Select(x => x));
                criteria.AutoField = GetAutoField(feedbackDate, communicationTool, licenseeManagers);
                criteria.MessageId = message.MessageId;
                criteria.ListOrigCustIds = string.Join(",", custIds);
                IEnumerable<int> requestIds = transactions.Select(x => x.RequestId).Distinct().ToList();
                criteria.RequestIds = string.Join(",", requestIds);
                criteria.Description = GetDescriptionForDepositeSite(message.MessageId, criteria.RequestIds);
                try
                {
                    logService.LogInformation(string.Format("Begin create feedback for MessageId: {0}\n\rHeadlineId: {1}\n\rRelatedCustIds: {2}\n\rCustIds: {3}",
                            message.MessageId,
                            licenseeInfo.CustId,
                            string.Join(",", batch),
                            string.Join(",", custIds)));
                    CommandResult commandResult = await baseService.ExecuteNonQueryAsync(criteria);
                    if (commandResult.IsSuccessful)
                    {
                        logService.LogInformation(string.Format("{0}. MessageId: {1}\n\rHeadlineId: {2}\n\rRelatedCustIds: {3}\n\rCustIds: {4}",
                            "Create feedback successfully",
                            message.MessageId,
                            licenseeInfo.CustId,
                            string.Join(",", batch),
                            string.Join(",", custIds)));
                    }
                    else
                    {
                        logService.LogInformation(string.Format("{0}. MessageId: {1}\n\rHeadlineId: {2}\n\rRelatedCustIds: {3}\n\rCustIds: {4}",
                            commandResult.Message,
                            message.MessageId,
                            licenseeInfo.CustId,
                            string.Join(",", batch),
                            string.Join(",", custIds)));
                        await ticketVoidingService.EditFeedbackInfoAsync(message.MessageId, requestIds, custIds, CreateFeedbackJobStatus.Fail, commandResult.Message);
                        await logService.ErrorAsync(commandResult, criteria);
                    }
                }
                catch (Exception ex)
                {
                    logService.LogInformation(string.Format("{0}. MessageId: {1}\n\rHeadlineId: {2}\n\rRelatedCustIds: {3}\n\rCustIds: {4}",
                        ex.Message,
                        message.MessageId,
                        licenseeInfo.CustId,
                        string.Join(",", batch),
                        string.Join(",", custIds)));
                    await ticketVoidingService.EditFeedbackInfoAsync(message.MessageId, requestIds, custIds, CreateFeedbackJobStatus.Fail, ex.Message);
                    await logService.ErrorAsync(ex);
                }
            }
        }

        protected internal virtual void GetLicenseeManager(
            IEnumerable<TransactionInfo> transactionInfos,
            IEnumerable<SiteSettingMapping> siteSettingMappings)
        {
            foreach (TransactionInfo transaction in transactionInfos)
            {
                SiteSettingMapping licenseeManager = siteSettingMappings.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.SettingSource)
                        && x.SettingSource.Equals(transaction.Site, StringComparison.OrdinalIgnoreCase));

                if (licenseeManager != null)
                {
                    transaction.LicenseeManagerId = licenseeManager.LicenseeManagerId;
                    transaction.LicenseeManager = licenseeManager.LicenseeManager;
                }
            }
        }

        protected internal virtual string GetCommunicationTool(IEnumerable<SettingItem> settings)
        {
            var communication = settings.FirstOrDefault(x => x.SettingName.Equals(SettingName.CommunicationTool, StringComparison.OrdinalIgnoreCase)
                && x.SettingValue.Equals("Skype", StringComparison.OrdinalIgnoreCase));

            return communication != null ? communication.SettingId.ToString() : Constants.CommunicationNA.ToString();
        }

        protected internal virtual string GetProductXml(
            MatchInfo matchInfo,
            IEnumerable<TransactionInfo> transactionInfos,
            IEnumerable<SettingItem> settings,
            IEnumerable<SystemSettingMapping> settingMappings)
        {
            logService.LogInformation("Begin mapping product, sub-product, bet type.");
            const string XmlTemplate = "<Product ProductID='{0}' SubProductID='{1}' BetTypeID='{2}' />";
            SettingItem sportsbook = GetSportsbookProduct(settings);
            IList<SettingItem> subProducts = GetSubProduct(sportsbook.SettingId, matchInfo, settings, settingMappings);
            IList<SettingItem> bettypes = GetBettype(subProducts.Select(x => x.SettingId), transactionInfos, settings, settingMappings);
            var stringBuilder = new StringBuilder(Constants.BeginRootTag);

            foreach (SettingItem subProduct in subProducts)
            {
                IList<SettingItem> tempBettypes = bettypes.Where(x => x.ParentIds.Any(y => y == subProduct.SettingId)).ToList();
                if (tempBettypes.Any())
                {
                    foreach (SettingItem bettype in tempBettypes)
                    {
                        stringBuilder.AppendFormat(XmlTemplate, sportsbook.SettingId, subProduct.SettingId, bettype.SettingId);
                    }
                }
                else
                {
                    stringBuilder.AppendFormat(XmlTemplate, sportsbook.SettingId, subProduct.SettingId, Constants.BettypeNA);
                }
            }

            logService.LogInformation("End mapping product, sub-product, bet type.");
            return stringBuilder.Append(Constants.EndRootTag).ToString();
        }

        protected internal virtual SettingItem GetSportsbookProduct(IEnumerable<SettingItem> settings)
        {
            var product = settings.FirstOrDefault(x => x.SettingName.Equals(SettingName.Product, StringComparison.OrdinalIgnoreCase)
                && x.SettingValue.Equals("Sportsbook", StringComparison.OrdinalIgnoreCase));

            return product ?? new SettingItem { SettingId = Constants.ProductNA };
        }

        protected internal virtual IList<SettingItem> GetSubProduct(
            int productId,
            MatchInfo matchInfo,
            IEnumerable<SettingItem> settings,
            IEnumerable<SystemSettingMapping> settingMappings)
        {
            var sport = matchInfo.SportName;
            IList<SystemSettingMapping> mappings = settingMappings.Where(mapping => mapping.SettingName.Equals(SettingName.SubProduct, StringComparison.OrdinalIgnoreCase)
                && !string.IsNullOrWhiteSpace(mapping.SettingSource)
                && mapping.SettingSource.Equals(sport, StringComparison.OrdinalIgnoreCase)).ToList();
            IList<SettingItem> subProducts = settings.Where(sub => sub.SettingName.Equals(SettingName.SubProduct, StringComparison.OrdinalIgnoreCase)
                && sub.ParentIds.Any(parent => parent == productId)
                && mappings != null
                && mappings.Any(mapping => mapping.SettingId == sub.SettingId)).ToList();

            return subProducts != null && subProducts.Any()
                ? subProducts.GroupBy(x => x.SettingId).Select(x => x.First()).ToList()
                : new List<SettingItem>
                {
                    new SettingItem { SettingId = Constants.SubProductNA, ParentIds = new List<int> { Constants.ProductNA } }
                };
        }

        protected internal virtual IList<SettingItem> GetBettype(
            IEnumerable<int> subProducts,
            IEnumerable<TransactionInfo> transactionInfos,
            IEnumerable<SettingItem> settings,
            IEnumerable<SystemSettingMapping> settingMappings)
        {
            IList<SystemSettingMapping> mappings = settingMappings.Where(mapping => mapping.SettingName.Equals(SettingName.Bettype, StringComparison.OrdinalIgnoreCase)
                && !string.IsNullOrWhiteSpace(mapping.SettingSource)
                && transactionInfos.Any(tran => tran.Bettype.Equals(mapping.SettingSource, StringComparison.OrdinalIgnoreCase))).ToList();
            IList<SettingItem> bettypes = settings.Where(bettype => bettype.SettingName.Equals(SettingName.Bettype, StringComparison.OrdinalIgnoreCase)
                && subProducts.Any(sub => bettype.ParentIds.Any(parent => parent == sub))
                && mappings != null
                && mappings.Any(mapping => mapping.SettingId == bettype.SettingId)).ToList();

            return bettypes != null && bettypes.Any()
                ? bettypes.GroupBy(x => x.SettingId).Select(x => x.First()).ToList()
                : new List<SettingItem>
                {
                    new SettingItem { SettingId = Constants.BettypeNA, ParentIds = new List<int> { Constants.SubProductNA } }
                };
        }

        protected internal virtual Tuple<int, int, string> GetCategorySubCategory(IEnumerable<SettingItem> settings)
        {
            var category = settings.FirstOrDefault(x => x.SettingName.Equals(SettingName.MainCategory, StringComparison.OrdinalIgnoreCase)
                && x.SettingValue.Equals("Special Notification", StringComparison.OrdinalIgnoreCase));
            var subCategoryId = 0;

            if (category != null)
            {
                var subCategory = settings.FirstOrDefault(x => x.SettingName.Equals(SettingName.SubCategory, StringComparison.OrdinalIgnoreCase)
                    && x.ParentIds.Any(y => y == category.SettingId)
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

        protected internal virtual string GetRelatedCustomerXml(int headlineId, IEnumerable<TransactionInfo> transactionInfos)
        {
            const string XmlTemplate = "<Row CustId='{0}' RelatedCustId='{1}' Username='{2}' />";
            var stringBuilder = new StringBuilder(Constants.BeginRootTag);
            var transactions = transactionInfos.GroupBy(x => x.CustId).Select(x => x.First());
            foreach (var transaction in transactions)
            {
                stringBuilder.AppendFormat(XmlTemplate, headlineId, transaction.CustId, transaction.Username);
            }

            return stringBuilder.Append(Constants.EndRootTag).ToString();
        }

        protected internal virtual string GetAutoField(
            DateTime feedbackDate,
            string communicationTool,
            IEnumerable<string> licenseeManagers)
        {
            var stringBuilder = new StringBuilder("Date Time & Tool: ");
            stringBuilder.AppendFormat("{0}/{1}/{2}", feedbackDate.ToString("yyyyMMdd"), feedbackDate.ToString("hh:mmtt"), communicationTool);
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.AppendFormat("System ID: {0}", string.Join(", ", licenseeManagers));

            return stringBuilder.ToString();
        }

        protected internal virtual string GetDescriptionForDepositeSite(int messageId, string requestIds)
        {
            var stringBuilder = new StringBuilder("<span style=\"color: rgb(255, 0, 0);\">This is an auto-generated entry.</span>");
            stringBuilder.AppendFormat("<br /><span style=\"color: rgb(255, 0, 0);\">Msg ID: {0}, Request No.: {1}</span>", messageId, requestIds);
            stringBuilder.Append("<br />Announcement sent to affected Licensees and informed our related department found abnormal tickets of members for the match.");
            stringBuilder.Append("<br /><br />Please refer to the system email subject \"Voided Bets Report\" for those affected customers &amp; tickets for reference.");
            return stringBuilder.ToString();
        }

        protected internal virtual string GetDescriptionForCreditSite(int messageId, string requestIds, MatchInfo matchInfo)
        {
            var stringBuilder = new StringBuilder("<span style=\"color: rgb(255, 0, 0);\">This is an auto-generated entry.</span>");
            stringBuilder.AppendFormat("<br /><span style=\"color: rgb(255, 0, 0);\">Msg ID: {0}, Request No.: {1}</span>", messageId, requestIds);
            stringBuilder.AppendFormat("<br />Date: {0}", matchInfo.EventDate.ToString("dd-MMM-yyyy"));
            stringBuilder.AppendFormat("<br />Sport: {0}", matchInfo.SportName);
            stringBuilder.AppendFormat("<br />Event Start Time: {0}", matchInfo.KickOffTime);
            stringBuilder.AppendFormat("<br />League: {0}", matchInfo.LeagueName);
            stringBuilder.AppendFormat("<br />Match: {0} - vs - {1}", matchInfo.HomeName, matchInfo.AwayName);
            return stringBuilder.ToString();
        }

        protected internal virtual string GetSolution(Message message)
        {
            var stringBuilder = new StringBuilder(message.Content_en);
            stringBuilder.Append("<br /><br />").Append(message.Content_cs);
            return stringBuilder.ToString();
        }

        protected internal virtual string GetSolutionForCreditSite(Message message, IEnumerable<TransactionInfo> transactionInfos)
        {
            var stringBuilder = new StringBuilder(message.Content_en);
            stringBuilder.Append("<br /><br />").Append(message.Content_cs);
            stringBuilder.Append("<br /><br />Kindly refer to Agency site \"Cancelled Bets\" for Voided user.");
            stringBuilder.Append("<br /><br />Affected users and tickets:");
            foreach (var transaction in transactionInfos)
            {
                stringBuilder.AppendFormat("<br />{0} {1}", transaction.Username, transaction.TransId);
            }

            return stringBuilder.ToString();
        }

        protected internal virtual async Task<CommandResult> EditSystemSettingAsync()
        {
            var settingJobStatus = new List<SystemSettingItem>
            {
                new SystemSettingItem { SettingName = SystemSettingName.JobStatus, Value = JobStatus.Completed },
                new SystemSettingItem { SettingName = SystemSettingName.LastRuntime, Value = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff tt") }
            };
            return await systemSettingService.EditSystemSettingAsync(settingJobStatus);
        }
    }
}