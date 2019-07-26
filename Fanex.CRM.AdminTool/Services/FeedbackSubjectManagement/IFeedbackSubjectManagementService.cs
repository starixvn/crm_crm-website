namespace Fanex.CRM.AdminTool.Services.FeedbackSubjectManagement
{
    using Fanex.CRM.AdminTool.Criteria.FeedbackSubjectManagement;
    using Fanex.CRM.AdminTool.Models.FeedbackSubjectManagement;
    using Fanex.CRM.Common.Models;

    public interface IFeedbackSubjectManagementService
    {
        FeedbackSubjectTemplateView SearchFeedbackSubjectTemplate(SearchFeedbackSubjectTemplateCriteria criteria);

        CommandResult ChangeTemplateState(ChangeSubjectTemplateStateCriteria criteria);

        FeedbackSubjectTemplate GetFeedbackSubjectTemplate(FeedbackSubjectTemplateByIdCriteria criteria);

        CommandResult EditTemplate(EditFeedbackSubjectTemplateCriteria criteria);

        CommandResult InsertTemplate(InsertFeedbackSubjectTemplateCriteria criteria);
    }
}