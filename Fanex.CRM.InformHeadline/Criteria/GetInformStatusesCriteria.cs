namespace Fanex.CRM.InformHeadline.Criteria
{
    using Common.Criteria;
    using Fanex.Data.Repository;

    public class GetInformStatusesCriteria : BasedCriteria
    {
        [SpParam(SqlParamName = "Mode")]
        public bool IsIncludeKeptForRefence { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CRM_InformHeadlineStatusSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}