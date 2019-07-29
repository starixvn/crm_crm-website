namespace Fanex.CRM.Setting.Models
{
    using Fanex.CRM.Common.Models;

    public class SiteSetting : BaseModel
    {
        public string AliasName { get; set; }

        public bool IsActive { get; set; }
    }
}