namespace Fanex.CRM.Feedback.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Services;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Models;
    using Fanex.CRM.Feedback.Repositories;
    using Fanex.CRM.Feedback.Resources;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Services;
    using Fanex.Data.Repository;

    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly ISettingService _serviceSetting;
        private readonly IBaseService baseService;

        public FeedbackService() : this(null, null, null)
        {
        }

        public FeedbackService(IFeedbackRepository feedbackRepository,
            IBaseService baseService,
            ISettingService serviceSetting)
        {
            _feedbackRepository = feedbackRepository ?? new FeedbackRepository();
            _serviceSetting = serviceSetting ?? new SettingService();
            this.baseService = baseService ?? new BaseService();
        }

        public IEnumerable<BaseModel> GetTopItems
        {
            get
            {
                return new List<BaseModel>
                {
                    new BaseModel { Id = 10, Name = "10" },
                    new BaseModel { Id = 20, Name = "20" },
                    new BaseModel { Id = 30, Name = "30" },
                    new BaseModel { Id = 40, Name = "40" },
                    new BaseModel { Id = 50, Name = "50" }
                };
            }
        }

        public dynamic GetAllSettings()
        {
            return new
            {
                CustomerType = GetCustomerTypes(),
                Review = GetReviews(),
                CustomerResponse = GetCustomerResponse(),
                MainCategory = GetMainCategories(),
                Communication = GetCommunicationTools(),
                Status = GetStatus()
            };
        }

        public IEnumerable<FeedbackDetail> GetManageFeedbacks(ManageFeedbackCriteria criteria)
        {
            return _feedbackRepository.GetManagerFeedbacks(criteria);
        }

        public IEnumerable<DeletedFeedback> GetLatestFeedbackReport(LatestFeedbackCriteria criteria)
        {
            return GetFeedbacks(criteria);
        }

        public IEnumerable<DeletedFeedback> GetDeletedFeedbackReport(DeletedFeedbackCriteria criteria)
        {
            criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
            criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);
            var feedbacks = GetFeedbacks(criteria);

            if (criteria.IsExportExcel)
            {
                return feedbacks;
            }

            // pagination.
            int totalItems = feedbacks.Count();
            if (totalItems > 0)
            {
                feedbacks = feedbacks.Skip(criteria.PageSize * (criteria.PageIndex - 1)).Take(criteria.PageSize).ToList();
            }

            criteria.TotalItems = totalItems;
            return feedbacks;
        }

        public CommandResult UpdateSiteLicenseeTypes(UpdateSiteLicenseeTypesCriteria criteria)
        {
            CommandResult result = baseService.ExecuteNonQuery(criteria);
            if (result.IsSuccessful)
            {
                result.Message = Labels.UpdateSiteLicenseeTypeSuccess;
            }

            return result;
        }

        public FeedbackDuplicationResult CheckFeedbackDuplication(CheckFeedbackDuplicationCriteria criteria)
        {
            var feedbackId = _feedbackRepository.CheckFeedbackDuplication(criteria);
            return new FeedbackDuplicationResult { FeedbackId = feedbackId };
        }

        private IEnumerable<DeletedFeedback> GetFeedbacks(ICriteria criteria)
        {
            var statusList = _serviceSetting.GetAllStatuses(ViewTypeConstants.All);
            var statusHash = statusList.ToDictionary(k => k.Id, v => v.Name);
            statusHash.Add(1, Labels.Symbol_NA);

            var feedbacks = _feedbackRepository.GetDeletedFeedbacks(criteria);
            foreach (var feedback in feedbacks)
            {
                feedback.Status = statusHash.ContainsKey(feedback.StatusId) ? statusHash[feedback.StatusId] : string.Empty;
                var autoFieldText = string.IsNullOrEmpty(feedback.AutoField) ?
                        string.Empty :
                        (new System.Text.RegularExpressions.Regex("\n")).Replace(feedback.AutoField, "<br>") + "<br><br>";
                feedback.DescriptionTooltip = autoFieldText + feedback.Description;
            }

            return feedbacks;
        }

        #region Setting helper methods

        private IEnumerable<Setting> GetCustomerTypes()
        {
            return new List<Setting>
            {
                new Setting { Id = -1, Name = Labels.All },
                new Setting { Id = 0, Name = Labels.Customer },
                new Setting { Id = 1, Name = Labels.Guest }
            };
        }

        private IEnumerable<Setting> GetReviews()
        {
            return new List<Setting>
            {
                new Setting { Id = 0, Name = Labels.All },
                new Setting { Id = -1, Name = Labels.NotReviewed },
                new Setting { Id = 1, Name = Labels.Reviewed }
            };
        }

        private IEnumerable<SettingRelation> GetCustomerResponse()
        {
            var response = _serviceSetting.GetAllResponses();
            response.Insert(
                0,
                new SettingRelation
                {
                    Id = SettingConstants.NotApply,
                    Value = Labels.All
                });

            return response;
        }

        private IEnumerable<Setting> GetMainCategories()
        {
            var allSubSettingOption = new Setting
            {
                Id = SettingConstants.NotApply,
                Name = Labels.All,
            };

            var mainCategories = new List<Setting>
            {
                new Setting
                {
                    Id = SettingConstants.NotApply,
                    Name = Labels.All,
                    ChildSettings = new List<Setting>
                    {
                        allSubSettingOption
                    }
                }
            };

            var rawData = _serviceSetting.GetAllMainCategories();
            foreach (var rawMainCategory in rawData)
            {
                rawMainCategory.ChildSettings.Insert(0, allSubSettingOption);
                mainCategories.Add(rawMainCategory);
            }

            return mainCategories;
        }

        private IEnumerable<Setting> GetCommunicationTools()
        {
            var tools = _serviceSetting.GetAllCommunicationTools()
                .Where(r => r.Id != NAConstants.GetCommunicationNA(AppSession.CurrentUser.UserRole))
                .ToList();
            tools.Insert(0, new Setting { Id = SettingConstants.NotApply, Name = Labels.All });

            return tools;
        }

        private IEnumerable<BaseModel> GetStatus()
        {
            var status = _serviceSetting.GetAllStatuses(ViewTypeConstants.CommunicationTool)
                .Where(r => !string.Equals(r.Name, "Deleted", StringComparison.OrdinalIgnoreCase))
                .ToList();

            status.Insert(0, new BaseModel { Id = SettingConstants.NotApply, Name = Labels.All });
            return status;
        }

        #endregion Setting helper methods
    }
}