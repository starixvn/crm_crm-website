namespace CRM.Criteria.Headline
{
    using Fanex.Data.Repository;

    public class GetAllCriteria : CriteriaBase
    {
        public int? Limit { get; set; }

        public string Username { get; set; }

        public bool? ExcludeNotAssignedGroup { get; set; }

        public override string GetSettingKey() => "Acc_CRM_HeadlineSel";

        public override bool IsValid() => true;
    }
}