namespace Fanex.CRM.Criteria.LicenseeMapping
{
    using Fanex.Data.Repository;

    public class GetLicenseeCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Acc_CRM_LicenseeMappingSel";

        public override bool IsValid() => true;
    }
}