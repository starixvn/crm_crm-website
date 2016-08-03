// --------------------------------------------------------------------------
// <copyright file="GetStationsCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Setting.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// GetStationsCriteria class.
    /// </summary>
    public class GetStationsCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {            
            return "Acc_CRM_StationList";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
