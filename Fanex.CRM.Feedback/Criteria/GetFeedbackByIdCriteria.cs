namespace Fanex.CRM.Feedback.Criteria
{
    using Common.Criteria;
    using Fanex.Data.Repository;

    public class GetFeedbackByIdCriteria : BasedCriteria
    {
        [SpParam(SqlParamName = "FeedbackID")]
        public int Id { get; set; }

        public override string GetSettingKey()
        {
            return "FeedbackSelByID";
        }

        public override bool IsValid()
        {
            return Id > 0;
        }
    }
}
