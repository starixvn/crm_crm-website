namespace Fanex.CRM.AdminTool.Criteria.SettingRelation
{
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.Data.Repository;

    public class AddSettingRelationsCriteria : CriteriaBase
    {
        public int ParentId { get; set; }

        public string SettingName { get; set; }

        public List<Setting> SubSettings { get; set; }

        public int CreatorId => AppSession.CurrentUser.Id;

        public int UserId => AppSession.CurrentUser.Id;

        public string IP => IPHelper.ClientIP;

        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("settingId", GetSettingId());
            param.Add("settingName", SettingName);
            param.Add("parentId", ParentId);

            param.Add("userid", UserId);
            param.Add("ip", IP);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            return param;
        }

        public override string GetSettingKey()
        {
            return "SettingRelationUpd";
        }

        public override bool IsValid()
        {
            return true;
        }

        private string GetSettingId()
        {
            if (SubSettings == null)
            {
                return string.Empty;
            }

            var settingId = new List<string>();
            var subSettingLength = SubSettings.Count;
            for (var i = 0; i < subSettingLength; i++)
            {
                var subSetting = SubSettings[i];
                if (subSetting.IsChild)
                {
                    settingId.Add(string.Format("{0}_{1}", subSetting.Id, subSetting.DisplayOrder));
                }
            }

            return string.Join("|", settingId);
        }
    }
}