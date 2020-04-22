namespace Fanex.CRM.BackgroundJob.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetNewMessageCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "LastMsgID")]
        public int LastMessageId { get; set; }
        public int BatchSize { get; set; }

        public override string GetSettingKey() => "Acc_CRM_MessageAbnormalTicketsSel";

        public override bool IsValid() => LastMessageId >= 0 && BatchSize > 0;
    }
}