namespace Fanex.CRM.Services
{
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Criteria.PicAndHeadline;
    using Fanex.CRM.Models;
    using Fanex.CRM.Models.PicAndHeadline;
    using Fanex.CRM.Services.Interfaces;

    public class ChangesLogService : IChangesLogService
    {
        private readonly IBaseService baseService;

        public ChangesLogService() : this(null)
        {
        }

        public ChangesLogService(IBaseService baseService)
        {
            this.baseService = baseService ?? new BaseService();
        }

        public BaseReport<ChangeHeadlineDALogInfo> GetHeadlineDAReport(GetHeadlineDALogReportCriteria criteria)
        {
            var logInfos = baseService.GetData<ChangeHeadlineDALogInfo>(criteria);

            return new BaseReport<ChangeHeadlineDALogInfo>
            {
                DataList = logInfos,
                Totals = criteria.TotalRows
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