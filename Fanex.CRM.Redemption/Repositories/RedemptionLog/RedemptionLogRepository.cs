// --------------------------------------------------------------------------
// <copyright file="RedemptionLogRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Redemption.Repositories.RedemptionLog
{
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Repositories;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Redemption.Criteria;
    using Fanex.CRM.Redemption.Models;
    using Fanex.Data;

    /// <summary>
    /// Redemption Log Repository.
    /// </summary>
    public class RedemptionLogRepository : BaseRepository, IRedemptionLogRepository
    {
        public IEnumerable<LogInfo> GetLogsByCustomerId(GetLogsByCustomerIdCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    var logs = new List<LogInfo>();
                    while (reader.Read())
                    {
                        var log = new LogInfo
                        {
                            CustomerId = criteria.CustomerId,
                            Id = Converter.ToInt(reader["Id"]),
                            ClaimedTime = DateTimeUtilize.ChangeTimeZoneToClientDateTime(Converter.ToDateTime(reader["ClaimTime"])).ToShortFormatedDate(),
                            Subject = Converter.ToString2(reader["Subject"]),
                            Status = new Setting
                            {
                                Value = Converter.ToString2(reader["Status"])
                            },
                            Currency = new BaseModel { Name = Converter.ToString2(reader["Currency"]) },
                            DoneBy = Converter.ToString2(reader["DoneBy"]),
                            Amount = Converter.ToDecimal(reader["Amount"])
                        };

                        logs.Add(log);
                    }

                    return logs;
                }
            }
        }

        public LogInfo GetLogById(GetLogByIdCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader.Read())
                    {
                        return new LogInfo
                        {
                            Id = Converter.ToInt(reader["Id"]),
                            CustomerId = Converter.ToInt(reader["CustId"]),
                            ClaimedTime = DateTimeUtilize.ChangeTimeZoneToClientDateTime(Converter.ToDateTime(reader["ClaimTime"])).ToShortFormatedDate(),
                            Currency = new BaseModel { Id = Converter.ToInt(reader["CurrencyId"]) },
                            DoneBy = Converter.ToString2(reader["DoneBy"]),
                            Amount = Converter.ToDecimal(reader["Amount"]),
                            Subject = Converter.ToString2(reader["Subject"]).HtmlEncode(),
                            Remark = Converter.ToString2(reader["Remark"]),
                            Status = new Setting
                            {
                                Id = Converter.ToInt(reader["StatusID"])
                            }
                        };
                    }
                }
            }

            return new LogInfo();
        }

        public Result UpdateLog(UpdateLogCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        public Result InsertLog(InsertLogCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }
    }
}
