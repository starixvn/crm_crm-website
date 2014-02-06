// --------------------------------------------------------------------------
// <copyright file="ICommunicationLogRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Communication.Repositories.Interfaces
{
    using System.Collections.Generic;
    using Fanex.CRM.Communication.Criteria;
    using Fanex.CRM.Communication.Models;
    using Fanex.CRM.Infrastructure.Models;

    public interface ICommunicationLogRepository
    {
        List<LogInfo> GetLogsByCustomerId(GetLogsByCustomerIdCriteria criteria);

        Result UpdateLog(UpdateLogCriteria criteria);
    }
}
