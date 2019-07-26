namespace Fanex.CRM.Report.Repositories.DynamicReport
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common.Models;
    using Fanex.Common;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Criteria.DynamicReport;
    using Fanex.CRM.Report.Models;
    using Fanex.Data;

    public class DynamicReportRepository : IDynamicReportRepository
    {
        public IEnumerable<DynamicReportInfo> GetDynamicReport(GetDynamicReportCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    var feedbacks = new List<DynamicReportInfo>();
                    while (reader.Read())
                    {
                        feedbacks.Add(ReadDynamicReportInfo(reader));
                    }

                    var settings = new List<SettingItem>();
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            settings.Add(new SettingItem
                            {
                                FeedbackID = Converter.ToInt(reader["FeedbackID"]),
                                SettingIdLv1 = Converter.ToInt(reader["MainCategoryID"]),
                                SettingIdLv2 = Converter.ToInt(reader["SubCategoryID"])
                            });
                        }
                    }

                    return GenerateFeedBackDetailWithSetting(feedbacks, settings);
                }
            }
        }

        private IEnumerable<DynamicReportInfo> GenerateFeedBackDetailWithSetting(IEnumerable<DynamicReportInfo> feedbacks, IEnumerable<SettingItem> settings)
        {
            Func<DynamicReportInfo, List<SettingItem>, DynamicReportInfo> regenerateFeedBackDetail = delegate (DynamicReportInfo f, List<SettingItem> s)
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

        private DynamicReportInfo ReadDynamicReportInfo(IDataReader reader)
        {
            var durationTime = Converter.ToString2(reader["Duration"]);
            var durationTimeArray = durationTime.Split(':');
            var communication = new DynamicReportInfo
            {
                RunningNumber = Converter.ToInt(reader["FeedbackID"]),
                PeriodRange = new PeriodRange
                {
                    FromDate = DateTimeUtilize.ChangeTimeZoneToClientDateTime(Converter.ToDateTime(reader["FeedbackDate"]))
                },
                Subject = Converter.ToString2(reader["Subject"]),
                StatusName = Converter.ToString2(reader["Status"]),
                Duration = new TimeInfo
                {
                    Hours = Converter.ToInt(durationTimeArray[0]),
                    Minutes = Converter.ToInt(durationTimeArray[1]),
                    Seconds = Converter.ToInt(durationTimeArray[2])
                },
                Handler = Converter.ToString2(reader["HandledBy"]),
                Reviewer = Converter.ToString2(reader["ReviewedBy"]),
                Username = Converter.ToString2(reader["Username"])
            };

            return communication;
        }
    }
}