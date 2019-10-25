namespace Fanex.CRM.AdminTool.Criteria.CommunicationToolSetting
{
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Services;
    using Fanex.Data.Repository;

    public class DeleteCommunicationToolSettingCriteria : CriteriaBase
    {
        public static int UserId => AppSession.CurrentUser.Id;
        public static string IP => IPHelper.ClientIP;
        public int DefaultCommId { get; set; }

        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Acc_CRM_crmDefaultCommToolDel";

        public override bool IsValid() => true;
    }
}