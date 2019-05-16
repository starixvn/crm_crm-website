// --------------------------------------------------------------------------
// <copyright file="FeedbackSubjectTemplateByIdCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Criteria.FeedbackSubjectManagement
{
    using Common.Criteria;
    using System.Collections;

    /// <summary>
    /// Feedback Subject Template By Identify Criteria.
    /// </summary>
    public class FeedbackSubjectTemplateByIdCriteria : BasedCriteria
    {
        public int TemplateId { get; set; }

        /// <summary>
        /// Gets the SP parameters.
        /// </summary>
        /// <returns>The SP parameters.</returns>
        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("templateid", TemplateId);
            param.Add("UserRole", UserRole);
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
