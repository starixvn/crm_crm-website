using Fanex.Data.Repository;

namespace Fanex.CRM.PicAndHeadline.Criteria
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