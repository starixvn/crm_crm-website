// --------------------------------------------------------------------------
// <copyright file="DailyStatisticsExportExcel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Service.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Web;
    using DNAide.Core;
    using ExcelUtil.Spreadsheet;
    using Fanex.CRM.Entities.ViewModel;
    using Fanex.CRM.Infrastructure.Configuration;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Helper;
    using Fanex.CRM.Infrastructure.Logging;

    /// <summary>
    /// Export 2 excel service.
    /// </summary>
    public partial class FeedbackService
    {
        /// <summary>
        /// Export 2 excel - daily summary report.
        /// </summary>
        /// <param name="strFromDate">From date.</param>
        /// <param name="strToDate">To date value.</param>
        /// <param name="reportTitle">Report title.</param>
        public void ExportExcelIBCDailySummaryReport(string strFromDate, string strToDate, string reportTitle)
        {
            try
            {
                var excelFile = new ExcelFile();
                var reportName = "IBCSummaryReport";

                var getData = (DailySummaryBO)SessionExtension.GetSession(HttpContext.Current.Session, _ibcDailySummary);
                var fromDate = CastAide.AsDateTime(strFromDate);
                var toDate = CastAide.AsDateTime(strToDate);
                if (getData == null)
                {
                    getData = GetIBCDailySummaryReport(strFromDate, strToDate);
                }

                ExportIBCDailySummaryReport(getData, fromDate, toDate, excelFile, reportTitle);
                ReponseExcelFile(excelFile, string.Format("{0}-{1:MMddyyyy}-{2:MMddyyyy}", reportName, fromDate, toDate));
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(ExportExcelIBCDailySummaryReport) - {0}", ex.Message));
            }
        }

        /// <summary>
        /// Export 2 excel - daily detail report.
        /// </summary>
        /// <param name="strFromDate">From date.</param>
        /// <param name="strToDate">To date value.</param>
        /// <param name="reportTitle">Report title.</param>
        /// <param name="email">Email address.</param>
        /// <param name="isSendMail">Is sending mail.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public void ExportExcelIBCDailyDetailReport(string strFromDate, string strToDate, string reportTitle, string email, bool isSendMail = false)
        {
            try
            {
                var excelFile = new ExcelFile();
                var reportName = "IBCDetailReport";
                var getData = (DailyComplaintsDTO)SessionExtension.GetSession(HttpContext.Current.Session, _ibcDailyDetail);

                var fromDate = CastAide.AsDateTime(strFromDate);
                var toDate = CastAide.AsDateTime(strToDate);
                if (isSendMail)
                {
                    DataTable sessionData = null;
                    if (getData == null)
                    {
                        sessionData = (DataTable)SessionExtension.GetSession(HttpContext.Current.Session, _ibcDailyDetailNotMapped);
                    }

                    var dailyComplaintsData = new DailyComplaintsDTO();
                    dailyComplaintsData.DailyComplaints = new List<DailyComplaintsDetail>();

                    var fileName = string.Format("{0}-{1:MMddyyyy}-{2:MMddyyyy}", reportName, fromDate, toDate);
                    getData = getData ?? MapperIBCDailyDetailReport(sessionData.AsEnumerable().Select(c => c));
                    if (getData != null)
                    {
                        ExportDailyComplaintsDetail(getData, fromDate, toDate, excelFile, reportTitle);
                        if (!Directory.Exists(WebConfigApplicationSettings.ReportFolderPath))
                        {
                            Directory.CreateDirectory(WebConfigApplicationSettings.ReportFolderPath);
                        }

                        excelFile.SaveXls(string.Format(@"{0}\{1}.xls", WebConfigApplicationSettings.ReportFolderPath, fileName));
                        SendMailAndDeleteFile(email, fileName, strFromDate, strToDate, "Daily");
                    }
                }
                else if (getData != null)
                {
                    ExportDailyComplaintsDetail(getData, fromDate, toDate, excelFile, reportTitle);
                    ReponseExcelFile(excelFile, string.Format("{0}-{1:MMddyyyy}-{2:MMddyyyy}", reportName, fromDate, toDate));
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }
    }
}
