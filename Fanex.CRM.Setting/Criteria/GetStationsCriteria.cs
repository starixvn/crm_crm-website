// --------------------------------------------------------------------------
// <copyright file="GetStationsCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Setting.Criteria
{
    using Common.Criteria;

    /// <summary>
    /// GetStationsCriteria class.
    /// </summary>
    public class GetStationsCriteria : BasedCriteria
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
