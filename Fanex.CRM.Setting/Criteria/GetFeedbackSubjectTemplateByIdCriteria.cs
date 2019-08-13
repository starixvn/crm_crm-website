namespace Fanex.CRM.Setting.Criteria
{
    using Common.Criteria;
    using Fanex.Data.Repository;

    public class GetFeedbackSubjectTemplateByIdCriteria : BasedCriteria
    {
        [SpParam(SqlParamName = "templateid")]
        public int TemplateId { get; set; }

        public override string GetSettingKey()
        {
            return "Setting_FeedbackTemplateSubject_Get";
        }

        public override bool IsValid() => true;
    }
}