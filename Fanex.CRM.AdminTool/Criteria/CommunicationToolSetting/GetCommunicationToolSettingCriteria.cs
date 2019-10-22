namespace Fanex.CRM.AdminTool.Criteria.CommunicationToolSetting
{
    using Fanex.Data.Repository;

    public class GetCommunicationToolSettingCriteria : CriteriaBase
    {
        public string Username { get; set; }

        [SpParam(SqlParamName = "Category")]
        public string CategoryIds { get; set; }

        [SpParam(SqlParamName = "SubCategory")]
        public string SubCategoryIds { get; set; }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRows { get; set; }

        public override string GetSettingKey() => "Acc_CRM_crmDefaultCommToolSearch";

        public override bool IsValid() => true;
    }
}