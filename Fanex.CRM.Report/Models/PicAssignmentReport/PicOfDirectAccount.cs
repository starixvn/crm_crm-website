namespace Fanex.CRM.Report.Models.PicAssignmentReport
{
    using System.Collections.Generic;
    using Fanex.CRM.Models.PicAndHeadline;

    public class PicOfDirectAccount
    {
        public IEnumerable<PicModel> Pic { get; set; }

        public IEnumerable<DirectAccountInfo> DirectAccounts { get; set; }
    }
}