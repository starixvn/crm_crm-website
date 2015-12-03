// -----------------------------------------------------------------------
// <copyright file="StatisticsReportEnhancementByDateModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Models.CasinoReportEnhancement
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Models.StatisticsReportByDate;

    public class StatisticsReportEnhancementByDateModel : StatisticsReportByDateModel
    {
        public List<BaseModel> Products { get; set; }
    }
}
