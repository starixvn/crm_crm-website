namespace CRM.Letstalk.Core.Criteria
{
    using CRM.Core.Models;

    public class AddBotToGroupCommand : BaseNonQueryCommand
    {
        public string GroupName { get; set; }

        public string ConversationGroupId { get; set; }

        public int BotId { get; set; }

        public string BotName { get; set; }

        public override string GetSettingKey() => "Acc_CRM_AddBotToGroup_Ins";

        public override bool IsValid() => !string.IsNullOrWhiteSpace(ConversationGroupId) &&
            !string.IsNullOrWhiteSpace(GroupName) &&
            !string.IsNullOrWhiteSpace(BotName) &&
            BotId > 0;
    }
}