namespace Fanex.CRM.Feedback.Criteria
{
    using Fanex.Data.Repository;

    public class GetFeedbackByIdCriteria : CriteriaBase
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
