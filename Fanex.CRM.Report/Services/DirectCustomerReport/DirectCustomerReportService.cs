namespace Fanex.CRM.Report.Repositories.CustomerReport
{
    using Criteria.DirectCustomerReport;
    using Models.DirectCustomerReport;
    using System.Collections.Generic;

    public class DirectCustomerReportService : IDirectCustomerReportService
    {
        private readonly IDirectCustomerReportRepository _customerReportRepository;

        public DirectCustomerReportService()
            : this(null)
        {
        }

        public DirectCustomerReportService(IDirectCustomerReportRepository customerReportRepository)
        {
            _customerReportRepository = customerReportRepository ?? new DirectCustomerReportRepository();
        }

        public DirectCustomerReportInfo GetReport(DirectCustomerReportCriteria criteria)
        {
            //return new DirectCustomerReportInfo
            //{
            //    CustomerList = _customerReportRepository.GetReport(criteria).ToList(),
            //    TotalCustomer = criteria.TotalRows
            //};

            return FakeReport();
        }

        private DirectCustomerReportInfo FakeReport()
        {
            return new DirectCustomerReportInfo
            {
                CustomerList = new List<DirectCustomerInfo>
                {
                    FakeCustomerInfo(),
                     new DirectCustomerInfo {
                        GroupHeadline = "group headline 2",
                        Username = "user name 2",
                        Status = "status 2"
                    },
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                     FakeCustomerInfo(),
                },
                TotalCustomer = 70
            };
        }

        private DirectCustomerInfo FakeCustomerInfo()
        {
            return new DirectCustomerInfo
            {
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
                PIC3 = "pic 3 1",
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
            };
        }
    }
}