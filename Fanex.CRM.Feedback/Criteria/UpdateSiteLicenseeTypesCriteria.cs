using Fanex.CRM.Common.Criteria;

namespace Fanex.CRM.Feedback.Criteria
{
    public class UpdateSiteLicenseeTypesCriteria : UpdateCriteria
    {
        public string ListFeedbackID { get; set; }

        public override string GetSettingKey()
        {
            return "UpdateSiteLicenseeTypes";
        }
    }
}
