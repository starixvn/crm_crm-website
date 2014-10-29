// --------------------------------------------------------------------------
// <copyright file="InsertFeedbackSubjectTemplateCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Criteria.FeedbackSubjectManagement
{
    using System.Collections;
    using Fanex.Data.Repository;

    /// <summary>
    /// Insert Feedback Subject Template Criteria.
    /// </summary>
    public class InsertFeedbackSubjectTemplateCriteria : CriteriaBase
    {
        public string TemplateContent { get; set; }

        /// <summary>
        /// Gets the SP parameters.
        /// </summary>
        /// <returns>The SP parameters.</returns>
        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("subject", TemplateContent);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            return param;
        }

        public override string GetSettingKey()
        {
            return "FeedSubjectTemplateInsert";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
