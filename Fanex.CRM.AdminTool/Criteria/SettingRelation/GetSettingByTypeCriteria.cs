namespace Fanex.CRM.AdminTool.Criteria.SettingRelation
{
    using System.Collections;
    using Common.Criteria;
    using Fanex.CRM.Common.Models;

    public class GetSettingByTypeCriteria : BasedCriteria
    {
        public Setting MainSetting { get; set; }

        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("UserRole", UserRole);
            return param;
        }

        public override string GetSettingKey()
        {
            return "Setting_FeedbackTemplateSubject_Get";
        }

        public override bool IsValid() => true;
    }
}