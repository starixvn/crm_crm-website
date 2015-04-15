// -----------------------------------------------------------------------
// <copyright file="InformHeadlineController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.InformHeadline.Controllers
{
    using System.Web.Mvc;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.InformHeadline.Criteria;
    using Fanex.CRM.InformHeadline.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class InformHeadlineController : BaseController
    {
        private readonly IInformHeadlineService _informHeadlineService;

        public InformHeadlineController()
            : this(null)
        {
        }

        public InformHeadlineController(IInformHeadlineService informHeadlineService)
        {
            _informHeadlineService = informHeadlineService ?? new InformHeadlineService();
        }

        #region InformHeadline Cases

        /// <summary>
        /// Index action.
        /// </summary>
        /// <returns>Action Result.</returns>
        [NapAuth(FunctionKey = FunctionPermissions.InformHeadline, Policy = Constant.Policy.Read)]
        [HttpGet]
        public ActionResult Index()
        {
            var permission = GetPermission();

            if (permission != null)
            {
                ViewBag.Policy = permission.Policy;
                ViewBag.PermissionType = permission.PermissionType;
            }

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

        [AjaxNapAuth(FunctionKey = FunctionPermissions.InformHeadline, Policy = Constant.Policy.Read)]
        public void ExportInforms(GetInformHeadlinesCriteria criteria)
        {
            const string FILENAME = "InformHeadlines";
            var excel = _informHeadlineService.GetInformHeadlineExcel(criteria, FILENAME);
            excel.BuildWorkBook();
            excel.ResponseStreamExcel();
        }

        [HttpPost]
        [AjaxNapAuth(FunctionKey = FunctionPermissions.InformHeadlineSearch, Policy = Constant.Policy.Host)]
        public JsonResult UpdateInformStatus(UpdateInformStatusCriteria criteria)
        {            
            var result = _informHeadlineService.UpdateInformStatus(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion InformHeadline Cases

        [NapAuth(FunctionKey = FunctionPermissions.InformHeadlineSearch, Policy = Constant.Policy.Host)]
        [HttpGet]
        public ActionResult NewInform()
        {
            return View("~/Areas/InformHeadline/NewInform/Index.cshtml");
        }

        [NapAuth(FunctionKey = FunctionPermissions.InformHeadlineSearch, Policy = Constant.Policy.Host)]
        [HttpPost]
        public ActionResult GetDirectAccounts(GetDirectAccountsCriteria criteria)
        {
            var data = _informHeadlineService.GetDirectAccounts(criteria);
            var result = new { Items = data };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [NapAuth(FunctionKey = FunctionPermissions.InformHeadlineSearch, Policy = Constant.Policy.Host)]
        public ActionResult AllSettings()
        {
            return Json(_informHeadlineService.GetNewInformSettings(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Permission.
        /// </summary>
        /// <returns>Null if don't have permission.</returns>
        private dynamic GetPermission()
        {
            var searchPolicy = GetPolicy(FunctionPermissions.InformHeadlineSearch);
            if (searchPolicy != Constant.Policy.None)
            {
                return new 
                {
                    PermissionType = "Search",
                    Policy = searchPolicy.ToString()
                };
            }

            var contactPolicy = GetPolicy(FunctionPermissions.InformHeadlineContact);
            if (contactPolicy != Constant.Policy.None)
            {
                return new
                {
                    PermissionType = "Contact",
                    Policy = contactPolicy.ToString()
                };
            }

            return null;
        }
    }
}
