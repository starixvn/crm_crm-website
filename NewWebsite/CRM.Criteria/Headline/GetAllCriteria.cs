namespace CRM.Criteria.Headline
{
    using Fanex.Data.Repository;

    public class GetAllCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public override string GetSettingKey() => "Acc_CRM_HeadlineSel";

        public override bool IsValid() => true;
    }
}