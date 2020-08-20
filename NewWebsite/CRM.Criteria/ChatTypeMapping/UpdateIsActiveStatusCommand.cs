namespace CRM.Criteria.ChatTypeMapping
{
    using CRM.Core.Models;

    public class UpdateIsActiveStatusCommand : BaseNonQueryCommand
    {
        public int ConversationGroupId { get; set; }

        public bool IsActive { get; set; }

        public int UserId { get; set; }

        public override string GetSettingKey() => "Acc_CRM_ChatBot_ChatTypeMappingStatus_Upd";

        public override bool IsValid() => ConversationGroupId > 0 && UserId > 0;
    }
}