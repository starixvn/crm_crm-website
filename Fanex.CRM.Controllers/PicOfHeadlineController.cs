using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fanex.CRM.Common.Constants;
using Fanex.CRM.Common.Controllers;
using Fanex.CRM.Common.Helpers;
using Fanex.CRM.Common.Services;
using Fanex.CRM.Criteria.PicAndHeadline;
using Fanex.CRM.Models.PicAndHeadline;
using Fanex.CRM.Report.Excels.PICAssignment;
using Fanex.CRM.Report.Models.PicAssignmentReport;
using Fanex.CRM.Resources;
using Fanex.CRM.Services;
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
        private readonly IPicAssignmentService _picAssignmentService;

        public PicOfHeadlineController()
            : this(null, null, null, null)
        {
        }

        public PicOfHeadlineController(
            ISettingService settingService,
            IBaseService baseService,
            IPermissionService permissionService,
            IPicAssignmentService picAssignmentService)
        {
            _settingService = settingService ?? new SettingService();
            _baseService = baseService ?? new BaseService();
            _permissionService = permissionService ?? new PermissionService();
            _picAssignmentService = picAssignmentService ?? new PicAssignmentService();
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
                        IsTest = true,
                        Active = 1,
                        OfferSite = 1
                    },
                    false).OrderBy(c => c.Name);

            var positions = _settingService.GetPositions(true);

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

            var result = _picAssignmentService.ValidateAssignmentDirectAccount(criteria);

            var data = new
            {
                IsSuccessful = criteria.ErrCode == 0,
                Message = criteria.ErrMsg,
                ErrorAccounts = result.Item1,
                AssignedDirectAccounts = result.Item2
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

        [HttpGet]
        [NapAuth(FunctionKey = FunctionPermissions.PICAssignment, Policy = Constant.Policy.Host)]
        public ActionResult GetPicList()
        {
            var pics = _baseService.GetData<PicInfo>(new GetPICCriteria { IsActive = true });
            return Json(pics, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.PICAssignment, Policy = Constant.Policy.Host)]
        public ActionResult RemoveAssignment(IList<RemoveAssignment> removeAssignments)
        {
            var criteria = new RemoveAssignmentCriteria
            {
                RemovedPICs = SerializerHelper.SerializeList(removeAssignments)
            };

            var result = _baseService.ExecuteNonQuery(criteria);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.PICAssignment, Policy = Constant.Policy.Read)]
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

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.PICAssignment, Policy = Constant.Policy.Host)]
        public ActionResult SetDefaultPIC(int headlineId, string picIds)
        {
            var criteria = new SetDefaultPicCriteria
            {
                HeadlineId = headlineId,
                PicIds = picIds
            };
            var result = _baseService.ExecuteNonQuery(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionPermissions.PICAssignment, Policy = Constant.Policy.Host)]
        public ActionResult RemoveDefaultPIC(int headlineId)
        {
            var criteria = new RemoveDefaultPicCriteria
            {
                HeadlineId = headlineId
            };
            var result = _baseService.ExecuteNonQuery(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public void ExportExcelReport(GetAssignmentCriteria criteria)
        {
            criteria.PicName = criteria.PicName ?? string.Empty;
            criteria.DirectAccount = criteria.DirectAccount ?? string.Empty;
            criteria.GroupHeadline = criteria.GroupHeadline ?? string.Empty;
            criteria.PageIndex = null;
            criteria.PageSize = null;
            var picInfo = _baseService.GetData<PICAssignmentInfo>(criteria);
            var data = GeneratePicList(picInfo, criteria.PageIndex, criteria.PageSize);

            var reportData = MappingReportData(data);

            var excel = new PICAssignmentExportExcel(reportData, Labels.PicAssignment);
            excel.BuildWorkBook();
        }

        private static IEnumerable<PICAssignmentInfo> GeneratePicList(IEnumerable<PICAssignmentInfo> assignments, int? pageIndex, int? pageSize)
        {
            foreach (var assignment in assignments)
            {
                if (!string.IsNullOrWhiteSpace(assignment.PIC))
                {
                    var listPic = SerializerHelper.DeserializeList<PicModel>(assignment.PIC);
                    assignment.PicList = listPic.OrderBy(p => p.Order).ThenBy(a => a.RoleId).ThenBy(r => r.PicName).ToList();
                    assignment.PicForMapping = listPic.Any() ? string.Join(",", listPic.Select(pic => pic.PicId)) : string.Empty;
                }

                if (!string.IsNullOrWhiteSpace(assignment.DefaultPIC))
                {
                    assignment.DefaultPicList = SerializerHelper.DeserializeList<PicModel>(assignment.DefaultPIC);
                }
            }

            var data = assignments
                .OrderBy(i => i.HeadlineName)
                .ThenByDescending(p => p.PicList?.Count)
                .ThenBy(p => p.PicList != null)
                .ThenBy(o => o.PicForMapping)
                .ThenBy(i => i.DirectAccount);

            var results = data.OrderBy(i => i.HeadlineID == 0).ToList();

            for (int i = 0; i < results.Count; i++)
            {
                results[i].RowNum = (pageIndex != null && pageSize != null) ? i + ((pageIndex - 1) * pageSize) : i;
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
                    DefaultPIC = g.FirstOrDefault().DefaultPicList,
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