using System.Collections.Generic;

namespace Fanex.CRM.Models.PicAndHeadline
{
    public class EditPicModel
    {
        public int PicId { get; set; }

        public string Username { get; set; }

        public int RoleId { get; set; }

        public IEnumerable<CommunicationTool> CommunicationTools { get; set; }

        public string Remark { get; set; }
    }
}