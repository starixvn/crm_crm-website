using Fanex.CRM.Common.Models;
using Fanex.CRM.PicAndHeadline.Criteria;
using Fanex.CRM.PicAndHeadline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.CRM.PicAndHeadline.Services
{
    public interface IPicReportService
    {
        PicReport GetReport(GetReportCriteria criteria);
        CommandResult InsertPic(InsertPicCriteria criteria);
    }
}
