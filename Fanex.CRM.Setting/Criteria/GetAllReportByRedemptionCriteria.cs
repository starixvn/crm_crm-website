// --------------------------------------------------------------------------
// <copyright file="GetAllReportByRedemptionCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.11.14">Created</change>
// </history>

namespace Fanex.CRM.Setting.Criteria
{
    using System.Collections.Generic;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get All Report By Redemption Statistics Criteria.
    /// </summary>
    public class GetAllReportByRedemptionCriteria : CriteriaBase
    {
        public static List<BaseModel> ReportByList
        {
            get
            {
                var reportBy = new List<BaseModel>();

                BaseModel setting = null;

                setting = new BaseModel
                {
                    Id = 1,
                    Name = "Month"
                };
                reportBy.Add(setting);

                setting = new BaseModel
                {
                    Id = 2,
                    Name = "Year"
                };
                reportBy.Add(setting);

                return reportBy;
            }
        }

        public override string GetSettingKey()
        {
            return "Redemption_Statisics_GetReportBy";
        } 
    }
}
