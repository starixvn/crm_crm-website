namespace CRM.Letstalk.Core.Criteria
{
    using CRM.Core.Models;

    public class AddBotToGroupCommand : BaseNonQueryCommand
    {
        public string GroupName { get; set; }

        public string ConversationId { get; set; }

        public int BotId { get; set; }

        public string BotName { get; set; }

        public override string GetSettingKey() => "Acc_CRM_ChatBot_AddBotToGroup_Ins";

        public override bool IsValid() => !string.IsNullOrWhiteSpace(ConversationId) &&
            !string.IsNullOrWhiteSpace(GroupName) &&
            !string.IsNullOrWhiteSpace(BotName) &&
            BotId > 0;
    }
}