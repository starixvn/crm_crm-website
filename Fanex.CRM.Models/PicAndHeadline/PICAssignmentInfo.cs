using System.Collections.Generic;

namespace Fanex.CRM.Models.PicAndHeadline
{
    public class PICAssignmentInfo
    {
        public int HeadlineID { get; set; }

        public string HeadlineName { get; set; }

        public string HeadlineType { get; set; }

        public int DirectAccountID { get; set; }

        public string DirectAccount { get; set; }

        public string Status { get; set; }

        public string Site { get; set; }

        public string Currency { get; set; }

        public string Position { get; set; }

        public string PIC { get; set; }

        public IList<PicModel> PicList { get; set; }

        public string PicForMapping { get; set; }

        public int RowNum { get; set; }
    }
}