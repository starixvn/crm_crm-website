// --------------------------------------------------------------------------
// <copyright file="InsertSettingCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Criteria.SettingManagement
{
    using Common.Criteria;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.Data.Repository;
    using System.Collections;

    /// <summary>
    /// Insert Setting Criteria.
    /// </summary>
    public class InsertSettingCN88Criteria : BasedCriteria
    {
        public SettingCN88 NewSetting { get; set; }

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
                return IPHelper.ClientIP;
            }
        }

        /// <summary>
        /// Gets the SP parameters.
        /// </summary>
        /// <returns>The SP parameters.</returns>
        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();

            param.Add("settingname", NewSetting.SettingName);
            param.Add("settingnamecs", NewSetting.SettingNameCS);
            param.Add("settingnamect", NewSetting.SettingNameCT);
            param.Add("settingvalue", NewSetting.SettingValue);
            param.Add("settingvaluecs", NewSetting.SettingValueCS);
            param.Add("settingvaluect", NewSetting.SettingValueCT);
            param.Add("displayorder", NewSetting.DisplayOrder);
            param.Add("parentid", NewSetting.ParentId);

            param.Add("userid", UserId);
            param.Add("ip", IP);
            param.Add("UserRole", UserRole);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            return param;
        }

        public override string GetSettingKey()
        {
            return "Acc_CRM_CN88_SettingIns";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
