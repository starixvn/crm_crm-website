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
    using Fanex.CRM.BackgroundJob.Repositories;
    using Microsoft.Extensions.Logging;
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
        private readonly ISettingService settingService;
        private readonly ISystemSettingService systemSettingService;
        private readonly ILogger mlogger;
        private readonly ICRMLogService logService;

        public FeedbackService(
            IOptionsSnapshot<AppSettings> appSettings,
            ICustomerRepository customerRepository,
            ICRMLogService logService,
            IBaseService baseService,
            ISettingService settingService,
            ISystemSettingService systemSettingService,
            ILogger<FeedbackService> mlogger)
        {
            this.appSettings = appSettings.Value;
            this.customerRepository = customerRepository;
            this.logService = logService;
            this.baseService = baseService;
            this.settingService = settingService;
            this.systemSettingService = systemSettingService;
            this.mlogger = mlogger;
        }

        public async Task CreateFeedbackForVoidingAbnormalBetAsync(AbnormalBetSystemSetting setting)
        {
            var settingJobStatus = new List<SystemSettingItem>
            {
                new SystemSettingItem { SettingName = SystemSettingName.JobStatus, Value = JobStatus.Running },
                new SystemSettingItem { SettingName = SystemSettingName.LastRuntime, Value = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff tt") }
            };
            await systemSettingService.UpdateSystemSettingAsync(settingJobStatus);

            IEnumerable<int> failMessageIds = await GetFailMessageAsync();
            ValueTuple<IEnumerable<MatchInfo>, IEnumerable<TransactionInfo>> ticketInfo = await GetTicketInfoAsync(failMessageIds, setting);

            if (!ticketInfo.Item1.Any())
            {
                await UpdateSystemSettingAsync(setting.LastMsgId);
                return;
            }

            var messageIds = string.Join(",", ticketInfo.Item1.Select(x => x.MessageId));
            IEnumerable<Message> messages = await GetMessageAsync(messageIds);

            mlogger.LogInformation(string.Format("Create feedback for messages: {0}", messageIds));

            var settingNames = new[] { SettingName.SubProduct, SettingName.Bettype };
            IEnumerable<SettingItem> settings = await settingService.GetAllSettingAsync();
            IEnumerable<SystemSettingMapping> settingMappings = await GetSystemSettingMappingAsync(settingNames);
            IEnumerable<SiteSettingMapping> siteSettingMappings = await GetSiteSettingMappingAsync();
            CustomerInfo licenseeInfo = await GetLicenseeInfoAsync();
            IEnumerable<GroupHeadline> headlines = await GetGroupHeadlineAsync(ticketInfo.Item2);

            ticketInfo = await StoreInfoAndGetCustomerNeedToCreateFeedbackAsync(
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

                if (!credits.Any() && !deposits.Any())
                {
                    mlogger.LogInformation(string.Format("Don't have ticket info: {0}", message.MessageId));
                    continue;
                }

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

            var lastMessageId = messages.Max(x => x.MessageId);
            await UpdateSystemSettingAsync(lastMessageId > setting.LastMsgId ? lastMessageId : setting.LastMsgId);
        }

        private async Task<IEnumerable<Message>> GetMessageAsync(string messageIds)
        {
            var criteria = new GetNewMessageCriteria
            {
                MessageIds = messageIds
            };

            var messages = await baseService.GetDataAsync<Message>(criteria);
            foreach (var message in messages)
            {
                message.Content_en = message.Content_en?.Trim().Replace("@@", string.Empty);
                message.Content_cs = message.Content_cs?.Trim().Replace("@@", string.Empty);
            }

            return messages;
        }

        private async Task<IEnumerable<int>> GetFailMessageAsync()
        {
            var criteria = new GetFailMessageCriteria();
            return await baseService.GetDataAsync<int>(criteria);
        }

        private async Task<ValueTuple<IEnumerable<MatchInfo>, IEnumerable<TransactionInfo>>> GetTicketInfoAsync(
            IEnumerable<int> failMessageIds,
            AbnormalBetSystemSetting setting)
        {
            var criteria = new GetAbnormalTicketInfoCriteria
            {
                LastMessageId = setting.LastMsgId,
                BatchSize = setting.BatchSize,
                FailMessageIds = string.Join(",", failMessageIds),
                ConfigUAT = setting.ConfigUAT,
                ConfigPRO = setting.ConfigPRO
            };

            return await baseService.FetchMultipleAsync<MatchInfo, TransactionInfo>(criteria);
        }

        private async Task<IEnumerable<GroupHeadline>> GetGroupHeadlineAsync(IEnumerable<TransactionInfo> transactionInfos)
        {
            var custIds = string.Join(",", transactionInfos.Where(x => !x.IsDeposit).Select(x => x.CustId).Distinct());
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
            var criteria = new GetCustomerInfoByUsernameCriteria
            {
                UserRole = Constants.UserRole,
                Username = appSettings.LicenseeUsername,
                IsGuest = true
            };
            return await customerRepository.GetCustomerInfoByUsernameAsync(criteria);
        }

        private async Task CreateFeedbackForCreditSiteAsync(
            Message message,
            MatchInfo matchInfo,
            IEnumerable<TransactionInfo> transactionInfos,
            IEnumerable<GroupHeadline> headlines,
            IEnumerable<SettingItem> settings,
            IEnumerable<SystemSettingMapping> settingMappings)
        {
            mlogger.LogInformation("Begin mapping common setting for MessageId: " + message.MessageId);
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
                AutoField1 = string.Empty,
                AutoField2 = string.Format("Date Time & Tool: {0}/{1}/", feedbackDate.ToString("yyyyMMdd"), feedbackDate.ToString("hh:mmtt")),
                AutoField3 = $"{Environment.NewLine}System ID: ",
                Description = GetDescription(matchInfo)
            };
            mlogger.LogInformation("End mapping common setting for MessageId: " + message.MessageId);

            var headlineIds = headlines.Select(x => x.HeadlineId).Distinct();
            foreach (var headlineId in headlineIds)
            {
                GroupHeadline headline = headlines.First(x => x.HeadlineId == headlineId);
                IEnumerable<int> relatedCustomerIds = headlines.Where(x => x.HeadlineId == headline.HeadlineId).Select(x => x.CustId);
                criteria.CustomerXml = string.Format("<ROOT><Cust CustId='{0}' Username='{1}' /></ROOT>", headline.HeadlineId, headline.HeadlineName);

                await CreateFeedbackByBatchAsync(headline, relatedCustomerIds, message, matchInfo, transactionInfos, settings, settingMappings, criteria);
            }
        }

#pragma warning disable S107 // Methods should not have too many parameters
        private async Task CreateFeedbackByBatchAsync(
            GroupHeadline headline,
            IEnumerable<int> relatedCustomerIds,
            Message message,
            MatchInfo matchInfo,
            IEnumerable<TransactionInfo> transactionInfos,
            IEnumerable<SettingItem> settings,
            IEnumerable<SystemSettingMapping> settingMappings,
            CreateMutipleFeedbackCriteria criteria)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            int itemEachBatch = appSettings.NoOfRelatedCustomerPerFeedback;
            int totalBatch = (int)Math.Ceiling((double)relatedCustomerIds.Count() / itemEachBatch);
            for (int i = 0; i < totalBatch; i++)
            {
                List<int> batch = relatedCustomerIds.Skip(i * itemEachBatch).Take(itemEachBatch).ToList();

                mlogger.LogInformation(string.Format("Begin mapping setting for MessageId: {0}\n\rHeadlineId: {1}\n\rRelatedCustIds: {2}",
                    message.MessageId,
                    headline.HeadlineId,
                    string.Join(",", batch)));

                var transactions = transactionInfos.Where(x => batch.Any(y => y == x.CustId));
                criteria.ProductXml = GetProductXml(matchInfo, transactions, settings, settingMappings);
                criteria.Solution = GetSolution(message, transactions);
                criteria.RelatedCustomerXml = GetRelatedCustomerXml(headline.HeadlineId, transactions);
                criteria.MessageId = message.MessageId;
                criteria.ListOrigCustIds = string.Join(",", batch);

                mlogger.LogInformation(string.Format("End mapping setting for MessageId: {0}\n\rHeadlineId: {1}\n\rRelatedCustIds: {2}",
                    message.MessageId,
                    headline.HeadlineId,
                    string.Join(",", batch)));
                try
                {
                    mlogger.LogInformation(string.Format("Begin create feedback for MessageId: {0}\n\rHeadlineId: {1}\n\rRelatedCustIds: {2}",
                        message.MessageId,
                        headline.HeadlineId,
                        string.Join(",", batch)));
                    CommandResult commandResult = await baseService.ExecuteNonQueryAsync(criteria);
                    if (commandResult.IsSuccessful)
                    {
                        mlogger.LogInformation(string.Format("Create feedback successfully. MessageId: {0}\n\rHeadlineId: {1}\n\rRelatedCustIds: {2}",
                            message.MessageId,
                            headline.HeadlineId,
                            string.Join(",", batch)));
                    }
                    else
                    {
                        mlogger.LogInformation(string.Format("{0}. MessageId: {1}\n\rHeadlineId: {2}\n\rRelatedCustIds: {3}",
                            commandResult.Message,
                            message.MessageId,
                            headline.HeadlineId,
                            string.Join(",", batch)));
                        await UpdateFeedbackInfoAsync(message.MessageId, batch, CreateFeedbackJobStatus.Fail, commandResult.Message);
                        await logService.ErrorAsync(commandResult, criteria);
                    }
                }
                catch (Exception ex)
                {
                    mlogger.LogInformation(string.Format("{0}. MessageId: {1}\n\rHeadlineId: {2}\n\rRelatedCustIds: {3}",
                        ex.Message,
                        message.MessageId,
                        headline.HeadlineId,
                        string.Join(",", batch)));
                    await UpdateFeedbackInfoAsync(message.MessageId, batch, CreateFeedbackJobStatus.Fail, ex.Message);
                    await logService.ErrorAsync(ex);
                }
            }
        }

        private async Task CreateFeedbackForDepositSiteAsync(
            CustomerInfo licenseeInfo,
            Message message,
            MatchInfo matchInfo,
            IEnumerable<TransactionInfo> transactionInfos,
            IEnumerable<SettingItem> settings,
            IEnumerable<SystemSettingMapping> settingMappings)
        {
            mlogger.LogInformation("Begin mapping setting for MessageId: " + message.MessageId);
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
                Subject = "Special Notification",
                Description = GetDescription(),
                Solution = GetSolution(message),
                LicenseeType = licenseeInfo.LicenseeType,
                TitleId = licenseeInfo.TitleId,
                SiteId = licenseeInfo.SiteId
            };
            mlogger.LogInformation("End mapping setting for MessageId: " + message.MessageId);

            await CreateFeedbackByBatchAsync(licenseeInfo, message, transactionInfos, feedbackDate, communicationTool, criteria);
        }

        private async Task CreateFeedbackByBatchAsync(
            CustomerInfo licenseeInfo,
            Message message,
            IEnumerable<TransactionInfo> transactionInfos,
            DateTime feedbackDate,
            string communicationTool,
            CreateFeedbackCriteria criteria)
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
                try
                {
                    mlogger.LogInformation(string.Format("Begin create feedback for MessageId: {0}\n\rHeadlineId: {1}\n\rRelatedCustIds: {2}\n\rCustIds: {3}",
                            message.MessageId,
                            licenseeInfo.CustId,
                            string.Join(",", batch),
                            string.Join(",", custIds)));
                    CommandResult commandResult = await baseService.ExecuteNonQueryAsync(criteria);
                    if (commandResult.IsSuccessful)
                    {
                        mlogger.LogInformation(string.Format("{0}. MessageId: {1}\n\rHeadlineId: {2}\n\rRelatedCustIds: {3}\n\rCustIds: {4}",
                            "Create feedback successfully",
                            message.MessageId,
                            licenseeInfo.CustId,
                            string.Join(",", batch),
                            string.Join(",", custIds)));
                    }
                    else
                    {
                        mlogger.LogInformation(string.Format("{0}. MessageId: {1}\n\rHeadlineId: {2}\n\rRelatedCustIds: {3}\n\rCustIds: {4}",
                            commandResult.Message,
                            message.MessageId,
                            licenseeInfo.CustId,
                            string.Join(",", batch),
                            string.Join(",", custIds)));
                        await UpdateFeedbackInfoAsync(message.MessageId, custIds, CreateFeedbackJobStatus.Fail, commandResult.Message);
                        await logService.ErrorAsync(commandResult, criteria);
                    }
                }
                catch (Exception ex)
                {
                    mlogger.LogInformation(string.Format("{0}. MessageId: {1}\n\rHeadlineId: {2}\n\rRelatedCustIds: {3}\n\rCustIds: {4}",
                        ex.Message,
                        message.MessageId,
                        licenseeInfo.CustId,
                        string.Join(",", batch),
                        string.Join(",", custIds)));
                    await UpdateFeedbackInfoAsync(message.MessageId, custIds, CreateFeedbackJobStatus.Fail, ex.Message);
                    await logService.ErrorAsync(ex);
                }
            }
        }

        private static void GetLicenseeManager(
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

        private static string GetCommunicationTool(IEnumerable<SettingItem> settings)
        {
            var communication = settings.FirstOrDefault(x => x.SettingName.Equals(SettingName.CommunicationTool, StringComparison.OrdinalIgnoreCase)
                && x.SettingValue.Equals("Skype", StringComparison.OrdinalIgnoreCase));

            return communication != null ? communication.SettingId.ToString() : Constants.CommunicationNA.ToString();
        }

        private string GetProductXml(
            MatchInfo matchInfo,
            IEnumerable<TransactionInfo> transactionInfos,
            IEnumerable<SettingItem> settings,
            IEnumerable<SystemSettingMapping> settingMappings)
        {
            mlogger.LogInformation("Begin mapping product, sub-product, bet type.");
            const string XmlTemplate = "<Product ProductID='{0}' SubProductID='{1}' BetTypeID='{2}' />";
            SettingItem sportsbook = GetSportsbookProduct(settings);
            IList<SettingItem> subProducts = GetSubProduct(sportsbook.SettingId, matchInfo, settings, settingMappings);
            IList<SettingItem> bettypes = GetBettype(subProducts.Select(x => x.SettingId), transactionInfos, settings, settingMappings);
            var stringBuilder = new StringBuilder("<ROOT>");

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

            mlogger.LogInformation("End mapping product, sub-product, bet type.");
            return stringBuilder.Append("</ROOT>").ToString();
        }

        private static SettingItem GetSportsbookProduct(IEnumerable<SettingItem> settings)
        {
            var product = settings.FirstOrDefault(x => x.SettingName.Equals(SettingName.Product, StringComparison.OrdinalIgnoreCase)
                && x.SettingValue.Equals("Sportsbook", StringComparison.OrdinalIgnoreCase));

            return product ?? new SettingItem { SettingId = Constants.ProductNA };
        }

        private static IList<SettingItem> GetSubProduct(
            int productId,
            MatchInfo matchInfo,
            IEnumerable<SettingItem> settings,
            IEnumerable<SystemSettingMapping> settingMappings)
        {
            var sport = matchInfo.SportName;
            IList<SystemSettingMapping> mappings = settingMappings.Where(x => x.SettingName.Equals(SettingName.SubProduct, StringComparison.OrdinalIgnoreCase)
                && !string.IsNullOrWhiteSpace(x.SettingSource)
                && x.SettingSource.Equals(sport, StringComparison.OrdinalIgnoreCase)).ToList();
            IList<SettingItem> subProducts = settings.Where(x => x.SettingName.Equals(SettingName.SubProduct, StringComparison.OrdinalIgnoreCase)
                && x.ParentIds.Any(y => y == productId)
                && mappings != null
                && mappings.Any(y => y.SettingId == x.SettingId)).ToList();

            return subProducts != null && subProducts.Any()
                ? subProducts.GroupBy(x => x.SettingId).Select(x => x.First()).ToList()
                : new List<SettingItem>
                {
                    new SettingItem { SettingId = Constants.SubProductNA, ParentIds = new List<int> { Constants.ProductNA } }
                };
        }

        private static IList<SettingItem> GetBettype(
            IEnumerable<int> subProducts,
            IEnumerable<TransactionInfo> transactionInfos,
            IEnumerable<SettingItem> settings,
            IEnumerable<SystemSettingMapping> settingMappings)
        {
            IList<SystemSettingMapping> mappings = settingMappings.Where(x => x.SettingName.Equals(SettingName.Bettype, StringComparison.OrdinalIgnoreCase)
                && !string.IsNullOrWhiteSpace(x.SettingSource)
                && transactionInfos.Any(y => y.Bettype.Equals(x.SettingSource, StringComparison.OrdinalIgnoreCase))).ToList();
            IList<SettingItem> bettypes = settings.Where(x => x.SettingName.Equals(SettingName.Bettype, StringComparison.OrdinalIgnoreCase)
                && subProducts.Any(y => x.ParentIds.Any(z => z == y))
                && mappings != null
                && mappings.Any(y => y.SettingId == x.SettingId)).ToList();

            return bettypes != null && bettypes.Any()
                ? bettypes.GroupBy(x => x.SettingId).Select(x => x.First()).ToList()
                : new List<SettingItem>
                {
                    new SettingItem { SettingId = Constants.BettypeNA, ParentIds = new List<int> { Constants.SubProductNA } }
                };
        }

        private static Tuple<int, int, string> GetCategorySubCategory(IEnumerable<SettingItem> settings)
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

        private static string GetRelatedCustomerXml(int headlineId, IEnumerable<TransactionInfo> transactionInfos)
        {
            const string XmlTemplate = "<Row CustId='{0}' RelatedCustId='{1}' Username='{2}' />";
            var stringBuilder = new StringBuilder("<ROOT>");
            var transactions = transactionInfos.GroupBy(x => x.CustId).Select(x => x.First());
            foreach (var transaction in transactions)
            {
                stringBuilder.AppendFormat(XmlTemplate, headlineId, transaction.CustId, transaction.Username);
            }

            return stringBuilder.Append("</ROOT>").ToString();
        }

        private static string GetAutoField(
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
            stringBuilder.Append("<br /><br />Affected users and tickets:");
            foreach (var transaction in transactionInfos)
            {
                stringBuilder.AppendFormat("<br />{0} {1}", transaction.Username, transaction.TransId);
            }

            return stringBuilder.ToString();
        }

        private async Task UpdateSystemSettingAsync(int lastMsgId)
        {
            mlogger.LogInformation("Begin update JobStatus, LastMsgId, LastRuntime.");
            var settingJobStatus = new List<SystemSettingItem>
            {
                new SystemSettingItem { SettingName = SystemSettingName.JobStatus, Value = JobStatus.Completed },
                new SystemSettingItem { SettingName = SystemSettingName.LastMsgId, Value = lastMsgId.ToString() },
                new SystemSettingItem { SettingName = SystemSettingName.LastRuntime, Value = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff tt") }
            };
            await systemSettingService.UpdateSystemSettingAsync(settingJobStatus);
            mlogger.LogInformation("End update JobStatus, LastMsgId, LastRuntime.");
        }

        private async Task<ValueTuple<IEnumerable<MatchInfo>, IEnumerable<TransactionInfo>>> StoreInfoAndGetCustomerNeedToCreateFeedbackAsync(
            IEnumerable<MatchInfo> matchInfos,
            IEnumerable<TransactionInfo> transactionInfos,
            IEnumerable<SiteSettingMapping> siteSettingMappings,
            IEnumerable<GroupHeadline> headlines,
            CustomerInfo licenseeInfo)
        {
            IEnumerable<TransactionInfo> transactions = transactionInfos.GroupBy(x => new { x.MessageId, x.CustId }).Select(x => x.First());

            const string XmlTemplate = "<r msgid='{0}' custid='{1}' hl='{2}' related='{3}' event='{4}' />";
            var stringBuilder = new StringBuilder("<ROOT>");
            foreach (var transaction in transactions)
            {
                var matchInfo = matchInfos.First(x => x.MessageId == transaction.MessageId);
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
                    transaction.CustId,
                    headlineId,
                    relatedCustId,
                    string.Format("{0} -vs- {1}", HttpUtility.HtmlEncode(matchInfo.HomeName), HttpUtility.HtmlEncode(matchInfo.AwayName)));
            }

            stringBuilder.Append("</ROOT>");

            var criteria = new StoreFeedbackInfoCriteria
            {
                Xml = stringBuilder.ToString()
            };
            var feedbackInfos = await baseService.GetDataAsync<AutoFeedbackInfo>(criteria);

            matchInfos = matchInfos.Where(x => feedbackInfos.Any(y => y.MessageId == x.MessageId));
            transactionInfos = transactionInfos.Where(x => feedbackInfos.Any(y => y.MessageId == x.MessageId && y.CustId == x.CustId));
            return new ValueTuple<IEnumerable<MatchInfo>, IEnumerable<TransactionInfo>>(matchInfos, transactionInfos);
        }

        private async Task UpdateFeedbackInfoAsync(int messageId, IEnumerable<int> custIds, string status, string description)
        {
            var criteria = new UpdateFeedbackInfoCriteria
            {
                MessageId = messageId,
                CustIds = string.Join(",", custIds),
                Status = status,
                Description = description
            };
            await baseService.ExecuteNonQueryAsync(criteria);
        }
    }
}