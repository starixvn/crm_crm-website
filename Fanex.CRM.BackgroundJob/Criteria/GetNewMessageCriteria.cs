namespace Fanex.CRM.BackgroundJob.Criteria
{
    using Fanex.Data.Repository;

    public class GetNewMessageCriteria : CriteriaBase
    {
        public long LastMessageId { get; set; }
        public int BatchSize { get; set; }

        public override string GetSettingKey() => "";

        public override bool IsValid() => BatchSize > 0;
    }
}