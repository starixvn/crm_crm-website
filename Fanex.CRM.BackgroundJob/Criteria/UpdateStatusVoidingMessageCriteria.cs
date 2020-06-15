namespace Fanex.CRM.BackgroundJob.Criteria
{
    using Fanex.Data.Repository;

    public class UpdateStatusVoidingMessageCriteria : NonQueryCommand
    {
        public string MessageIds { get; set; }

        public override string GetSettingKey() => "Acc_CRM_VoidingMessageBatchUpd";

        public override bool IsValid() => !string.IsNullOrWhiteSpace(MessageIds);
    }
}