namespace Fanex.CRM.Redemption.Criteria
{
    using Fanex.Data.Repository;

    public class GetLogByIdCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "id")]
        public int Id { get; set; }

        public override string GetSettingKey()
        {
            return "Redemption_Log_GetInfoById";
        }

        public override bool IsValid() => true;
    }
}