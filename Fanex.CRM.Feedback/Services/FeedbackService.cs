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
    using Fanex.CRM.Infrastructure.Helpers;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Services.Implements;
    using Fanex.CRM.Setting.Services.Interfaces;
    using Fanex.Data.Repository;

    /// <summary>
    /// Feedback Service.
    /// </summary>
    public class FeedbackService : IFeedbackService
    {
        // Session keys.
        private const string DeletedFeedbackSessionKey = "Fanex.CRM.Feedback.Services.DeletedFeedback";

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

        public IList<BaseModel> GetTop
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
        public dynamic GetAllSetting()
        {
            return new
            {
                CustomerType = GetCustomerType(),
                Review = GetReview(),
                CustomerResponse = GetCustomerResponse(),
                MainCategory = GetMainCategory(),
                Communication = GetCommunication(),
                Status = GetStatus()
            };
        }

        /// <summary>
        /// Gets the manager feedback.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The list of ManagerFeedback object.</returns>
        public IList<FeedbackDetail> GetManageFeedback(ManageFeedbackCriteria criteria)
        {
            IList<FeedbackDetail> feebacks = null;
            try
            {
                feebacks = _feedbackRepository.GetManagerFeedback(criteria);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return feebacks;
        }

        public IList<DeletedFeedback> GetLatestFeedbackReport(LatestFeedbackCriteria criteria)
        {
            return GetDeletedFeedback(criteria);
        }

        /// <summary>
        /// Gets the deleted feedback report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The list of deleted feedback.</returns>
        public IList<DeletedFeedback> GetDeletedFeedbackReport(DeletedFeedbackCriteria criteria)
        {
            string key = DeletedFeedbackSessionKey;
            
            IList<DeletedFeedback> feedbacks = null;
            if (criteria.FromCache)
            {
                feedbacks = (IList<DeletedFeedback>)SessionExtension.GetSession(HttpContext.Current.Session, key);
            }

            if (feedbacks == null)
            {
                // Get data from DB.
                criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
                criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);
                try
                {
                    feedbacks = GetDeletedFeedback(criteria);
                    SessionExtension.AddSession(HttpContext.Current.Session, key, feedbacks);
                }
                catch (Exception ex)
                {
                    LogExtensions.ExceptionHandler(ex);
                    throw;
                }
            }

            // pagination.
            int totalItems = feedbacks.Count;
            if (totalItems > 0)
            {
                feedbacks = feedbacks.Skip(criteria.PageSize * (criteria.PageIndex - 1)).Take(criteria.PageSize).ToList();
            }

            criteria.TotalItems = totalItems;
            return feedbacks;
        }

        /// <summary>
        /// Gets the deleted feedback.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The list of deleted feedback.</returns>
        private IList<DeletedFeedback> GetDeletedFeedback(ICriteria criteria)
        {
            var categoryHash = new Dictionary<int, string>();
            var subcategoryHash = new Dictionary<int, string>();
            var categoies = _serviceSetting.GetAllMainCategory();
            foreach (var category in categoies)
            {
                if (!categoryHash.ContainsKey(category.Id))
                {
                    categoryHash.Add(category.Id, category.Name);
                }

                foreach (var subcategory in category.ChildSettings)
                {
                    if (!subcategoryHash.ContainsKey(subcategory.Id))
                    {
                        subcategoryHash.Add(subcategory.Id, subcategory.Name);
                    }
                }
            }

            var statusList = _serviceSetting.GetAllStatus(new GetAllStatusCriteria { ViewType = ViewTypeConstants.All });
            var statusHash = statusList.ToDictionary(k => k.Id, v => v.Name);
            statusHash.Add(1, Labels.Symbol_NA);

            IList<DeletedFeedback> feedbacks = new List<DeletedFeedback>();
            var rawdata = _feedbackRepository.GetDeletedFeedback(criteria);
            foreach (var feedback in rawdata)
            {
                feedback.MainCategory = categoryHash[feedback.MainCategoryId];
                feedback.SubCategory = subcategoryHash.ContainsKey(feedback.SubCategoryId) ? subcategoryHash[feedback.SubCategoryId] : Labels.Symbol_NA;
                feedback.Status = statusHash.ContainsKey(feedback.StatusId) ? statusHash[feedback.StatusId] : string.Empty;
                feedbacks.Add(feedback);
            }

            return feedbacks;
        }

        #region Setting helper methods

        private IList<SettingInfo> GetCustomerType()
        {
            return new List<SettingInfo>
            {
                new SettingInfo { Id = -1, Name = Labels.All },
                new SettingInfo { Id = 0, Name = Labels.Customer },
                new SettingInfo { Id = 1, Name = Labels.Guest }
            };
        }

        private IList<SettingInfo> GetReview()
        {
            return new List<SettingInfo>
            {
                new SettingInfo { Id = 0, Name = Labels.All },
                new SettingInfo { Id = -1, Name = Labels.NotReviewed },
                new SettingInfo { Id = 1, Name = Labels.Reviewed }
            };
        }

        /// <summary>
        /// Gets the customer response.
        /// </summary>
        /// <returns>The list of SettingInfo object.</returns>
        private IList<SettingInfo> GetCustomerResponse()
        {
            var response = _serviceSetting.GetAllResponse();
            response.Insert(
                0,
                new SettingInfo 
                {
                    Id = SettingConstants.NotApply,
                    Value = Labels.All
                });

            return response;
        }

        /// <summary>
        /// Gets the main category.
        /// </summary>
        /// <returns>The list of SettingInfo object.</returns>
        private IList<SettingInfo> GetMainCategory()
        {
            var allSubSettingOption = new SettingInfo
            {
                Id = SettingConstants.NotApply,
                Name = Labels.All,
            };

            var mainCategory = new List<SettingInfo>() 
            {
                new SettingInfo
                {
                    Id = SettingConstants.NotApply,
                    Name = Labels.All,
                    ChildSettings = new List<SettingInfo>
                    {
                        allSubSettingOption
                    }
                }
            };

            var rawData = _serviceSetting.GetAllMainCategory();
            foreach (var rawMainCategory in rawData)
            {
                rawMainCategory.ChildSettings.Insert(0, allSubSettingOption);
                mainCategory.Add(rawMainCategory);
            }

            return mainCategory;
        }

        /// <summary>
        /// Gets the communication.
        /// </summary>
        /// <returns>The list of SettingInfo object.</returns>
        private IList<SettingInfo> GetCommunication()
        {
            var communication = _serviceSetting.GetAllCommunicationTool()
                .Where(r => r.Id != CommunicationToolConstants.NotApply)
                .ToList();
            communication.Insert(0, new SettingInfo { Id = SettingConstants.NotApply, Name = Labels.All });

            return communication;
        }

        /// <summary>
        /// Gets all status.
        /// </summary>
        /// <returns>The list of SettingInfo object.</returns>
        private IList<SettingInfo> GetStatus()
        {
            var status = _serviceSetting.GetAllStatus(new GetAllStatusCriteria
            {
                ViewType = ViewTypeConstants.CommunicationTool
            })
                .Where(r =>
                    string.Equals(r.Name, "Checked", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(r.Name, "Solved", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(r.Name, "Closed", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(r.Name, "Pending", StringComparison.OrdinalIgnoreCase))
                .ToList();

            status.Insert(0, new SettingInfo { Id = SettingConstants.NotApply, Name = Labels.All });
            return status;
        }

        #endregion
    }
}
