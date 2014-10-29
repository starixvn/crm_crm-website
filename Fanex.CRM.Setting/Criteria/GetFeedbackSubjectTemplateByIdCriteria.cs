// --------------------------------------------------------------------------
// <copyright file="GetFeedbackSubjectTemplateByIdCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Setting.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Feedback Subject Template By Identify Criteria.
    /// </summary>
    public class GetFeedbackSubjectTemplateByIdCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "templateid")]
        public int TemplateId { get; set; }

        public override string GetSettingKey()
        {
            return "Setting_FeedbackTemplateSubject_Get";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
