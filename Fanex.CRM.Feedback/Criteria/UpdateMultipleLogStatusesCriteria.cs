using Fanex.CRM.Common.Criteria;
using Fanex.CRM.Common.Services;
using Fanex.Data.Repository;

namespace Fanex.CRM.Feedback.Criteria
{
    public class UpdateMultipleLogStatusesCriteria : UpdateCriteria
    {
        [SpParam(SqlParamName = "listfeedbackid")]
        public string ListFeedbackId { get; set; }


        [SpParam(SqlParamName = "statusid")]
        public int StatusId { get; set; }

        public bool IsReviewed { get; set; }

        [SpParam(SqlParamName = "ReviewNote")]
        public string Review { get; set; }

        public int ReviewerId
        {
            get
            {
                return IsReviewed ? AppSession.CurrentUser.Id : 0;
            }
        }

        public override string GetSettingKey()
        {
            return "UpdMultipleReviewAndStatus";
        }
    }
}
