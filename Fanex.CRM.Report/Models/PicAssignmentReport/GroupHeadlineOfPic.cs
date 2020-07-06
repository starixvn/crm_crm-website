namespace Fanex.CRM.Report.Models.PicAssignmentReport
{
    using System.Collections.Generic;

    public class GroupHeadlineOfPic
    {
        public int GroupHeadlineId { get; set; }

        public string GroupHeadline { get; set; }

        public string HeadlineType { get; set; }

        public IEnumerable<PicOfDirectAccount> Pics { get; set; }
    }
}