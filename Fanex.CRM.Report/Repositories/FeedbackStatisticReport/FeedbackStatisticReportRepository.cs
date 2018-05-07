// --------------------------------------------------------------------------
// <copyright file="FeedbackStatisticReportRepository.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.10.10">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.FeedbackStatisticReport
{
    using Fanex.Common;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Models.FeedbackStatisticReport;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    /// <summary>
    /// Feedback statistic report repository.
    /// </summary>
    public class FeedbackStatisticReportRepository : IFeedbackStatisticReportRepository
    {
        public FeedbackStatistic GetFeedbackStatistic(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    return ReadFeedbackStatistic(reader);
                }
            }
        }

        public IEnumerable<FeedbackDetail> GetFeedbackDetail(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    var feebbacks = multi.Read<FeedbackDetail>();
                    var settings = multi.Read<SettingItem>();

                    feebbacks = feebbacks.Select(f =>
                    {
                        f.Settings = settings.Where(s => s.FeedbackID == f.FeedbackId).ToList();
                        return f;
                    });

                    return feebbacks;
                }
            }
        }

        private static FeedbackStatistic ReadFeedbackStatistic(IDataReader reader)
        {
            var userNames = new List<BaseModel>();
            for (int i = 4; i < reader.FieldCount; i++)
            {
                userNames.Add(new BaseModel { Name = reader.GetName(i) });
            }

            var userNamesCount = userNames.Count;
            var grandTotals = new int[userNamesCount];
            var report = new FeedbackStatistic { Usernames = userNames, FeedbackCategories = new List<FeedbackCategory>(), GrandTotals = grandTotals };
            int previousCategory = 0, categoryId = 0;
            FeedbackCategory feedbackCategory = null;
            while (reader.Read())
            {
                categoryId = Converter.ToInt(reader["MainCategoryId"]);
                if (categoryId != previousCategory)
                {
                    previousCategory = categoryId;
                    feedbackCategory = new FeedbackCategory
                    {
                        Id = Converter.ToInt(reader["MainCategoryId"]),
                        Name = Converter.ToString2(reader["MainCategory"]),
                        FeedbackSubcategories = new List<FeedbackSubcategory>(),
                        SubTotal = InitializeSubTotal(userNamesCount)
                    };

                    report.FeedbackCategories.Add(feedbackCategory);
                }

                var feedbackSubcategory = new FeedbackSubcategory
                {
                    Id = Converter.ToInt(reader["SubCategoryId"]),
                    Name = Converter.ToString2(reader["SubCategory"]),
                    Resolves = new List<int>(),
                };

                for (int i = 0; i < userNamesCount; i++)
                {
                    var resolve = Converter.ToInt(reader[userNames[i].Name]);
                    feedbackSubcategory.Resolves.Add(resolve);
                    feedbackCategory.SubTotal.Resolves[i] += resolve;
                    grandTotals[i] += resolve;
                }

                feedbackCategory.FeedbackSubcategories.Add(feedbackSubcategory);
            }

            ReadUsernameIds(reader, report);
            return report;
        }

        private static void ReadUsernameIds(IDataReader reader, FeedbackStatistic report)
        {
            if (reader.NextResult())
            {
                int index = 0;
                while (reader.Read())
                {
                    report.Usernames[index++].Id = Converter.ToInt(reader["Item"]);
                }
            }
        }

        private static FeedbackSubcategory InitializeSubTotal(int length)
        {
            var resolves = Enumerable.Repeat(0, length).ToList();
            return new FeedbackSubcategory { Resolves = resolves };
        }
    }
}
