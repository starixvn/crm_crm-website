namespace Fanex.CRM.InformHeadline.Controllers
{
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.InformHeadline.Criteria;
    using Fanex.CRM.InformHeadline.Models;
    using Fanex.CRM.InformHeadline.Services;
    using Fanex.CRM.Setting.Models;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    public class InformHeadlineController : ReadOnlyBaseController
    {
        private readonly IInformHeadlineService _informHeadlineService;
        private readonly IExportInformHeadlineService exportInformHeadlineService;

        public InformHeadlineController() : this(null, null)
        {
        }

        public InformHeadlineController(
            IInformHeadlineService informHeadlineService,
            IExportInformHeadlineService exportInformHeadlineService)
        {
            _informHeadlineService = informHeadlineService ?? new InformHeadlineService();
            this.exportInformHeadlineService = exportInformHeadlineService ?? new ExportInformHeadlineService();
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

            return View("~/Areas/InformHeadline/ViewList/Index.cshtml");
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
        public ActionResult ExportInforms(GetInformHeadlinesCriteria criteria)
        {
            IEnumerable<InformHeadline> informHeadlines = _informHeadlineService.GetInforms(criteria);
            byte[] bytes = exportInformHeadlineService.Export(informHeadlines);

            return File(bytes, "application/vnd.ms-excel", "InformHeadlines.xls");
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
            var data = _informHeadlineService.GetNewInforms(criteria);
            var result = new { Items = data };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.InformHeadlineSearch, Policy = Constant.Policy.Host)]
        public ActionResult InsertInforms(InsertInformHeadlineCriteria criteria)
        {
            criteria.Title = HttpUtility.HtmlEncode(criteria.Title);
            return Json(_informHeadlineService.InsertInformHeadline(criteria), JsonRequestBehavior.DenyGet);
        }

        [NapAuth(FunctionKey = FunctionPermissions.InformHeadlineSearch, Policy = Constant.Policy.Host)]
        public ActionResult ExportNewInforms(GetNewInformsCriteria criteria)
        {
            IEnumerable<NewInform> informHeadlines = _informHeadlineService.GetNewInforms(criteria);
            IEnumerable<InformStatus> informStatuses = _informHeadlineService.GetInformStatuses();
            byte[] bytes = exportInformHeadlineService.ExportNewInform(criteria.Title, informHeadlines, informStatuses);

            return File(bytes, "application/vnd.ms-excel", "NewInformHeadlines.xls");
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
            var data = _informHeadlineService.GetUpdateInforms(criteria);
            var result = new { Items = data };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = FunctionPermissions.InformHeadlineSearch, Policy = Constant.Policy.Host)]
        public ActionResult ExportUpdateInforms(GetUpdateInformsCriteria criteria)
        {
            TitleDetail title = _informHeadlineService.GetTitleDetail(criteria.TitleId);
            IEnumerable<UpdateInform> informHeadlines = _informHeadlineService.GetUpdateInforms(criteria);
            IEnumerable<InformStatus> informStatuses = _informHeadlineService.GetInformStatuses();
            IEnumerable<Status> productStatuses = _informHeadlineService.GetProductStatuses();
            IEnumerable<Station> stations = _informHeadlineService.GetStations();
            byte[] bytes = exportInformHeadlineService.ExportUpdateInform(title.Title, informHeadlines, informStatuses, productStatuses, stations);

            return File(bytes, "application/vnd.ms-excel", "UpdateInformHeadlines.xls");
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.InformHeadlineContact, Policy = Constant.Policy.Host)]
        public ActionResult UpdateContentInforms(UpdateContentInformHeadlineCriteria criteria)
        {
            return Json(_informHeadlineService.UpdateContentInformHeadline(criteria), JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.InformHeadlineSearch, Policy = Constant.Policy.Host)]
        public ActionResult UpdateInforms(UpdateInformHeadlineCriteria criteria)
        {
            criteria.Title = HttpUtility.HtmlEncode(criteria.Title);
            return Json(_informHeadlineService.UpdateInformHeadline(criteria), JsonRequestBehavior.DenyGet);
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
            var data = _informHeadlineService.GetContactInforms(criteria);
            var result = new { Items = data };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = FunctionPermissions.InformHeadlineContact, Policy = Constant.Policy.Read)]
        public ActionResult ExportContactInforms(GetContactInformsCriteria criteria)
        {
            TitleDetail title = _informHeadlineService.GetTitleDetail(criteria.TitleId);
            IEnumerable<UpdateInform> informHeadlines = _informHeadlineService.GetContactInforms(criteria);
            IEnumerable<InformStatus> informStatuses = _informHeadlineService.GetInformStatuses();
            IEnumerable<Status> productStatuses = _informHeadlineService.GetProductStatuses();
            IEnumerable<Station> stations = _informHeadlineService.GetStations();
            byte[] bytes = exportInformHeadlineService.ExportUpdateInform(title.Title, informHeadlines, informStatuses, productStatuses, stations);

            return File(bytes, "application/vnd.ms-excel", "ContactInformHeadlines.xls");
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
            IEnumerable<RemovedAccountLog> logs = _informHeadlineService.GetRemovedAccountLogs(criteria);
            return Json(logs, JsonRequestBehavior.AllowGet);
        }

        [NapAuth(FunctionKey = FunctionPermissions.InformHeadline, Policy = Constant.Policy.Read)]
        public ActionResult GetRemovedAccountLogDetails(GetRemovedAccountLogDetailsCriteria criteria)
        {
            IEnumerable<RemovedAccountLogDetails> logs = _informHeadlineService.GetRemovedAccountLogDetails(criteria);
            return Json(logs, JsonRequestBehavior.AllowGet);
        }
    }
}