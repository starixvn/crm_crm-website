using Fanex.CRM.Common.Models;
using Fanex.CRM.Criteria.PicAndHeadline;
using Fanex.CRM.Models.PicAndHeadline;

namespace Fanex.CRM.Services
{
    public interface IPicReportService
    {
        PicReport GetReport(GetReportCriteria criteria);

        CommandResult InsertPic(InsertPicCriteria criteria);

        CommandResult EditPic(EditPicCriteria criteria);

        CommandResult ChangePicState(ChangePicStateCriteria criteria);
    }
}