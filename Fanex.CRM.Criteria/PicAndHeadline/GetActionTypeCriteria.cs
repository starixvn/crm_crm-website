namespace Fanex.CRM.Criteria.PicAndHeadline
{
    using Data.Repository;

    public class GetActionTypeCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Acc_CRM_ActionTypeSel";

        public override bool IsValid() => true;
    }
}