using System.Collections.Generic;

namespace Fanex.CRM.Models.PicAndHeadline
{
    public class PicInfo
    {
        public int PicId { get; set; }

        public string Username { get; set; }

        public string Role { get; set; }

        public string Language { get; set; }

        public string Country { get; set; }

        public int RoleId { get; set; }

        public string Remark { get; set; }

        public string GroupHeadline { get; set; }

        public string Contact { get; set; }

        public List<CommunicationTool> CommunicationTools { get; set; }

        public bool IsActive { get; set; }
    }
}