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
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Report.Models.CasinoReportEnhancement;
    using Fanex.CRM.Setting.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Casino Report Enhancement Controller
    /// </summary>
    public class CasinoReportEnhancementController : BaseController
    {
        private readonly ISettingService _serviceSetting;

        public CasinoReportEnhancementController()
            : this(null)
        { 
        }

        public CasinoReportEnhancementController(ISettingService serviceSetting)
        {
            _serviceSetting = serviceSetting ?? new SettingService();
        }

        [NapAuth(FunctionKey = FunctionPermissions.CasinoReportEnhancement, Policy = Constant.Policy.Read)]
        public ActionResult CasinoDailyEnhancementStatistics()
        {
            return View("~/Areas/Report/CasinoDailyStatisticsEnhancement/Index.cshtml");
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
