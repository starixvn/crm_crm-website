using Fanex.CRM.PicAndHeadline.Criteria;
using Fanex.CRM.PicAndHeadline.Models;
using Fanex.CRM.PicAndHeadline.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.CRM.PicAndHeadline.Services
{
    public class PicReportService : IPicReportService
    {
        private readonly IPicReportRepository picReportRepository;

        public PicReportService()
            : this(null)
        {
        }

        public PicReportService(IPicReportRepository picReportRepository)
        {
            this.picReportRepository = picReportRepository ?? new PicReportRepository();
        }

        public PicReport GetReport(GetReportCriteria criteria)
        {
            return new PicReport
            {
                DataList = new List<PicInfo> {
                    new PicInfo { Username = "a" },
                    new PicInfo { Username = "b" },
                    new PicInfo { Username = "c" }
                },
                Total = 21
            };

            return new PicReport
            {
                DataList = picReportRepository.GetReport(criteria).ToList(),
                Total = criteria.TotalRows
            };
        }
    }
}
