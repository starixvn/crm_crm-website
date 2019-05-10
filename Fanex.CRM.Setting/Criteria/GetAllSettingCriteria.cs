// --------------------------------------------------------------------------
// <copyright file="GetAllSettingCriteria.cs" company="">
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
    /// Get All Setting Criteria.
    /// </summary>
    public class GetAllSettingCriteria : BasedCriteria
    {
        public override string GetSettingKey()
        {
            return "Acc_CRM_SettingSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
