// --------------------------------------------------------------------------
// <copyright file="DeleteFeedbackSubjectTemplateCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Delete Feedback Subject Template Criteria.
    /// </summary>
    public class DeleteFeedbackSubjectTemplateCriteria : CriteriaBase
    {
        public int TemplateId { get; set; }

        public bool IsActive { get; set; }

        /// <summary>
        /// Gets the SP parameters.
        /// </summary>
        /// <returns>The SP parameters.</returns>
        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("templateid", TemplateId);
            param.Add("isactive", IsActive);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            return param;
        }

        public override string GetSettingKey()
        {
            return "FeedSubjectTemplateDelete";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
