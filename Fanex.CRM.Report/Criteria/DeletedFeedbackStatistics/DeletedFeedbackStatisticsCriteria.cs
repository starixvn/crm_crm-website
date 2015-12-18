// -----------------------------------------------------------------------
// <copyright file="DeletedFeedbackStatisticsCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Criteria.DeletedFeedbackStatistics
{
    using System;
    using Fanex.Data.Repository;

    public class DeletedFeedbackStatisticsCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "CreatorName")]
        public string Handler { get; set; }

        [SpParam(SqlParamName = "DeletorName")]
        public string DeletedBy { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public override string GetSettingKey()
        {
            return "Report_DeletedFeedbackStatistics";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
