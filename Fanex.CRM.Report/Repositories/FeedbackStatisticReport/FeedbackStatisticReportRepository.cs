// --------------------------------------------------------------------------
// <copyright file="FeedbackStatisticReportRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.10.10">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.FeedbackStatisticReport
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Models.FeedbackStatisticReport;
    using Fanex.Data;
    using Fanex.Data.Repository;

    /// <summary>
    /// Feedback statistic report repository.
    /// </summary>
    public class FeedbackStatisticReportRepository : IFeedbackStatisticReportRepository
    {
        public FeedbackStatistic GetFeedbackStatistic(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (IDataReader reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    return ParseFeedbackStatistic(reader);
                }
            }
        }

        public IEnumerable<FeedbackDetail> GetFeedbackDetail(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.Query<FeedbackDetail>(criteria.GetSpParams());
            }    
        }

        /// <summary>
        /// Parses the feedback statistic.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>The Feedback statistic report.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private FeedbackStatistic ParseFeedbackStatistic(IDataReader reader)
        {
            IList<BaseModel> usernames = new List<BaseModel>();
            int fieldCount = reader.FieldCount;
            for (int i = 4; i < fieldCount; i++)
            {
                usernames.Add(new BaseModel { Name = reader.GetName(i) });
            }

            int usernamesLeng = usernames.Count;
            int[] grandTotals = new int[usernamesLeng];
            FeedbackStatistic report = new FeedbackStatistic { Usernames = usernames, FeedbackCategories = new List<FeedbackCategory>(), GrandTotals = grandTotals };
            int resolve;
            int previousCategory = 0, categoryId = 0;
            FeedbackCategory feedbackCategory = null;
            FeedbackSubcategory feedbackSubcategory;
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
                        SubTotal = InitializeSubTotal(usernamesLeng)
                    };

                    report.FeedbackCategories.Add(feedbackCategory);
                }

                feedbackSubcategory = new FeedbackSubcategory 
                {
                    Id = Converter.ToInt(reader["SubCategoryId"]),
                    Name = Converter.ToString2(reader["SubCategory"]),
                    Resolves = new List<int>(),
                };

                for (int i = 0; i < usernamesLeng; i++)
                {
                    resolve = Converter.ToInt(reader[usernames[i].Name]);
                    feedbackSubcategory.Resolves.Add(resolve);
                    feedbackCategory.SubTotal.Resolves[i] += resolve;
                    grandTotals[i] += resolve;
                }

                feedbackCategory.FeedbackSubcategories.Add(feedbackSubcategory);
            }

            if (reader.NextResult())
            {
                int index = 0;
                while (reader.Read())
                {
                    report.Usernames[index++].Id = Converter.ToInt(reader["Item"]);
                }
            }

            return report;
        }

        private FeedbackSubcategory InitializeSubTotal(int usernamesLen)
        {
            IList<int> resolves = new List<int>();
            for (int i = 0; i < usernamesLen; i++)
            {
                resolves.Add(0);    
            }

            return new FeedbackSubcategory { Resolves = resolves };
        }
    }
}
