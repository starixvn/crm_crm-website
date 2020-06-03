namespace Fanex.CRM.Criteria.PicAndHeadline
{
    using Fanex.Data.Repository;

    public class GetPicCommunicationToolsCriteria : CriteriaBase
    {
        public static string SettingName => "PICCommunicationTool";

        public override string GetSettingKey()
        {
            return "Acc_CRM_SettingSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}