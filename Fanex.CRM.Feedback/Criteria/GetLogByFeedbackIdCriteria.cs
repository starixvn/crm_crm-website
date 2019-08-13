namespace Fanex.CRM.Feedback.Criteria
{
    using Common.Criteria;
    using Fanex.Data.Repository;

    public class GetLogByFeedbackIdCriteria : BasedCriteria
    {
        [SpParam(SqlParamName = "feedbackid")]
        public int FeedbackId { get; set; }

        public override string GetSettingKey()
        {
            return "Communication_Log_GetInfoById";
        }

        public override bool IsValid() => true;
    }
}