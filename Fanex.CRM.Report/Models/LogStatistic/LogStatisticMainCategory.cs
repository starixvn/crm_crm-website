// -----------------------------------------------------------------------
// <copyright file="LogStatisticMainCategory.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Models.LogStatistic
{
    using System.Collections.Generic;

    public class LogStatisticMainCategory : LogStatisticBaseModel
    {
        public LogStatisticMainCategory()
        {
            SubCategories = new List<LogStatisticBaseModel>();
        }

        public IEnumerable<LogStatisticBaseModel> SubCategories { get; set; }
    }
}
