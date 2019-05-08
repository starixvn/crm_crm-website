using Fanex.CRM.Common.Enumerations;
using Fanex.NAP.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.CRM.Common.Models
{
    [Serializable]
    public class CRMUser : User
    {
        public CRMUser(User napUser)
        {
            this.Id = napUser.Id;
            this.Name = napUser.Name;
            this.Group = napUser.Group;
            this.Password = napUser.Password;
            this.LastLoginDate = napUser.LastLoginDate;
            this.Creator = napUser.Creator;
            this.CreatedDate = napUser.CreatedDate;
            this.ModifiedDate = napUser.ModifiedDate;
            this.FirstName = napUser.FirstName;
            this.LastName = napUser.LastName;
            this.Email = napUser.Email;
        }
        public UserRole UserRole { get; set; } = UserRole.None;
        public int OnUserId { get; set; }
        public int SiteID { get; set; }
    }
}
