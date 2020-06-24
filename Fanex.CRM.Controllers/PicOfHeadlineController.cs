using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Serialization;
using Fanex.CRM.Common.Constants;
using Fanex.CRM.Common.Controllers;
using Fanex.CRM.Common.Services;
using Fanex.CRM.Criteria.PicAndHeadline;
using Fanex.CRM.Models.PicAndHeadline;
using Fanex.CRM.Report.Excels.PICAssignment;
using Fanex.CRM.Report.Models.PicAssignmentReport;
using Fanex.CRM.Resources;
using Fanex.CRM.Setting.Criteria;
using Fanex.CRM.Setting.Services;
using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Web.Attribute;

namespace Fanex.CRM.Controllers
{
    [NapAuth(FunctionKey = FunctionPermissions.PICAssignment, Policy = Constant.Policy.Read)]
    public class PicOfHeadlineController : BaseController
    {
        private readonly ISettingService _settingService;
        private readonly IBaseService _baseService;
        private readonly IPermissionService _permissionService;

        public PicOfHeadlineController()
            : this(null, null, null)
        {
        }

        public PicOfHeadlineController(
            ISettingService settingService,
            IBaseService baseService,
            IPermissionService permissionService)
        {
            _settingService = settingService ?? new SettingService();
            _baseService = baseService ?? new BaseService();
            _permissionService = permissionService ?? new PermissionService();
        }

        public ActionResult Index()
        {
            ViewBag.IsHost = _permissionService.HasHostPermission(FunctionPermissions.PICAssignment);

            return View("~/Areas/PicAndHeadline/PicOfHeadline/Index.cshtml");
        }

        [HttpPost]
        public ActionResult AllSetting()
        {
            var sites = _baseService.GetData<SiteInfo>(new GetSystemSiteCriteria());

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
        [NapAuth(FunctionKey = FunctionPermissions.PICAssignment, Policy = Constant.Policy.Host)]
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
        [NapAuth(FunctionKey = FunctionPermissions.PICAssignment, Policy = Constant.Policy.Host)]
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
        [NapAuth(FunctionKey = FunctionPermissions.PICAssignment, Policy = Constant.Policy.Host)]
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
        [NapAuth(FunctionKey = FunctionPermissions.PICAssignment, Policy = Constant.Policy.Host)]
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
            var rawData = _baseService.GetData<PICAssignmentInfo>(criteria);
            var assignments = GeneratePicList(rawData, criteria.PageIndex, criteria.PageSize);
            bool IsHost = _permissionService.HasHostPermission(FunctionPermissions.PICAssignment);
            var response = new
            {
                Items = assignments,
                Total = criteria.TotalRows,
                TotalUnassignedDirectAccount = criteria.TotalUnassigned,
                IsHost
            };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public void ExportExcelReport(GetAssignmentCriteria criteria)
        {
            criteria.PicName = criteria.PicName ?? string.Empty;
            criteria.DirectAccount = criteria.DirectAccount ?? string.Empty;
            criteria.GroupHeadline = criteria.GroupHeadline ?? string.Empty;
            var picInfo = _baseService.GetData<PICAssignmentInfo>(criteria);
            var data = GeneratePicList(picInfo, criteria.PageIndex, criteria.PageSize);

            var reportData = MappingReportData(data);

            var excel = new PICAssignmentExportExcel(reportData, Labels.PicAssignment);
            excel.BuildWorkBook();
        }

        private static IEnumerable<PICAssignmentInfo> GeneratePicList(IEnumerable<PICAssignmentInfo> assignments, int pageIndex, int pageSize)
        {
            var index = 0;

            foreach (var assignment in assignments)
            {
                assignment.RowNum = index + ((pageIndex - 1) * pageSize);
                index++;
                if (!string.IsNullOrWhiteSpace(assignment.PIC))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<PicModel>), new XmlRootAttribute("ROOT"));
                    var listPic = (List<PicModel>)serializer.Deserialize(new StringReader(assignment.PIC));
                    assignment.PicList = listPic;

                    assignment.PicForMapping = listPic.Any() ? string.Join(",", listPic.Select(pic => pic.PicId)) : string.Empty;
                }
            }

            var results = assignments.OrderBy(i => i.HeadlineName).ThenBy(a => a.PicForMapping).ThenBy(i => i.DirectAccount).ToList();

            for (int i = 0; i < results.Count; i++)
            {
                results[i].RowNum = i;
            }
            return results;
        }

        private IEnumerable<GroupHeadlineOfPic> MappingReportData(IEnumerable<PICAssignmentInfo> assignments)
        {
            var result = assignments.GroupBy(a => a.HeadlineID,
                (key, g) => new GroupHeadlineOfPic
                {
                    GroupHeadlineId = key,
                    GroupHeadline = g.FirstOrDefault().HeadlineName,
                    HeadlineType = g.FirstOrDefault().HeadlineType,
                    Pics = g.GroupBy(p => p.PicForMapping, (k, pic) => new PicOfDirectAccount
                    {
                        Pic = pic.FirstOrDefault().PicList,
                        DirectAccounts = pic.Select(d => new DirectAccountInfo
                        {
                            DirectAccount = d.DirectAccount,
                            Status = d.Status,
                            Site = d.Site,
                            Currency = d.Currency,
                            Position = d.Position,
                        }).OrderBy(a => a.DirectAccount)
                    }).OrderBy(v => v.Pic != null)
                }).OrderBy(x => x.GroupHeadline);

            return result.ToList();
        }
    }
}