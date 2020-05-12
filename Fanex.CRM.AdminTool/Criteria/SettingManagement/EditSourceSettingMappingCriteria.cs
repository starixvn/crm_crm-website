namespace Fanex.CRM.AdminTool.Criteria.SettingManagement
{
    using System.Collections;
    using Fanex.CRM.Common.Criteria;

    public class EditSourceSettingMappingCriteria : UpdateCriteria
    {
        public int SettingID { get; set; }

        public string SettingSource { get; set; }

        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("settingid", SettingID);
            param.Add("settingsource", SettingSource);

            param.Add("userid", UserId);
            param.Add("ip", IP);
            return param;
        }

        public override string GetSettingKey()
        {
            return "Acc_CRM_SourceSettingMappingUpd";
        }

        public override bool IsValid() => true;
    }
}