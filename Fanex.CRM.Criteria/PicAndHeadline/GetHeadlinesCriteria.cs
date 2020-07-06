using Fanex.Data.Repository;

namespace Fanex.CRM.Criteria.PicAndHeadline
{
    public class GetHeadlinesCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CRM_HeadlineSel";
        }

        public override bool IsValid() => true;
    }
}