namespace Fanex.CRM.Communication.Criteria
{
    using Common.Criteria;

    public class GetDefaultSettingsCriteria : BasedCriteria
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