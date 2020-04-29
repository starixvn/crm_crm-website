using Fanex.CRM.PicAndHeadline.Criteria;
using Fanex.CRM.PicAndHeadline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.CRM.PicAndHeadline.Repositories
{
    public interface IPicReportRepository
    {
        IEnumerable<PicInfo> GetReport(GetReportCriteria criteria);
    }
}
