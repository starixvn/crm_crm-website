using Fanex.Data.Repository;

namespace Fanex.CRM.Criteria.PicAndHeadline
{
    public class GetSystemSiteCriteria : CriteriaBase
    {
        public bool ExcludeCN88Site { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CRM_SystemSiteSel";
        }

        public override bool IsValid() => true;
    }
}