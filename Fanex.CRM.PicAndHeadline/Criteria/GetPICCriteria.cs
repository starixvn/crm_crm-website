using Fanex.Data.Repository;

namespace Fanex.CRM.PicAndHeadline.Criteria
{
    public class GetPICCriteria : CriteriaBase
    {
        public string UserName { get; set; }
        public bool IsActive { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CRM_PicSel";
        }

        public override bool IsValid() => true;
    }
}