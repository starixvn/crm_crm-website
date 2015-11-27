// -----------------------------------------------------------------------
// <copyright file="SettingForCasinoDailyStatisticsEnhancement.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Models.CasinoReportEnhancement
{
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;

    /// <summary>
    /// Setting For Casino Daily Statistics Enhancement
    /// </summary>
    public class SettingForCasinoDailyStatisticsEnhancement
    {
        public List<SettingInfo> ProductList { get; set; }

        public List<SettingInfo> MainCategoryList { get; set; }

        public List<SettingInfo> CommunicationToolList { get; set; }
    }
}
