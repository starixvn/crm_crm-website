// --------------------------------------------------------------------------
// <copyright file="ActionLogService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.02.14">Created</change>
// </history>

namespace Fanex.CRM.ActionLog.Services.Implements
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.ActionLog.Models;
    using Fanex.CRM.ActionLog.Repositories.Implements;
    using Fanex.CRM.ActionLog.Repositories.Interfaces;
    using Fanex.CRM.ActionLog.Services.Interfaces;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// ActionLogService class.
    /// </summary>
    public class ActionLogService : IActionLogService
    {
        private readonly IActionLogRepository _instanceRepository;

        public ActionLogService()
            : this(null)
        {
        }

        public ActionLogService(IActionLogRepository instanceRepository)
        {
            instanceRepository = _instanceRepository ?? new ActionLogRepository();
        }

        #region IActionLogService Members

        public IList<ActivityLog> GetActivityLogs(ICriteria criteria)
        {
            throw new NotImplementedException();
        }

        public IList<SettingLog> GetSettingLogs(ICriteria criteria)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
