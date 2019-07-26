namespace Fanex.CRM.Dashboard.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Common;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Dashboard.Criteria;
    using Fanex.CRM.Dashboard.Models;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.Data;

    public class DashboardRepository : IDashboardRepository
    {
        public CustomerInfo GetCustomerInfo(GetCustomerInfoCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    CustomerInfo customerInfo = reader.Read() ?
                        ReadCustomerInfo(reader) :
                        new CustomerInfo();

                    customerInfo.Id = criteria.CustomerId;
                    return customerInfo;
                }
            }
        }

        public IEnumerable<RedemptionLogInfo> GetRedemptionLogs(GetRedemptionLogsCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    var redemptionLogs = new List<RedemptionLogInfo>();
                    while (reader.Read())
                    {
                        redemptionLogs.Add(ReadRedemptionLog(reader));
                    }

                    return redemptionLogs;
                }
            }
        }

        public IEnumerable<CommunicationLogInfo> GetCommunicationLogs(GetCommunicationLogsCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    var feedbacks = new List<CommunicationLogInfo>();
                    while (reader.Read())
                    {
                        feedbacks.Add(ReadCommunicationLog(reader));
                    }

                    var settings = new List<SettingItem>();
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            settings.Add(new SettingItem
                            {
                                FeedbackID = Converter.ToInt(reader["FeedbackID"]),
                                SettingType = Converter.ToString2(reader["SettingType"]),
                                SettingIdLv1 = Converter.ToInt(reader["SettingID"]),
                                SettingIdLv2 = Converter.ToInt(reader["SubSettingID"]),
                                SettingIdLv3 = Converter.ToInt(reader["SubSettingIDLevel3"]),
                            });
                        }
                    }

                    return GenerateFeedBackDetailWithSetting(feedbacks, settings);
                }
            }
        }

        private CustomerInfo ReadCustomerInfo(IDataReader reader)
        {
            var customerInfo = new CustomerInfo
            {
                Email = Converter.ToString2(reader["email"]),
                Passport = Converter.ToString2(reader["Passport"]),
                SecretCode = Converter.ToString2(reader["SecretCode"]),
                BirthDay = reader["BirthDay"] != DBNull.Value ?
                    Converter.ToDateTime(reader["BirthDay"]).ToShortFormatedDate() :
                    string.Empty,

                IsVerifyEmail = Converter.ToBool(reader["IsVerifyEmail"]),
                IsVerifyPassport = Converter.ToBool(reader["IsVerifyPassport"]),
                IsVerifyBirthday = Converter.ToBool(reader["IsVerifyBirthday"]),
                IsVerifySecretCode = Converter.ToBool(reader["IsVerifySecretCode"]),
                IsVerifyBetCredit = Converter.ToBool(reader["IsVerifyBetCredit"]),
                IsVerifyGivenCredit = Converter.ToBool(reader["IsVerifyGivenCredit"]),

                RoleId = Converter.ToInt(reader["RoleId"]),
                IsCredit = Converter.ToBool(reader["IsCredit"]),
                BetCredit = Converter.ToDecimal(reader["BetCredit"]),
                GivenCredit = Converter.ToDecimal(reader["GivenCredit"])
            };

            return customerInfo;
        }

        private RedemptionLogInfo ReadRedemptionLog(IDataReader reader)
        {
            var redemptionLog = new RedemptionLogInfo
            {
                Id = Converter.ToInt(reader["RedemptionId"]),
                ClaimedTime = DateTimeUtilize.ChangeTimeZoneToClientDateTime(Converter.ToDateTime(reader["ClaimTime"])).ToShortFormatedDate(),
                Subject = Converter.ToString2(reader["Subject"]),
                Status = new Setting
                {
                    Value = Converter.ToString2(reader["Status"])
                },
                Amount = Converter.ToDecimal(reader["Amount"]),
                DoneBy = Converter.ToString2(reader["DoneBy"]),
                Currency = new BaseModel { Name = Converter.ToString2(reader["Currency"]) }
            };

            return redemptionLog;
        }

        private CommunicationLogInfo ReadCommunicationLog(IDataReader reader)
        {
            var durationTime = Converter.ToString2(reader["Duration"]);
            var durationTimeArray = durationTime.Split(':');
            var communicationLog = new CommunicationLogInfo
            {
                RunningNumber = Converter.ToInt(reader["FeedbackID"]),
                FeedbackDate = DateTimeUtilize.ChangeTimeZoneToClientDateTime(Converter.ToDateTime(reader["FeedbackDate"])).ToLongFormatedDateTime(),
                Subject = Converter.ToString2(reader["Subject"]),
                StatusName = Converter.ToString2(reader["Status"]),
                Duration = new TimeInfo
                {
                    Hours = Converter.ToInt(durationTimeArray[0]),
                    Minutes = Converter.ToInt(durationTimeArray[1]),
                    Seconds = Converter.ToInt(durationTimeArray[2])
                },
                Handler = Converter.ToString2(reader["HandleBy"]),
                Reviewer = Converter.ToString2(reader["ReviewBy"])
            };

            return communicationLog;
        }

        private IEnumerable<CommunicationLogInfo> GenerateFeedBackDetailWithSetting(IEnumerable<CommunicationLogInfo> feedbacks, IEnumerable<SettingItem> settings)
        {
            Func<CommunicationLogInfo, List<SettingItem>, CommunicationLogInfo> regenerateFeedBackDetail = delegate (CommunicationLogInfo f, List<SettingItem> s)
            {
                f.Settings = s;
                return f;
            };
            var result = from f in feedbacks
                         join s in settings on f.RunningNumber equals s.FeedbackID
                         select new { f, s } into t1
                         group t1 by t1.f.RunningNumber into g
                         select regenerateFeedBackDetail(g.FirstOrDefault().f, g.Select(i => i.s).ToList());

            return result.ToList();
        }
    }
}