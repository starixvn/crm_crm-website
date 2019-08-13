namespace Fanex.CRM.AdminTool.Criteria.SettingRelation
{
    using System.Collections;
    using Common.Criteria;
    using Fanex.CRM.Common.Models;

    public class GetMainSettingRelationsCriteria : BasedLanguageCriteria
    {
        public int ParentId { get; set; }

        public string ParentName { get; set; }

        public Setting MainSetting { get; set; }

        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("settingname", MainSetting.SettingType);
            param.Add("parentid", ParentId);
            param.Add("UserRole", UserRole);
            return param;
        }

        public override string GetSettingKey()
        {
            return "Acc_CRM_SettingGetRelationBySettingName";
        }

        public override bool IsValid() => true;
    }
}