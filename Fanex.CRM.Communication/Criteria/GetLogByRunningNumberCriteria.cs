// --------------------------------------------------------------------------
// <copyright file="GetLogByRunningNumberCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Communication.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Log By Running Number Criteria.
    /// </summary>
    public class GetLogByRunningNumberCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "feedbackid")]
        public int RunningNumber { get; set; }

        public override string GetSettingKey()
        {
            return "Communication_Log_GetInfoById";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
