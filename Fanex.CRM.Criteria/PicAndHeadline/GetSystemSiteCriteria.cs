using Fanex.Data.Repository;

namespace Fanex.CRM.Criteria.PicAndHeadline
{
    public class GetSystemSiteCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Acc_SiteSel_Credit";
        }

        public override bool IsValid() => true;
    }
}