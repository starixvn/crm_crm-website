// --------------------------------------------------------------------------
// <copyright file="CommunicationLogService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Communication.Services.Implements
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Communication.Criteria;
    using Fanex.CRM.Communication.Models;
    using Fanex.CRM.Communication.Repositories.Implements;
    using Fanex.CRM.Communication.Repositories.Interfaces;
    using Fanex.CRM.Communication.Services.Interfaces;
    using Fanex.CRM.Infrastructure.Logging;

    /// <summary>
    /// Communication Log Service.
    /// </summary>
    public class CommunicationLogService : ICommunicationLogService
    {
        private readonly ICommunicationLogRepository _repositoryInstance;

        public CommunicationLogService()
            : this(null)
        {
        }

        public CommunicationLogService(ICommunicationLogRepository repositoryInstance)
        {
            _repositoryInstance = repositoryInstance ?? new CommunicationLogRepository();
        }

        public List<LogInfo> GetLogsByCustomerId(GetLogsByCustomerIdCriteria criteria)
        {
            List<LogInfo> result = null;
            try
            {
                var rawData = _repositoryInstance.GetLogsByCustomerId(criteria);
                result = rawData;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }
    }
}
