namespace Fanex.CRM.Setting.Criteria
{
    using Fanex.Data.Repository;

    public class GetSettingByIdCriteria : CriteriaBase
    {
        public int SettingId { get; set; }

        public override string GetSettingKey()
        {
            return string.Empty;
        }

        public override bool IsValid() => true;
    }
}