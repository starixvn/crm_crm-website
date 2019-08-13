namespace Fanex.CRM.AdminTool.Criteria.SettingManagement
{
    using System.Collections;
    using Fanex.CRM.Common.Criteria;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.Data.Repository;

    public class EditSettingCN88Criteria : UpdateCriteria
    {
        public SettingCN88 OldSetting { get; set; }

        public SettingCN88 UpdatedSetting { get; set; }

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
            param.Add("settingid", UpdatedSetting.SettingId);
            param.Add("settingvalue", UpdatedSetting.SettingValue);
            param.Add("settingvaluecs", UpdatedSetting.SettingValueCS);
            param.Add("settingvaluect", UpdatedSetting.SettingValueCT);
            param.Add("displayorder", UpdatedSetting.DisplayOrder);

            param.Add("userid", UserId);
            param.Add("ip", IP);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            return param;
        }

        public override string GetSettingKey()
        {
            return "Acc_CRM_CN88_SettingUpd";
        }

        public override bool IsValid() => true;
    }
}