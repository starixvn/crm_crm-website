namespace Fanex.CRM.Setting.Criteria
{
    using Common.Criteria;

    public class GetStationsCriteria : BasedCriteria
    {
        public override string GetSettingKey()
        {
            return "Acc_CRM_StationList";
        }

        public override bool IsValid() => true;
    }
}