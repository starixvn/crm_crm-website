namespace Fanex.CRM.BackgroundJob.Criteria
{
    using Fanex.Data.Repository;

    public class UpdateFeedbackInfoCriteria : NonQueryCommand
    {
        public int MessageId { get; set; }
        public string CustIds { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }

        public override string GetSettingKey() => "Acc_CRM_AutoFeedbackInfoUpd";

        public override bool IsValid() => true;
    }
}