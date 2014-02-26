// --------------------------------------------------------------------------
// <copyright file="DynamicReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.Implements
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.CRM.Infrastructure.Core;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Criteria;
    using Fanex.CRM.Report.Models;
    using Fanex.CRM.Report.Repositories.Implements;
    using Fanex.CRM.Report.Repositories.Interfaces;
    using Fanex.CRM.Report.Resources;
    using Fanex.CRM.Report.Services.Interfaces;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Models;
    using Fanex.CRM.Setting.Services.Implements;
    using Fanex.CRM.Setting.Services.Interfaces;
    using Fanex.CRM.Infrastructure.Extension;

    /// <summary>
    /// Dynamic Report Service.
    /// </summary>
    public class DynamicReportService : IDynamicReportService
    {
        private IDynamicReportRepository _dynamicReportRepository;

        private ISettingService _serviceSetting;

        public DynamicReportService()
            : this(null, null)
        {
        }

        public DynamicReportService(
            IDynamicReportRepository dynamicReportRepository,
            ISettingService serviceSetting)
        {
            _dynamicReportRepository = dynamicReportRepository ?? new DynamicReportRepository();
            _serviceSetting = serviceSetting ?? new SettingService();
        }

        /// <summary>
        /// Gets all period type.
        /// </summary>
        /// <returns>All period type.</returns>
        public List<SettingInfo> GetAllPeriodType()
        {
            var allPeriodType = new List<SettingInfo>();
            try
            {
                allPeriodType.Add(new SettingInfo
                {
                    Id = PeriodTypeConstants.Daily,
                    Name = Labels.Daily
                });
                allPeriodType.Add(new SettingInfo
                {
                    Id = PeriodTypeConstants.Weekly,
                    Name = Labels.Weekly
                });
                allPeriodType.Add(new SettingInfo
                {
                    Id = PeriodTypeConstants.Monthly,
                    Name = Labels.Monthly
                });
                allPeriodType.Add(new SettingInfo
                {
                    Id = PeriodTypeConstants.Yearly,
                    Name = Labels.Yearly
                });
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allPeriodType;
        }

        /// <summary>
        /// Gets all product.
        /// </summary>
        /// <returns>All product.</returns>      
        public List<SettingInfo> GetAllProduct()
        {
            var allProduct = new List<SettingInfo>();
            try
            {
                var rawData = _serviceSetting.GetAllProduct();
                foreach (var rawProduct in rawData)
                {
                    var product = rawProduct;
                    if (rawProduct.Id == SettingConstants.NotApply)
                    {
                        product.Value = Labels.All;
                    }

                    allProduct.Add(product);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allProduct;
        }

        /// <summary>
        /// Gets all site.
        /// </summary>
        /// <returns>All site value.</returns>
        public List<SiteInfo> GetAllSite()
        {
            var allSite = new List<SiteInfo>();
            try
            {
                allSite = _serviceSetting.GetAllSite();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allSite;
        }

        /// <summary>
        /// Gets all status.
        /// </summary>
        /// <returns>All status.</returns>
        public List<SettingInfo> GetAllStatus()
        {
            var allStatus = new List<SettingInfo>();
            try
            {
                var rawData = _serviceSetting.GetAllStatus();
                allStatus.Add(new SettingInfo
                {
                    Id = SettingConstants.NotApply,
                    Name = Labels.All
                });

                foreach (var rawStatus in rawData)
                {
                    allStatus.Add(rawStatus);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allStatus;
        }

        /// <summary>
        /// Gets all response.
        /// </summary>
        /// <returns>All response.</returns>
        public List<SettingInfo> GetAllResponse()
        {
            var allResponse = new List<SettingInfo>();
            try
            {
                var rawData = _serviceSetting.GetAllResponse();
                allResponse.Add(new SettingInfo
                {
                    Id = SettingConstants.NotApply,
                    Value = Labels.All
                });

                foreach (var rawResponse in rawData)
                {
                    allResponse.Add(rawResponse);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allResponse;
        }

        /// <summary>
        /// Gets all role.
        /// </summary>
        /// <returns>All role value.</returns>
        public List<SettingInfo> GetAllRole()
        {
            var allRole = new List<SettingInfo>();
            try
            {
                var rawData = _serviceSetting.GetAllResponse();
                allRole.Add(new SettingInfo
                {
                    Id = SettingConstants.NotApply,
                    Value = Labels.All
                });

                foreach (var rawRole in rawData)
                {
                    allRole.Add(rawRole);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allRole;
        }

        /// <summary>
        /// Gets all communication tool.
        /// </summary>
        /// <returns>All communication tool.</returns>
        public List<SettingInfo> GetAllCommunicationTool()
        {
            var allCommunicationTool = new List<SettingInfo>();
            try
            {
                var rawData = _serviceSetting.GetAllResponse();
                allCommunicationTool.Add(new SettingInfo
                {
                    Id = SettingConstants.NotApply,
                    Value = Labels.All
                });

                foreach (var rawCommunicationTool in rawData)
                {
                    allCommunicationTool.Add(rawCommunicationTool);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allCommunicationTool;
        }

        /// <summary>
        /// Gets all main category.
        /// </summary>
        /// <returns>All main category.</returns>
        public List<SettingInfo> GetAllMainCategory()
        {
            var allMainCategory = new List<SettingInfo>();
            try
            {
                var rawData = _serviceSetting.GetAllMainCategory();
                allMainCategory.Add(new SettingInfo
                {
                    Id = SettingConstants.NotApply,
                    Name = Labels.All,
                    ChildSettings = new List<SettingInfo>
                    {
                        new SettingInfo
                        {
                            Id = SettingConstants.NotApply,
                            Name = Labels.All,
                        }
                    }
                });

                foreach (var rawMainCategory in rawData)
                {
                    allMainCategory.Add(rawMainCategory);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allMainCategory;
        }

        /// <summary>
        /// Gets all extension.
        /// </summary>
        /// <returns>All extension.</returns>
        public List<SettingInfo> GetAllExtension()
        {
            var allExtension = new List<SettingInfo>();
            try
            {
                var rawData = _serviceSetting.GetAllExtension();
                foreach (var rawExtension in rawData)
                {
                    if (rawExtension.Id == ExtensionConstants.NotApply)
                    {
                        rawExtension.Value = Labels.All;
                    }

                    allExtension.Add(rawExtension);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allExtension;
        }

        /// <summary>
        /// Gets the dynamic report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The dynamic report.</returns>      
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public DynamicReport GetDynamicReport(GetDynamicReportCriteria criteria)
        {
            DynamicReport dynamicReport = null;
            try
            {
                criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
                criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);
                //var rawData = _dynamicReportRepository.GetDynamicReport(criteria);
                var rawData = new List<DynamicReport>
                {
                    new DynamicReport 
                    {
                        RunningNumber = 10500,
                        FeedbackDate = new DateTime(2014,2,26),
                        MainCategory = new SettingInfo
                        {
                            Name = "General Network/Database Issues"
                        },
                        SubCategory = new SettingInfo
                        {
                            Name = "Agency Site Network Issue"
                        },
                        Subject = "New super 2",
                        StatusName = "Open",
                        Duration = new TimeInfo
                        {
                            Hours = 1,
                            Minutes = 0,
                            Seconds = 0
                        },
                        Handler = "sa",
                        Reviewer ="sa"
                    },
                    new DynamicReport 
                    {
                        RunningNumber = 41844,
                        FeedbackDate = new DateTime(2014,2,26),
                        MainCategory = new SettingInfo
                        {
                            Name = "General Network/Database Issues"
                        },
                        SubCategory = new SettingInfo
                        {
                            Name = "Agency Site Network Issue"
                        },
                        Subject = "New super 2",
                        StatusName = "Open",
                        Duration = new TimeInfo
                        {
                            Hours = 1,
                            Minutes = 0,
                            Seconds = 0
                        },
                        Handler = "sa",
                        Reviewer ="sa"
                    },
                    new DynamicReport 
                    {
                        RunningNumber = 41843,
                        FeedbackDate = new DateTime(2014,2,26),
                        MainCategory = new SettingInfo
                        {
                            Name = "General Network/Database Issues"
                        },
                        SubCategory = new SettingInfo
                        {
                            Name = "Agency Site Network Issue"
                        },
                        Subject = "New super 2",
                        StatusName = "Open",
                        Duration = new TimeInfo
                        {
                            Hours = 1,
                            Minutes = 0,
                            Seconds = 0
                        },
                        Handler = "sa",
                        Reviewer ="sa"
                    },
                    new DynamicReport 
                    {
                        RunningNumber = 41840,
                        FeedbackDate = new DateTime(2014,1,25),
                        MainCategory = new SettingInfo
                        {
                            Name = "General Network/Database Issues"
                        },
                        SubCategory = new SettingInfo
                        {
                            Name = "Agency Site Network Issue"
                        },
                        Subject = "New super 2",
                        StatusName = "Open",
                        Duration = new TimeInfo
                        {
                            Hours = 1,
                            Minutes = 0,
                            Seconds = 0
                        },
                        Handler = "sa",
                        Reviewer ="sa"
                    },
                    new DynamicReport 
                    {
                        RunningNumber = 41838,
                        FeedbackDate = new DateTime(2012,1,24),
                        MainCategory = new SettingInfo
                        {
                            Name = "General Network/Database Issues"
                        },
                        SubCategory = new SettingInfo
                        {
                            Name = "Agency Site Network Issue"
                        },
                        Subject = "New super 2",
                        StatusName = "Open",
                        Duration = new TimeInfo
                        {
                            Hours = 1,
                            Minutes = 0,
                            Seconds = 0
                        },
                        Handler = "sa",
                        Reviewer ="sa"
                    },
                    new DynamicReport 
                    {
                        RunningNumber = 41836,
                        FeedbackDate = new DateTime(2012,1,24),
                        MainCategory = new SettingInfo
                        {
                            Name = "General Network/Database Issues"
                        },
                        SubCategory = new SettingInfo
                        {
                            Name = "Agency Site Network Issue"
                        },
                        Subject = "New super 2",
                        StatusName = "Open",
                        Duration = new TimeInfo
                        {
                            Hours = 1,
                            Minutes = 0,
                            Seconds = 0
                        },
                        Handler = "sa",
                        Reviewer ="sa"
                    }
                };

                if (rawData == null ||
                    rawData.Count == 0)
                {
                    return dynamicReport;
                }

                switch (criteria.ReportType)
                {
                    case PeriodTypeConstants.Daily:
                        dynamicReport = GetDynamicReportByDaily(rawData, criteria);
                        break;
                    case PeriodTypeConstants.Weekly:
                        dynamicReport = GetDynamicReportByDaily(rawData, criteria);
                        break;
                    case PeriodTypeConstants.Monthly:
                        dynamicReport = GetDynamicReportByDaily(rawData, criteria);
                        break;
                    case PeriodTypeConstants.Yearly:
                        dynamicReport = GetDynamicReportByDaily(rawData, criteria);
                        break;
                    default:
                        break;
                }

                dynamicReport.ReportTypeId = criteria.ReportType;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return dynamicReport;
        }

        private DynamicReport GetDynamicReportByDaily(
            List<DynamicReport> rawData,
            GetDynamicReportCriteria criteria)
        {
            DynamicReport dynamicReport = new DynamicReport
            {
                ChildReports = new List<DynamicReport>(),
                Name = string.Format(
                    Labels.DynamicReport_Title,
                    Labels.Daily,
                    DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToShortFormatedDate(),
                    DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.ToDate).ToShortFormatedDate())
            };

            var periodGroups = rawData.GroupBy(data => data.FeedbackDate);
            foreach (var periodGroup in periodGroups)
            {
                var periodReport = new DynamicReport
                {
                    Name = string.Format(
                        Labels.DynamicReport_DailyReport_Title,
                        Labels.Daily,
                        periodGroup.Key.ToShortFormatedDate()),
                    ChildReports = new List<DynamicReport>()
                };

                foreach (var data in periodGroup)
                {
                    data.FeedbackDate = DateTimeUtilize.ChangeTimeZoneToClientDateTime(data.FeedbackDate);
                    periodReport.ChildReports.Add(data);
                }

                dynamicReport.ChildReports.Add(periodReport);
            }

            return dynamicReport;
        }
    }
}
