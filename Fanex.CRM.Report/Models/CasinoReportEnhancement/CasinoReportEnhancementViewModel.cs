// -----------------------------------------------------------------------
// <copyright file="StatisticsReportEnhancementByDateModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Models.CasinoReportEnhancement
{
    using System.Collections.Generic;
    using Fanex.CRM.Infrastructure.Models;

    public class CasinoReportEnhancementViewModel : BaseModel
    {
        public CasinoReportEnhancementViewModel()
        {
            MainCategories = new List<EnhancementReportBaseModel>();
            Products = new List<EnhancementReportProduct>();
        }

        public IEnumerable<EnhancementReportBaseModel> MainCategories { get; set; }

        public IEnumerable<EnhancementReportProduct> Products { get; set; }

        public IEnumerable<BaseModel> ProductColumns { get; set; }

        public IEnumerable<BaseModel> CommunicationColumns { get; set; }
    }
}
