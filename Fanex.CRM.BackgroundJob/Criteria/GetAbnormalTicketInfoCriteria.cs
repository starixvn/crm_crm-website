namespace Fanex.CRM.BackgroundJob.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetAbnormalTicketInfoCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "LastMsgID")]
        public int LastMessageId { get; set; }

        public int BatchSize { get; set; }

        [SpParam(SqlParamName = "FailMsgIDs")]
        public string FailMessageIds { get; set; }

        public bool ConfigUAT { get; set; }
        public bool ConfigPRO { get; set; }

        public override string GetSettingKey() => "Acc_CRM_AbnormalTicketsSel";

        public override bool IsValid() => LastMessageId >= 0 && BatchSize > 0;
    }
}