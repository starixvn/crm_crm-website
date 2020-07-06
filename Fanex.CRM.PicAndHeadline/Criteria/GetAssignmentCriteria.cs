using Fanex.Data.Repository;

namespace Fanex.CRM.PicAndHeadline.Criteria
{
    public class GetAssignmentCriteria : CriteriaBase
    {
        public string SiteID { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public string HeadlineType { get; set; }
        public string PIC { get; set; }
        public string DirectAccount { get; set; }
        public string GroupHeadline { get; set; }
        public string Position { get; set; }
        public bool ExcludeIgnoredAcc { get; set; }
        public bool ExcludeIsTest { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int TotalRows { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CRM_PICAssignmentSel";
        }

        public override bool IsValid() => true;
    }
}