namespace Fanex.CRM.Setting.Criteria
{
    using System.Collections;
    using Common.Criteria;
    using Fanex.CRM.Setting.Constants;

    public class GetActionCriteria : BasedCriteria
    {
        public int ActionType { get; set; }

        public override Hashtable GetSpParams()
        {
            if (ActionType == SettingConstants.ActivityLog)
            {
                var hastableParam = new Hashtable();
                hastableParam.Add("userrole", UserRole);

                return hastableParam;
            }

            return null;
        }

        public override string GetSettingKey()
        {
            var settingKey = string.Empty;
            switch (ActionType)
            {
                case SettingConstants.ActivityLog:
                    settingKey = "Acc_CRM_ActivityLogList";
                    break;

                case SettingConstants.SettingLog:
                    settingKey = "Acc_CRM_SettingLogList";
                    break;

                default:
                    // nothing
                    break;
            }

            return settingKey;
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}