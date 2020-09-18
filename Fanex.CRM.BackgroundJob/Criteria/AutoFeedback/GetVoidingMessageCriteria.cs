namespace Fanex.CRM.BackgroundJob.Criteria
{
    using Fanex.Data.Repository;

    public class GetVoidingMessageCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Acc_CRM_VoidingMessageSel";

        public override bool IsValid() => true;
    }
}