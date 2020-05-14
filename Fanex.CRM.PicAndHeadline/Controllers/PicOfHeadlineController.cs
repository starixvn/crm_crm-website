using System.Web.Mvc;
using Fanex.CRM.Common.Controllers;
using Fanex.CRM.Setting.Criteria;
using Fanex.CRM.Setting.Services;

namespace Fanex.CRM.PicAndHeadline.Controllers
{
    public class PicOfHeadlineController : BaseController
    {
        private readonly ISettingService _settingService;

        public PicOfHeadlineController()
            : this(null)
        {

        }

        public PicOfHeadlineController(
            ISettingService settingService)
        {
            _settingService = settingService ?? new SettingService();
        }

        public ActionResult Index()
        {
            return View("~/Areas/PicAndHeadline/PicOfHeadline/Index.cshtml");
        }

        [HttpPost]
        public ActionResult AllSetting()
        {
            var sites = _settingService.GetAllSitesWithNA();
            var currency = _settingService.GetAllCurrencies(
                    new GetAllCurrencyCriteria
                    {
                        IsTest = true
                    },
                    false);
            var positions = _settingService.GetPositions();

            var allSetting = new
            {
                SiteList = sites,
                CurrencyList = currency,
                CustomerTypeList = _settingService.GetAllCustomerTypes(),
                PositionList = positions
            };

            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }
    }
}