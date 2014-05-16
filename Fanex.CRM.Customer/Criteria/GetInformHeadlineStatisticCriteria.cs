// --------------------------------------------------------------------------
// <copyright file="GetInformHeadlineStatisticCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.05.14">Created</change>
// </history>

namespace Fanex.CRM.Customer.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// GetInformHeadlineStatisticCriteria class.
    /// </summary>
    public class GetInformHeadlineStatisticCriteria : CriteriaBase
    {
        [SpParam(Ignored = true)]
        public bool IsCached { get; set; }

        public override string GetSettingKey()
        {
            return "Customer_InformHeadline_Statistics";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
