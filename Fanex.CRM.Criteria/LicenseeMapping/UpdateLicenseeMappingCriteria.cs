namespace Fanex.CRM.Criteria.LicenseeMapping
{
    using Fanex.CRM.Common.Criteria;

    public class UpdateLicenseeMappingCriteria : UpdateCriteria
    {
        public int SettingId { get; set; }
        public string SourceMapping { get; set; }
        public string LicenseeManager { get; set; }
        public override string GetSettingKey() => "Acc_CRM_SourceLicenseeMappingUpd";

        public override bool IsValid() => true;
    }
}