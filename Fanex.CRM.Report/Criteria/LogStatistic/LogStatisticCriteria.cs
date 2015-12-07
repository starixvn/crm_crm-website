// -----------------------------------------------------------------------
// <copyright file="LogStatisticCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Criteria.LogStatistic
{
    using System;
    using Fanex.Data.Repository;

    public class LogStatisticCriteria : CriteriaBase
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
            return true;
        }
    }
}
