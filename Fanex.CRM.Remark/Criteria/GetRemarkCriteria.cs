// --------------------------------------------------------------------------
// <copyright file="GetRemarkCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.02.10">Created</change>
// </history>

namespace Fanex.CRM.Remark.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get remark criteria.
    /// </summary>
    public class GetRemarkCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CRM_CustomerRemarkSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
