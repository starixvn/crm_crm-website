namespace Fanex.CRM.InformHeadline.Criteria
{
    using Fanex.Data.Repository;

    public class GetNewInformsCriteria : CriteriaBase
    {
        [SpParam(Ignored = true)]
        public string Title { get; set; }

        public int TypeAccount { get; set; }

        public string Currency { get; set; }

        public string Level { get; set; }

        public string Site { get; set; }

        [SpParam(SqlParamName = "ignored")]
        public bool ExcludeIgnoredAccounts { get; set; }

        public bool IsVip { get; set; }

        public int PageIndex { get; set; }

        public int ItemsPerPage { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CRM_InformHeadlineSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}