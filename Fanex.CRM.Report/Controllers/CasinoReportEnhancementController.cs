// -----------------------------------------------------------------------
// <copyright file="CasinoReportEnhancementController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Controllers;
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Report.Criteria.CasinoEnhancementReport;
    using Fanex.CRM.Report.Models.CasinoReportEnhancement;
    using Fanex.CRM.Report.Services.CasinoEnhancementReport;
    using Fanex.CRM.Setting.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Casino Report Enhancement Controller
    /// </summary>
    public class CasinoReportEnhancementController : BaseController
    {
        private readonly ISettingService _serviceSetting;
        private readonly ICasinoEnhancementReportService _casinoEnhancementReportService;

        public CasinoReportEnhancementController()
            : this(null, null)
        { 
        }

        public CasinoReportEnhancementController(
            ICasinoEnhancementReportService casinoEnhancementReportService,
            ISettingService serviceSetting)
        {
            _casinoEnhancementReportService = casinoEnhancementReportService ?? new CasinoEnhancementReportService();
            _serviceSetting = serviceSetting ?? new SettingService();
        }

        [NapAuth(FunctionKey = FunctionPermissions.CasinoReportEnhancement, Policy = Constant.Policy.Read)]
        public ActionResult CasinoDailyEnhancementStatistics()
        {
            return View("~/Areas/Report/CasinoDailyStatisticsEnhancement/Index.cshtml");
        }

        [AjaxNapAuth(FunctionKey = FunctionPermissions.CasinoReportEnhancement, Policy = Constant.Policy.Read)]
        [HttpPost]
        public ActionResult GetMainReport(CasinoEnhancementReportCriteria criteria)
        {
            criteria.ProductType = (int)ProductType.Detail;
            criteria.CategoryType = (int)CategoryType.SubCategory;
            criteria.SiteType = (int)SiteType.IBC;

            var jsonResult = Json(new
            {
                IBCReport = _casinoEnhancementReportService.GetReportData(criteria),
            });

            return jsonResult;
        }

        [HttpPost]
        [AjaxNapAuth(FunctionKey = FunctionPermissions.CasinoReportEnhancement, Policy = Constant.Policy.Read)]
        public ActionResult GetAllSettings()
        {
            var products = _serviceSetting.AllProduct().Select(p => new
                {
                    Id = p.Id,
                    Name = p.Value
                });
            var communicationTools = _serviceSetting.AllCommunicationTool().Select(p => new
                {
                    Id = p.Id,
                    Name = p.Name
                });
            var mainCategories = _serviceSetting.AllMainCategory().Select(p => new
                {
                    Id = p.Id,
                    Name = p.Name
                });

            var allSetting = new SettingForCasinoDailyStatisticsEnhancement
            {
                CommunicationToolList = communicationTools,
                MainCategoryList = mainCategories,
                ProductList = products
            };

            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }
    }
}
