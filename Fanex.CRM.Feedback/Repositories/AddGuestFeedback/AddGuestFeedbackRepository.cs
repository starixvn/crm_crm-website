// --------------------------------------------------------------------------
// <copyright file="AddGuestFeedbackRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Feedback.Repositories.AddCustomerFeedback
{
    using Fanex.Common;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.Data;

    /// <summary>
    /// Add Guest Feedback Repository.
    /// </summary>
    public class AddGuestFeedbackRepository : IAddGuestFeedbackRepository
    {
        /// <summary>
        /// Adds the guest feedback.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Add result.</returns>
        public Result AddGuestFeedback(AddGuestFeedbackCriteria criteria)
        {
            Result addResult = null;
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                objectDB.ExecuteNonQuery();
                var outputParams = objectDB.GetOuputParams();
                addResult = new Result
                {
                    Code = Converter.ToInt(outputParams["errcode"]),
                    Message = Converter.ToString2(outputParams["errmsg"])
                };
            }

            return addResult;
        }
    }
}
