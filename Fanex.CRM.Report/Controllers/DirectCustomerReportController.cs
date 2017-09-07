using Fanex.CRM.Common.Constants;
using Fanex.CRM.Common.Controllers;
using Fanex.CRM.Report.Criteria.DirectCustomerReport;
using Fanex.CRM.Report.Models.DirectCustomerReport;
using Fanex.CRM.Report.Repositories.CustomerReport;
using Fanex.CRM.Setting.Services;
using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Web.Attribute;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fanex.CRM.Report.Controllers
{
    [NapAuth(FunctionKey = FunctionPermissions.DirectCustomerReport, Policy = Constant.Policy.Read)]
    public class DirectCustomerReportController : ReadOnlyBaseController
    {
        private readonly ICustomerReportService _customerReportService;

        private readonly ISettingService _settingService;

        public DirectCustomerReportController()
            : this(null, null)
        {
        }

        public DirectCustomerReportController(
            ICustomerReportService customerReportService,
            ISettingService settingService)
        {
            _customerReportService = customerReportService ?? new CustomerReportService();
            _settingService = settingService ?? new SettingService();
        }

        public ActionResult Index()
        {
            return View("~/Areas/Report/DirectCustomerReport/Index.cshtml");
        }

        [HttpPost]
        public ActionResult GetReport(DirectCustomerReportCriteria criteria)
        {
            //            var customerReport = _customerReportService.GetCustomerReport(criteria);
            var customerReport = FakeReport();
            return Json(customerReport, JsonRequestBehavior.AllowGet);
        }

        private DirectCustomerReportInfo FakeReport()
        {
            return new DirectCustomerReportInfo
            {
                CustomerList = new List<DirectCustomerInfo>
                {
                    new DirectCustomerInfo {
                        GroupHeadline = "group headline 1",
                        Username = "user name 1",
                        Status = "status 1",
                        Site = "site 1",
                        Currency = "currency 1",
                        Position = "position 1",
                        IntroduceBy = "introduce by 1",
                        Languages = "language 1",
                        Country = "country 1",
                        Checker = "checker 1",
                        VIP = "vip 1",
                        FirstName = "first name 1",
                        LastName = "last name 1",
                        Phone1 = "phone 1 1 ",
                        Phone2 = "phone 2 1",
                        Phone3 = "phone 3 1",
                        PIC1 = "pic 1 1",
                        PIC2 = "pic 2 1",
                        PIC3  = "pic 3 1",
                        PIC4 = "pic 4 1",
                        Mobile = "mobile 1",
                        Email = "email 1",
                        Skype = "skype 1",
                        MSN = "msn 1",
                        WhatsApp = "whatsapp 1",
                        Line = "line 1",
                        WeChat = "wechat 1",
                        Yahoo = "yahoo 1",
                        BBM = "bbm 1",
                        QQ = "qq 1"
                    },
                     new DirectCustomerInfo {
                        GroupHeadline = "group headline 2",
                        Username = "user name 2",
                        Status = "status 2"
                    }
                },
                TotalCustomer = 2
            };
        }

    }
}
