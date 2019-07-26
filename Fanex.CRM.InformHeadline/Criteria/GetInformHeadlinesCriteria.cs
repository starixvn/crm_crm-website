namespace Fanex.CRM.InformHeadline.Criteria
{
    using Fanex.Data.Repository;

    public class GetInformHeadlinesCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Acc_CRM_InformHeadlineList";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}