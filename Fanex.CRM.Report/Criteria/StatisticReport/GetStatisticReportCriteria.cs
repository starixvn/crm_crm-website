namespace Fanex.CRM.Report.Criteria.StatisticReport
{
    using System;
    using System.Collections;
    using Common.Criteria;
    using Fanex.CRM.Setting.Constants;
    using Fanex.Data.Repository;

    public class GetStatisticReportCriteria : BasedCriteria
    {
        [SpParam(Ignored = true)]
        public int ReportType { get; set; }

        [SpParam(SqlParamName = "fromdate")]
        public DateTime FromDate { get; set; }

        [SpParam(SqlParamName = "todate")]
        public DateTime ToDate { get; set; }

        [SpParam(SqlParamName = "maincategoryid")]
        public int MainCategoryId { get; set; }

        [SpParam(SqlParamName = "subcategoryid")]
        public int SubCategoryId { get; set; }

        [SpParam(Ignored = true)]
        public int PageIndex { get; set; }

        [SpParam(Ignored = true)]
        public int PageSize { get; set; }

        [SpParam(Ignored = true)]
        public bool IsGetFullReport { get; set; }

        public override string GetSettingKey()
        {
            return "Report_StatisticReport_GetReport";
        }

        public override bool IsValid() => true;

        public override Hashtable GetSpParams()
        {
            var hastableParam = new Hashtable();
            hastableParam.Add("fromdate", FromDate);
            hastableParam.Add("todate", ToDate);
            hastableParam.Add("maincategoryid", MainCategoryId == SettingConstants.AllSetting ? 0 : MainCategoryId);
            hastableParam.Add("subcategoryid", SubCategoryId == SettingConstants.AllSetting ? 0 : SubCategoryId);
            hastableParam.Add("userrole", UserRole);
            return hastableParam;
        }
    }
}