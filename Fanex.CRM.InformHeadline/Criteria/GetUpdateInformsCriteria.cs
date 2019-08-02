namespace Fanex.CRM.InformHeadline.Criteria
{
    using Fanex.Data.Repository;

    public class GetUpdateInformsCriteria : CriteriaBase
    {
        public int TitleId { get; set; }

        public int TypeAccount { get; set; }

        public string Currency { get; set; }

        public string Level { get; set; }

        public string Site { get; set; }

        public int IsVip { get; set; }

        [SpParam(SqlParamName = "ignored")]
        public bool ExcludeIgnoredAccounts { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CRM_InformHeadlineSearchSel";
        }

        public override bool IsValid()
        {
            return TitleId > 0;
        }
    }
}