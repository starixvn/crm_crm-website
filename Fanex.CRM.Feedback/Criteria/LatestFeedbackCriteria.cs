// --------------------------------------------------------------------------
// <copyright file="LatestFeedbackCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.09.05">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Feedback.Criteria
{
    using System.Collections;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Latest Feedback Criteria.
    /// </summary>
    public class LatestFeedbackCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public int Top { get; set; }

        public override string GetSettingKey()
        {
            if (CustId == 0)
            {
                return "FeedbackSelTopFeedback";
            }

            return "FeedbackSelTopFeedbackByCustId";
        }

        public override Hashtable GetSpParams()
        {
            var inputParams = new Hashtable();
            inputParams.Add("top", Top);
            if (CustId > 0)
            {
                inputParams.Add("custid", CustId);    
            }

            return inputParams;
        }

        public override bool IsValid()
        {
            return Top > 0;
        }
    }
}
