namespace Fanex.CRM.AdminTool.Criteria.CommunicationToolSetting
{
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Services;
    using Fanex.Data.Repository;

    public class CreateCommunicationToolSettingCriteria : CriteriaBase
    {
        public int UserId => AppSession.CurrentUser.Id;
        public string IP => IPHelper.ClientIP;

        public string Username { get; set; }

        [SpParam(SqlParamName = "Category")]
        public string CategoryIds { get; set; }

        [SpParam(SqlParamName = "SubCategory")]
        public string SubCategoryIds { get; set; }

        [SpParam(SqlParamName = "Comm")]
        public string CommunicationIds { get; set; }

        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Acc_CRM_crmDefaultCommToolIns";

        public override bool IsValid() => true;
    }
}