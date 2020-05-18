using System.Web.Mvc;
using Fanex.CRM.Common.Controllers;
using Fanex.CRM.Common.Services;
using Fanex.CRM.Criteria.PicAndHeadline;
using Fanex.CRM.Models.PicAndHeadline;
using Fanex.CRM.Setting.Criteria;
using Fanex.CRM.Setting.Services;

namespace Fanex.CRM.Controllers
{
    public class PicOfHeadlineController : BaseController
    {
        private readonly ISettingService _settingService;
        private readonly IBaseService _baseService;

        public PicOfHeadlineController()
            : this(null, null)
        {
        }

        public PicOfHeadlineController(
            ISettingService settingService,
            IBaseService baseService)
        {
            _settingService = settingService ?? new SettingService();
            _baseService = baseService ?? new BaseService();
        }

        public ActionResult Index()
        {
            return View("~/Areas/PicAndHeadline/PicOfHeadline/Index.cshtml");
        }

        [HttpPost]
        public ActionResult AllSetting()
        {
            var sites = _baseService.GetData<SiteInfo>(new GetSystemSiteCriteria
            {
                ExcludeCN88Site = true
            });

            var currency = _settingService.GetAllCurrencies(
                    new GetAllCurrencyCriteria
                    {
                        IsTest = true
                    },
                    false);

            var positions = _settingService.GetPositions();

            var pics = _baseService.GetData<PicInfo>(new GetPICCriteria());

            var headlines = _baseService.GetData<HeadlineInfo>(new GetHeadlinesCriteria());

            var allSetting = new
            {
                SiteList = sites,
                CurrencyList = currency,
                CustomerTypeList = _settingService.GetAllCustomerTypes(),
                PositionList = positions,
                picList = pics,
                headlineList = headlines
            };

            return Json(allSetting, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetAllPICAssignment(GetAssignmentCriteria criteria)
        {
            criteria.PIC = criteria.PIC ?? string.Empty;
            criteria.DirectAccount = criteria.DirectAccount ?? string.Empty;
            criteria.GroupHeadline = criteria.GroupHeadline ?? string.Empty;

            var data = _baseService.GetData<PICAssignmentInfo>(criteria);

            var response = new
            {
                Items = data,
                Total = criteria.TotalRows
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}