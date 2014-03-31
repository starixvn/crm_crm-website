// --------------------------------------------------------------------------
// <copyright file="StatisticReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.Implements.StatisticReport
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Core;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Infrastructure.Service.Implements;
    using Fanex.CRM.Report.Criteria.DynamicReport;
    using Fanex.CRM.Report.Entities.StatisticReport;
    using Fanex.CRM.Report.Models.StatisticReport;
    using Fanex.CRM.Report.Resources;
    using Fanex.CRM.Report.Services.Interfaces;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Models;
    using Fanex.CRM.Setting.Services.Implements;
    using Fanex.CRM.Setting.Services.Interfaces;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// Statistic Report Service.
    /// </summary>
    public class StatisticReportService : ServiceBase, IStatisticReportService
    {
        public readonly string StatisticReportSessionKey = "Fanex.CRM.Report.StatisticReport.StatisticReportSessionKey_" +
            (AppSession.CurrentUser != null ? AppSession.CurrentUser.Id : 0);

        private ISettingService _serviceSetting;

        public StatisticReportService()
            : this(null)
        {
        }

        public StatisticReportService(ISettingService serviceSetting)
        {
            _serviceSetting = serviceSetting ?? new SettingService();
        }

        /// <summary>
        /// Gets all period type.
        /// </summary>
        /// <returns>All period type.</returns>
        public List<SettingInfo> GetAllPeriodType()
        {
            try
            {
                var allPeriodType = new List<SettingInfo>();
                allPeriodType.Add(new SettingInfo
                {
                    Id = PeriodTypeConstants.Daily,
                    Name = Labels.Daily
                });

                allPeriodType.Add(new SettingInfo
                {
                    Id = PeriodTypeConstants.Monthly,
                    Name = Labels.Monthly
                });

                return allPeriodType;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return new List<SettingInfo>();
        }

        /// <summary>
        /// Gets all main category.
        /// </summary>
        /// <returns>All main category.</returns>
        public List<SettingInfo> GetAllMainCategory()
        {
            try
            {
                var allMainCategory = new List<SettingInfo>();
                var rawData = _serviceSetting.GetAllMainCategory();
                var allSubSettingOption = new SettingInfo
                {
                    Id = SettingConstants.NotApply,
                    Name = Labels.All,
                };

                allMainCategory.Add(new SettingInfo
                {
                    Id = SettingConstants.NotApply,
                    Name = Labels.All,
                    ChildSettings = new List<SettingInfo>
                    {
                       allSubSettingOption
                    }
                });

                foreach (var rawMainCategory in rawData)
                {
                    rawMainCategory.ChildSettings.Insert(0, allSubSettingOption);
                    allMainCategory.Add(rawMainCategory);
                }

                return allMainCategory;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return new List<SettingInfo>();
        }

        /// <summary>
        /// Gets the statistic report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The statistic report.</returns>      
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public StatisticReportInfo GetStatisticReport(GetStatisticReportCriteria criteria)
        {
            try
            {
                StatisticReportInfo statisticReport = null;
                if (criteria.IsGetFromCached)
                {
                    statisticReport = (StatisticReportInfo)SessionExtension.GetSession(HttpContext.Current.Session, StatisticReportSessionKey);
                }

                if (statisticReport == null)
                {
                    criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
                    criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);

                    // var rawData = GetDynamicData<StatisticReportEntity>(criteria);
                    var dummyDateTime = new DateTime(2014, 3, 21, 0, 20, 3);
                    var rawData = new List<StatisticReportEntity>
                    {
                        new StatisticReportEntity {
                            DateTime = dummyDateTime,
                            CountryId = 2,
                            CountryName = "Africa",
                            NumberOfIssues = 1000
                        },

                        new StatisticReportEntity {
                            DateTime = dummyDateTime,
                            CountryId = 3,
                            CountryName = "Africa2",
                            NumberOfIssues = 1000
                        },

                        new StatisticReportEntity {
                            DateTime = dummyDateTime.AddHours(3),
                            CountryId = 2,
                            CountryName = "Africa",
                            NumberOfIssues = 9000
                        },

                        new StatisticReportEntity {
                            DateTime = dummyDateTime.AddHours(5),
                            CountryId = 3,
                            CountryName = "Africa2",
                            NumberOfIssues = 8000
                        },
                        
                        new StatisticReportEntity {
                            DateTime = dummyDateTime.AddDays(1),
                            CountryId = 2,
                            CountryName = "Africa",
                            NumberOfIssues = 10
                        },

                        new StatisticReportEntity {
                            DateTime = dummyDateTime.AddDays(1),
                            CountryId = 3,
                            CountryName = "Africa2",
                            NumberOfIssues = 10
                        },

                        new StatisticReportEntity {
                            DateTime = dummyDateTime.AddHours(3).AddDays(1),
                            CountryId = 2,
                            CountryName = "Africa",
                            NumberOfIssues = 9
                        },

                        new StatisticReportEntity {
                            DateTime = dummyDateTime.AddHours(5).AddDays(1),
                            CountryId = 3,
                            CountryName = "Africa2",
                            NumberOfIssues = 8
                        }
                    };

                    var countryList = rawData
                        .Select(statisticReportEntity => new CountryInfo
                        {
                            Id = statisticReportEntity.CountryId,
                            Name = statisticReportEntity.CountryName
                        })
                        .ToList();
                    var countryDictionary = GetCountryDictionary(countryList);
                    switch (criteria.ReportType)
                    {
                        case PeriodTypeConstants.Daily:
                            statisticReport = GetStatisticReportByDaily(
                                rawData,
                                countryList,
                                countryDictionary,
                                criteria);
                            break;
                        case PeriodTypeConstants.Monthly:
                            statisticReport = GetStatisticReportByMonthly(
                                rawData,
                                countryList,
                                countryDictionary,
                                criteria);
                            break;
                        default:
                            break;
                    }

                    statisticReport.ReportTypeId = criteria.ReportType;
                    statisticReport.TotalChildReports = statisticReport.ChildReports.Count;
                    statisticReport.ChartInfo = GetChartInfo(statisticReport, countryList, countryDictionary);
                    SessionExtension.AddSession(HttpContext.Current.Session, StatisticReportSessionKey, statisticReport);
                }

                return new StatisticReportInfo
                {
                    Name = statisticReport.Name,
                    ShortName = statisticReport.ShortName,
                    ChildReports = criteria.IsGetFullReport ?
                        statisticReport.ChildReports :
                        GetPagingLogList(criteria, statisticReport.ChildReports),
                    Total = statisticReport.Total,
                    TotalChildReports = statisticReport.TotalChildReports,
                    ChartInfo = statisticReport.ChartInfo
                };
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw new Exception();
            }
        }

        /// <summary>
        /// Gets the statistic report by monthly.
        /// </summary>
        /// <param name="rawData">The raw data.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The statistic report by monthly.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private StatisticReportInfo GetStatisticReportByMonthly(
            List<StatisticReportEntity> rawData,
            List<CountryInfo> countryList,
            Dictionary<int, int> countryDictionary,
            GetStatisticReportCriteria criteria)
        {
            var countryLength = countryDictionary.Count;
            StatisticReportInfo statisticReport = new StatisticReportInfo
            {
                ChildReports = new List<StatisticReportInfo>(),
                Name = string.Format(
                    Labels.StatisticReport_Title,
                    Labels.Monthly,
                    DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToShortFormatedDate(),
                    DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.ToDate).ToShortFormatedDate()),
                ShortName = string.Format(
                    Labels.StatisticReport_ShortTitle,
                    DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToShortFormatedDate(),
                    DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.ToDate).ToShortFormatedDate()),
                Total = new StatisticReportInfo
                {
                    ShortName = Labels.Grand_Total,
                    ChildReports = GetDummyCountryList(countryLength, countryList)
                }
            };

            if (rawData == null ||
                rawData.Count == 0)
            {
                return statisticReport;
            }

            var dateGroups = rawData.GroupBy(data => new DateTime(data.DateTime.Year, data.DateTime.Month, 1));
            foreach (var dateGroup in dateGroups)
            {
                var dateReport = new StatisticReportInfo
                {
                    ShortName = dateGroup.Key.ToMonthYear(),
                    ChildReports = new List<StatisticReportInfo>(),
                    PeriodRange = new PeriodRange
                    {
                        FromDate = dateGroup.Key,
                        ToDate = dateGroup.Key.LastDayOfMonth()
                    },
                    Total = new StatisticReportInfo
                    {
                        ShortName = string.Format(
                            Labels.StatisticReport_TotalTitle,
                            dateGroup.Key.ToMonthYear()),
                        ChildReports = GetDummyCountryList(countryLength, countryList)
                    }
                };

                dateReport.PeriodRange.FromDateString = dateReport.PeriodRange.FromDate.ToShortFormatedDate();
                dateReport.PeriodRange.ToDateString = dateReport.PeriodRange.ToDate.ToShortFormatedDate();
                var timeGroups = dateGroup.ToList()
                    .GroupBy(data => new DateTime(
                        dateGroup.Key.Year,
                        dateGroup.Key.Month,
                        dateGroup.Key.Day,
                        data.DateTime.Hour,
                        data.DateTime.Minute,
                        0));
                foreach (var timeGroup in timeGroups)
                {
                    var timeReport = new StatisticReportInfo
                    {
                        ShortName = timeGroup.Key.ToHourMinuteFormated(),
                        ChildReports = GetDummyCountryList(countryLength, countryList)
                    };

                    foreach (var timeData in timeGroup)
                    {
                        var countryIndex = countryDictionary[timeData.CountryId];
                        var reportInfo = new StatisticReportInfo
                        {
                            CountryInfo = new CountryInfo
                            {
                                Id = timeData.CountryId,
                                Name = timeData.CountryName
                            },
                            HasData = true,
                            NumberOfIssues = timeData.NumberOfIssues
                        };

                        if (!timeReport.ChildReports[countryIndex].HasData)
                        {
                            timeReport.ChildReports[countryIndex] = reportInfo;
                        }
                        else
                        {
                            timeReport.ChildReports[countryIndex].NumberOfIssues += reportInfo.NumberOfIssues;
                        }

                        timeReport.NumberOfIssues += reportInfo.NumberOfIssues;
                        dateReport.Total.ChildReports[countryIndex].NumberOfIssues += reportInfo.NumberOfIssues;
                        statisticReport.Total.ChildReports[countryIndex].NumberOfIssues += reportInfo.NumberOfIssues;
                    }

                    dateReport.Total.NumberOfIssues += timeReport.NumberOfIssues;
                    dateReport.ChildReports.Add(timeReport);
                }

                statisticReport.Total.NumberOfIssues += dateReport.Total.NumberOfIssues;
                statisticReport.ChildReports.Add(dateReport);
            }

            return statisticReport;
        }

        /// <summary>
        /// Gets the statistic report by daily.
        /// </summary>
        /// <param name="rawData">The raw data.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The statistic report by daily.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private StatisticReportInfo GetStatisticReportByDaily(
            List<StatisticReportEntity> rawData,
            List<CountryInfo> countryList,
            Dictionary<int, int> countryDictionary,
            GetStatisticReportCriteria criteria)
        {
            var countryLength = countryDictionary.Count;
            StatisticReportInfo statisticReport = new StatisticReportInfo
            {
                ChildReports = new List<StatisticReportInfo>(),
                Name = string.Format(
                    Labels.StatisticReport_Title,
                    Labels.Daily,
                    DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToShortFormatedDate(),
                    DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.ToDate).ToShortFormatedDate()),
                ShortName = string.Format(
                    Labels.StatisticReport_ShortTitle,
                    DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToShortFormatedDate(),
                    DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.ToDate).ToShortFormatedDate()),
                Total = new StatisticReportInfo
                {
                    ShortName = Labels.Grand_Total,
                    ChildReports = GetDummyCountryList(countryLength, countryList)
                }
            };

            if (rawData == null ||
                rawData.Count == 0)
            {
                return statisticReport;
            }

            var dateGroups = rawData.GroupBy(data => data.DateTime.ToShortFormatedDate());
            foreach (var dateGroup in dateGroups)
            {
                var dateReport = new StatisticReportInfo
                {
                    ShortName = dateGroup.Key.ToString(),
                    ChildReports = new List<StatisticReportInfo>(),
                    Total = new StatisticReportInfo
                    {
                        ShortName = string.Format(
                            Labels.StatisticReport_TotalTitle,
                            dateGroup.Key.ToString()),
                        ChildReports = GetDummyCountryList(countryLength, countryList)
                    }
                };

                var timeGroups = dateGroup.ToList()
                    .GroupBy(data => data.DateTime.ToHourMinuteFormated());
                foreach (var timeGroup in timeGroups)
                {
                    var timeReport = new StatisticReportInfo
                    {
                        ShortName = timeGroup.Key.ToString(),
                        ChildReports = GetDummyCountryList(countryLength, countryList)
                    };

                    foreach (var timeData in timeGroup)
                    {
                        var countryIndex = countryDictionary[timeData.CountryId];
                        var reportInfo = new StatisticReportInfo
                        {
                            CountryInfo = new CountryInfo
                            {
                                Id = timeData.CountryId,
                                Name = timeData.CountryName
                            },
                            HasData = true,
                            NumberOfIssues = timeData.NumberOfIssues
                        };

                        timeReport.ChildReports[countryIndex] = reportInfo;
                        timeReport.NumberOfIssues += reportInfo.NumberOfIssues;
                        dateReport.Total.ChildReports[countryIndex].NumberOfIssues += reportInfo.NumberOfIssues;
                        statisticReport.Total.ChildReports[countryIndex].NumberOfIssues += reportInfo.NumberOfIssues;
                    }

                    dateReport.Total.NumberOfIssues += timeReport.NumberOfIssues;
                    dateReport.ChildReports.Add(timeReport);
                }

                statisticReport.Total.NumberOfIssues += dateReport.Total.NumberOfIssues;
                statisticReport.ChildReports.Add(dateReport);
            }

            return statisticReport;
        }

        /// <summary>
        /// Gets the paging log list.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="dynamicReportList">The statistic report list.</param>
        /// <returns>The paging log list.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private List<StatisticReportInfo> GetPagingLogList(
            GetStatisticReportCriteria criteria,
            List<StatisticReportInfo> statisticReportList)
        {
            return statisticReportList.Skip(criteria.PageSize * criteria.PageIndex)
                .Take(criteria.PageSize)
                .ToList();
        }

        /// <summary>
        /// Gets the country dictionary.
        /// </summary>
        /// <param name="countryList">The country list.</param>
        /// <returns>The country dictionary.</returns>
        private Dictionary<int, int> GetCountryDictionary(List<CountryInfo> countryList)
        {
            Dictionary<int, int> countryDictionary = new Dictionary<int, int>();
            var countryLength = countryList.Count;
            for (var i = 0; i < countryLength; i++)
            {
                var country = countryList[i];
                if (countryDictionary.ContainsKey(country.Id))
                {
                    continue;
                }

                countryDictionary.Add(country.Id, i);
            }

            return countryDictionary;
        }

        private List<StatisticReportInfo> GetDummyCountryList(
            int countryLength,
            List<CountryInfo> countryList)
        {
            List<StatisticReportInfo> dummyCountries = new List<StatisticReportInfo>();
            for (var i = 0; i < countryLength; i++)
            {
                dummyCountries.Add(new StatisticReportInfo
                {
                    CountryInfo = countryList[i]
                });
            }

            return dummyCountries;
        }

        private StatisticReportInfo GetChartInfo(
            StatisticReportInfo statisticReport,
            List<CountryInfo> countryList,
            Dictionary<int, int> countryDictionary)
        {
            var chartInfo = new StatisticReportInfo
            {
                Name = statisticReport.Name,
                ShortName = statisticReport.ShortName,
                NumberOfIssues = statisticReport.Total.NumberOfIssues,
                ChildReports = new List<StatisticReportInfo>()
            };

            var countryLength = countryDictionary.Count;
            var dateReportLength = statisticReport.ChildReports.Count;
            for (var dateIndex = 0; dateIndex < dateReportLength; dateIndex++)
            {
                var dateReport = statisticReport.ChildReports[dateIndex];
                var dateChart = new StatisticReportInfo
                {
                    ShortName = dateReport.ShortName,
                    NumberOfIssues = dateReport.Total.NumberOfIssues,
                    ChildReports = new List<StatisticReportInfo>()
                };

                var timeReportLength = dateReport.ChildReports.Count;
                for (var timeIndex = 0; timeIndex < timeReportLength; timeIndex++)
                {
                    var timeReport = dateReport.ChildReports[timeIndex];
                    var timeChart = new StatisticReportInfo
                    {
                        ShortName = timeReport.ShortName,
                        NumberOfIssues = timeReport.NumberOfIssues,
                        ChildReports = new List<StatisticReportInfo>()
                    };

                    for (var i = 0; i < countryLength; i++)
                    {
                        var countryReport = timeReport.ChildReports[i];
                        timeChart.ChildReports.Add(new StatisticReportInfo
                        {
                            HasData = countryReport.HasData,
                            NumberOfIssues = countryReport.NumberOfIssues,
                            CountryInfo = countryReport.CountryInfo
                        });
                    }

                    dateChart.ChildReports.Add(timeChart);
                }

                chartInfo.ChildReports.Add(dateChart);
            }

            return chartInfo;
        }
    }
}
