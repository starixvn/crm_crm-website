using System;
using Fanex.CRM.Common.Enumerations;
using Fanex.NAP.Core.Models;

namespace Fanex.CRM.Common.Models
{
    [Serializable]
    public class CRMUser : User
    {
        public CRMUser(User napUser)
        {
            this.Id = napUser.Id;
            this.Name = napUser.Name;
            this.Username = napUser.Username;
            this.GroupId = napUser.GroupId;
            this.Password = napUser.Password;
            this.FirstName = napUser.FirstName;
            this.LastName = napUser.LastName;
            this.Email = napUser.Email;
        }
        public UserRole UserRole { get; set; } = UserRole.None;
        public string Language { get; set; }
        public int OnUserId { get; set; }
        public int SiteID { get; set; }
    }
}
