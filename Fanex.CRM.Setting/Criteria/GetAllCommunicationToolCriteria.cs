// --------------------------------------------------------------------------
// <copyright file="GetAllCommunicationToolCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Setting.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get All Communication Tool Criteria.
    /// </summary>
    public class GetAllCommunicationToolCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Setting_SettingInfo_GetAllCommunicationTool";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
