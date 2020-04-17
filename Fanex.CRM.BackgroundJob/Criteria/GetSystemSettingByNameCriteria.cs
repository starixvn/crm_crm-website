namespace Fanex.CRM.BackgroundJob.Criteria
{
    using Fanex.Data.Repository;

    public class GetSystemSettingByNameCriteria : CriteriaBase
    {
        public string SettingNames { get; set; }

        public override string GetSettingKey() => "";

        public override bool IsValid() => !string.IsNullOrWhiteSpace(SettingNames);
    }
}