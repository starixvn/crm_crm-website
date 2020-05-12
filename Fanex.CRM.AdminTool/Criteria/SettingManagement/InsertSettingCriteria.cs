namespace Fanex.CRM.AdminTool.Criteria.SettingManagement
{
    using System.Collections;
    using Common.Criteria;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.Data.Repository;

    public class InsertSettingCriteria : BasedCriteria
    {
        public Setting NewSetting { get; set; }

        [SpParam(SqlParamName = "creatorid")]
        public int CreatorId
        {
            get
            {
                return AppSession.CurrentUser.Id;
            }
        }

        [SpParam(SqlParamName = "userid")]
        public int UserId
        {
            get
            {
                return AppSession.CurrentUser.Id;
            }
        }

        [SpParam(SqlParamName = "ip")]
        public string IP
        {
            get
            {
                return IPHelper.ClientIP;
            }
        }

        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("settingname", NewSetting.SettingType);
            param.Add("settingvalue", NewSetting.Value);
            param.Add("displayorder", NewSetting.DisplayOrder);
            param.Add("parentid", NewSetting.ParentId);
            param.Add("settingSource", NewSetting.SettingSource);

            param.Add("userid", UserId);
            param.Add("ip", IP);
            param.Add("UserRole", UserRole);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            return param;
        }

        public override string GetSettingKey()
        {
            return "Acc_CRM_SettingIns";
        }

        public override bool IsValid() => true;
    }
}