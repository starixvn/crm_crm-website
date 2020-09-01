namespace CRM.Letstalk.Core.Criteria
{
    using CRM.Core.Models;

    public class RemoveBotFromGroupCommand : BaseNonQueryCommand
    {
        public int BotId { get; set; }

        public string ConversationId { get; set; }

        public override string GetSettingKey() => "Acc_CRM_ChatBot_KickOutBotFromGroup_Del";

        public override bool IsValid() => BotId > 0 && !string.IsNullOrWhiteSpace(ConversationId);
    }
}