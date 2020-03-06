namespace Fanex.CRM.Criteria.MultipleLogs
{
    using Fanex.CRM.Common.Criteria;

    public class GetLogByFeedbackIdCriteria : BasedCriteria
    {
        public int FeedbackId { get; set; }

        public override string GetSettingKey() => "Communication_Log_GetInfoById";

        public override bool IsValid() => true;
    }
}