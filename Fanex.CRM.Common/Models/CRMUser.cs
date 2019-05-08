using Fanex.CRM.Common.Enumerations;
using Fanex.NAP.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.CRM.Common.Models
{
    public class CRMUser : User
    {
        public UserRole UserRole { get; set; } = UserRole.None;
        public int OnUserId { get; set; }
    }
}
