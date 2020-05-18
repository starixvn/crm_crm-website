using Fanex.CRM.Common.Models;
using Fanex.CRM.PicAndHeadline.Criteria;
using Fanex.CRM.PicAndHeadline.Models;

namespace Fanex.CRM.PicAndHeadline.Services
{
    public interface IPicReportService
    {
        PicReport GetReport(GetReportCriteria criteria);

        CommandResult InsertPic(InsertPicCriteria criteria);

        CommandResult EditPic(EditPicCriteria criteria);

        CommandResult ChangePicState(ChangePicStateCriteria criteria);
    }
}