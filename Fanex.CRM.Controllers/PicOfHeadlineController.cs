using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Serialization;
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
                    false).OrderBy(c => c.Name);

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
        public ActionResult ValidateAssignPic(AssignPic model)
        {
            var criteria = new ValidateAssignmentCriteria
            {
                HeadlineId = model.HeadlineId,
                PicId = model.PicIds,
                Username = model.DirectAccountIds
            };

            var result = _baseService.GetData<AssignedDirectAccount>(criteria);
            var data = new
            {
                IsSuccessful = criteria.ErrCode == 0,
                Message = criteria.ErrMsg,
                AssignedDirectAccounts = result
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AssignPIC(AssignPic model)
        {
            var command = new AssignPicCriteria
            {
                HeadlineId = model.HeadlineId,
                PicId = model.PicIds,
                DirectAccount = model.DirectAccountIds,
                IsKeppAssign = model.IsKeepAssign
            };

            var result = _baseService.ExecuteNonQuery(command);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAssignPICSettings()
        {
            var headlines = _baseService.GetData<HeadlineInfo>(new GetHeadlinesCriteria());
            var pics = _baseService.GetData<PicInfo>(new GetPICCriteria { IsActive = true });

            var assignPicSettings = new
            {
                groupHeadlines = headlines,
                pics
            };
            return Json(assignPicSettings, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RemoveAssignment(IList<RemoveAssignment> removeAssignments)
        {
            var serializer = new XmlSerializer(typeof(List<RemoveAssignment>),
                                   new XmlRootAttribute("ROOT"));

            var stream = new StringWriter();
            serializer.Serialize(stream, removeAssignments);

            var criteria = new RemoveAssignmentCriteria
            {
                RemovedPICs = stream.ToString()
            };

            var result = _baseService.ExecuteNonQuery(criteria);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetAllPICAssignment(GetAssignmentCriteria criteria)
        {
            criteria.PicName = criteria.PicName ?? string.Empty;
            criteria.DirectAccount = criteria.DirectAccount ?? string.Empty;
            criteria.GroupHeadline = criteria.GroupHeadline ?? string.Empty;
            var data = _baseService.GetData<PICAssignmentInfo>(criteria);
            GeneratePicList(data);
            var response = new
            {
                Items = data,
                Total = criteria.TotalRows,
                TotalUnassignedDirectAccount = criteria.TotalUnassigned
            };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        private static void GeneratePicList(IEnumerable<PICAssignmentInfo> assignments)
        {
            foreach (var assignment in assignments)
            {
                if (!string.IsNullOrWhiteSpace(assignment.PIC))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<PicModel>), new XmlRootAttribute("ROOT"));
                    assignment.PicList = (List<PicModel>)serializer.Deserialize(new StringReader(assignment.PIC));
                }
            }
        }
    }
}