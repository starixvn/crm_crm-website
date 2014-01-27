// --------------------------------------------------------------------------
// <copyright file="CommunicationLogRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Communication.Repositories.Implements
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Common;
    using Fanex.CRM.Communication.Criteria;
    using Fanex.CRM.Communication.Models;
    using Fanex.CRM.Communication.Repositories.Interfaces;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.Data;

    /// <summary>
    /// Communication Log Repository.
    /// </summary>
    public class CommunicationLogRepository : ICommunicationLogRepository
    {
        public List<LogInfo> GetLogsByCustomerId(GetLogsByCustomerIdCriteria criteria)
        {
            List<LogInfo> logs = new List<LogInfo>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                using (var reader = objectDB.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var log = MappingDataFromSqlToLogInfoObject(reader);
                            logs.Add(log);
                        }
                    }
                }
            }

            return logs;
        }

        private LogInfo MappingDataFromSqlToLogInfoObject(IDataReader reader)
        {
            var durationTime = reader.GetStringNotNull("Duration");
            var durationTimeArray = durationTime.Split(':');
            return new LogInfo
            {
                RunningNumber = reader.GetInt32NotNull("FeedbackID"),
                FeedbackDate = reader.GetDateTimeNotNull("FeedbackDate"),
                MainCategory = new BaseModel
                {
                    Name = reader.GetStringNotNull("MainCategory")
                },
                SubCategory = new BaseModel
                {
                    Name = reader.GetStringNotNull("SubCategory")
                },
                Subject = reader.GetStringNotNull("Subject"),
                StatusName = reader.GetStringNotNull("Status"),
                Duration = new TimeSpan(
                    Converter.ToInt(durationTimeArray[0]),
                    Converter.ToInt(durationTimeArray[1]),
                    Converter.ToInt(durationTimeArray[2])),
                Handler = reader.GetStringNotNull("HandleBy"),
                Reviewer = reader.GetStringNotNull("ReviewBy")
            };
        }
    }
}
