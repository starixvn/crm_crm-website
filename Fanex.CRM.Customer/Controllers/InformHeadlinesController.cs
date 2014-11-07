// --------------------------------------------------------------------------
// <copyright file="InformHeadlinesController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Customer.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Customer.Criteria.InformHeadlines;
    using Fanex.CRM.Customer.Resources;
    using Fanex.CRM.Customer.Services;
    using Fanex.CRM.Infrastructure.Constants;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Inform Headlines Controller.
    /// </summary>
    public class InformHeadlinesController : BaseController
    {
        private readonly IInformHeadlineService _informHeadlineService;

        public InformHeadlinesController()
            : this(null)
        {
        }

        public InformHeadlinesController(IInformHeadlineService informHeadlineService)
        {
            _informHeadlineService = informHeadlineService ?? new InformHeadlineService();
        }

        [NapAuth(FunctionKey = FunctionPermissions.InformHeadline, Policy = Constant.Policy.Read)]
        [HttpGet]
        public ActionResult AllInformHeadlineSettings()
        {
            var settings = _informHeadlineService.AllInformHeadlineSettings;
            return Json(settings, JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = FunctionPermissions.InformHeadline, Policy = Constant.Policy.Read)]
        [HttpGet]
        public ActionResult ViewInforms()
        {
            ViewBag.CanCreateTitle = HasHostPermission(FunctionPermissions.InformHeadlineCreateTitle);
            ViewBag.CanUpdateStatus = HasReadPermission(FunctionPermissions.InformHeadlineUpdateStatus);
            return View("~/Areas/Customer/InformHeadlines/InformStatistic/Index.cshtml");
        }

        [NapAuth(FunctionKey = FunctionPermissions.InformHeadline, Policy = Constant.Policy.Read)]
        [HttpGet]
        public ActionResult GetInformStatistic(GetInformHeadlineStatisticCriteria criteria)
        {
            var statistic = _informHeadlineService.GetInformStatistic(criteria);
            return Json(statistic, JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = FunctionPermissions.InformHeadline, Policy = Constant.Policy.Read)]
        [HttpGet]
        public ActionResult ExportInformStatistic(GetInformHeadlineStatisticCriteria criteria)
        {
            _informHeadlineService.ExportInformStatistic(criteria);
            return new EmptyResult();
        }

        [NapAuth(FunctionKey = FunctionPermissions.InformHeadlineCreateTitle, Policy = Constant.Policy.Host)]
        [HttpGet]
        public ActionResult OpenCreateTitle()
        {
            return View("~/Areas/Customer/InformHeadlines/CreateTitle/Index.cshtml");
        }

        [NapAuth(FunctionKey = FunctionPermissions.InformHeadlineCreateTitle, Policy = Constant.Policy.Host)]
        [HttpPost]
        [SuppressMessage("Microsoft.Design",
            "CA1011:ConsiderPassingBaseTypesAsParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        public ActionResult CreateTitle(CreateInformCriteria criteria)
        {
            Result result = !criteria.IsValid()
                ? new Result { Code = -1, Message = Labels.InformHeadline_TitleEmpty }
                : _informHeadlineService.CreateTitle(criteria);
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [NapAuth(FunctionKey = FunctionPermissions.InformHeadlineUpdateStatus, Policy = Constant.Policy.Read)]
        [HttpGet]
        public ActionResult OpenUpdateInform(int titleId)
        {
            ViewBag.TitleId = titleId;
            ViewBag.CanUpdateTitle = HasHostPermission(FunctionPermissions.InformHeadlineCreateTitle);
            ViewBag.CanUpdateStatus = HasHostPermission(FunctionPermissions.InformHeadlineUpdateStatus);
            return View("~/Areas/Customer/InformHeadlines/UpdateInform/Index.cshtml");
        }

        [NapAuth(FunctionKey = FunctionPermissions.InformHeadlineUpdateStatus, Policy = Constant.Policy.Read)]
        [HttpGet]
        public ActionResult GetInforms(GetInformHeadlineCriteria criteria)
        {
            var informs = _informHeadlineService.GetInforms(criteria);
            int totalItem = criteria.TotalItem;
            string title = informs.Count > 0 ? informs[0].Title : string.Empty;

            var result = new { TotalItem = totalItem, ChildItems = informs, Title = title };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = FunctionPermissions.InformHeadline, Policy = Constant.Policy.Read)]
        [HttpGet]
        public ActionResult ExportInforms(GetInformHeadlineCriteria criteria)
        {
            _informHeadlineService.ExportInforms(criteria);
            return new EmptyResult();
        }

        /// <summary>
        /// Updates the informs.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// The JSON result.
        /// </returns>
        [HttpPost]
        public ActionResult UpdateInforms(UpdateInformHeadlinesCriteria criteria)
        {
            JsonRequestBehavior requestBehavior = JsonRequestBehavior.DenyGet;
            if (criteria.IsTitleChanged && !HasHostPermission(FunctionPermissions.InformHeadlineCreateTitle))
            {
                return Json(new Result { Code = ErrorCodeConstants.NoPermission, Message = Labels.NoPermission }, requestBehavior);
            }

            if (!HasHostPermission(FunctionPermissions.InformHeadlineUpdateStatus))
            {
                return Json(new Result { Code = ErrorCodeConstants.NoPermission, Message = Labels.NoPermission }, requestBehavior);
            }

            var result = _informHeadlineService.UpdateInforms(criteria);
            return Json(result, requestBehavior);
        }
    }
}
