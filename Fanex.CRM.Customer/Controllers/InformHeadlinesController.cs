// --------------------------------------------------------------------------
// <copyright file="InformHeadlinesController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Dashboard.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Customer.Criteria;
    using Fanex.CRM.Customer.Resources;
    using Fanex.CRM.Customer.Services.Implements;
    using Fanex.CRM.Customer.Services.Interfaces;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Infrastructure.Web.Controllers;
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

        [NapAuth(FunctionKey = FunctionPermission.InformHeadline, Policy = Constant.Policy.Read)]
        [HttpGet]
        public ActionResult ViewInforms()
        {
            ViewBag.CanCreateTitle = HasHostPermission(FunctionPermission.InformHeadline_CreateTitle);
            ViewBag.CanUpdateStatus = HasReadPermission(FunctionPermission.InformHeadline_UpdateStatus);
            return View("~/Areas/Customer/InformHeadlines/InformStatistic.cshtml");
        }

        [NapAuth(FunctionKey = FunctionPermission.InformHeadline, Policy = Constant.Policy.Read)]
        [HttpGet]
        public ActionResult GetInformStatistic(GetInformHeadlineStatisticCriteria criteria)
        {
            var statistic = _informHeadlineService.GetInformStatistic(criteria);
            return Json(statistic, JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = FunctionPermission.InformHeadline, Policy = Constant.Policy.Read)]
        [HttpGet]
        public ActionResult ExportInformStatistic(GetInformHeadlineStatisticCriteria criteria)
        {
            _informHeadlineService.ExportInformStatistic(criteria);
            return new EmptyResult();
        }

        [NapAuth(FunctionKey = FunctionPermission.InformHeadline_CreateTitle, Policy = Constant.Policy.Host)]
        [HttpGet]
        public ActionResult OpenCreateTitle()
        {
            return View("~/Areas/Customer/InformHeadlines/CreateTitle.cshtml");
        }

        [NapAuth(FunctionKey = FunctionPermission.InformHeadline_CreateTitle, Policy = Constant.Policy.Host)]
        [HttpPost]
        public ActionResult CreateTitle(CreateInformCriteria criteria)
        {
            Result result = !criteria.IsValid()
                ? new Result { Code = -1, Message = Labels.InformHeadline_TitleEmpty }
                : _informHeadlineService.CreateTitle(criteria);
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [NapAuth(FunctionKey = FunctionPermission.InformHeadline_UpdateStatus, Policy = Constant.Policy.Read)]
        [HttpGet]
        public ActionResult OpenUpdateInform(int titleId, string title)
        {
            ViewBag.CanUpdateTitle = HasHostPermission(FunctionPermission.InformHeadline_CreateTitle);
            ViewBag.CanUpdateStatus = HasHostPermission(FunctionPermission.InformHeadline_UpdateStatus);
            return View("~/Areas/Customer/InformHeadlines/UpdateInform.cshtml");
        }

        [NapAuth(FunctionKey = FunctionPermission.InformHeadline_UpdateStatus, Policy = Constant.Policy.Read)]
        [HttpGet]
        public ActionResult GetInforms(GetInformHeadlineCriteria criteria)
        {
            var informs = _informHeadlineService.GetInforms(criteria);
            var result = new { TotalItem = criteria.TotalItem, ChildItems = informs };
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = FunctionPermission.InformHeadline_UpdateStatus, Policy = Constant.Policy.Host)]
        [HttpPost]
        public ActionResult UpdateInforms(UpdateInformHeadlinesCriteria criteria)
        {
            var result = _informHeadlineService.UpdateInforms(criteria);
            return Json(result, JsonRequestBehavior.DenyGet);
        }
    }
}
