// --------------------------------------------------------------------------
// <copyright file="FeedbackService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Feedback.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Models;
    using Fanex.CRM.Feedback.Repositories;
    using Fanex.CRM.Feedback.Resources;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Services;
    using Fanex.Data.Repository;

    /// <summary>
    /// Feedback Service.
    /// </summary>
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly ISettingService _serviceSetting;

        public FeedbackService()
            : this(null, null)
        {
        }

        public FeedbackService(IFeedbackRepository feedbackRepository, ISettingService serviceSetting)
        {
            _feedbackRepository = feedbackRepository ?? new FeedbackRepository();
            _serviceSetting = serviceSetting ?? new SettingService();
        }

        public IEnumerable<BaseModel> GetTopItems
        {
            get
            {
                return new List<BaseModel>()
                {
                    new BaseModel { Id = 10, Name = "10" },
                    new BaseModel { Id = 20, Name = "20" },
                    new BaseModel { Id = 30, Name = "30" },
                    new BaseModel { Id = 40, Name = "40" },
                    new BaseModel { Id = 50, Name = "50" }
                };
            }
        }

        /// <summary>
        /// Gets all setting.
        /// </summary>
        /// <returns>The all setting object.</returns>
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

        /// <summary>
        /// Gets the deleted feedback report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The list of deleted feedback.</returns>
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

        private IEnumerable<DeletedFeedback> GetFeedbacks(ICriteria criteria)
        {
            var categoryHash = new Dictionary<int, string>();
            var subcategoryHash = new Dictionary<int, string>();
            var categoies = _serviceSetting.GetAllMainCategoriesFromAllSettings();
            foreach (var category in categoies)
            {
                if (!categoryHash.ContainsKey(category.Id))
                {
                    categoryHash.Add(category.Id, category.Value);
                }

                foreach (var subcategory in category.ChildSettings)
                {
                    if (!subcategoryHash.ContainsKey(subcategory.Id))
                    {
                        subcategoryHash.Add(subcategory.Id, subcategory.Name);
                    }
                }
            }

            var statusList = _serviceSetting.GetAllStatuses(ViewTypeConstants.All);
            var statusHash = statusList.ToDictionary(k => k.Id, v => v.Name);
            statusHash.Add(1, Labels.Symbol_NA);

            var feedbacks = _feedbackRepository.GetDeletedFeedbacks(criteria);
            foreach (var feedback in feedbacks)
            {
                feedback.MainCategory = categoryHash[feedback.MainCategoryId];
                feedback.SubCategory = subcategoryHash.ContainsKey(feedback.SubCategoryId) ? subcategoryHash[feedback.SubCategoryId] : Labels.Symbol_NA;
                feedback.Status = statusHash.ContainsKey(feedback.StatusId) ? statusHash[feedback.StatusId] : string.Empty;
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
            var response = _serviceSetting.GetAllResponses().ToList();
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

            var mainCategories = new List<Setting>() 
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
                .Where(r => r.Id != CommunicationToolConstants.NotApply)
                .ToList();
            tools.Insert(0, new Setting { Id = SettingConstants.NotApply, Name = Labels.All });

            return tools;
        }

        private IEnumerable<Setting> GetStatus()
        {
            var status = _serviceSetting.GetAllStatuses(ViewTypeConstants.CommunicationTool)
                .Where(r => !string.Equals(r.Name, "Deleted", StringComparison.OrdinalIgnoreCase))
                .ToList();

            status.Insert(0, new Setting { Id = SettingConstants.NotApply, Name = Labels.All });
            return status;
        }

        #endregion
    }
}
