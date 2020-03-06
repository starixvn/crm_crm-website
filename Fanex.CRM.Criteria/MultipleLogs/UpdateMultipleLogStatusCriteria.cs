namespace Fanex.CRM.Criteria.MultipleLogs
{
    using Fanex.CRM.Common.Criteria;
    using Fanex.CRM.Common.Services;
    using Fanex.Data.Repository;

    public class UpdateMultipleLogStatusCriteria : UpdateCriteria
    {
        public string ListFeedbackId { get; set; }

        public int StatusId { get; set; }

        public bool IsReviewed { get; set; }

        [SpParam(SqlParamName = "ReviewNote")]
        public string Review { get; set; }

        public int ReviewerId => IsReviewed ? AppSession.CurrentUser.Id : 0;

        public override string GetSettingKey() => "UpdMultipleReviewAndStatus";

        public override bool IsValid() => true;
    }
}