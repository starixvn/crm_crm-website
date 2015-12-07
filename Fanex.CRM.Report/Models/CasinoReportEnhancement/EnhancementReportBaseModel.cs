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
            ListOfCommunicationCount = new List<int>();
        }

        public List<int> ListOfCommunicationCount { get; set; }

        public List<int> ListOfCommunicationSumByProduct { get; set; }

        public int TotalCommunications
        {
            get
            {
                return ListOfCommunicationCount.Sum();
            }
        }
    }
}
