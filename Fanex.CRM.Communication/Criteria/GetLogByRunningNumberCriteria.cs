namespace Fanex.CRM.Communication.Criteria
{
    using Fanex.Data.Repository;

    public class GetLogByRunningNumberCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "feedbackid")]
        public int RunningNumber { get; set; }

        public override string GetSettingKey()
        {
            return "Communication_Log_GetInfoById";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
