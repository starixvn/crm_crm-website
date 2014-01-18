// --------------------------------------------------------------------------
// <copyright file="SettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Setting.Services.Implements
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CRM.Infrastructure.Helper;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Infrastructure.Service.Implements;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Models;
    using Fanex.CRM.Setting.Repositories.Implements;
    using Fanex.CRM.Setting.Repositories.Interfaces;
    using Fanex.CRM.Setting.Resources;
    using Fanex.CRM.Setting.Services.Interfaces;

    /// <summary>
    /// Setting Service.
    /// </summary>
    public class SettingService : ISettingService
    {
        public const string GetAllSettingSessionKey = "Fanex.CRM.Setting.GetAllSettingSessionKey";

        private readonly ISettingRepository _repositoryInstance;

        public SettingService()
            : this(null)
        {
        }

        public SettingService(ISettingRepository repositoryInstance)
        {
            _repositoryInstance = repositoryInstance ?? new SettingRepository();
        }

        public SettingInfo GetSettingById(GetSettingByIdCriteria criteria)
        {
            SettingInfo setting = null;
            try
            {
                var settings = GetAllSetting();
                setting = settings.Where(s => s.Id == criteria.SettingId).FirstOrDefault();
                if (setting == null)
                {
                    setting = new SettingInfo()
                    {
                        Value = SettingConstants.NotApply
                    };
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format(Labels.Error_GetSetting, ex.Message, criteria.SettingId));
            }

            return setting;
        }

        public List<SettingInfo> GetAllSetting()
        {
            var allSetting = new List<SettingInfo>();
            try
            {
                allSetting = CacheService.Get<List<SettingInfo>>(GetAllSettingSessionKey);
                if (allSetting == null)
                {
                    allSetting = _repositoryInstance.GetAllSetting(new GetAllSettingCriteria());
                }

                CacheService.Set(GetAllSettingSessionKey, allSetting);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format(Labels.Error_GetAllSetting, ex.Message));
            }

            return allSetting;
        }
    }
}
