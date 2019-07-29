namespace Fanex.CRM.Common.Models
{
    using System;

    [Serializable]
    public class UserInfo : BaseModel
    {
        public int RoleId { get; set; }

        public string Password { get; set; }
    }
}