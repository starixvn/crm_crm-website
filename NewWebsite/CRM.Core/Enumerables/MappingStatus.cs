using System.ComponentModel;

namespace CRM.Core.Enumerables
{
    public enum MappingStatus
    {
        [Description("Bot Remove")]
        BotRemoved = 0,

        [Description("Not Mapped")]
        NotMapped = 1,

        [Description("Mapped")]
        Mapped = 2
    }
}