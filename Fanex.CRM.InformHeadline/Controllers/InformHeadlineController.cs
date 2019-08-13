namespace Fanex.CRM.InformHeadline.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.InformHeadline.Criteria;
    using Fanex.CRM.InformHeadline.Models;
    using Fanex.CRM.InformHeadline.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    public class InformHeadlineController : ReadOnlyBaseController
    {
        private readonly IInformHeadlineService _informHeadlineService;

        public InformHeadlineController() : this(null)
        {
        }

        public InformHeadlineController(IInformHeadlineService informHeadlineService)
        {
            _informHeadlineService = informHeadlineService ?? new InformHeadlineService();
        }

        #region InformHeadline Cases

        [NapAuth(FunctionKey = FunctionPermissions.InformHeadline, Policy = Constant.Policy.Read)]
        [HttpGet]
        public ActionResult Index()
        {
            var contactPolicy = GetPolicy(FunctionPermissions.InformHeadlineContact);
            var searchPolicy = GetPolicy(FunctionPermissions.InformHeadlineSearch);

            ViewBag.ContactPolicy = contactPolicy.ToString();
            ViewBag.SearchPolicy = searchPolicy.ToString();

            return View("~/Areas/InformHeadline/Index.cshtml");
        }

        [NapAuth(FunctionKey = FunctionPermissions.InformHeadline, Policy = Constant.Policy.Read)]
        [HttpPost]
        public ActionResult GetInforms(GetInformHeadlinesCriteria criteria)
        {
            var informs = _informHeadlineService.GetInforms(criteria);
            var result = new { Items = informs };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = FunctionPermissions.InformHeadline, Policy = Constant.Policy.Read)]
        public void ExportInforms(GetInformHeadlinesCriteria criteria)
        {
            const string FILENAME = "InformHeadlines";
            var excel = _informHeadlineService.GetInformHeadlineExcel(criteria, FILENAME);
            excel.BuildWorkBook();
            excel.ResponseStreamExcel();
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.InformHeadlineSearch, Policy = Constant.Policy.Host)]
        public JsonResult UpdateInformStatus(UpdateInformStatusCriteria criteria)
        {
            var result = _informHeadlineService.UpdateInformStatus(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion InformHeadline Cases

        #region New Inform

        [NapAuth(FunctionKey = FunctionPermissions.InformHeadlineSearch, Policy = Constant.Policy.Host)]
        [HttpGet]
        public ActionResult NewInform()
        {
            return View("~/Areas/InformHeadline/NewInform/Index.cshtml");
        }

        [NapAuth(FunctionKey = FunctionPermissions.InformHeadlineSearch, Policy = Constant.Policy.Host)]
        [HttpPost]
        public ActionResult GetNewInforms(GetNewInformsCriteria criteria)
        {
            IEnumerable<NewInform> data = _informHeadlineService.GetNewInforms(criteria);
            var result = new { Items = data };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.InformHeadlineSearch, Policy = Constant.Policy.Host)]
        public ActionResult InsertInforms(InsertInformHeadlineCriteria criteria)
        {
            criteria.Title = HttpUtility.HtmlEncode(criteria.Title);
            CommandResult commandResult = _informHeadlineService.InsertInformHeadline(criteria);
            return Json(commandResult, JsonRequestBehavior.DenyGet);
        }

        [NapAuth(FunctionKey = FunctionPermissions.InformHeadlineSearch, Policy = Constant.Policy.Host)]
        public void ExportNewInforms(GetNewInformsCriteria criteria)
        {
            const string FILENAME = "NewInformHeadlines";
            var excel = _informHeadlineService.GetNewInformHeadlineExcel(criteria, FILENAME);
            excel.BuildWorkBook();
            excel.ResponseStreamExcel();
        }

        #endregion New Inform

        #region Update Inform

        [NapAuth(FunctionKey = FunctionPermissions.InformHeadlineSearch, Policy = Constant.Policy.Host)]
        [HttpGet]
        public ActionResult UpdateInform(int titleId)
        {
            ViewBag.TitleId = titleId;
            return View("~/Areas/InformHeadline/UpdateInform/Index.cshtml");
        }

        [NapAuth(FunctionKey = FunctionPermissions.InformHeadlineSearch, Policy = Constant.Policy.Host)]
        [HttpPost]
        public ActionResult GetUpdateInforms(GetUpdateInformsCriteria criteria)
        {
            IEnumerable<UpdateInform> data = _informHeadlineService.GetUpdateInforms(criteria);
            var result = new { Items = data };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = FunctionPermissions.InformHeadlineSearch, Policy = Constant.Policy.Host)]
        public void ExportUpdateInforms(GetUpdateInformsCriteria criteria)
        {
            const string FILENAME = "UpdateInformHeadlines";
            var excel = _informHeadlineService.GetUpdateInformHeadlineExcel(criteria, FILENAME);
            excel.BuildWorkBook();
            excel.ResponseStreamExcel();
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.InformHeadlineContact, Policy = Constant.Policy.Host)]
        public ActionResult UpdateContentInforms(UpdateContentInformHeadlineCriteria criteria)
        {
            CommandResult commandResult = _informHeadlineService.UpdateContentInformHeadline(criteria);
            return Json(commandResult, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.InformHeadlineSearch, Policy = Constant.Policy.Host)]
        public ActionResult UpdateInforms(UpdateInformHeadlineCriteria criteria)
        {
            criteria.Title = HttpUtility.HtmlEncode(criteria.Title);
            CommandResult commandResult = _informHeadlineService.UpdateInformHeadline(criteria);
            return Json(commandResult, JsonRequestBehavior.DenyGet);
        }

        #endregion Update Inform

        #region Contact Informs

        [NapAuth(FunctionKey = FunctionPermissions.InformHeadlineContact, Policy = Constant.Policy.Read)]
        [HttpGet]
        public ActionResult ContactInform(int titleId, bool isClosed)
        {
            ViewBag.HasHostPermission = HasHostPermission(FunctionPermissions.InformHeadlineContact);
            ViewBag.TitleId = titleId;
            ViewBag.IsClosed = isClosed;
            return View("~/Areas/InformHeadline/ContactInform/Index.cshtml");
        }

        [NapAuth(FunctionKey = FunctionPermissions.InformHeadlineContact, Policy = Constant.Policy.Read)]
        [HttpPost]
        public ActionResult GetContactInforms(GetContactInformsCriteria criteria)
        {
            IEnumerable<UpdateInform> data = _informHeadlineService.GetContactInforms(criteria);
            var result = new { Items = data };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = FunctionPermissions.InformHeadlineContact, Policy = Constant.Policy.Read)]
        public void ExportContactInforms(GetContactInformsCriteria criteria)
        {
            const string FILENAME = "ContactInformHeadlines";
            var excel = _informHeadlineService.GetContactInformHeadlineExcel(criteria, FILENAME);
            excel.BuildWorkBook();
            excel.ResponseStreamExcel();
        }

        #endregion Contact Informs

        #region Common

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.InformHeadline, Policy = Constant.Policy.Read)]
        public ActionResult AllSettings()
        {
            return Json(_informHeadlineService.GetNewInformSettings(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.InformHeadlineContact, Policy = Constant.Policy.Read)]
        public ActionResult ContactInformSettings()
        {
            return Json(_informHeadlineService.GetContactInformSettings(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.InformHeadline, Policy = Constant.Policy.Read)]
        public ActionResult GetTitleDetail(int titleId)
        {
            TitleDetail title = _informHeadlineService.GetTitleDetail(titleId);
            return Json(title, JsonRequestBehavior.AllowGet);
        }

        #endregion Common

        [NapAuth(FunctionKey = FunctionPermissions.InformHeadline, Policy = Constant.Policy.Read)]
        public JsonResult GetRemovedAccountLogs(GetRemovedAccountLogsCriteria criteria)
        {
            Tuple<IEnumerable<RemovedAccountLog>, int> logs = _informHeadlineService.GetRemovedAccountLogs(criteria);
            return Json(new { Items = logs.Item1, TotalItems = logs.Item2 }, JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = FunctionPermissions.InformHeadline, Policy = Constant.Policy.Read)]
        public ActionResult GetRemovedAccountLogDetails(GetRemovedAccountLogDetailsCriteria criteria)
        {
            Tuple<IEnumerable<RemovedAccountLogDetails>, int> logs = _informHeadlineService.GetRemovedAccountLogDetails(criteria);
            return Json(new { Items = logs.Item1, TotalItems = logs.Item2 }, JsonRequestBehavior.AllowGet);
        }
    }
}