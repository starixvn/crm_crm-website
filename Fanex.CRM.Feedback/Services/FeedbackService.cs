namespace Fanex.CRM.Feedback.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

        private const int NUMBER_10 = 10;
        private const int NUMBER_20 = 20;
        private const int NUMBER_30 = 30;
        private const int NUMBER_40 = 40;
        private const int NUMBER_50 = 50;

        public FeedbackService() : this(null, null)
        {
        }

        public FeedbackService(IFeedbackRepository feedbackRepository,
            ISettingService serviceSetting)
        {
            _feedbackRepository = feedbackRepository ?? new FeedbackRepository();
            _serviceSetting = serviceSetting ?? new SettingService();
        }

        public IEnumerable<BaseModel> GetTopItems
        {
            get
            {
                return new List<BaseModel>
                {
                    new BaseModel { Id = NUMBER_10, Name = "10" },
                    new BaseModel { Id = NUMBER_20, Name = "20" },
                    new BaseModel { Id = NUMBER_30, Name = "30" },
                    new BaseModel { Id = NUMBER_40, Name = "40" },
                    new BaseModel { Id = NUMBER_50, Name = "50" }
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

        public IEnumerable<DeletedFeedback> GetManageFeedbacks(ManageFeedbackCriteria criteria)
        {
            var feedbacks = _feedbackRepository.GetManagerFeedbacks(criteria);
            foreach (var feedback in feedbacks)
            {
                var description = string.IsNullOrEmpty(feedback.Description) ?
                       string.Empty :
                       (new System.Text.RegularExpressions.Regex("\n")).Replace(feedback.Description, "<br>") + "<br>";
                var descriptionText = "Description: <br>" + description;

                var solution = string.IsNullOrEmpty(feedback.Solution) ?
                        string.Empty :
                        (new System.Text.RegularExpressions.Regex("\n")).Replace(feedback.Solution, "<br>") + "<br>";
                var solutionText = "Solution: <br>" + solution;

                feedback.DescriptionTooltip = descriptionText + solutionText;
            }

            return feedbacks;
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
                var description = string.IsNullOrEmpty(feedback.Description) ?
                       string.Empty :
                       (new System.Text.RegularExpressions.Regex("\n")).Replace(feedback.Description, "<br>") + "<br>";
                var descriptionText = "Description: <br>" + description;

                var solution = string.IsNullOrEmpty(feedback.Solution) ?
                        string.Empty :
                        (new System.Text.RegularExpressions.Regex("\n")).Replace(feedback.Solution, "<br>") + "<br>";
                var solutionText = "Solution: <br>" + solution;

                feedback.DescriptionTooltip = descriptionText + solutionText;
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
            var tools = _serviceSetting.GetAllCommunicationTools(true);
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