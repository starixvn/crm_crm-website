// --------------------------------------------------------------------------
// <copyright file="DynamicReportRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.Implements
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common;
    using Fanex.CRM.Infrastructure.Core;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Criteria.DynamicReport;
    using Fanex.CRM.Report.Models;
    using Fanex.CRM.Report.Repositories.Interfaces;
    using Fanex.CRM.Setting.Models;
    using Fanex.Data;

    /// <summary>
    /// Dynamic Report Repository.
    /// </summary>
    public class DynamicReportRepository : IDynamicReportRepository
    {
        /// <summary>
        /// Gets the dynamic report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The dynamic report.</returns>
        public List<DynamicReportInfo> GetDynamicReport(GetDynamicReportCriteria criteria)
        {
            List<DynamicReportInfo> communicationList = new List<DynamicReportInfo>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                using (var reader = objectDB.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var communication = MappingDataFromSqlToCommunicationInfoObject(reader, criteria);
                            communicationList.Add(communication);
                        }
                    }
                }
            }

            return communicationList;
        }

        /// <summary>
        /// Mappings the data from SQL to log information object.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// Log Info Object.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private DynamicReportInfo MappingDataFromSqlToCommunicationInfoObject(
            IDataReader reader,
            GetDynamicReportCriteria criteria)
        {
            var durationTime = reader.GetStringNotNull("Duration");
            var durationTimeArray = durationTime.Split(':');
            var communication = new DynamicReportInfo
            {
                RunningNumber = reader.GetInt32NotNull("FeedbackID"),
                PeriodRange = new PeriodRange
                {
                    FromDate = DateTimeUtilize.ChangeTimeZoneToClientDateTime(reader.GetDateTimeNotNull("FeedbackDate"))
                },
                MainCategory = new SettingInfo
                {
                    Name = reader.GetStringNotNull("MainCategory")
                },
                SubCategory = new SettingInfo
                {
                    Name = reader.GetStringNotNull("SubCategory")
                },
                Subject = reader.GetStringNotNull("Subject"),
                StatusName = reader.GetStringNotNull("Status"),
                Duration = new TimeInfo
                {
                    Hours = Converter.ToInt(durationTimeArray[0]),
                    Minutes = Converter.ToInt(durationTimeArray[1]),
                    Seconds = Converter.ToInt(durationTimeArray[2])
                },
                Handler = reader.GetStringNotNull("HandledBy"),
                Reviewer = reader.GetStringNotNull("ReviewedBy")
            };

            return communication;
        }
    }
}
