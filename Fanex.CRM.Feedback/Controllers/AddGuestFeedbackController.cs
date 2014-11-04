// --------------------------------------------------------------------------
// <copyright file="AddGuestFeedbackController.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.CRM.Feedback.Resources;
    using Fanex.CRM.Feedback.Services.AddCustomerFeedback;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Add Guest Feedback Controller.
    /// </summary>
    public class AddGuestFeedbackController : AddFeedbackController
    {
        private IAddGuestFeedbackService _feedbackService;

        public AddGuestFeedbackController()
            : this(null)
        {
        }

        public AddGuestFeedbackController(IAddGuestFeedbackService feedbackService)
            IAddGuestFeedbackService feedbackService, ISettingService serviceSetting)
        {
            _feedbackService = feedbackService ?? new AddGuestFeedbackService();
            _serviceSetting = serviceSetting ?? new SettingService();
        }

        [NapAuth(FunctionKey = FunctionPermission.CreateFeedback, Policy = Constant.Policy.Host)]
        public ActionResult Index()
        {
            return View("~/Areas/Feedbacks/AddGuestFeedback/Index.cshtml");
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
        public JsonResult AddGuestFeedback(AddGuestFeedbackCriteria criteria)
        {
            Result insertResult = CheckAddCustomerFeedbackPermission(criteria);
            if (insertResult.Code == ErrorCodeConstants.NoneError)
            {
                insertResult = _feedbackService.AddGuestFeedback(criteria);
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
        private Result CheckAddCustomerFeedbackPermission(AddGuestFeedbackCriteria criteria)
        {
            if (!HasPermission(FunctionPermission.CreateFeedback, Constant.Policy.Host))
            {
                return new Result(ErrorCodeConstants.NoPermission, Labels.Feedback_AddCustomerFeedback_NoCreateFeedbackPermission);
            }

            if (criteria.Status.Id == (int)Enumerations.Functions.CRMStatus.Deleted &&
                !HasPermission(FunctionPermission.DeleteFeedback.ToString(), Constant.Policy.Host))
            {
                return new Result(ErrorCodeConstants.NoPermission, Labels.Feedback_AddCustomerFeedback_NoDeletedFeedbackPermission);
            }

            if (!HasPermission(FunctionPermission.CustomerInfo, Constant.Policy.Host))
            {
                return new Result(ErrorCodeConstants.NoPermission, Labels.Feedback_AddCustomerFeedback_NoUpdateCustomerPermission);
            }

            return new Result();
        }
    }
}
