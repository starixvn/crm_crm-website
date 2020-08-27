namespace CRM.Criteria.ChatTypeMapping
{
    using Fanex.Data.Repository;

    public class GetChatTypeMappingCriteria : CriteriaBase
    {
        public int? ChatTypeId { get; set; }

        public string ConversationGroupName { get; set; }

        public string Headline { get; set; }

        public string RecipientName { get; set; }

        public string LetstalkId { get; set; }

        public int? LanguageId { get; set; }

        public bool? IsActive { get; set; }

        public int? BotId { get; set; }

        public string MappingStatus { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalRows { get; set; }

        public override string GetSettingKey() => "Acc_CRM_ChatBot_ChatTypeMapping_Sel";

        public override bool IsValid() => !string.IsNullOrWhiteSpace(MappingStatus) && PageIndex > 0 && PageSize > 0;
    }
}