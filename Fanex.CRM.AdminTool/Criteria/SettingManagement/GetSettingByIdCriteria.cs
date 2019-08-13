namespace Fanex.CRM.AdminTool.Criteria.SettingManagement
{
    using Fanex.Data.Repository;

    public class GetSettingByIdCriteria : CriteriaBase
    {
        public int SettingId { get; set; }

        public override bool IsValid()
        {
            return true;
        }
    }
}