namespace Fanex.CRM.Setting.Criteria
{
    using Fanex.Data.Repository;

    public class GetAllStatusInformHeadlineCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Customer_InformHeadline_StatusList";
        }

        public override bool IsValid() => true;
    }
}