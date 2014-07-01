// --------------------------------------------------------------------------
// <copyright file="FeedbackStatisticsExportExcel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Service.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using DNAide.Core;
    using ExcelUtil.Spreadsheet;
    using Fanex.CRM.Entities;
    using Fanex.CRM.Entities.ViewModel;
    using Fanex.CRM.Infrastructure.Configuration;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Helper;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Infrastructure.Service.Implements;

    /// <summary>
    /// Export 2 excel service.
    /// </summary>
    public partial class FeedbackService
    {
        #region feedback statistic detail
        /// <summary>
        /// Export 2 Excel Feedback Statistic Detail.
        /// </summary>
        /// <param name="type">Filter by: Creator/country/role...</param>
        /// <param name="valueId">Value of creator/country/role...</param>
        /// <param name="mainCategoryId">Main Category Id.</param>
        /// <param name="subCategoryId">Sub Category Id.</param>
        /// <param name="fromDate">From Date.</param>
        /// <param name="toDate">To Date value.</param>
        /// <param name="reportTitle">Report Title.</param>
        public void ExportExcelFeedbackStatisticDetail(int type, int valueId, int mainCategoryId, int subCategoryId, string fromDate, string toDate, string reportTitle)
        {
            try
            {
                var getSessionData = CacheService.Get<DataGridViewDTO>(_feedbackStatisticDetailMappedSessionKey);

                var startRow = 2;
                var startCol = 0;
                var reportName = "FeedbackStatisticDetail";
                var excelFile = new ExcelFile();
                var sheetData = excelFile.Worksheets.Add(reportName);
                var nextRow = 0;

                sheetData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 4).Merged = true;
                sheetData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 4).Style.Font.Weight = ExcelFont.BoldWeight;
                sheetData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 4).Value = reportTitle;
                ExportExcelFeedbackStatisticDetailAssignHeader(ref sheetData, startRow, startCol);

                if (getSessionData != null && getSessionData.Rows.Count > 0)
                {
                    foreach (FeedbackView feedbackView in getSessionData.Rows)
                    {
                        ExportExcelFeedbackStatisticDetailAssignContent(ref sheetData, startRow, startCol, ref nextRow, feedbackView);
                    }
                }
                else
                {
                    sheetData.Cells[startRow, startCol].Style.Font.Color = Color.Red;
                    sheetData.Cells[startRow, startCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblThereisnoinfomation");
                }

                ReponseExcelFile(excelFile, string.Format("{0}-{1:MMddyyyy}-{2:MMddyyyy}", reportName, CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate)));
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }
        #endregion

        #region feedback statistic

        /// <summary>
        /// Export 2 Excel Feedback Statistic.
        /// </summary>
        /// <param name="type">Filter by: Creator/country/role...</param>
        /// <param name="fromDate">From Date.</param>
        /// <param name="toDate">To Date value.</param>
        /// <param name="reportTitle">Report Title.</param>
        public void ExportExcelFeedbackStatistic(int type, string fromDate, string toDate, string reportTitle)
        {
            try
            {
                var param = new Params()
                {
                    Type = type,
                    FromDate = fromDate,
                    ToDate = toDate,
                    FromDuration = new SolvingDuration() { Day = "0", Hour = "0", Minute = "0", Second = "0" },
                    ToDuration = new SolvingDuration() { Day = "0", Hour = "0", Minute = "0", Second = "0" }
                };

                var getSessionData =
                    (FeedbackStatisticsDTO)SessionExtension.GetSession(HttpContext.Current.Session, _feedbackStatisticSessionKey) ?? SelAllFeedbackStatistic(param);
                var startRow = 2;
                var startCol = 0;
                var excelFile = new ExcelFile();
                var sheetData = excelFile.Worksheets.Add("FeedbackStatistic");
                var nextRow = 0;

                if (getSessionData != null)
                {
                    sheetData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 2).Merged = true;
                    sheetData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 2).Style.Font.Weight = ExcelFont.BoldWeight;
                    sheetData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 2).Value = reportTitle;

                    ExportExcelFeedbackStatisticAssignHeader(ref sheetData, startRow, startCol, getSessionData.ColumnNames);
                    foreach (FeedbackStatisticsGroup feedbackGroup in getSessionData.Rows)
                    {
                        ExportExcelFeedbackStatisticAssignContent(ref sheetData, startRow, startCol, ref nextRow, feedbackGroup, getSessionData.ColumnNames);
                    }

                    ExportExcelFeedbackStatisticAssignFooter(ref sheetData, startRow, startCol, ref nextRow, getSessionData.GrandTotal);
                }

                ReponseExcelFile(excelFile, string.Format("{0}-{1:MMddyyyy}-{2:MMddyyyy}", "FeedbackStatistic", CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate)));
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }
        #endregion

        /// <summary>
        /// Checking to allow exporting excel.
        /// </summary>
        /// <returns>Return result.</returns>
        public Result AllowExportFeedbackStatisticDetail()
        {
            var result = new Result();
            try
            {
                var getSessionData = CacheService.Get<DataGridViewDTO>(_feedbackStatisticDetailMappedSessionKey);
                if (getSessionData != null && getSessionData.Rows.Count > 0)
                {
                    result = new Result(0, "Succesfully");
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ref result, ex);
                Helper.SendMailErrorNotice(string.Format("(AllowExportFeedbackStatisticDetail) - {0}", ex.Message));
            }

            return result;
        }

        #region export licensee summary
        /// <summary>
        /// Excel 2 excel - Licensee daily summary report.
        /// </summary>
        /// <param name="strFromDate">From date.</param>
        /// <param name="strToDate">To date value.</param>
        /// <param name="reportTitle">Report title.</param>
        public void ExportExcelLicenseeDailySummaryReport(string strFromDate, string strToDate, string reportTitle)
        {
            try
            {
                var excelFile = new ExcelFile();
                var reportName = "LicenseeSummaryReport";
                var getData = (DailySummaryBO)SessionExtension.GetSession(HttpContext.Current.Session, _licenseeDailySummary);
                var fromDate = CastAide.AsDateTime(strFromDate);
                var toDate = CastAide.AsDateTime(strToDate);
                if (getData == null)
                {
                    getData = GetIBCDailySummaryReport(strFromDate, strToDate);
                }

                ExportLicenseeDailySummaryReport(getData, fromDate, toDate, excelFile, reportTitle);
                ReponseExcelFile(excelFile, string.Format("{0}-{1:MMddyyyy}-{2:MMddyyyy}", reportName, fromDate, toDate));
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        #endregion

        #region export licensee detail report
        /// <summary>
        /// Export 2 excel - Licensee detail report.
        /// </summary>
        /// <param name="strFromDate">From date.</param>
        /// <param name="strToDate">To date value.</param>
        /// <param name="reportTitle">Report Title.</param>
        /// <param name="email">Email address.</param>
        /// <param name="isSendMail">Is sending mail.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public void ExportExcelLicenseeDetailReport(string strFromDate, string strToDate, string reportTitle, string email, bool isSendMail)
        {
            try
            {
                var excelFile = new ExcelFile();
                var reportName = "LicenseeDetailReport";
                var getData = (DailyComplaintsDTO)SessionExtension.GetSession(HttpContext.Current.Session, _licenseeDailyDetail);

                var fromDate = CastAide.AsDateTime(strFromDate);
                var toDate = CastAide.AsDateTime(strToDate);
                if (isSendMail)
                {
                    DataTable sessionData = null;
                    if (getData == null)
                    {
                        sessionData = (DataTable)SessionExtension.GetSession(HttpContext.Current.Session, _licenseeDailyDetailNotMapped);
                    }

                    //// export excel file to Hardware and send mail confirm to CSD
                    var dailyComplaintsData = new DailyComplaintsDTO();
                    dailyComplaintsData.DailyComplaints = new List<DailyComplaintsDetail>();

                    var fileName = string.Format("{0}-{1:MMddyyyy}-{2:MMddyyyy}", reportName, fromDate, toDate);
                    getData = getData ?? MapperLicenseeDailyDetailReport(sessionData.AsEnumerable().Select(c => c));
                    if (getData != null)
                    {
                        ExportLicenseeComplaintsDetail(getData, fromDate, toDate, excelFile, reportTitle);
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
                    ExportLicenseeComplaintsDetail(getData, fromDate, toDate, excelFile, reportTitle);
                    ReponseExcelFile(excelFile, string.Format("{0}-{1:MMddyyyy}-{2:MMddyyyy}", reportName, fromDate, toDate));
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        #endregion

        #region export casino summary
        /// <summary>
        /// Export 2 excel - Casino daily summary report.
        /// </summary>
        /// <param name="strFromDate">From date.</param>
        /// <param name="strToDate">To date value.</param>
        /// <param name="reportTitle">Report title.</param>
        public void ExportExcelCasinoDailySummaryReport(string strFromDate, string strToDate, string reportTitle)
        {
            try
            {
                var excelFile = new ExcelFile();
                var getCasinoData = (CasinoDailySummary)SessionExtension.GetSession(HttpContext.Current.Session, _casinoDailySummary);
                var fromDate = CastAide.AsDateTime(strFromDate);
                var toDate = CastAide.AsDateTime(strToDate);
                if (getCasinoData == null)
                {
                    getCasinoData = GetCasinoDailySummaryReport(strFromDate, strToDate);
                }

                ExportCasinoDailySummary(getCasinoData, fromDate, toDate, reportTitle, excelFile);
                ReponseExcelFile(excelFile, string.Format("{0}-{1:MMddyyyy}-{2:MMddyyyy}", "CasinoSummaryReport", fromDate, toDate));
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        #endregion

        #region export casino detail
        /// <summary>
        /// Export 2 excel - Casino daily report.
        /// </summary>
        /// <param name="strFromDate">From date.</param>
        /// <param name="strToDate">To date value.</param>
        /// <param name="reportTitle">Report title.</param>
        /// <param name="email">Email address.</param>
        /// <param name="isSendMail">Is sending mail.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public void ExportExcelCasinoDailyDetailReport(string strFromDate, string strToDate, string reportTitle, string email, bool isSendMail)
        {
            try
            {
                var excelFile = new ExcelFile();
                var reportName = "CasinoDetailReport";
                var getData = CacheService.Get<DailyComplaintsDTO>(_casinoDailyDetail);
                var fromDate = CastAide.AsDateTime(strFromDate);
                var toDate = CastAide.AsDateTime(strToDate);
                if (isSendMail)
                {
                    DataTable sessionData = null;
                    if (getData == null)
                    {
                        sessionData = (DataTable)SessionExtension.GetSession(HttpContext.Current.Session, _casinoDailyDetailNotMapped);
                    }

                    //// export excel file to Hardware and send mail confirm to CSD
                    var dailyComplaintsData = new DailyComplaintsDTO();
                    dailyComplaintsData.DailyComplaints = new List<DailyComplaintsDetail>();
                    Task.Factory.StartNew(() =>
                    {
                        Thread.Sleep(200);
                        var fileName = string.Format("{0}-{1:MMddyyyy}-{2:MMddyyyy}", reportName, fromDate, toDate);
                        getData = getData ?? MapperCasinoDailyDetailReport(sessionData.AsEnumerable().Select(c => c));
                        if (getData != null)
                        {
                            ExportCasinoDailyComplaintsDetail(getData, fromDate, toDate, excelFile, reportTitle);
                            if (!Directory.Exists(WebConfigApplicationSettings.ReportFolderPath))
                            {
                                Directory.CreateDirectory(WebConfigApplicationSettings.ReportFolderPath);
                            }

                            excelFile.SaveXls(string.Format(@"{0}\{1}.xls", WebConfigApplicationSettings.ReportFolderPath, fileName));
                            SendMailAndDeleteFile(email, fileName, strFromDate, strToDate, "Daily");
                        }
                    });
                }
                else if (getData != null)
                {
                    ExportCasinoDailyComplaintsDetail(getData, fromDate, toDate, excelFile, reportTitle);
                    ReponseExcelFile(excelFile, string.Format("{0}-{1:MMddyyyy}-{2:MMddyyyy}", reportName, fromDate, toDate));
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        #endregion

        #region export casino weekly summary
        /// <summary>
        /// Export 2 excel - Casino weekly summary report.
        /// </summary>
        /// <param name="strFromDate">From date.</param>
        /// <param name="strToDate">To date value.</param>
        /// <param name="reportTitle">Report title.</param>
        public void ExportExcelCasinoWeeklySummaryReport(string strFromDate, string strToDate, string reportTitle)
        {
            try
            {
                var excelFile = new ExcelFile();
                var reportName = "CasinoSummaryReport";
                var getCasinoData = (DailySummaryBO)SessionExtension.GetSession(HttpContext.Current.Session, _casinoWeeklySummary);
                if (getCasinoData == null)
                {
                    getCasinoData = GetCasinoWeeklySummaryReport(strFromDate, strToDate);
                }

                var fromDate = CastAide.AsDateTime(strFromDate);
                var toDate = CastAide.AsDateTime(strToDate);
                ExportCasinoWeeklySummaryReport(getCasinoData, fromDate, toDate, excelFile, reportTitle);
                ReponseExcelFile(excelFile, string.Format("{0}-{1:MMddyyyy}-{2:MMddyyyy}", reportName, fromDate, toDate));
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        #endregion

        /// <summary>
        /// Checking to allow export statistic detail.
        /// </summary>
        /// <returns>Return result.</returns>
        public Result AllowExportIbcStatisticDetail()
        {
            var result = new Result();
            try
            {
                var getSessionData = (DailyComplaintsDTO)SessionExtension.GetSession(HttpContext.Current.Session, _ibcDailyDetail);
                result = getSessionData != null && getSessionData.DailyComplaints.Count > 0 ?
                    new Result(0, "Succesfully") :
                    new Result(1, "Data has not mapped yet");
            }
            catch (Exception ex)
            {
                ExceptionHandler(ref result, ex);
            }

            return result;
        }

        /// <summary>
        /// Checking to allow export casino statistic detail.
        /// </summary>
        /// <returns>Return result.</returns>
        public Result AllowExportCasinoStatisticDetail()
        {
            var result = new Result();
            try
            {
                var getSessionData = CacheService.Get<DailyComplaintsDTO>(_casinoDailyDetail);
                result = getSessionData != null && getSessionData.DailyComplaints.Count > 0 ?
                    new Result(0, "Succesfully") :
                    new Result(1, "Data has not mapped yet");
            }
            catch (Exception ex)
            {
                ExceptionHandler(ref result, ex);
            }

            return result;
        }

        /// <summary>
        /// Checking to allow export licensee statistic detail.
        /// </summary>
        /// <returns>Return result.</returns>
        public Result AllowExportLicenseeStatisticDetail()
        {
            var result = new Result();
            try
            {
                var getSessionData = (DailyComplaintsDTO)SessionExtension.GetSession(HttpContext.Current.Session, _licenseeDailyDetail);

                result = getSessionData != null && getSessionData.DailyComplaints.Count > 0 ?
                    new Result(0, "Succesfully") :
                    new Result(1, "Data has not mapped yet");
            }
            catch (Exception ex)
            {
                ExceptionHandler(ref result, ex);
            }

            return result;
        }

        #region all feedback statistics detail
        /// <summary>
        /// Export 2 Excel Feedback Statistic Detail.
        /// </summary>
        /// <param name="type">Filter by: Creator/country/role...</param>
        /// <param name="fromDate">From Date.</param>
        /// <param name="toDate">To Date value.</param>
        /// <param name="reportTitle">Report Title.</param>
        public void ExportExcelAllFeedbackStatisticsDetail(int type, string fromDate, string toDate, string reportTitle)
        {
            try
            {
                var getSessionData = CacheService.Get<DataGridViewDTO>(_feedbackStatisticDetailMappedSessionKey);

                var startRow = 2;
                var startCol = 0;
                var reportName = "FeedbackStatisticDetail";
                var excelFile = new ExcelFile();
                var sheetData = excelFile.Worksheets.Add(reportName);
                var nextRow = 0;

                sheetData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 5).Merged = true;
                sheetData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 5).Style.Font.Weight = ExcelFont.BoldWeight;
                sheetData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 5).Value = reportTitle;
                ExportExcelFeedbackStatisticDetailAssignHeaderSplittingMainCategoryAndSubcategory(ref sheetData, startRow, startCol);

                if (getSessionData != null && getSessionData.Rows.Count > 0)
                {
                    foreach (FeedbackView feedbackView in getSessionData.Rows)
                    {
                        ExportExcelFeedbackStatisticDetailAssignContentSplittingtMainCategoryAndSubcategory(ref sheetData, startRow, startCol, ref nextRow, feedbackView);
                    }
                }
                else
                {
                    sheetData.Cells[startRow, startCol].Style.Font.Color = Color.Red;
                    sheetData.Cells[startRow, startCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblThereisnoinfomation");
                }

                ReponseExcelFile(excelFile, string.Format("{0}-{1:MMddyyyy}-{2:MMddyyyy}", reportName, CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate)));
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }
        #endregion

        #region private function

        /// <summary>
        /// Assign data to header excel report.
        /// </summary>
        /// <param name="sheetData">Report work sheet.</param>
        /// <param name="startRow">Start row index.</param>
        /// <param name="startCol">Start column index.</param>
        private static void ExportExcelFeedbackStatisticDetailAssignHeader(ref ExcelWorksheet sheetData, int startRow, int startCol)
        {
            var nextCol = 0;
            var headerFormart = new ExcelUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            sheetData.Columns[startCol].Width = 45 * 256;
            sheetData.Columns[startCol + 1].Width = 15 * 256;
            sheetData.Columns[startCol + 2].Width = 25 * 256;
            sheetData.Columns[startCol + 3].Width = 10 * 256;
            sheetData.Columns[startCol + 4].Width = 15 * 256;

            sheetData.Cells[startRow - 1, startCol].Style = headerFormart;
            sheetData.Cells[startRow - 1, startCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblFeedback");
            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCustomer");
            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCategory");
            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblStatus");
            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblRelatedCustomer");
        }

        /// <summary>
        /// Assign data to content report.
        /// </summary>
        /// <param name="sheetData">Report work sheet.</param>
        /// <param name="startRow">Start row index.</param>
        /// <param name="startCol">Start column index.</param>
        /// <param name="nextRow">Next column index.</param>
        /// <param name="feedbackView">Feedback view.</param>
        private static void ExportExcelFeedbackStatisticDetailAssignContent(
            ref ExcelWorksheet sheetData,
            int startRow,
            int startCol,
            ref int nextRow,
            FeedbackView feedbackView)
        {
            var nextCol = 0;
            Color color = GetFeedbackRowColor(feedbackView.Status);
            var currentRowFormat = new ExcelUtils().GetStyleTitle(color, HorizontalAlignmentStyle.Left, ExcelFont.NormalWeight);

            sheetData.Rows[startRow + nextRow].Height = 4 * 256;
            var reviewBy =
                string.IsNullOrEmpty(feedbackView.Reviewer) ? string.Empty : string.Format("\n Reviewed by {0} on {1}", feedbackView.Reviewer, feedbackView.ReviewDate);
            var createBy = string.Format("\n By {0} on {1} (GMT -4)", feedbackView.Creator, feedbackView.CreateDate);
            var solvingDuration = string.IsNullOrEmpty(feedbackView.DurationTime) ? string.Empty : string.Format("\n Solving Duration: {0}", feedbackView.DurationTime);
            sheetData.Cells[startRow + nextRow, startCol].Style = currentRowFormat;
            sheetData.Cells[startRow + nextRow, startCol].Value =
                string.Format("{0} \n Feedback date {1} {2} {3} {4}", feedbackView.Subject, feedbackView.FeedbackDate, createBy, solvingDuration, reviewBy);
            currentRowFormat = new ExcelUtils().GetStyleTitle(color, HorizontalAlignmentStyle.Center, ExcelFont.NormalWeight);
            sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = currentRowFormat;
            sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = feedbackView.Username;
            sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = currentRowFormat;
            sheetData.Cells[startRow + nextRow, startCol + nextCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = string.Format("{0} \n  {1}", feedbackView.MainCategory, feedbackView.SubCategory);
            sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = currentRowFormat;
            sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = feedbackView.Status;
            sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = currentRowFormat;
            sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = feedbackView.RelatedUsername;
            nextRow++;
        }

        /// <summary>
        /// Assign data to header excel report.
        /// </summary>
        /// <param name="sheetData">Report work sheet.</param>
        /// <param name="startRow">Start row index.</param>
        /// <param name="startCol">Start column index.</param>
        private static void ExportExcelFeedbackStatisticDetailAssignHeaderSplittingMainCategoryAndSubcategory(ref ExcelWorksheet sheetData, int startRow, int startCol)
        {
            var nextCol = 0;
            var headerFormart = new ExcelUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            sheetData.Columns[startCol].Width = 45 * 256;
            sheetData.Columns[startCol + 1].Width = 15 * 256;
            sheetData.Columns[startCol + 2].Width = 25 * 256;
            sheetData.Columns[startCol + 3].Width = 25 * 256;
            sheetData.Columns[startCol + 4].Width = 10 * 256;
            sheetData.Columns[startCol + 5].Width = 15 * 256;

            sheetData.Cells[startRow - 1, startCol].Style = headerFormart;
            sheetData.Cells[startRow - 1, startCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblFeedback");
            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCustomer");
            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMainCategory");
            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblSubCategory");
            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblStatus");
            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblRelatedCustomer");
        }

        /// <summary>
        /// Gets the color of the feedback row.
        /// </summary>
        /// <param name="statusName">Name of the status.</param>
        /// <returns>The color of the feedback row.</returns>
        private static Color GetFeedbackRowColor(string statusName)
        {
            switch (statusName.ToLower())
            {
                case "closed":
                    return Color.FromArgb(204, 199, 186);
                case "pending":
                    return Color.FromArgb(251, 236, 136);
                default:
                    return Color.FromArgb(255, 255, 255);
            }
        }

        /// <summary>
        /// Assign data to content report.
        /// </summary>
        /// <param name="sheetData">Report work sheet.</param>
        /// <param name="startRow">Start row index.</param>
        /// <param name="startCol">Start column index.</param>
        /// <param name="nextRow">Next column index.</param>
        /// <param name="feedbackView">Feedback view.</param>
        private static void ExportExcelFeedbackStatisticDetailAssignContentSplittingtMainCategoryAndSubcategory(
            ref ExcelWorksheet sheetData,
            int startRow,
            int startCol,
            ref int nextRow,
            FeedbackView feedbackView)
        {
            var nextCol = 0;
            Color color = GetFeedbackRowColor(feedbackView.Status);
            var currentRowFormat = new ExcelUtils().GetStyleTitle(color, HorizontalAlignmentStyle.Left, ExcelFont.NormalWeight);
            var reviewBy =
                string.IsNullOrEmpty(feedbackView.Reviewer) ? string.Empty : string.Format("\n Reviewed by {0} on {1}", feedbackView.Reviewer, feedbackView.ReviewDate);
            var createBy = string.Format("\n By {0} on {1} (GMT -4)", feedbackView.Creator, feedbackView.CreateDate);
            var solvingDuration = string.IsNullOrEmpty(feedbackView.DurationTime) ? string.Empty : string.Format("\n Solving Duration: {0}", feedbackView.DurationTime);

            sheetData.Rows[startRow + nextRow].Height = 4 * 256;
            sheetData.Cells[startRow + nextRow, startCol].Style = currentRowFormat;
            sheetData.Cells[startRow + nextRow, startCol].Value =
                string.Format("{0} \n Feedback date {1} {2} {3} {4}", feedbackView.Subject, feedbackView.FeedbackDate, createBy, solvingDuration, reviewBy);
            currentRowFormat = new ExcelUtils().GetStyleTitle(color, HorizontalAlignmentStyle.Center, ExcelFont.NormalWeight);
            sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = currentRowFormat;
            sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = feedbackView.Username;
            sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = currentRowFormat;
            sheetData.Cells[startRow + nextRow, startCol + nextCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = string.Format("{0}", feedbackView.MainCategory);
            sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = currentRowFormat;
            sheetData.Cells[startRow + nextRow, startCol + nextCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = string.Format("{0}", feedbackView.SubCategory);
            sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = currentRowFormat;
            sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = feedbackView.Status;
            sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = currentRowFormat;
            sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = feedbackView.RelatedUsername;
            nextRow++;
        }

        /// <summary>
        /// Assign to header.
        /// </summary>
        /// <param name="sheetData">Report work sheet.</param>
        /// <param name="startRow">Start row index.</param>
        /// <param name="startCol">Start column index.</param>
        /// <param name="columnNames">Column names.</param>
        private static void ExportExcelFeedbackStatisticAssignHeader(ref ExcelWorksheet sheetData, int startRow, int startCol, ArrayList columnNames)
        {
            var nextCol = 0;
            var headerFormart = new ExcelUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            sheetData.Columns[startCol].Width = 30 * 256;
            sheetData.Cells[startRow - 1, startCol].Style = headerFormart;
            sheetData.Cells[startRow - 1, startCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblSubCategory");
            foreach (var columnName in columnNames)
            {
                sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
                sheetData.Cells[startRow - 1, startCol + nextCol].Value = columnName;
            }
        }

        /// <summary>
        /// Assign to content.
        /// </summary>
        /// <param name="sheetData">Report work sheet.</param>
        /// <param name="startRow">Start row index.</param>
        /// <param name="startCol">Start column index.</param>
        /// <param name="nextRow">Next row index.</param>
        /// <param name="feedbacks">Feedbacks info.</param>
        /// <param name="columnNames">Column names.</param>
        private static void ExportExcelFeedbackStatisticAssignContent(
            ref ExcelWorksheet sheetData,
            int startRow,
            int startCol,
            ref int nextRow,
            FeedbackStatisticsGroup feedbacks,
            ArrayList columnNames)
        {
            if (feedbacks != null)
            {
                var groupTitleFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(255, 238, 221));
                var totalChild = columnNames != null ? columnNames.Count : 0;
                var cellRange = sheetData.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + totalChild);
                cellRange.Merged = true;
                cellRange.Style = groupTitleFormat;
                cellRange.Value = feedbacks.Title;
                cellRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                nextRow++;
                var index = 0;
                foreach (FeedbackStatistics feedback in feedbacks.FeedbackStatistics)
                {
                    var nextCol = 0;
                    var currentRowFormat = new ExcelUtils().GetStyleTitle(
                        index % 2 > 0 ? Color.FromArgb(255, 255, 255) : Color.FromArgb(255, 243, 200),
                        HorizontalAlignmentStyle.Center,
                        feedback.IsTotal ? ExcelFont.BoldWeight : ExcelFont.NormalWeight);
                    sheetData.Cells[startRow + nextRow, startCol + nextCol].Style = currentRowFormat;
                    sheetData.Cells[startRow + nextRow, startCol + nextCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                    sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = string.Format("  {0}", feedback.SubCategory);
                    foreach (var cell in feedback.Cells)
                    {
                        sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = currentRowFormat;
                        sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = cell;
                    }

                    index++;
                    nextRow++;
                }
            }
        }

        /// <summary>
        /// Assign to footer.
        /// </summary>
        /// <param name="sheetData">Report work sheet.</param>
        /// <param name="startRow">Start row index.</param>
        /// <param name="startCol">Start column index.</param>
        /// <param name="nextRow">Next row index.</param>
        /// <param name="feedback">Footer info.</param>
        private static void ExportExcelFeedbackStatisticAssignFooter(
            ref ExcelWorksheet sheetData,
            int startRow,
            int startCol,
            ref int nextRow,
            FeedbackStatistics feedback)
        {
            if (feedback != null)
            {
                var nextCol = 0;
                var currentRowFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(211, 225, 200), HorizontalAlignmentStyle.Center, ExcelFont.BoldWeight);
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Style = currentRowFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = feedback.SubCategory;
                foreach (var cell in feedback.Cells)
                {
                    sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = currentRowFormat;
                    sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = cell;
                }

                nextRow++;
            }
        }

        /// <summary>
        /// Export 2 excel - daily summary report.
        /// </summary>
        /// <param name="dailySummaryBO">Daily summary data.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="excelFile">Excel file.</param>
        /// <param name="reportTitle">Report title.</param>
        private void ExportIBCDailySummaryReport(DailySummaryBO dailySummaryBO, DateTime fromDate, DateTime toDate, ExcelFile excelFile, string reportTitle)
        {
            var startCol = 0;
            var startRow = 2;
            var nextRow = 0;
            var sheetData = excelFile.Worksheets.Add("Credit Site Summary");
            sheetData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 8).Merged = true;
            sheetData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 8).Style.Font.Weight = ExcelFont.BoldWeight;
            sheetData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 8).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            sheetData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 8).Style.Font.Color = Color.Blue;
            sheetData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 8).Value = reportTitle.ToUpper();
            ExportIBCDailySummaryReportAssignHeader(ref sheetData, startRow, startCol, ref nextRow, fromDate, toDate, dailySummaryBO, false);
            if (dailySummaryBO != null && dailySummaryBO.DailySummaries != null && dailySummaryBO.DailySummaries.Count > 0)
            {
                foreach (var dailySummaryDetail in dailySummaryBO.DailySummaries)
                {
                    ExportIBCDailySummaryReportAssignContent(ref sheetData, startRow, startCol, ref nextRow, dailySummaryDetail.Total, false, true);
                    foreach (var row in dailySummaryDetail.DailySummaries)
                    {
                        ExportIBCDailySummaryReportAssignContent(ref sheetData, startRow, startCol, ref nextRow, row, false, false);
                    }
                }

                ExportIBCDailySummaryReportAssignContent(ref sheetData, startRow, startCol, ref nextRow, dailySummaryBO.GrandTotal, true, false);
            }
        }

        /// <summary>
        /// Assign to header.
        /// </summary>
        /// <param name="sheetData">Excel work sheet.</param>
        /// <param name="startRow">Start row index.</param>
        /// <param name="startCol">Start column index.</param>
        /// <param name="nextRow">Next row index.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="dalySummaryBO">Daily summary data.</param>
        /// <param name="isLicensee">Is licensee.</param>
        private void ExportIBCDailySummaryReportAssignHeader(
            ref ExcelWorksheet sheetData,
            int startRow,
            int startCol,
            ref int nextRow,
            DateTime fromDate,
            DateTime toDate,
            DailySummaryBO dalySummaryBO,
            bool isLicensee)
        {
            var backgroundHeaderNormalFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var backgroundHeaderMainFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(130, 199, 255));
            var mergeCols = 0;
            if (dalySummaryBO != null && dalySummaryBO.DailySummaries != null && dalySummaryBO.DailySummaries.Count > 0)
            {
                mergeCols = dalySummaryBO.DailySummaries[0].Total.Cells.Count + 1;
            }

            var cellRange = sheetData.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + mergeCols);
            cellRange.Merged = true;
            cellRange.Style = backgroundHeaderMainFormat;
            cellRange.Value = string.Format(fromDate == toDate ? "{0}" : "From {0} To {1}", fromDate.ToString("MMMM dd, yyyy"), toDate.ToString("MMMM dd, yyyy"));

            if (mergeCols > 1)
            {
                nextRow++;
                var nextCol = 0;
                sheetData.Columns[startCol].Width = 255 * 30;
                sheetData.Cells[startRow + nextRow, startCol].Style = backgroundHeaderNormalFormat;
                sheetData.Cells[startRow + nextRow, startCol].Value = isLicensee ?
                    ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "licensee") :
                    ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "exportComplaintsEnquiriesTitle");
                var cells = dalySummaryBO.DailySummaries[0].Total.Cells;
                for (int i = 0; i < cells.Count; i++)
                {
                    sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
                    sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = cells[i].Title;
                }

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "grandTotal");
            }

            nextRow++;
        }

        /// <summary>
        /// Assign to content.
        /// </summary>
        /// <param name="sheetData">Excel work sheet.</param>
        /// <param name="startRow">Start row index.</param>
        /// <param name="startCol">Start column index.</param>
        /// <param name="nextRow">Next row index.</param>
        /// <param name="summaryDatail">Daily summary data.</param>
        /// <param name="isGrandTotal">Is grand total.</param>
        /// <param name="isTotal">Is total value.</param>
        private void ExportIBCDailySummaryReportAssignContent(
            ref ExcelWorksheet sheetData,
            int startRow,
            int startCol,
            ref int nextRow,
            DailySummaryDetail summaryDatail,
            bool isGrandTotal,
            bool isTotal)
        {
            var color = isGrandTotal ? Color.FromArgb(130, 199, 255) : (isTotal ? Color.FromArgb(245, 238, 211) : Color.FromArgb(255, 255, 255));
            var backgroundNormalFormat = new ExcelUtils().GetStyleIntNumber(string.Empty, color, isGrandTotal || isTotal, false);
            sheetData.Cells[startRow + nextRow, startCol].Style = backgroundNormalFormat;
            sheetData.Cells[startRow + nextRow, startCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            sheetData.Cells[startRow + nextRow, startCol].Value = string.Format("{0}{1}", isTotal || isGrandTotal ? string.Empty : " - ", summaryDatail.MainCategory);
            var cells = summaryDatail.Cells;
            var nextCol = 0;
            for (int i = 0; i < cells.Count; i++)
            {
                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundNormalFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = cells[i].Value;
            }

            sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundNormalFormat;
            sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = summaryDatail.Total;
            nextRow++;
        }

        private void ExportDailyComplaintsDetail(DailyComplaintsDTO complaints, DateTime fromDate, DateTime toDate, ExcelFile excelFile, string title)
        {
            var startCol = 0;
            var startRow = 2;
            var nextRow = 0;
            var complaintsDetail = string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "exportDailyComplaintsDetail"), "Detail");
            var sheetData = excelFile.Worksheets.Add(complaintsDetail);
            ExportDailyComplaintsDetailAssignHeader(ref sheetData, startRow, startCol, fromDate, title);
            ExportDailyComplaintsDetailAssignContent(ref sheetData, startRow, startCol, nextRow, fromDate, complaints);
        }

        /// <summary>
        /// Assign to header.
        /// </summary>
        /// <param name="sheetData">Excel work sheet.</param>
        /// <param name="startRow">Start row index.</param>
        /// <param name="startCol">Start column index.</param>
        /// <param name="reportDate">Report date.</param>
        /// <param name="title">Report title.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private void ExportDailyComplaintsDetailAssignHeader(ref ExcelWorksheet sheetData, int startRow, int startCol, DateTime reportDate, string title)
        {
            var backgroundHeaderNormalFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var backgroundHeaderMainFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(130, 199, 255));

            var nextCol = 0;
            sheetData.Rows[startRow - 2].Height = 255 * 2;
            sheetData.Rows[startRow - 1].Height = 255 * 2;

            sheetData.Cells[startRow - 1, startCol].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "date");
            sheetData.Columns[startCol].Width = 255 * 10;
            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "StartTime");
            sheetData.Columns[startCol].Width = 255 * 10;
            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "EndTime");
            sheetData.Columns[startCol].Width = 255 * 10;
            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Duration");
            sheetData.Columns[startCol].Width = 255 * 10;
            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCustomer");
            sheetData.Columns[startCol + nextCol].Width = 255 * 15;
            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = "Site";
            sheetData.Columns[startCol + nextCol].Width = 255 * 15;
            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "typeofcontact");
            sheetData.Columns[startCol + nextCol].Width = 255 * 15;
            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "ExtNo");
            sheetData.Columns[startCol + nextCol].Width = 255 * 10;
            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "ContactMSNSKypeEmail");
            sheetData.Columns[startCol + nextCol].Width = 255 * 30;
            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Product");
            sheetData.Columns[startCol + nextCol].Width = 255 * 15;
            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "country");
            sheetData.Columns[startCol + nextCol].Width = 255 * 15;
            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCategory");
            sheetData.Columns[startCol + nextCol].Width = 255 * 30;
            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "complaintsEnquiries");
            sheetData.Columns[startCol + nextCol].Width = 255 * 60;
            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "solution");
            sheetData.Columns[startCol + nextCol].Width = 255 * 60;
            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblStatus");
            sheetData.Columns[startCol + nextCol].Width = 255 * 15;
            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = "Response";
            sheetData.Columns[startCol + nextCol].Width = 255 * 15;
            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "handleBy");
            sheetData.Columns[startCol + nextCol].Width = 255 * 15;
            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "CreationTime");
            sheetData.Columns[startCol + nextCol].Width = 255 * 15;
            sheetData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Merged = true;
            sheetData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Style = backgroundHeaderMainFormat;
            sheetData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Value = title.ToUpper();
        }

        /// <summary>
        /// Assign to content.
        /// </summary>
        /// <param name="sheetData">Excel work sheet.</param>
        /// <param name="startRow">Start row index.</param>
        /// <param name="startCol">Start column index.</param>
        /// <param name="nextRow">Next row index.</param>
        /// <param name="reportDate">Report date.</param>
        /// <param name="complaints">Customer complaints.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private void ExportDailyComplaintsDetailAssignContent(
            ref ExcelWorksheet sheetData,
            int startRow,
            int startCol,
            int nextRow,
            DateTime reportDate,
            DailyComplaintsDTO complaints)
        {
            var backgroundFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(255, 255, 255), HorizontalAlignmentStyle.Center, ExcelFont.NormalWeight);

            foreach (DailyComplaintsDetail detail in complaints.DailyComplaints)
            {
                var nextCol = 0;
                var complains = Helper.ReplaceHtmlTag(detail.Complaints);
                var solution = Helper.ReplaceHtmlTag(detail.Solution);

                sheetData.Rows[startRow].Height = 255 * 2;
                sheetData.Cells[startRow + nextRow, startCol].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol].Value = detail.FeedbackDate;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.FeedbackTime;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = string.Format("{0: hh:mm tt}", CastAide.AsDateTime(detail.ResolutionDate));

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.Duration;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.CustomerName;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.Licensee;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.CommunicationTool.ContactBy;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.ExtNo;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = Regex.Replace(detail.CommunicationTool.Address, "<[^<>]+>", string.Empty);

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.ProductName;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.Country;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.Category + "\n" + detail.SubCategory;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = complains;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = solution;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.Status;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.Response;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.HandlyBy;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.CreationTime;

                startRow++;
            }
        }

        /// <summary>
        /// Excel 2 excel - Licensee daily summary report.
        /// </summary>
        /// <param name="dailySummaryBO">Daily summary report.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="excelFile">Excel file.</param>
        /// <param name="reportTitle">Report title.</param>
        private void ExportLicenseeDailySummaryReport(DailySummaryBO dailySummaryBO, DateTime fromDate, DateTime toDate, ExcelFile excelFile, string reportTitle)
        {
            var startCol = 0;
            var startRow = 2;
            var nextRow = 0;
            var sheetData = excelFile.Worksheets.Add("Licensee Summary");

            sheetData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 8).Merged = true;
            sheetData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 8).Style.Font.Weight = ExcelFont.BoldWeight;
            sheetData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 8).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            sheetData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 8).Style.Font.Color = Color.Blue;
            sheetData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 8).Value = reportTitle.ToUpper();
            ExportLicenseeDailySummaryReportAssignHeader(ref sheetData, startRow, startCol, ref nextRow, fromDate, toDate, dailySummaryBO, false);

            if (dailySummaryBO != null && dailySummaryBO.DailySummaries != null && dailySummaryBO.DailySummaries.Count > 0)
            {
                foreach (var dailySummaryDetail in dailySummaryBO.DailySummaries)
                {
                    ExportLicenseeDailySummaryReportAssignContent(ref sheetData, startRow, startCol, ref nextRow, dailySummaryDetail.Total, false, true);
                    foreach (var row in dailySummaryDetail.DailySummaries)
                    {
                        ExportLicenseeDailySummaryReportAssignContent(ref sheetData, startRow, startCol, ref nextRow, row, false, false);
                    }
                }

                ExportLicenseeDailySummaryReportAssignContent(ref sheetData, startRow, startCol, ref nextRow, dailySummaryBO.GrandTotal, true, false);
            }
        }

        /// <summary>
        /// Assign to header.
        /// </summary>
        /// <param name="sheetData">Excel work sheet.</param>
        /// <param name="startRow">Start row index.</param>
        /// <param name="startCol">Start column index.</param>
        /// <param name="nextRow">Next row index.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="dalySummaryBO">The daly summary bo.</param>
        /// <param name="isLicensee">Is licensee.</param>
        private void ExportLicenseeDailySummaryReportAssignHeader(
            ref ExcelWorksheet sheetData,
            int startRow,
            int startCol,
            ref int nextRow,
            DateTime fromDate,
            DateTime toDate,
            DailySummaryBO dalySummaryBO,
            bool isLicensee)
        {
            var backgroundHeaderNormalFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var backgroundHeaderMainFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(130, 199, 255));
            var mergeCols = 0;
            if (dalySummaryBO != null && dalySummaryBO.DailySummaries != null && dalySummaryBO.DailySummaries.Count > 0)
            {
                mergeCols = dalySummaryBO.DailySummaries[0].Total.Cells.Count + 1;
            }

            var cellRange = sheetData.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + mergeCols);
            cellRange.Merged = true;
            cellRange.Style = backgroundHeaderMainFormat;
            cellRange.Value = string.Format(fromDate == toDate ? "{0}" : "From {0} To {1}", fromDate.ToString("MMMM dd, yyyy"), toDate.ToString("MMMM dd, yyyy"));

            if (mergeCols > 1)
            {
                nextRow++;
                var nextCol = 0;
                sheetData.Columns[startCol].Width = 255 * 30;
                sheetData.Cells[startRow + nextRow, startCol].Style = backgroundHeaderNormalFormat;
                sheetData.Cells[startRow + nextRow, startCol].Value = isLicensee ?
                    ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "licensee") :
                    ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "exportComplaintsEnquiriesTitle");
                var cells = dalySummaryBO.DailySummaries[0].Total.Cells;

                for (int i = 0; i < cells.Count; i++)
                {
                    sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
                    sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = cells[i].Title;
                }

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "grandTotal");
            }

            nextRow++;
        }

        /// <summary>
        /// Assign to content.
        /// </summary>
        /// <param name="sheetData">Excel work sheet.</param>
        /// <param name="startRow">Start row index.</param>
        /// <param name="startCol">Start column index.</param>
        /// <param name="nextRow">Next row index.</param>
        /// <param name="summaryDetail">The summary detail.</param>
        /// <param name="isGrandTotal">Is grand total.</param>
        /// <param name="isTotal">Is total value.</param>
        private void ExportLicenseeDailySummaryReportAssignContent(
            ref ExcelWorksheet sheetData,
            int startRow,
            int startCol,
            ref int nextRow,
            DailySummaryDetail summaryDetail,
            bool isGrandTotal,
            bool isTotal)
        {
            var color = isGrandTotal ? Color.FromArgb(130, 199, 255) : (isTotal ? Color.FromArgb(245, 238, 211) : Color.FromArgb(255, 255, 255));
            var backgroundNormalFormat = new ExcelUtils().GetStyleIntNumber(string.Empty, color, isGrandTotal || isTotal, false);

            sheetData.Cells[startRow + nextRow, startCol].Style = backgroundNormalFormat;
            sheetData.Cells[startRow + nextRow, startCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            sheetData.Cells[startRow + nextRow, startCol].Value = string.Format("{0}{1}", isTotal || isGrandTotal ? string.Empty : " - ", summaryDetail.MainCategory);
            var cells = summaryDetail.Cells;

            var nextCol = 0;
            for (int i = 0; i < cells.Count; i++)
            {
                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundNormalFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = cells[i].Value;
            }

            sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundNormalFormat;
            sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = summaryDetail.Total;
            nextRow++;
        }

        /// <summary>
        /// Export 2 excel - Licensee detail report.
        /// </summary>
        /// <param name="complaints">Customer complaints.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="excelFile">Excel file.</param>
        /// <param name="title">Report title.</param>
        private void ExportLicenseeComplaintsDetail(DailyComplaintsDTO complaints, DateTime fromDate, DateTime toDate, ExcelFile excelFile, string title)
        {
            var startCol = 0;
            var startRow = 2;
            var nextRow = 0;
            var sheetData = excelFile.Worksheets.Add("All Licensee");

            ExportLicenseeComplaintsDetailAssignHeader(ref sheetData, startRow, startCol, fromDate, title);
            ExportLicenseeComplaintsDetailAssignContent(ref sheetData, startRow, startCol, nextRow, fromDate, complaints.DailyComplaints);

            var licenseese = (from q in complaints.DailyComplaints select q.Licensee).Distinct().ToList();
            foreach (var licensee in licenseese)
            {
                var licenseeData = (from q in complaints.DailyComplaints where q.Licensee.Equals(licensee) select q).ToList();
                nextRow = 0;
                var newSheetData = excelFile.Worksheets.Add(licensee.Replace("N/A", "N_A"));

                ExportLicenseeComplaintsDetailAssignHeader(ref newSheetData, startRow, startCol, fromDate, title.Replace("Licensee", licensee));
                ExportLicenseeComplaintsDetailAssignContent(ref newSheetData, startRow, startCol, nextRow, fromDate, licenseeData);
            }
        }

        /// <summary>
        /// Assign to header.
        /// </summary>
        /// <param name="sheetData">Excel work sheet.</param>
        /// <param name="startRow">Start row index.</param>
        /// <param name="startCol">Start column index.</param>
        /// <param name="reportDate">Report date.</param>
        /// <param name="title">Report title.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private void ExportLicenseeComplaintsDetailAssignHeader(ref ExcelWorksheet sheetData, int startRow, int startCol, DateTime reportDate, string title)
        {
            var backgroundHeaderNormalFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var backgroundHeaderMainFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(130, 199, 255));
            var nextCol = 0;
            sheetData.Rows[startRow - 2].Height = 255 * 2;
            sheetData.Rows[startRow - 1].Height = 255 * 2;

            sheetData.Cells[startRow - 1, startCol].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "date");
            sheetData.Columns[startCol].Width = 255 * 10;

            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "StartTime");
            sheetData.Columns[startCol].Width = 255 * 10;

            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "EndTime");
            sheetData.Columns[startCol].Width = 255 * 10;

            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Duration");
            sheetData.Columns[startCol].Width = 255 * 10;

            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCustomer");
            sheetData.Columns[startCol + nextCol].Width = 255 * 15;

            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = "Site";
            sheetData.Columns[startCol + nextCol].Width = 255 * 15;

            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "typeofcontact");
            sheetData.Columns[startCol + nextCol].Width = 255 * 15;

            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "ExtNo");
            sheetData.Columns[startCol + nextCol].Width = 255 * 10;

            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "ContactMSNSKypeEmail");
            sheetData.Columns[startCol + nextCol].Width = 255 * 30;

            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Product");
            sheetData.Columns[startCol + nextCol].Width = 255 * 15;

            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "country");
            sheetData.Columns[startCol + nextCol].Width = 255 * 15;

            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCategory");
            sheetData.Columns[startCol + nextCol].Width = 255 * 30;

            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "complaintsEnquiries");
            sheetData.Columns[startCol + nextCol].Width = 255 * 60;

            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "solution");
            sheetData.Columns[startCol + nextCol].Width = 255 * 60;

            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblStatus");
            sheetData.Columns[startCol + nextCol].Width = 255 * 15;

            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = "Response";
            sheetData.Columns[startCol + nextCol].Width = 255 * 15;

            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "handleBy");
            sheetData.Columns[startCol + nextCol].Width = 255 * 15;

            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "CreationTime");
            sheetData.Columns[startCol + nextCol].Width = 255 * 15;

            sheetData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Merged = true;
            sheetData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Style = backgroundHeaderMainFormat;
            sheetData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Value = title.ToUpper();
        }

        /// <summary>
        /// Assign to content.
        /// </summary>
        /// <param name="sheetData">Excel work sheet.</param>
        /// <param name="startRow">Start row index.</param>
        /// <param name="startCol">Start column index.</param>
        /// <param name="nextRow">Next row index.</param>
        /// <param name="reportDate">Report date.</param>
        /// <param name="complaints">Customer complaints.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private void ExportLicenseeComplaintsDetailAssignContent(
            ref ExcelWorksheet sheetData,
            int startRow,
            int startCol,
            int nextRow,
            DateTime reportDate,
            List<DailyComplaintsDetail> complaints)
        {
            var backgroundFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(255, 255, 255), HorizontalAlignmentStyle.Center, ExcelFont.NormalWeight);
            foreach (DailyComplaintsDetail detail in complaints)
            {
                var nextCol = 0;
                var complains = Helper.ReplaceHtmlTag(detail.Complaints);
                var solution = Helper.ReplaceHtmlTag(detail.Solution);

                sheetData.Rows[startRow].Height = 255 * 2;
                sheetData.Cells[startRow + nextRow, startCol].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol].Value = detail.FeedbackDate;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.FeedbackTime;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = string.Format("{0: hh:mm tt}", CastAide.AsDateTime(detail.ResolutionDate));

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.Duration;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.CustomerName;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.Licensee;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.CommunicationTool.ContactBy;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.ExtNo;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = Regex.Replace(detail.CommunicationTool.Address, "<[^<>]+>", string.Empty);

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.ProductName;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.Country;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.Category + "\n" + detail.SubCategory;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = complains;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = solution;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.Status;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.Response;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.HandlyBy;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.CreationTime;

                startRow++;
            }
        }

        /// <summary>
        /// Export 2 excel - Casino daily summary report.
        /// </summary>
        /// <param name="casinoDailySummary">Casino summary report.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="reportTitle">The report title.</param>
        /// <param name="excelFile">Excel file.</param>
        private void ExportCasinoDailySummary(CasinoDailySummary casinoDailySummary, DateTime fromDate, DateTime toDate, string reportTitle, ExcelFile excelFile)
        {
            var startCol = 0;
            var startRow = 2;
            var nextRow = 0;
            var sheetData = excelFile.Worksheets.Add("Casino Summary");

            sheetData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 8).Merged = true;
            sheetData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 8).Style.Font.Weight = ExcelFont.BoldWeight;
            sheetData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 8).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            sheetData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 8).Style.Font.Color = Color.Blue;
            sheetData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 8).Value = reportTitle;

            if (casinoDailySummary.DailySummary != null && casinoDailySummary.DailySummary.Count > 0)
            {
                ExportCasinoDailySummaryAssignHeader(ref sheetData, startRow, startCol, ref nextRow, fromDate, casinoDailySummary, false);
                foreach (CasinoDailySummaryDetail dailySummary in casinoDailySummary.DailySummary)
                {
                    ExportCasinoDailySummaryAssignContent(ref sheetData, startRow, startCol, ref nextRow, dailySummary, false);
                }

                ExportCasinoDailySummaryAssignContent(ref sheetData, startRow, startCol, ref nextRow, casinoDailySummary.GrandTotal, true);
                ExportCasinoDailySummaryAssignTotal(ref sheetData, startRow, startCol, ref nextRow, casinoDailySummary.GrandTotal, casinoDailySummary.SumAllFeedbacks);
            }
            else
            {
                sheetData.Cells[startRow + nextRow, startCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata");
            }

            nextRow++;
        }

        /// <summary>
        /// Assign to header.
        /// </summary>
        /// <param name="sheetData">Excel work sheet.</param>
        /// <param name="startRow">Start row index.</param>
        /// <param name="startCol">Start column index.</param>
        /// <param name="nextRow">Next row index.</param>
        /// <param name="reportDate">Report date.</param>
        /// <param name="casinoDailySummary">Casino summary report.</param>
        /// <param name="isWeekly">Is weekly report.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private void ExportCasinoDailySummaryAssignHeader(
            ref ExcelWorksheet sheetData,
            int startRow,
            int startCol,
            ref int nextRow,
            DateTime reportDate,
            CasinoDailySummary casinoDailySummary,
            bool isWeekly)
        {
            var weeklyHeaderColor = isWeekly ? Color.FromArgb(30, 134, 218) : Color.FromArgb(130, 199, 255);
            var backgroundHeaderNormalFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var backgroundHeaderMainFormat = new ExcelUtils().GetStyleTitle(weeklyHeaderColor);
            var mergeCol = 0;

            sheetData.Columns[startCol].Width = 255 * 30;
            if (casinoDailySummary != null && casinoDailySummary.DailySummary != null && casinoDailySummary.DailySummary.Count > 0)
            {
                var totalCells = 0;
                for (int i = 0; i < casinoDailySummary.DailySummary[0].SystemSummary.Count; i++)
                {
                    totalCells += casinoDailySummary.DailySummary[0].SystemSummary[i].Cells.Count;
                }

                mergeCol = totalCells;
            }

            if (mergeCol > 1)
            {
                var nextCol = 0;

                //// left col - main cateogry
                CellRange cellRange;
                cellRange = sheetData.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + nextCol, startRow + nextRow + 1, startCol + nextCol);
                cellRange.Merged = true;
                cellRange.Style = backgroundHeaderNormalFormat;
                cellRange.Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCategory");

                //// right cols - system data
                for (int i = 0; i < casinoDailySummary.DailySummary[0].SystemSummary.Count; i++)
                {
                    var systemSummary = casinoDailySummary.DailySummary[0].SystemSummary[i];
                    var totalMergeCols = systemSummary.Cells.Count - 1;

                    sheetData.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol += 1), startRow + nextRow, startCol + nextCol + totalMergeCols).Merged = true;
                    cellRange = sheetData.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + nextCol, startRow + nextRow, startCol + nextCol + totalMergeCols);
                    cellRange.Style = backgroundHeaderNormalFormat;
                    cellRange.Value = systemSummary.Sytem;

                    var nextCol2 = 0;
                    for (int j = 0; j < systemSummary.Cells.Count; j++)
                    {
                        sheetData.Cells[startRow + nextRow + 1, startCol + nextCol2 + nextCol].Style = backgroundHeaderNormalFormat;
                        sheetData.Cells[startRow + nextRow + 1, startCol + nextCol2 + nextCol].Value = systemSummary.Cells[j].Title;
                        nextCol2 += 1;
                    }

                    nextCol += totalMergeCols;
                }

                nextRow++;
            }
            else
            {
                sheetData.Cells[startRow + nextRow, startCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata");
            }

            nextRow++;
        }

        /// <summary>
        /// Assign to content.
        /// </summary>
        /// <param name="sheetData">Excel work sheet.</param>
        /// <param name="startRow">Start row index.</param>
        /// <param name="startCol">Start column index.</param>
        /// <param name="nextRow">Next row index.</param>
        /// <param name="summaryDetail">Casino summary report.</param>
        /// <param name="isGrandTotal">Is grand total.</param>
        private void ExportCasinoDailySummaryAssignContent(
            ref ExcelWorksheet sheetData,
            int startRow,
            int startCol,
            ref int nextRow,
            CasinoDailySummaryDetail summaryDetail,
            bool isGrandTotal)
        {
            if (summaryDetail != null && summaryDetail.SystemSummary.Count > 0)
            {
                var color = isGrandTotal ? Color.FromArgb(205, 222, 255) : Color.FromArgb(255, 255, 255);
                var backgroundNormalFormat = new ExcelUtils().GetStyleIntNumber(string.Empty, color, isGrandTotal, false);
                sheetData.Cells[startRow + nextRow, startCol].Style = backgroundNormalFormat;
                sheetData.Cells[startRow + nextRow, startCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                sheetData.Cells[startRow + nextRow, startCol].Value = summaryDetail.MainCategory;

                var nextCol = 0;
                for (int i = 0; i < summaryDetail.SystemSummary.Count; i++)
                {
                    var systemSummary = summaryDetail.SystemSummary[i];
                    for (int j = 0; j < systemSummary.Cells.Count; j++)
                    {
                        sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundNormalFormat;
                        sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = systemSummary.Cells[j].Value;
                    }
                }

                nextRow++;
            }
        }

        /// <summary>
        /// Assign to total row.
        /// </summary>
        /// <param name="sheetData">Excel work sheet.</param>
        /// <param name="startRow">Start row index.</param>
        /// <param name="startCol">Start column index.</param>
        /// <param name="nextRow">Next row index.</param>
        /// <param name="grandTotal">Grand total.</param>
        /// <param name="groupTotal">Group total.</param>
        private void ExportCasinoDailySummaryAssignTotal(
            ref ExcelWorksheet sheetData,
            int startRow,
            int startCol,
            ref int nextRow,
            CasinoDailySummaryDetail grandTotal,
            List<int> groupTotal)
        {
            if (grandTotal != null && grandTotal.SystemSummary != null)
            {
                var nextCol = 0;
                var backgroundNormalFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(255, 255, 255));
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Style = backgroundNormalFormat;
                backgroundNormalFormat.Font.Color = Color.Blue;

                for (int i = 0; i < grandTotal.SystemSummary.Count; i++)
                {
                    var mergeCols = grandTotal.SystemSummary[i].Cells.Count - 1;
                    sheetData.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol += 1), startRow + nextRow, startCol + nextCol + mergeCols).Merged = true;
                    sheetData.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + nextCol, startRow + nextRow, startCol + nextCol + mergeCols).Style = backgroundNormalFormat;
                    sheetData.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + nextCol, startRow + nextRow, startCol + nextCol + mergeCols).Value = groupTotal[i];
                    nextCol += mergeCols;
                }

                nextRow++;
            }
        }

        private void ExportCasinoDailyComplaintsDetail(DailyComplaintsDTO complaints, DateTime fromDate, DateTime toDate, ExcelFile excelFile, string title)
        {
            var startCol = 0;
            var startRow = 2;
            var nextRow = 0;
            var sheetData = excelFile.Worksheets.Add("Casino Detail");
            ExportCasinoDailyComplaintsDetailAssignHeader(ref sheetData, startRow, startCol, fromDate, title);
            ExportCasinoDailyComplaintsDetailAssignContent(ref sheetData, startRow, startCol, nextRow, fromDate, complaints);
        }

        /// <summary>
        /// Assign to header.
        /// </summary>
        /// <param name="sheetData">Excel work sheet.</param>
        /// <param name="startRow">Start row index.</param>
        /// <param name="startCol">Start column index.</param>
        /// <param name="reportDate">Report date.</param>
        /// <param name="title">Report title.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private void ExportCasinoDailyComplaintsDetailAssignHeader(ref ExcelWorksheet sheetData, int startRow, int startCol, DateTime reportDate, string title)
        {
            var backgroundHeaderNormalFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var backgroundHeaderMainFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(130, 199, 255));

            var nextCol = 0;
            sheetData.Rows[startRow - 2].Height = 255 * 2;
            sheetData.Rows[startRow - 1].Height = 255 * 2;

            sheetData.Cells[startRow - 1, startCol].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "date");
            sheetData.Columns[startCol].Width = 255 * 10;

            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "StartTime");
            sheetData.Columns[startCol].Width = 255 * 10;

            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "EndTime");
            sheetData.Columns[startCol].Width = 255 * 10;

            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Duration");
            sheetData.Columns[startCol].Width = 255 * 10;

            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCustomer");
            sheetData.Columns[startCol + nextCol].Width = 255 * 15;

            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = "Site";
            sheetData.Columns[startCol + nextCol].Width = 255 * 15;

            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "productname");
            sheetData.Columns[startCol + nextCol].Width = 255 * 15;

            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "typeofcontact");
            sheetData.Columns[startCol + nextCol].Width = 255 * 15;

            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "ExtNo");
            sheetData.Columns[startCol + nextCol].Width = 255 * 10;

            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "ContactMSNSKypeEmail");
            sheetData.Columns[startCol + nextCol].Width = 255 * 30;

            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "country");
            sheetData.Columns[startCol + nextCol].Width = 255 * 15;

            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCategory");
            sheetData.Columns[startCol + nextCol].Width = 255 * 30;

            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "complaintsEnquiries");
            sheetData.Columns[startCol + nextCol].Width = 255 * 60;

            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "solution");
            sheetData.Columns[startCol + nextCol].Width = 255 * 60;

            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblStatus");
            sheetData.Columns[startCol + nextCol].Width = 255 * 15;

            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = "Response";
            sheetData.Columns[startCol + nextCol].Width = 255 * 15;

            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "handleBy");
            sheetData.Columns[startCol + nextCol].Width = 255 * 15;

            sheetData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
            sheetData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "CreationTime");
            sheetData.Columns[startCol + nextCol].Width = 255 * 15;

            sheetData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Merged = true;
            sheetData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Style = backgroundHeaderMainFormat;
            sheetData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Value = title.ToUpper();
        }

        /// <summary>
        /// Assign to content.
        /// </summary>
        /// <param name="sheetData">Excel work sheet.</param>
        /// <param name="startRow">Start row index.</param>
        /// <param name="startCol">Start column index.</param>
        /// <param name="nextRow">Next row index.</param>
        /// <param name="reportDate">Report date.</param>
        /// <param name="complaints">Customer complaints.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private void ExportCasinoDailyComplaintsDetailAssignContent(
            ref ExcelWorksheet sheetData,
            int startRow,
            int startCol,
            int nextRow,
            DateTime reportDate,
            DailyComplaintsDTO complaints)
        {
            var backgroundFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(255, 255, 255), HorizontalAlignmentStyle.Center, ExcelFont.NormalWeight);

            foreach (DailyComplaintsDetail detail in complaints.DailyComplaints)
            {
                var nextCol = 0;
                var complains = Helper.ReplaceHtmlTag(detail.Complaints);
                var solution = Helper.ReplaceHtmlTag(detail.Solution);

                sheetData.Cells[startRow + nextRow, startCol].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol].Value = detail.FeedbackDate;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.FeedbackTime;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = string.Format("{0: hh:mm tt}", CastAide.AsDateTime(detail.ResolutionDate));

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.Duration;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.CustomerName;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.Licensee;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.ProductName;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.CommunicationTool.ContactBy;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.ExtNo;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = Regex.Replace(detail.CommunicationTool.Address, "<[^<>]+>", string.Empty);

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.Country;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.Category;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = complains;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = solution;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.Status;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.Response;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.HandlyBy;

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.CreationTime;

                startRow++;
            }
        }

        /// <summary>
        /// Export 2 excel - Casino weekly summary report.
        /// </summary>
        /// <param name="weeklySummaryBO">Weekly summary report.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="excelFile">Excel file.</param>
        /// <param name="reportTitle">Report title.</param>
        private void ExportCasinoWeeklySummaryReport(DailySummaryBO weeklySummaryBO, DateTime fromDate, DateTime toDate, ExcelFile excelFile, string reportTitle)
        {
            var startCol = 0;
            var startRow = 2;
            var nextRow = 0;
            var sheetData = excelFile.Worksheets.Add("Casino Weekly Summary");

            ExportCasinoWeeklySummaryReportAssignHeader(ref sheetData, startRow, startCol, ref nextRow, fromDate, toDate, weeklySummaryBO, reportTitle);
            if (weeklySummaryBO != null && weeklySummaryBO.DailySummaries != null && weeklySummaryBO.DailySummaries.Count > 0)
            {
                foreach (var dailySummaryDetail in weeklySummaryBO.DailySummaries)
                {
                    ExportCasinoDailySummaryReportAssignContent(ref sheetData, startRow, startCol, ref nextRow, dailySummaryDetail.Total, false, false);
                }

                ExportCasinoDailySummaryReportAssignContent(ref sheetData, startRow, startCol, ref nextRow, weeklySummaryBO.GrandTotal, true, false);
            }

            nextRow++;
        }

        /// <summary>
        /// Assign to header.
        /// </summary>
        /// <param name="sheetData">The sheet data.</param>
        /// <param name="startRow">Start row index.</param>
        /// <param name="startCol">Start column index.</param>
        /// <param name="nextRow">Next row index.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="dalySummaryBO">Daily summary report.</param>
        /// <param name="title">Report title.</param>
        private void ExportCasinoWeeklySummaryReportAssignHeader(
            ref ExcelWorksheet sheetData,
            int startRow,
            int startCol,
            ref int nextRow,
            DateTime fromDate,
            DateTime toDate,
            DailySummaryBO dalySummaryBO,
            string title)
        {
            var backgroundHeaderNormalFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var backgroundHeaderMainFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(130, 199, 255));
            var mergeCols = 0;
            if (dalySummaryBO != null && dalySummaryBO.DailySummaries != null && dalySummaryBO.DailySummaries.Count > 0)
            {
                mergeCols = dalySummaryBO.DailySummaries[0].Total.Cells.Count + 1;
            }

            sheetData.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + mergeCols).Merged = true;
            sheetData.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + mergeCols).Style = backgroundHeaderMainFormat;
            sheetData.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + mergeCols).Value = title;

            if (mergeCols > 1)
            {
                nextRow++;
                var nextCol = 0;
                sheetData.Columns[startCol].Width = 255 * 30;
                sheetData.Cells[startRow + nextRow, startCol].Style = backgroundHeaderNormalFormat;
                var complaintsEnquiriesTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "exportComplaintsEnquiriesTitle");
                sheetData.Cells[startRow + nextRow, startCol].Value = complaintsEnquiriesTitle;
                var cells = dalySummaryBO.DailySummaries[0].Total.Cells;

                for (int i = 0; i < cells.Count; i++)
                {
                    sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
                    sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = cells[i].Title;
                }

                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundHeaderNormalFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "grandTotal");
            }

            nextRow++;
        }

        /// <summary>
        /// Assign to content.
        /// </summary>
        /// <param name="sheetData">The sheet data.</param>
        /// <param name="startRow">Start row index.</param>
        /// <param name="startCol">Start column index.</param>
        /// <param name="nextRow">Next row index.</param>
        /// <param name="summaryDatail">Summary report detail.</param>
        /// <param name="isGrandTotal">Is grand total.</param>
        /// <param name="isTotal">Is total row.</param>
        private void ExportCasinoDailySummaryReportAssignContent(
            ref ExcelWorksheet sheetData,
            int startRow,
            int startCol,
            ref int nextRow,
            DailySummaryDetail summaryDatail,
            bool isGrandTotal,
            bool isTotal)
        {
            var color = isGrandTotal ? Color.FromArgb(130, 199, 255) : (isTotal ? Color.FromArgb(245, 238, 211) : Color.FromArgb(255, 255, 255));
            var backgroundNormalFormat = new ExcelUtils().GetStyleIntNumber(string.Empty, color, isGrandTotal || isTotal, false);

            sheetData.Cells[startRow + nextRow, startCol].Style = backgroundNormalFormat;
            sheetData.Cells[startRow + nextRow, startCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            sheetData.Cells[startRow + nextRow, startCol].Value = string.Format("{0}{1}", isTotal || isGrandTotal ? string.Empty : " - ", summaryDatail.MainCategory);
            var cells = summaryDatail.Cells;

            var nextCol = 0;
            for (int i = 0; i < cells.Count; i++)
            {
                sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundNormalFormat;
                sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = cells[i].Value;
            }

            sheetData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = backgroundNormalFormat;
            sheetData.Cells[startRow + nextRow, startCol + nextCol].Value = summaryDatail.Total;
            nextRow++;
        }

        /// <summary>
        /// Sending mail and deleting file.
        /// </summary>
        /// <param name="email">Email address.</param>
        /// <param name="reportName">Report name.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="reportType">Report type.</param>
        private void SendMailAndDeleteFile(string email, string reportName, string fromDate, string toDate, string reportType)
        {
            var mailSubject = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "reportMailSubject");
            var mailBody = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "reportMailBody");

            var subject = string.Format(mailSubject, reportType, string.Format("{0}-{1}", fromDate, toDate));
            var body = string.Format(mailBody, WebConfigApplicationSettings.ReportDownloadUrl, reportName);
            var mailContent = new MailSend()
            {
                HostName = MailSection.HostName.Gmail,
                To = email,
                Cc = string.Empty,
                Bcc = string.Empty,
                Title = string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "reportMailTitle"), reportType),
                Subject = subject,
                Body = body
            };

            SendMailExtension.SendMail(mailContent);
        }

        private void ReponseExcelFile(ExcelFile excelFile, string reportName)
        {
            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", reportName));
            excelFile.SaveXls(response.OutputStream);
        }

        private void ExceptionHandler(ref Result result, Exception ex)
        {
            result = new Result(1, ex.Message);
            LogExtensions.ExceptionHandler(ex);
        }

        #endregion
    }
}