namespace Fanex.CRM.AdminTool.Criteria.SettingManagement
{
    using System.Collections;
    using Fanex.CRM.Common.Criteria;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.Data.Repository;

    public class EditSettingCriteria : BasedUpdateCriteria
    {
        public Setting OldSetting { get; set; }

        public Setting UpdatedSetting { get; set; }

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
            param.Add("settingname", UpdatedSetting.SettingType);
            param.Add("settingvalue", UpdatedSetting.Value);
            param.Add("displayorder", UpdatedSetting.DisplayOrder);
            param.Add("settingid", UpdatedSetting.Id);

            param.Add("userid", UserId);
            param.Add("ip", IP);
            param.Add("UserRole", UserRole);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            return param;
        }

        public override string GetSettingKey()
        {
            return "Acc_CRM_SettingUpd";
        }

        public override bool IsValid() => true;
    }
}