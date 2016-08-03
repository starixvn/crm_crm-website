// --------------------------------------------------------------------------
// <copyright file="IFeedbackSubjectManagementService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Services.FeedbackSubjectManagement
{
    using Fanex.CRM.AdminTool.Criteria.FeedbackSubjectManagement;
    using Fanex.CRM.AdminTool.Models.FeedbackSubjectManagement;
    using Fanex.CRM.Infrastructure.Models;

    public interface IFeedbackSubjectManagementService
    {
        FeedbackSubjectTemplateView SearchFeedbackSubjectTemplate(SearchFeedbackSubjectTemplateCriteria criteria);

        Result ChangeTemplateState(ChangeSubjectTemplateStateCriteria criteria);

        FeedbackSubjectTemplate GetFeedbackSubjectTemplate(FeedbackSubjectTemplateByIdCriteria criteria);

        Result EditTemplate(EditFeedbackSubjectTemplateCriteria criteria);

        Result InsertTemplate(InsertFeedbackSubjectTemplateCriteria criteria);
    }
}
