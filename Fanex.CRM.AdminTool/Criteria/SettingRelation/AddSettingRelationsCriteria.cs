// --------------------------------------------------------------------------
// <copyright file="AddSettingRelationsCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Criteria.SettingRelation
{
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Helper;
    using Fanex.CRM.Setting.Models;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get All Setting Criteria.
    /// </summary>
    public class AddSettingRelationsCriteria : CriteriaBase
    {
        public int ParentId { get; set; }

        public string SettingName { get; set; }

        public List<SettingInfo> SubSettings { get; set; }

        public int CreatorId
        {
            get
            {
                return AppSession.CurrentUser.Id;
            }
        }

        public int UserId
        {
            get
            {
                return AppSession.CurrentUser.Id;
            }
        }

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
            param.Add("settingId", GetSettingId());
            param.Add("settingName", SettingName);
            param.Add("parentId", ParentId);

            param.Add("userid", UserId);
            param.Add("ip", IP);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            return param;
        }

        public override string GetSettingKey()
        {
            return "SettingRelationUpd";
        }

        public override bool IsValid()
        {
            return true;
        }

        /// <summary>
        /// Gets the setting identifiers.
        /// </summary>
        /// <returns>The setting identifiers.</returns>
        private string GetSettingId()
        {
            if (SubSettings == null)
            {
                return string.Empty;
            }

            var settingId = new List<string>();
            var subSettingLength = SubSettings.Count;
            for (var i = 0; i < subSettingLength; i++)
            {
                var subSetting = SubSettings[i];
                if (subSetting.IsSubSetting)
                {
                    settingId.Add(string.Format("{0}_{1}", subSetting.Id, subSetting.DisplayOrder));
                }
            }

            return string.Join("|", settingId);
        }
    }
}
