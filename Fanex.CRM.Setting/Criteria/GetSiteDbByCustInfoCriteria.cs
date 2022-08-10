
namespace Fanex.CRM.Setting.Criteria
{
    using Fanex.Data.Repository;

    public class GetSiteDbByCustInfoCriteria : CriteriaBase
    {
        public int CustId { get; set; }
        public bool IsGuest { get; set; }
        public override string GetSettingKey()
        {
            return "Acc_CRM_CustomerGetSite";
        }

        public override bool IsValid() => true;
    }
}
