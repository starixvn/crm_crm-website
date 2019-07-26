namespace Fanex.CRM.AdminTool.Repositories.FeedbackSubjectManagement
{
    using System.Collections.Generic;
    using Fanex.CRM.AdminTool.Criteria.FeedbackSubjectManagement;
    using Fanex.CRM.AdminTool.Models.FeedbackSubjectManagement;

    public interface IFeedbackSubjectManagementRepository
    {
        IEnumerable<FeedbackSubjectTemplate> SearchFeedbackSubjectTemplate(SearchFeedbackSubjectTemplateCriteria criteria);

        FeedbackSubjectTemplate GetFeedbackSubjectTemplate(FeedbackSubjectTemplateByIdCriteria criteria);
    }
}