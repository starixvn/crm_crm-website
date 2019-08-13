namespace Fanex.CRM.Communication.Criteria
{
    using Common.Criteria;
    using Fanex.Data.Repository;

    public class GetLogByRunningNumberCriteria : BasedCriteria
    {
        [SpParam(SqlParamName = "feedbackid")]
        public int RunningNumber { get; set; }

        public override string GetSettingKey()
        {
            return "Communication_Log_GetInfoById";
        }

        public override bool IsValid() => true;
    }
}