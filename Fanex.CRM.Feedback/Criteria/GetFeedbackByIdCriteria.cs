// --------------------------------------------------------------------------
// <copyright file="GetFeedbackByIdCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.10.20">Created</change>
// </history>

namespace Fanex.CRM.Feedback.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Feedback by ID criteria.
    /// </summary>
    public class GetFeedbackByIdCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "FeedbackID")]
        public int Id { get; set; }

        public override string GetSettingKey()
        {
            return "FeedbackSelByID";
        }

        public override bool IsValid()
        {
            return Id > 0;
        }
    }
}
