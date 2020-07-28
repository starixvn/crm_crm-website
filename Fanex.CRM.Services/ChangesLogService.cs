namespace Fanex.CRM.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Criteria.PicAndHeadline;
    using Fanex.CRM.Models;
    using Fanex.CRM.Models.PicAndHeadline;
    using Fanex.CRM.Services.Interfaces;

    public class ChangesLogService : IChangesLogService
    {
        public readonly IBaseService baseService;

        public ChangesLogService() : this(null)
        {
        }

        public ChangesLogService(IBaseService baseService)
        {
            this.baseService = baseService ?? new BaseService();
        }

        public BaseReport<ChangeDALogInfo> GetHeadlineDAReport(GetHeadlineDALogReportCriteria criteria)
        {
            var logInfos = baseService.GetData<ChangeDALogInfo>(criteria);

            return new BaseReport<ChangeDALogInfo>
            {
                DataList = logInfos,
                Totals = 14
            };
        }

        public BaseReport<ChangePicLogInfo> GetPicReport(GetPicLogReportCriteria criteria)
        {
            var logInfos = baseService.GetData<ChangePicLogInfo>(criteria);

            return new BaseReport<ChangePicLogInfo>
            {
                DataList = logInfos,
                Totals = criteria.TotalRows
            };
        }

        public IEnumerable<BaseModel> GetActionType(GetActionTypeCriteria criteria)
        {
            return baseService.GetData<BaseModel>(criteria);
        }
    }
}