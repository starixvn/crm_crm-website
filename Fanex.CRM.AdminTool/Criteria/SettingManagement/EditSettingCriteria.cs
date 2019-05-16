// --------------------------------------------------------------------------
// <copyright file="EditSettingCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Criteria.SettingManagement
{
    using Fanex.CRM.Common.Criteria;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.Data.Repository;
    using System.Collections;

    /// <summary>
    /// Edit Setting Criteria.
    /// </summary>
    public class EditSettingCriteria : BasedUpdateCriteria
    {
        public Setting OldSetting { get; set; }

        public Setting UpdatedSetting { get; set; }

        [SpParam(SqlParamName = "creatorid")]
        public int CreatorId
        {
            get
            {
                return AppSession.CurrentUser.Id;
            }
        }

        /// <summary>
        /// Gets the SP parameters.
        /// </summary>
        /// <returns>The SP parameters.</returns>
        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("settingname", UpdatedSetting.SettingType);
            param.Add("settingvalue", UpdatedSetting.Value);
            param.Add("displayorder", UpdatedSetting.DisplayOrder);
            param.Add("settingid", UpdatedSetting.Id);

            param.Add("userid", UserId);
            param.Add("ip", IP);
            param.Add("UserRole", UserRole);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            return param;
        }

        public override string GetSettingKey()
        {
            return "Acc_CRM_SettingUpd";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
