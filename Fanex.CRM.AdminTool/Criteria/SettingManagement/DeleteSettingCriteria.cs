namespace Fanex.CRM.AdminTool.Criteria.SettingManagement
{
    using System.Collections;
    using Fanex.CRM.Common.Criteria;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.Data.Repository;

    public class DeleteSettingCriteria : BasedUpdateCriteria
    {
        public Setting OldSetting { get; set; }

        [SpParam(SqlParamName = "creatorid")]
        public int CreatorId
        {
            get
            {
                return AppSession.CurrentUser.Id;
            }
        }

        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("settingId", OldSetting.Id);

            param.Add("userid", UserId);
            param.Add("ip", IP);
            param.Add("UserRole", UserRole);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            param.Add("isactive", !OldSetting.IsActive);
            return param;
        }

        public override string GetSettingKey()
        {
            return "Acc_CRM_SettingDel";
        }

        public override bool IsValid() => true;
    }
}