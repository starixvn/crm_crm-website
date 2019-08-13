namespace Fanex.CRM.Report.Criteria.LogStatistics
{
    using System;
    using Fanex.Data.Repository;

    public class LogStatisticsCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int ProductType { get; set; }

        public int CategoryType { get; set; }

        public int SiteType { get; set; }

        public string ProductId { get; set; }

        public string MainCategoryId { get; set; }

        public string CommunicationId { get; set; }

        public override string GetSettingKey()
        {
            return "Report_LogStatistic";
        }

        public override bool IsValid()
        {
            return ProductId != string.Empty &&
                    MainCategoryId != string.Empty &&
                    CommunicationId != string.Empty;
        }
    }
}