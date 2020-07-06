using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.CRM.PicAndHeadline.Models
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
