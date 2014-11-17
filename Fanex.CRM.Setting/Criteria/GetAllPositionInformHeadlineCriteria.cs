// --------------------------------------------------------------------------
// <copyright file="GetAllPositionInformHeadlineCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
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
    /// Get All Position Inform Headline Criteria.
    /// </summary>
    public class GetAllPositionInformHeadlineCriteria : CriteriaBase
    {
        /// <summary>
        /// Gets the position list.
        /// </summary>
        /// <value>
        /// Get the position list.
        /// </value>
        public List<BaseModel> PositionList
        {
            get
            {
                var position = new List<BaseModel>();

                BaseModel setting = null;

                setting = new BaseModel
                {
                    Id = 1,
                    Name = "Member"
                };
                position.Add(setting);

                setting = new BaseModel
                {
                    Id = 2,
                    Name = "Agent"
                };
                position.Add(setting);

                setting = new BaseModel
                {
                    Id = 3,
                    Name = "Master"
                };
                position.Add(setting);

                setting = new BaseModel
                {
                    Id = 4,
                    Name = "Super"
                };
                position.Add(setting);

                return position;
            }
        }

        public override string GetSettingKey()
        {
            return "Customer_InformHeadline_PositionList";
        } 
    }
}
