// --------------------------------------------------------------------------
// <copyright file="SearchFeedbackSubjectTemplateCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Criteria.FeedbackSubjectManagement
{
    using System.Collections;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// Search Feedback Subject Template Criteria.
    /// </summary>
    public class SearchFeedbackSubjectTemplateCriteria : PagingCriteria
    {
        public string Subject { get; set; }

        /// <summary>
        /// Gets the SP parameters.
        /// </summary>
        /// <returns>The SP parameters.</returns>
        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("templateid", 0);
            return param;
        }

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
