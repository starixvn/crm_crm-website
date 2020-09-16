namespace CRM.Letstalk.Core.Criteria
{
    using CRM.Core.Models;

    public class UpdateGroupNameCommand : BaseNonQueryCommand
    {
        public string ConversationId { get; set; }

        public string DisplayName { get; set; }

        public override string GetSettingKey() => "Acc_CRM_ChatBot_ChangeGroupName_Upd";

        public override bool IsValid() => !string.IsNullOrWhiteSpace(ConversationId) &&
            !string.IsNullOrWhiteSpace(DisplayName);
    }
}