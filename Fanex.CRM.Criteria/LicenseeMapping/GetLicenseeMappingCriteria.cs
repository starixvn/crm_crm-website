namespace Fanex.CRM.Criteria.LicenseeMapping
{
    using Fanex.Data.Repository;

    public class GetLicenseeMappingCriteria : CriteriaBase
    {
        public string Setting { get; set; }

        public override string GetSettingKey() => "Acc_CRM_SourceLicenseeMappingSel";

        public override bool IsValid() => true;
    }
}