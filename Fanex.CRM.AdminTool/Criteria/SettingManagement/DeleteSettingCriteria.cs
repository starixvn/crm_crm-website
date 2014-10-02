// --------------------------------------------------------------------------
// <copyright file="DeleteSettingCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Criteria.SettingManagement
{
    using System.Collections;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Helper;
    using Fanex.CRM.Setting.Models;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Delete Setting Criteria.
    /// </summary>
    public class DeleteSettingCriteria : CriteriaBase
    {
        public SettingInfo OldSetting { get; set; }

        [SpParam(SqlParamName = "creatorid")]
        public int CreatorId
        {
            get
            {
                return AppSession.CurrentUser.Id;
            }
        }

        [SpParam(SqlParamName = "userid")]
        public int UserId
        {
            get
            {
                return AppSession.CurrentUser.Id;
            }
        }

        [SpParam(SqlParamName = "ip")]
        public string IP
        {
            get
            {
                return Helper.GetClientIP();
            }
        }

        /// <summary>
        /// Gets the SP parameters.
        /// </summary>
        /// <returns>The SP parameters.</returns>
        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("settingId", OldSetting.Id);

            param.Add("userid", UserId);
            param.Add("ip", IP);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            return param;
        }

        public override string GetSettingKey()
        {
            return "Acc_CRM_SettingDel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
