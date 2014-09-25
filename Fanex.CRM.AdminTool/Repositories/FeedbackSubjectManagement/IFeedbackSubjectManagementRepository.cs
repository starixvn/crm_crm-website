// --------------------------------------------------------------------------
// <copyright file="IFeedbackSubjectManagementRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Repositories.FeedbackSubjectManagement
{
    using System.Collections.Generic;
    using Fanex.CRM.AdminTool.Criteria.FeedbackSubjectManagement;
    using Fanex.CRM.AdminTool.Models.FeedbackSubjectManagement;
    using Fanex.CRM.Infrastructure.Models;

    public interface IFeedbackSubjectManagementRepository
    {
        List<FeedbackSubjectTemplateInfo> SearchFeedbackSubjectTemplate(SearchFeedbackSubjectTemplateCriteria criteria);

        Result DeleteTemplate(DeleteFeedbackSubjectTemplateCriteria criteria);

        FeedbackSubjectTemplateInfo FeedbackSubjectTemplateById(FeedbackSubjectTemplateByIdCriteria criteria);

        Result EditTemplate(EditFeedbackSubjectTemplateCriteria criteria);

        Result InsertTemplate(InsertFeedbackSubjectTemplateCriteria criteria);
    }
}
