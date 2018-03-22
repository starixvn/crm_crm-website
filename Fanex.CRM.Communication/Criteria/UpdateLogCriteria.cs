// --------------------------------------------------------------------------
// <copyright file="UpdateLogCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Communication.Criteria
{
    using Fanex.CRM.Common.Services;
    using Fanex.Data.Repository;

    /// <summary>
    /// Update Log Criteria.
    /// </summary>
    public class UpdateLogCriteria : ModifyLogCriteria
    {
        [SpParam(SqlParamName = "feedbackid")]
        public int FeedbackId { get; set; }

        [SpParam(SqlParamName = "isreviewed")]
        public bool IsReviewed { get; set; }

        [SpParam(SqlParamName = "review")]
        public string Review { get; set; }

        [SpParam(SqlParamName = "updatereviewedmode")]
        public int UpdateReviewedMode { get; set; }

        [SpParam(SqlParamName = "reviewerid")]
        public int ReviewerId
        {
            get
            {
                return IsReviewed ? AppSession.CurrentUser.Id : 0;
            }
        }

        public string AutoField { get; set; }

        public override string GetSettingKey()
        {
            return "Communication_Log_Update";
        }
    }
}
