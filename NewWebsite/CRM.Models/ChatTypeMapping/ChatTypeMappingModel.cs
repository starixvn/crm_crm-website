using System.Collections.Generic;
using CRM.Core.Enumerables;
using CRM.Core.Extensions;

namespace CRM.Models.ChatTypeMapping
{
    public class ChatTypeMappingModel
    {
        public int ChatTypeId { get; set; }

        public string ChatType
        {
            get
            {
                return EnumHelper.GetDesciption((ChatType)ChatTypeId);
            }
        }

        public int BotId { get; set; }

        public string BotName { get; set; }

        public int ConversationGroupId { get; set; }

        public string ConversationGroupName { get; set; }

        public int RecipientTypeId { get; set; }

        public string RecipientTypeName { get; set; }

        public string GroupHeadline { get; set; }

        public int RecipientId { get; set; }

        public string RecipientName { get; set; }

        public string LetstalkId { get; set; }

        public IEnumerable<ConversationRecipient> Recipients { get; set; }

        public IEnumerable<ConversationHeadlineModel> Headlines { get; set; }

        public int MappingStatusId { get; set; }

        public string MappingStatus
        {
            get
            {
                return EnumHelper.GetDesciption((MappingStatus)MappingStatusId);
            }
        }

        public int LanguageId { get; set; }

        public string Language { get; set; }

        public bool IsActive { get; set; }

        public string Remark { get; set; }
    }
}