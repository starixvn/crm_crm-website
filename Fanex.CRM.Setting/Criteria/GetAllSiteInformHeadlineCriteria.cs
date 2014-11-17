// --------------------------------------------------------------------------
// <copyright file="GetAllSiteInformHeadlineCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.11.14">Created</change>
// </history>

namespace Fanex.CRM.Setting.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// GetAllStatusInformHeadline Criteria.
    /// </summary>
    public class GetAllSiteInformHeadlineCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Customer_InformHeadline_SiteList";
        } 
    }
}
