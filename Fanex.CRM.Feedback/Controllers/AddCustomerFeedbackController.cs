// --------------------------------------------------------------------------
// <copyright file="AddCustomerFeedbackController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Feedback.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Models;
    using Fanex.CRM.Feedback.Resources;
    using Fanex.CRM.Feedback.Services.AddCustomerFeedback;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Infrastructure.Web.Controllers;
    using Fanex.CRM.Setting.Models;
    using Fanex.CRM.Setting.Services.Implements;
    using Fanex.CRM.Setting.Services.Interfaces;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Feedbacks Controller.
    /// </summary>
    public class AddCustomerFeedbackController : BaseController
    {
        private IAddCustomerFeedbackService _feedbackService;

        private ISettingService _serviceSetting;

        public AddCustomerFeedbackController()
            : this(null, null)
        {
        }

        public AddCustomerFeedbackController(
            IAddCustomerFeedbackService feedbackService,
            ISettingService serviceSetting)
        {
            _feedbackService = feedbackService ?? new AddCustomerFeedbackService();
            _serviceSetting = serviceSetting ?? new SettingService();
        }

        [NapAuth(FunctionKey = FunctionPermission.CreateFeedback, Policy = Constant.Policy.Host)]
        public ActionResult Index(int custid, string custName)
        {
            var customerInfo = new CustomerInfo
            {
                Id = custid,
                Name = custName
            };
            return View("~/Areas/Feedbacks/AddCustomerFeedback/Index.cshtml", customerInfo);
        }

        /// <summary>
        /// Gets all setting for edit.
        /// </summary>
        /// <returns>All setting for edit.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermission.EditFeedback, Policy = Constant.Policy.Read)]
        public ActionResult GetAllSetting()
        {
            var languageList = _serviceSetting.GetAllLanguage();
            languageList.Insert(
                0,
                new SettingInfo
                {
                    Id = 0,
                    Name = Labels.Symbol_NA
                });

            var countryList = _serviceSetting.GetAllCountry();
            countryList.Insert(
                0,
                new CountryInfo
                {
                    Id = 0,
                    Name = Labels.Symbol_NA
                });

            var allSetting = new
            {
                ExtensionList = _serviceSetting.GetAllExtension(),
                CountryList = countryList,
                CommunicationToolList = _serviceSetting.GetAllCommunicationTool(),
                LanguageList = languageList,
                MainCategoryList = _serviceSetting.GetAllMainCategory(),
                ProductList = _serviceSetting.GetAllProduct(),
                SiteList = _serviceSetting.GetAllSite(),
                RoleList = _serviceSetting.GetAllRole(),
                ResponseList = _serviceSetting.GetAllResponse(),
                StatusList = _feedbackService.GetAllStatus(),
                TitleList = _serviceSetting.GetAllTitle()
            };

            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Adds the feedback.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// Add result.
        /// </returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermission.CreateFeedback, Policy = Constant.Policy.Host)]
        public JsonResult AddCustomerFeedback(AddCustomerFeedbackCriteria criteria)
        {
            Result insertResult = CheckAddCustomerFeedbackPermission(criteria);
            if (insertResult.Code == ErrorCodeConstants.NoneError)
            {
                insertResult = _feedbackService.AddCustomerFeedback(criteria);
            }

            var jsonResult = Json(insertResult, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        /// <summary>
        /// Checks the add customer feedback permission.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// Check result.
        /// </returns>
        private Result CheckAddCustomerFeedbackPermission(AddCustomerFeedbackCriteria criteria)
        {
            if (!HasPermission(FunctionPermission.CreateFeedback.ToString(), Constant.Policy.Host))
            {
                return new Result(ErrorCodeConstants.NoPermission, Labels.Feedback_AddCustomerFeedback_NoCreateFeedbackPermission);
            }

            if (criteria.IsOverrideCustomerInfo && !HasPermission(FunctionPermission.CustomerInfo, Constant.Policy.Host))
            {
                return new Result(ErrorCodeConstants.NoPermission, Labels.Feedback_AddCustomerFeedback_NoOverrideCustomerInfoPermission);
            }

            if (criteria.Status.Id == (int)Enumerations.Functions.CRMStatus.Deleted &&
                !HasPermission(FunctionPermission.DeleteFeedback.ToString(), Constant.Policy.Host))
            {
                return new Result(ErrorCodeConstants.NoPermission, Labels.Feedback_AddCustomerFeedback_NoDeletedFeedbackPermission);
            }

            return new Result();
        }
    }
}
