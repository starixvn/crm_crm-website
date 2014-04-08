// --------------------------------------------------------------------------
// <copyright file="SummaryReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.04.08">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.Implements.SummaryReport
{
    using System;
    using Fanex.CRM.Report.Criteria.SummaryReport;
    using Fanex.CRM.Report.Models.SummaryReport;
    using Fanex.CRM.Report.Repositories.Implements;
    using Fanex.CRM.Report.Repositories.Interfaces;
    using Fanex.CRM.Report.Services.Interfaces;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Report.Resources;
    using System.Web;
    using Fanex.CRM.Infrastructure.Logging;
    using System.Collections.Generic;

    /// <summary>
    /// SummaryReportService class.
    /// </summary>
    public class SummaryReportService : ISummaryReportService
    {
        private const string SummaryReportSessionKey = "Fanex.CRM.Report.SummaryReport.SummaryReportSessionKey";
        private readonly ISummaryReportRepository _summaryRepository;

        public SummaryReportService()
            : this(null)
        {
        }

        public SummaryReportService(ISummaryReportRepository summaryRepository)
        {
            _summaryRepository = summaryRepository ?? new SummaryReportRepository();
        }

        public SummaryReportModel GetSummaryReport(GetSummaryReportCriteria criteria)
        {
            SummaryReportModel report = null;
#if DEBUG
            report = new SummaryReportModel
            {
                ReportTitle = "Summary Report for Year 2014",
                GrandTotal = new SubCategorySummary
                {
                    SubCategoryName = "Grand Total",
                    Summaries = new List<Summary>
                    {
                        new Summary { Id = 1, Month = "Jan", Amount = 100 },
                        new Summary { Id = 2, Month = "Feb", Amount = 200 },
                        new Summary { Id = 3, Month = "Mar", Amount = 300 },
                        new Summary { Id = 4, Month = "Apr", Amount = 400 },
                        new Summary { Id = 5, Month = "May", Amount = 500 },
                        new Summary { Id = 6, Month = "Jun", Amount = 600 },
                        new Summary { Id = 7, Month = "Jul", Amount = 700 },
                        new Summary { Id = 8, Month = "Aug", Amount = 800 },
                        new Summary { Id = 9, Month = "Sep", Amount = 900 },
                        new Summary { Id = 10, Month = "Oct", Amount = 1000 },
                        new Summary { Id = 11, Month = "Nov", Amount = 1100 },
                        new Summary { Id = 12, Month = "Dec", Amount = 1200 }
                    }
                },
                CategorySummaries = new List<CategorySummary> 
                {
                    new CategorySummary
                    {
                        CategoryName = "Agency DB/System Issue",
                        CategorySummaries = new List<Summary>
                        {
                            new Summary { Id = 1, Month = "Jan", Amount = 12 },
                            new Summary { Id = 2, Month = "Feb", Amount = 4 },
                            new Summary { Id = 3, Month = "Mar", Amount = 9 },
                            new Summary { Id = 4, Month = "Apr", Amount = 8 },
                            new Summary { Id = 5, Month = "May", Amount = 5 },
                            new Summary { Id = 6, Month = "Jun", Amount = 19 },
                            new Summary { Id = 7, Month = "Jul", Amount = 7 },
                            new Summary { Id = 8, Month = "Aug", Amount = 0 },
                            new Summary { Id = 9, Month = "Sep", Amount = 0 },
                            new Summary { Id = 10, Month = "Oct", Amount = 100 },
                            new Summary { Id = 11, Month = "Nov", Amount = 88 },
                            new Summary { Id = 12, Month = "Dec", Amount = 98765 }
                        }
                    },
                    new CategorySummary
                    {
                        CategoryName = "Agency Network Issue",
                        CategorySummaries = new List<Summary>
                        {
                            new Summary { Id = 1, Month = "Jan", Amount = 12 },
                            new Summary { Id = 2, Month = "Feb", Amount = 4 },
                            new Summary { Id = 3, Month = "Mar", Amount = 9 },
                            new Summary { Id = 4, Month = "Apr", Amount = 8 },
                            new Summary { Id = 5, Month = "May", Amount = 5 },
                            new Summary { Id = 6, Month = "Jun", Amount = 19 },
                            new Summary { Id = 7, Month = "Jul", Amount = 7 },
                            new Summary { Id = 8, Month = "Aug", Amount = 0 },
                            new Summary { Id = 9, Month = "Sep", Amount = 0 },
                            new Summary { Id = 10, Month = "Oct", Amount = 100 },
                            new Summary { Id = 11, Month = "Nov", Amount = 88 },
                            new Summary { Id = 12, Month = "Dec", Amount = 98765 }
                        },
                        SubCategorySummaries = new List<SubCategorySummary>
                        {
                            new SubCategorySummary
                            {
                                SubCategoryName = "Member Issue",
                                Summaries = new List<Summary>
                                {
                                    new Summary { Id = 1, Month = "Jan", Amount = 12 },
                                    new Summary { Id = 2, Month = "Feb", Amount = 4 },
                                    new Summary { Id = 3, Month = "Mar", Amount = 9 },
                                    new Summary { Id = 4, Month = "Apr", Amount = 8 },
                                    new Summary { Id = 5, Month = "May", Amount = 5 },
                                    new Summary { Id = 6, Month = "Jun", Amount = 19 },
                                    new Summary { Id = 7, Month = "Jul", Amount = 7 },
                                    new Summary { Id = 8, Month = "Aug", Amount = 0 },
                                    new Summary { Id = 9, Month = "Sep", Amount = 0 },
                                    new Summary { Id = 10, Month = "Oct", Amount = 100 },
                                    new Summary { Id = 11, Month = "Nov", Amount = 88 },
                                    new Summary { Id = 12, Month = "Dec", Amount = 98765 }
                                }
                            },
                            new SubCategorySummary
                            {
                                SubCategoryName = "SMA Issue",
                                Summaries = new List<Summary>
                                {
                                    new Summary { Id = 1, Month = "Jan", Amount = 12 },
                                    new Summary { Id = 2, Month = "Feb", Amount = 4 },
                                    new Summary { Id = 3, Month = "Mar", Amount = 9 },
                                    new Summary { Id = 4, Month = "Apr", Amount = 8 },
                                    new Summary { Id = 5, Month = "May", Amount = 5 },
                                    new Summary { Id = 6, Month = "Jun", Amount = 19 },
                                    new Summary { Id = 7, Month = "Jul", Amount = 7 },
                                    new Summary { Id = 8, Month = "Aug", Amount = 0 },
                                    new Summary { Id = 9, Month = "Sep", Amount = 0 },
                                    new Summary { Id = 10, Month = "Oct", Amount = 100 },
                                    new Summary { Id = 11, Month = "Nov", Amount = 88 },
                                    new Summary { Id = 12, Month = "Dec", Amount = 98765 }
                                }
                            }
                        }
                    }
                }
            };
#else
            string sessionKey = SummaryReportSessionKey;
            try
            {
                report = (SummaryReportModel)SessionExtension.GetSession(HttpContext.Current.Session, sessionKey);
                if (report == null)
                {
                    report = _summaryRepository.GetSummaryReport(criteria);
                    report.ReportTitle = string.Format(Labels.SummaryReportTitle, criteria.Year);

                    // Add to session.
                    if (report != null)
                    {
                        SessionExtension.AddSession(HttpContext.Current.Session, sessionKey, report);
                    }
                }
            }
            catch (Exception ex)
            {
                report = null;
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
#endif
            return report;
        }

        public void ExportSummaryReportExcel(GetSummaryReportCriteria criteria)
        {
            throw new NotImplementedException();
        }
    }
}
