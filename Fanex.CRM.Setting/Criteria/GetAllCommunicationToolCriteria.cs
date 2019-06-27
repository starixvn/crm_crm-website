// --------------------------------------------------------------------------
// <copyright file="GetAllCommunicationToolCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Setting.Criteria
{
    using Common.Criteria;

    /// <summary>
    /// Get All Communication Tool Criteria.
    /// </summary>
    public class GetAllCommunicationToolCriteria : BasedLanguageCriteria
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
