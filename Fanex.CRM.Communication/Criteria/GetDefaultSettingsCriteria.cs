namespace Fanex.CRM.Communication.Criteria
{
    using Data.Repository;

    public class GetDefaultSettingsCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "Communication_Log_GetDefaultSettings";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}
