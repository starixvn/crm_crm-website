// -----------------------------------------------------------------------
// <copyright file="EnhancementReportProductModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Models.CasinoReportEnhancement
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CRM.Infrastructure.Models;

    public class EnhancementReportBaseModel : BaseModel
    {
        public EnhancementReportBaseModel()
        {
            ChildReports = new List<EnhancementReportBaseModel>();
            NumberOfCommunication = new List<int>();
        }

        public IEnumerable<int> NumberOfCommunication { get; set; }

        public int TotalCommunications
        {
            get
            {
                return NumberOfCommunication.Sum();
            }
        }

        public IEnumerable<EnhancementReportBaseModel> ChildReports { get; set; }
    }
}
