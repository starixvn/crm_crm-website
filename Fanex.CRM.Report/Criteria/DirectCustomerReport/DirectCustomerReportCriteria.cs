using Fanex.Data.Repository;

namespace Fanex.CRM.Report.Criteria.DirectCustomerReport
{
    public class DirectCustomerReportCriteria : CriteriaBase
    {
        public string Headline { get; set; }

        public bool ExcludeIgnoredAccounts { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_Crm_Rpt_DirectCustomerReport";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
