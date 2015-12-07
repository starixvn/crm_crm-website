// -----------------------------------------------------------------------
// <copyright file="EnhancementReportMainCategory.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Models.CasinoReportEnhancement
{
    using System.Collections.Generic;

    public class EnhancementReportMainCategory : EnhancementReportBaseModel
    {
        public EnhancementReportMainCategory()
        {
            SubCategories = new List<EnhancementReportBaseModel>();
        }

        public IEnumerable<EnhancementReportBaseModel> SubCategories { get; set; }
    }
}
