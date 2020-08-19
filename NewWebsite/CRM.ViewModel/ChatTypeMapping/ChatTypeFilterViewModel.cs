namespace CRM.ViewModels.ChatTypeMapping
{
    public class ChatTypeFilterViewModel
    {
        public string BotIds { get; set; }

        public int? ChatType { get; set; }

        public string Name { get; set; }

        public string GroupHeadline { get; set; }

        public string RecipientIds { get; set; }

        public string LetstalkId { get; set; }

        public string MappingStatuses { get; set; }

        public string LanguageIds { get; set; }

        public bool? IsActive { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}