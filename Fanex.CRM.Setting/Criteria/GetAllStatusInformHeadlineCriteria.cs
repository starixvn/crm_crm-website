// --------------------------------------------------------------------------
// <copyright file="GetAllStatusInformHeadlineCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.05.12">Created</change>
// </history>

namespace Fanex.CRM.Setting.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// GetAllStatusInformHeadline Criteria.
    /// </summary>
    public class GetAllStatusInformHeadlineCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Customer_InformHeadline_StatusList";
        } 
    }
}
