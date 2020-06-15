namespace Fanex.CRM.BackgroundJob.Criteria
{
    using Fanex.Data.Repository;

    public class StoreVoidingMessageCriteria : CriteriaBase
    {
        public string MessageIds { get; set; }
        public int BatchSize { get; set; }

        public override string GetSettingKey() => "Acc_CRM_VoidingMessageIns";

        public override bool IsValid() => !string.IsNullOrWhiteSpace(MessageIds) && BatchSize > 0;
    }
}