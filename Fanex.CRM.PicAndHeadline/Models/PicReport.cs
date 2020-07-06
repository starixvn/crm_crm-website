using System.Collections.Generic;

namespace Fanex.CRM.PicAndHeadline.Models
{
    public class PicReport
    {
        public IList<PicInfo> DataList { get; set; }

        public int Total { get; set; }
    }
}
