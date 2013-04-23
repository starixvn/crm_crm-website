namespace Fanex.CRM.Service.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Web;
    using DNAide.Core;
    using ExcelUtil.Spreadsheet;
    using Fanex.CRM.Entities;
    using Fanex.CRM.Entities.ViewModel;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Helper;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Infrastructure.Configuration;
    using System.IO;
    using System.Threading;

    public partial class FeedbackService
    {
        #region export 2 excel
        #region feedback statistic detail
        public void ExportExcelFeedbackStatisticDetail(int type, int valueId, int mainCategoryId, int subCategoryId, string fromDate, string toDate, string reportTitle)
        {
            try
            {
                #region local variables
                var getSessionData = CacheService.Get<DataGridViewDTO>(_FeedbackStatisticDetailMappedSessionKey);

                var startRow = 2;
                var startCol = 0;
                var reportName = "FeedbackStatisticDetail";
                var excelFile = new ExcelFile();
                var wsReport = excelFile.Worksheets.Add(reportName);
                var nextRow = 0;
                #endregion

                #region header

                wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 4).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 4).Style.Font.Weight
                    = ExcelFont.BoldWeight;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 4).Value =
                    reportTitle;
                ExportExcelFeedbackStatisticDetailAssignHeader(ref wsReport, startRow, startCol);
                #endregion

                #region content
                if (getSessionData != null && getSessionData.rows.Count > 0)
                {
                    foreach (FeedbackView feedbackView in getSessionData.rows)
                    {
                        ExportExcelFeedbackStatisticDetailAssignContent(ref wsReport, startRow, startCol, ref nextRow,
                                                                        feedbackView);
                    }
                }
                else
                {
                    wsReport.Cells[startRow, startCol].Style.Font.Color = Color.Red;
                    wsReport.Cells[startRow, startCol].Value =
                        ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language,
                                                           "lblThereisnoinfomation");
                }
                #endregion

                #region response file
                var response = HttpContext.Current.Response;
                response.Clear();
                response.ContentType = "application/vnd.ms-excel";
                response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}-{1:MMddyyyy}-{2:MMddyyyy}.xls", 
                    reportName, CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate)));
                excelFile.SaveXls(response.OutputStream);
                #endregion
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(ExportExcelFeedbackStatisticDetail) - {0}", ex.Message));
            }
        }

        private static void ExportExcelFeedbackStatisticDetailAssignHeader(ref ExcelWorksheet wsReport, int startRow, int startCol)
        {
            var nextCol = 0;
            var headerFormart = new ExcelUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            wsReport.Columns[startCol].Width = 45 * 256;
            wsReport.Columns[startCol + 1].Width = 15 * 256;
            wsReport.Columns[startCol + 2].Width = 25 * 256;
            wsReport.Columns[startCol + 3].Width = 10 * 256;
            wsReport.Columns[startCol + 4].Width = 15 * 256;

            wsReport.Cells[startRow - 1, startCol].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblFeedback");

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCustomer");

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCategory");

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblStatus");

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblRelatedCustomer");
        }

        private static void ExportExcelFeedbackStatisticDetailAssignContent(ref ExcelWorksheet wsReport, int startRow, int startCol, ref int nextRow, FeedbackView feedbackView)
        {
            var nextCol = 0;
            #region set color
            Color color;
            switch (feedbackView.Status.ToLower())
            {
                case "closed":
                    color = Color.FromArgb(204, 199, 186);
                    break;
                case "pending":
                    color = Color.FromArgb(251, 236, 136);
                    break;
                default:
                    color = Color.FromArgb(255, 255, 255);
                    break;

            }// <font color="Green">' Cell range width is 5 (F G H I J).</font>
            #endregion
            var currentRowFormat =
                        new ExcelUtils().GetStyleTitle(color, HorizontalAlignmentStyle.Left, ExcelFont.NormalWeight);

            wsReport.Rows[startRow + nextRow].Height = 4 * 256;
            var reviewBy = string.IsNullOrEmpty(feedbackView.Reviewer) ? string.Empty : string.Format("\n Reviewed by {0} on {1}", feedbackView.Reviewer, feedbackView.ReviewDate);
            var createBy = string.Format("\n By {0} on {1} (GMT -4)", feedbackView.Creator, feedbackView.CreateDate);
            var solvingDuration = string.IsNullOrEmpty(feedbackView.DurationTime) ? string.Empty : string.Format("\n Solving Duration: {0}", feedbackView.DurationTime);
            wsReport.Cells[startRow + nextRow, startCol].Style = currentRowFormat;
            wsReport.Cells[startRow + nextRow, startCol].Value = string.Format("{0} \n Feedback date {1} {2} {3} {4}", feedbackView.Subject, feedbackView.FeedbackDate, createBy, solvingDuration, reviewBy);

            currentRowFormat =
                        new ExcelUtils().GetStyleTitle(color, HorizontalAlignmentStyle.Center, ExcelFont.NormalWeight);

            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = currentRowFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = feedbackView.Username;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = currentRowFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = string.Format("{0} \n  {1}", feedbackView.MainCategory, feedbackView.SubCategory);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = currentRowFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = feedbackView.Status;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = currentRowFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = feedbackView.RelatedUsername;

            nextRow++;
        }
        #endregion

        #region feedback statistic
        public void ExportExcelFeedbackStatistic(int type, string fromDate, string toDate, string reportTitle)
        {
            try
            {
                #region local variables
                var param = new Params()
                {
                    Type = type,
                    FromDate = fromDate,
                    ToDate = toDate,
                    FromDuration = new SolvingDuration() { Day = "0", Hour = "0", Minute = "0", Second = "0" },
                    ToDuration = new SolvingDuration() { Day = "0", Hour = "0", Minute = "0", Second = "0" }
                };
                var getSessionData =
                (FeedbackStatisticsDTO)
                SessionExtension.GetSession(HttpContext.Current.Session, _FeedbackStatisticSessionKey) ??
                SelAllFeedbackStatistic(param);
                var startRow = 2;
                var startCol = 0;
                var reportName = "FeedbackStatistic";
                var excelFile = new ExcelFile();
                var wsReport = excelFile.Worksheets.Add(reportName);
                var nextRow = 0;
                #endregion

                if (getSessionData != null)
                {
                    #region Assign Header
                    #region title

                    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 2).Merged = true;
                    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 2).Style.Font.
                        Weight = ExcelFont.BoldWeight;
                    wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 2).Value =
                        reportTitle;

                    #endregion
                    ExportExcelFeedbackStatisticAssignHeader(ref wsReport, startRow, startCol, getSessionData.ColumnNames);
                    #endregion

                    #region content

                    foreach (FeedbackStatisticsGroup feedbackGroup in getSessionData.rows)
                    {
                        ExportExcelFeedbackStatisticAssignContent(ref wsReport, startRow, startCol, ref nextRow, feedbackGroup, getSessionData.ColumnNames);
                    }
                    #endregion

                    #region footer

                    ExportExcelFeedbackStatisticAssignFooter(ref wsReport, startRow, startCol, ref nextRow,
                                                             getSessionData.GrandTotal);

                    #endregion
                }

                #region response file
                var response = HttpContext.Current.Response;
                response.Clear();
                response.ContentType = "application/vnd.ms-excel";
                response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}-{1:MMddyyyy}-{2:MMddyyyy}.xls", 
                    reportName, CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate)));
                excelFile.SaveXls(response.OutputStream);
                #endregion
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(ExportExcelFeedbackStatistic) - {0}", ex.Message));
            }

        }

        private static void ExportExcelFeedbackStatisticAssignHeader(ref ExcelWorksheet wsReport, int startRow, int startCol, ArrayList columnNames)
        {
            var nextCol = 0;
            var headerFormart = new ExcelUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));

            #region left cols
            wsReport.Columns[startCol].Width = 30 * 256;
            wsReport.Cells[startRow - 1, startCol].Style = headerFormart;
            wsReport.Cells[startRow - 1, startCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblSubCategory");
            #endregion

            #region right cols

            foreach (var columnName in columnNames)
            {
                wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = headerFormart;
                wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = columnName;
            }
            #endregion
        }

        private static void ExportExcelFeedbackStatisticAssignContent(ref ExcelWorksheet wsReport, int startRow, int startCol, ref int nextRow, FeedbackStatisticsGroup feedbacks, ArrayList columnNames)
        {
            if (feedbacks != null)
            {
                var groupTitleFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(255, 238, 221));
                var totalChild = columnNames != null ? columnNames.Count : 0;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + totalChild).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + totalChild).Style =
                    groupTitleFormat;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + totalChild).Value =
                    feedbacks.Title;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + totalChild).Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                nextRow++;
                var index = 0;
                foreach (FeedbackStatistics feedback in feedbacks.FeedbackStatistics)
                {
                    var nextCol = 0;
                    var currentRowFormat =
                        new ExcelUtils().GetStyleTitle(
                            (index % 2 > 0 ? Color.FromArgb(255, 255, 255) : Color.FromArgb(255, 243, 200)),
                            HorizontalAlignmentStyle.Center,
                            feedback.IsTotal ? ExcelFont.BoldWeight : ExcelFont.NormalWeight);
                    #region left cols
                    wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = currentRowFormat;
                    wsReport.Cells[startRow + nextRow, startCol + nextCol].Style.HorizontalAlignment =
                        HorizontalAlignmentStyle.Left;
                    wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = string.Format("  {0}", feedback.SubCategory);
                    #endregion

                    #region right cols

                    foreach (var cell in feedback.Cells)
                    {
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = currentRowFormat;
                        wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = cell;

                    }
                    #endregion

                    index++;
                    nextRow++;
                }
            }
        }

        private static void ExportExcelFeedbackStatisticAssignFooter(ref ExcelWorksheet wsReport, int startRow, int startCol, ref int nextRow, FeedbackStatistics feedback)
        {
            if (feedback != null)
            {
                var nextCol = 0;
                var currentRowFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(211, 225, 200), HorizontalAlignmentStyle.Center, ExcelFont.BoldWeight);
                #region left cols
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = currentRowFormat;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = feedback.SubCategory;
                #endregion
                #region right cols
                foreach (var cell in feedback.Cells)
                {
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = currentRowFormat;
                    wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = cell;
                }
                #endregion
                nextRow++;
            }
        }

        #endregion

        public Result AllowExportFeedbackStatisticDetail()
        {
            var result = new Result();
            try
            {
                var getSessionData = CacheService.Get<DataGridViewDTO>(_FeedbackStatisticDetailMappedSessionKey);
                if (getSessionData != null && getSessionData.rows.Count > 0)
                {
                    result.ErrCode = 0;
                    result.ErrMsg = "Succesfully";
                }

            }
            catch (Exception ex)
            {
                result.ErrCode = 1;
                result.ErrMsg = ex.Message;
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(AllowExportFeedbackStatisticDetail) - {0}", ex.Message));
            }
            return result;
        }
        #endregion

        #region export daily summary
        public void ExportExcelIBCDailySummaryReport(string fromDate, string toDate, string reportTitle)
        {
            try
            {
                var excelFile = new ExcelFile();
                var reportName = "IBCSummaryReport";

                var getData = (DailySummaryBO)SessionExtension.GetSession(HttpContext.Current.Session, _IbcDailySummary);
                var fDate = CastAide.AsDateTime(fromDate);
                var tDate = CastAide.AsDateTime(toDate);

                if (getData != null)
                    ExportIBCDailySummaryReport(getData, fDate, tDate, excelFile, reportTitle);
                else
                {
                    var newData = GetIBCDailySummaryReport(fromDate, toDate);
                    ExportIBCDailySummaryReport(newData, fDate, tDate, excelFile, reportTitle);
                }
                #region response file
                var response = HttpContext.Current.Response;
                response.Clear();
                response.ContentType = "application/vnd.ms-excel";
                response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}-{1:MMddyyyy}-{2:MMddyyyy}.xls", reportName, fDate, tDate));
                excelFile.SaveXls(response.OutputStream);
                #endregion
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(ExportExcelIBCDailySummaryReport) - {0}", ex.Message));
            }
        }

        private void ExportIBCDailySummaryReport(DailySummaryBO dailySummaryBO, DateTime fromDate, DateTime toDate, ExcelFile excelFile, string reportTitle)
        {
            #region private variables
            var startCol = 0;
            var startRow = 2;
            var nextRow = 0;
            var wsDailySummary = excelFile.Worksheets.Add("Credit Site Summary");
            #endregion

            #region Header
            //title

            wsDailySummary.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 8).Merged = true;
            wsDailySummary.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 8).Style.Font.Weight = ExcelFont.BoldWeight;
            wsDailySummary.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 8).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsDailySummary.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 8).Style.Font.
                Color = Color.Blue;
            wsDailySummary.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 8).Value = reportTitle.ToUpper();
            ExportIBCDailySummaryReportAssignHeader(ref wsDailySummary, startRow, startCol, ref nextRow, fromDate, toDate, dailySummaryBO, false);
            #endregion

            #region Content
            if (dailySummaryBO != null && dailySummaryBO.DailySummaries != null && dailySummaryBO.DailySummaries.Count > 0)
            {
                foreach (var dailySummaryDetail in dailySummaryBO.DailySummaries)
                {
                    ExportIBCDailySummaryReportAssignContent(ref wsDailySummary, startRow, startCol, ref nextRow, dailySummaryDetail.Total, false, true);
                    foreach (var row in dailySummaryDetail.DailySummaries)
                    {
                        ExportIBCDailySummaryReportAssignContent(ref wsDailySummary, startRow, startCol, ref nextRow, row, false, false);
                    }

                }
                ExportIBCDailySummaryReportAssignContent(ref wsDailySummary, startRow, startCol, ref nextRow, dailySummaryBO.GrandTotal, true, false);

            }
            #endregion

        }

        private void ExportIBCDailySummaryReportAssignHeader(ref ExcelWorksheet wsDailySummary, int startRow, int startCol, ref int nextRow, DateTime fromDate, DateTime toDate, DailySummaryBO dalySummaryBO, bool isLicensee)
        {
            var bgHeaderNormalFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var bgHeaderMainFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(130, 199, 255));
            var mergeCols = 0;
            if (dalySummaryBO != null && dalySummaryBO.DailySummaries != null && dalySummaryBO.DailySummaries.Count > 0)
                mergeCols = dalySummaryBO.DailySummaries[0].Total.Cells.Count + 1;

            wsDailySummary.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + mergeCols).Merged = true;
            wsDailySummary.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + mergeCols).Style = bgHeaderMainFormat;
            wsDailySummary.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + mergeCols).Value =
                fromDate == toDate ? fromDate.ToString("MMMM dd, yyyy") : string.Format("From {0} To {1}", fromDate.ToString("MMMM dd, yyyy"), toDate.ToString("MMMM dd, yyyy"));

            if (mergeCols > 1)
            {
                nextRow++;
                var nextCol = 0;
                wsDailySummary.Columns[startCol].Width = 255 * 30;
                wsDailySummary.Cells[startRow + nextRow, startCol].Style = bgHeaderNormalFormat;
                wsDailySummary.Cells[startRow + nextRow, startCol].Value = isLicensee
                                                                               ? ResourceExtension.GetResourceValue(
                                                                                   ResourceExtension.Resources.Language,
                                                                                   "licensee")
                                                                               : ResourceExtension.GetResourceValue(
                                                                                   ResourceExtension.Resources.Language,
                                                                                   "exportComplaintsEnquiriesTitle");
                var cells = dalySummaryBO.DailySummaries[0].Total.Cells;

                for (int i = 0; i < cells.Count; i++)
                {
                    wsDailySummary.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
                    wsDailySummary.Cells[startRow + nextRow, startCol + (nextCol)].Value = cells[i].Title;
                }

                wsDailySummary.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
                wsDailySummary.Cells[startRow + nextRow, startCol + (nextCol)].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "grandTotal");
            }
            nextRow++;
        }

        private void ExportIBCDailySummaryReportAssignContent(ref ExcelWorksheet wsDailySummary, int startRow, int startCol, ref int nextRow, DailySummaryDetail summaryDatail, bool isGrandTotal, bool isTotal)
        {
            var color = isGrandTotal ? Color.FromArgb(130, 199, 255) : (isTotal ? Color.FromArgb(245, 238, 211) : Color.FromArgb(255, 255, 255));
            var bgNormalFormat = new ExcelUtils().GetStyleIntNumber("", color, isGrandTotal || isTotal, false);

            wsDailySummary.Cells[startRow + nextRow, startCol].Style = bgNormalFormat;
            wsDailySummary.Cells[startRow + nextRow, startCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            wsDailySummary.Cells[startRow + nextRow, startCol].Value = string.Format("{0}{1}", (isTotal || isGrandTotal ? "" : " - "), summaryDatail.MainCategory);
            var cells = summaryDatail.Cells;

            var nextCol = 0;
            for (int i = 0; i < cells.Count; i++)
            {
                wsDailySummary.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgNormalFormat;
                wsDailySummary.Cells[startRow + nextRow, startCol + (nextCol)].Value = cells[i].Value;
            }
            wsDailySummary.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgNormalFormat;
            wsDailySummary.Cells[startRow + nextRow, startCol + (nextCol)].Value = summaryDatail.Total;

            nextRow++;
        }
        #endregion

        #region Export daily detail
        public void ExportExcelIBCDailyDetailReport(string fromDate, string toDate, string reportTitle, string email, bool isSendMail = false)
        {
            try
            {
                var excelFile = new ExcelFile();
                var reportName = "IBCDetailReport";
                var getData = CacheService.Get<DailyComplaintsDTO>(_IbcDailyDetail);
                var fDate = CastAide.AsDateTime(fromDate);
                var tDate = CastAide.AsDateTime(toDate);
                if (isSendMail)
                {
                    DataTable sessionData = null;
                    if (getData == null)                    
                        sessionData = (DataTable)SessionExtension.GetSession(HttpContext.Current.Session, _IbcDailyDetailNotMapped);

                    #region export excel file to Hardware and send mail confirm to CSD
                    var dailyComplaintsData = new DailyComplaintsDTO();
                    dailyComplaintsData.DailyComplaints = new List<DailyComplaintsDetail>();

                    Task.Factory.StartNew(() =>
                    {                       
                        Thread.Sleep(200);
                        var fileName = string.Format("{0}-{1:MMddyyyy}-{2:MMddyyyy}", reportName, fDate, tDate);

                        getData = getData ?? MapperIBCDailyDetailReport(sessionData.AsEnumerable().Select(c => c));
                        if (getData != null)
                        {
                            ExportDailyComplaintsDetail(getData, fDate, tDate, excelFile, reportTitle);

                            if (!Directory.Exists(WebConfigApplicationSettings.ReportFolderPath))
                                Directory.CreateDirectory(WebConfigApplicationSettings.ReportFolderPath);
                            excelFile.SaveXls(string.Format(@"{0}\{1}.xls", WebConfigApplicationSettings.ReportFolderPath, fileName));

                            SendMailAndDeleteFile(email, fileName, fromDate, toDate, "Daily");
                        }
                    });
                    #endregion

                }
                else
                {
                    if (getData != null)
                    {
                        ExportDailyComplaintsDetail(getData, fDate, tDate, excelFile, reportTitle);
                        #region response file
                        var response = HttpContext.Current.Response;
                        response.Clear();
                        response.ContentType = "application/vnd.ms-excel";
                        response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", string.Format("{0}-{1:MMddyyyy}-{2:MMddyyyy}", reportName, fDate, tDate)));
                        excelFile.SaveXls(response.OutputStream);
                        #endregion
                    }
                }
                
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(ExportExcelIBCDailyDetailReport) - {0}", ex.Message));
            }
        }

        private void ExportDailyComplaintsDetail(DailyComplaintsDTO complaints, DateTime fromDate, DateTime toDate, ExcelFile excelFile, string title)
        {
            #region private variables
            var startCol = 0;
            var startRow = 2;
            var nextRow = 0;
            var wsDailyData = excelFile.Worksheets.Add(string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "exportDailyComplaintsDetail"), "Detail"));
            #endregion

            #region Header
            ExportDailyComplaintsDetailAssignHeader(ref wsDailyData, startRow, startCol, fromDate, title);
            #endregion

            #region Content
            ExportDailyComplaintsDetailAssignContent(ref wsDailyData, startRow, startCol, nextRow, fromDate, complaints);
            #endregion

        }
        private void ExportDailyComplaintsDetailAssignHeader(ref ExcelWorksheet wsDailyData, int startRow, int startCol, DateTime reportDate, string title)
        {
            var bgHeaderNormalFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var bgHeaderMainFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(130, 199, 255));

            #region sencond row
            var nextCol = 0;
            wsDailyData.Rows[startRow - 2].Height = 255 * 2;
            wsDailyData.Rows[startRow - 1].Height = 255 * 2;

            wsDailyData.Cells[startRow - 1, startCol].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "date");
            wsDailyData.Columns[startCol].Width = 255 * 10;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Time");
            wsDailyData.Columns[startCol].Width = 255 * 10;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCustomer");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 15;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value = "Site";
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 15;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "typeofcontact");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 15;


            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "ExtNo");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 10;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "ContactMSNSKypeEmail");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 30;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Product");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 15;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "country");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 15;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCategory");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 30;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "complaintsEnquiries");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 60;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "solution");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 60;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblStatus");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 15;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value = "Response";
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 15;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "handleBy");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 15;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "CreationTime");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 15;
            #endregion

            #region first row
            wsDailyData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Merged = true;
            wsDailyData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Style = bgHeaderMainFormat;
            wsDailyData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Value = title.ToUpper();
            #endregion
        }
        private void ExportDailyComplaintsDetailAssignContent(ref ExcelWorksheet wsDailyData, int startRow, int startCol, int nextRow, DateTime reportDate, DailyComplaintsDTO complaints)
        {
            var bgFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(255, 255, 255), HorizontalAlignmentStyle.Center, ExcelFont.NormalWeight);

            foreach (DailyComplaintsDetail detail in complaints.DailyComplaints)
            {
                var nextCol = 0;
                var complains = Helper.ReplaceHtmlTag(detail.Complaints);
                var solution = Helper.ReplaceHtmlTag(detail.Solution);

                wsDailyData.Cells[startRow + nextRow, startCol].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol].Value = detail.FeedbackDate;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.FeedbackTime;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.CustomerName;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.Licensee;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.CommunicationTool.ContactBy;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.ExtNo;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = Regex.Replace(detail.CommunicationTool.Address, "<[^<>]+>", "");

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.ProductName;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.Country;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.Category;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = complains;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = solution;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.Status;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.Response;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.HandlyBy;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.CreationTime;

                startRow++;
            }
        }
        #endregion

        #region export licensee summary
        public void ExportExcelLicenseeDailySummaryReport(string fromDate, string toDate, string reportTitle)
        {
            try
            {
                var excelFile = new ExcelFile();
                var reportName = "LicenseeSummaryReport";
                var getData = (DailySummaryBO)SessionExtension.GetSession(HttpContext.Current.Session, _licenseeDailySummary);
                var fDate = CastAide.AsDateTime(fromDate);
                var tDate = CastAide.AsDateTime(toDate);

                if (getData != null)
                    ExportLicenseeDailySummaryReport(getData, fDate, tDate, excelFile, reportTitle);
                else
                {
                    var newData = GetIBCDailySummaryReport(fromDate, toDate);
                    ExportLicenseeDailySummaryReport(newData, fDate, tDate, excelFile, reportTitle);
                }
                #region response file
                var response = HttpContext.Current.Response;
                response.Clear();
                response.ContentType = "application/vnd.ms-excel";
                response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}-{1:MMddyyyy}-{2:MMddyyyy}.xls", reportName, fDate, tDate));
                excelFile.SaveXls(response.OutputStream);
                #endregion
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(ExportExcelLicenseeDailySummaryReport) - {0}", ex.Message));
            }
        }

        private void ExportLicenseeDailySummaryReport(DailySummaryBO dailySummaryBO, DateTime fromDate, DateTime toDate, ExcelFile excelFile, string reportTitle)
        {
            #region private variables
            var startCol = 0;
            var startRow = 2;
            var nextRow = 0;
            var wsDailySummary = excelFile.Worksheets.Add("Licensee Summary");
            #endregion

            #region Header
            wsDailySummary.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 8).Merged = true;
            wsDailySummary.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 8).Style.Font.Weight = ExcelFont.BoldWeight;
            wsDailySummary.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 8).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsDailySummary.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 8).Style.Font.
                Color = Color.Blue;
            wsDailySummary.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 8).Value =
                reportTitle.ToUpper();
            ExportLicenseeDailySummaryReportAssignHeader(ref wsDailySummary, startRow, startCol, ref nextRow, fromDate, toDate, dailySummaryBO, false);
            #endregion

            #region Content
            if (dailySummaryBO != null && dailySummaryBO.DailySummaries != null && dailySummaryBO.DailySummaries.Count > 0)
            {
                foreach (var dailySummaryDetail in dailySummaryBO.DailySummaries)
                {
                    ExportLicenseeDailySummaryReportAssignContent(ref wsDailySummary, startRow, startCol, ref nextRow, dailySummaryDetail.Total, false, true);
                    foreach (var row in dailySummaryDetail.DailySummaries)
                    {
                        ExportLicenseeDailySummaryReportAssignContent(ref wsDailySummary, startRow, startCol, ref nextRow, row, false, false);
                    }

                }
                ExportLicenseeDailySummaryReportAssignContent(ref wsDailySummary, startRow, startCol, ref nextRow, dailySummaryBO.GrandTotal, true, false);

            }
            #endregion

        }

        private void ExportLicenseeDailySummaryReportAssignHeader(ref ExcelWorksheet wsDailySummary, int startRow, int startCol, ref int nextRow, DateTime fromDate, DateTime toDate, DailySummaryBO dalySummaryBO, bool isLicensee)
        {
            var bgHeaderNormalFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var bgHeaderMainFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(130, 199, 255));
            var mergeCols = 0;
            if (dalySummaryBO != null && dalySummaryBO.DailySummaries != null && dalySummaryBO.DailySummaries.Count > 0)
                mergeCols = dalySummaryBO.DailySummaries[0].Total.Cells.Count + 1;

            wsDailySummary.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + mergeCols).Merged = true;
            wsDailySummary.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + mergeCols).Style = bgHeaderMainFormat;
            wsDailySummary.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + mergeCols).Value =
                fromDate == toDate ? fromDate.ToString("MMMM dd, yyyy") : string.Format("From {0} To {1}", fromDate.ToString("MMMM dd, yyyy"), toDate.ToString("MMMM dd, yyyy"));

            if (mergeCols > 1)
            {
                nextRow++;
                var nextCol = 0;
                wsDailySummary.Columns[startCol].Width = 255 * 30;
                wsDailySummary.Cells[startRow + nextRow, startCol].Style = bgHeaderNormalFormat;
                wsDailySummary.Cells[startRow + nextRow, startCol].Value = isLicensee
                                                                               ? ResourceExtension.GetResourceValue(
                                                                                   ResourceExtension.Resources.Language,
                                                                                   "licensee")
                                                                               : ResourceExtension.GetResourceValue(
                                                                                   ResourceExtension.Resources.Language,
                                                                                   "exportComplaintsEnquiriesTitle");
                var cells = dalySummaryBO.DailySummaries[0].Total.Cells;

                for (int i = 0; i < cells.Count; i++)
                {
                    wsDailySummary.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
                    wsDailySummary.Cells[startRow + nextRow, startCol + (nextCol)].Value = cells[i].Title;
                }

                wsDailySummary.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
                wsDailySummary.Cells[startRow + nextRow, startCol + (nextCol)].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "grandTotal");
            }
            nextRow++;
        }

        private void ExportLicenseeDailySummaryReportAssignContent(ref ExcelWorksheet wsDailySummary, int startRow, int startCol, ref int nextRow, DailySummaryDetail summaryDatail, bool isGrandTotal, bool isTotal)
        {
            var color = isGrandTotal ? Color.FromArgb(130, 199, 255) : (isTotal ? Color.FromArgb(245, 238, 211) : Color.FromArgb(255, 255, 255));
            var bgNormalFormat = new ExcelUtils().GetStyleIntNumber("", color, isGrandTotal || isTotal, false);

            wsDailySummary.Cells[startRow + nextRow, startCol].Style = bgNormalFormat;
            wsDailySummary.Cells[startRow + nextRow, startCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            wsDailySummary.Cells[startRow + nextRow, startCol].Value = string.Format("{0}{1}", (isTotal || isGrandTotal ? "" : " - "), summaryDatail.MainCategory);
            var cells = summaryDatail.Cells;

            var nextCol = 0;
            for (int i = 0; i < cells.Count; i++)
            {
                wsDailySummary.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgNormalFormat;
                wsDailySummary.Cells[startRow + nextRow, startCol + (nextCol)].Value = cells[i].Value;
            }
            wsDailySummary.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgNormalFormat;
            wsDailySummary.Cells[startRow + nextRow, startCol + (nextCol)].Value = summaryDatail.Total;

            nextRow++;
        }
        #endregion

        #region export licensee detail report
        public void ExportExcelLicenseeDetailReport(string fromDate, string toDate, string reportTitle, string email, bool isSendMail)
        {
            try
            {
                var excelFile = new ExcelFile();
                var reportName = "LicenseeDetailReport";
                var getData = CacheService.Get<DailyComplaintsDTO>(_licenseeDailyDetail);
                var fDate = CastAide.AsDateTime(fromDate);
                var tDate = CastAide.AsDateTime(toDate);

                if (isSendMail)
                {
                    DataTable sessionData = null;
                    if (getData == null)
                        sessionData = (DataTable)SessionExtension.GetSession(HttpContext.Current.Session, _LicenseeDailyDetailNotMapped);
                    
                    
                    #region export excel file to Hardware and send mail confirm to CSD
                    var dailyComplaintsData = new DailyComplaintsDTO();
                    dailyComplaintsData.DailyComplaints = new List<DailyComplaintsDetail>();

                    Task.Factory.StartNew(() =>
                    {
                        Thread.Sleep(200);
                        var fileName = string.Format("{0}-{1:MMddyyyy}-{2:MMddyyyy}", reportName, fDate, tDate);
                        getData = getData ?? MapperLicenseeDailyDetailReport(sessionData.AsEnumerable().Select(c => c));
                        if (getData != null)
                        {
                            ExportLicenseeComplaintsDetail(getData, fDate, tDate, excelFile, reportTitle);

                            if (!Directory.Exists(WebConfigApplicationSettings.ReportFolderPath))
                                Directory.CreateDirectory(WebConfigApplicationSettings.ReportFolderPath);
                            excelFile.SaveXls(string.Format(@"{0}\{1}.xls", WebConfigApplicationSettings.ReportFolderPath, fileName));

                            SendMailAndDeleteFile(email, fileName, fromDate, toDate, "Daily");
                        }
                    });
                    #endregion

                }
                else
                {
                    if (getData != null)
                    {
                        ExportLicenseeComplaintsDetail(getData, fDate, tDate, excelFile, reportTitle);
                        #region response file
                        var response = HttpContext.Current.Response;
                        response.Clear();
                        response.ContentType = "application/vnd.ms-excel";
                        response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", string.Format("{0}-{1:MMddyyyy}-{2:MMddyyyy}", reportName, fDate, tDate)));
                        excelFile.SaveXls(response.OutputStream);
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(ExportExcelLicenseeDetailReport) - {0}", ex.Message));
            }
        }
        private void ExportLicenseeComplaintsDetail(DailyComplaintsDTO complaints, DateTime fromDate, DateTime toDate, ExcelFile excelFile, string title)
        {
            #region private variables
            var startCol = 0;
            var startRow = 2;

            var nextRow = 0;
            #region all licensee
            var wsDailyData = excelFile.Worksheets.Add("All Licensee");
            #endregion

            #region Header
            ExportLicenseeComplaintsDetailAssignHeader(ref wsDailyData, startRow, startCol, fromDate, title);
            #endregion

            #region Content
            ExportLicenseeComplaintsDetailAssignContent(ref wsDailyData, startRow, startCol, nextRow, fromDate, complaints.DailyComplaints);
            #endregion
            #endregion

            #region each licensee

            var licenseese = (from q in complaints.DailyComplaints select q.Licensee).Distinct().ToList();
            foreach (var licensee in licenseese)
            {
                var licenseeData =
                    (from q in complaints.DailyComplaints where q.Licensee.Equals(licensee) select q).ToList();
                nextRow = 0;
                var wsLicensee = excelFile.Worksheets.Add(licensee.Replace("N/A", "N_A"));
                #region Header
                ExportLicenseeComplaintsDetailAssignHeader(ref wsLicensee, startRow, startCol, fromDate, title.Replace("Licensee", licensee));
                #endregion

                #region Content
                ExportLicenseeComplaintsDetailAssignContent(ref wsLicensee, startRow, startCol, nextRow, fromDate, licenseeData);
                #endregion
            }

            #endregion


        }
        private void ExportLicenseeComplaintsDetailAssignHeader(ref ExcelWorksheet wsDailyData, int startRow, int startCol, DateTime reportDate, string title)
        {
            var bgHeaderNormalFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var bgHeaderMainFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(130, 199, 255));

            #region sencond row
            var nextCol = 0;
            wsDailyData.Rows[startRow - 2].Height = 255 * 2;
            wsDailyData.Rows[startRow - 1].Height = 255 * 2;

            wsDailyData.Cells[startRow - 1, startCol].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "date");
            wsDailyData.Columns[startCol].Width = 255 * 10;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Time");
            wsDailyData.Columns[startCol].Width = 255 * 10;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCustomer");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 15;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value = "Site";
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 15;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "typeofcontact");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 15;


            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "ExtNo");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 10;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "ContactMSNSKypeEmail");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 30;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Product");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 15;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "country");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 15;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCategory");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 30;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "complaintsEnquiries");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 60;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "solution");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 60;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblStatus");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 15;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value = "Response";
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 15;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "handleBy");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 15;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "CreationTime");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 15;
            #endregion

            #region first row
            wsDailyData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Merged = true;
            wsDailyData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Style = bgHeaderMainFormat;
            wsDailyData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Value = title.ToUpper();
            #endregion
        }
        private void ExportLicenseeComplaintsDetailAssignContent(ref ExcelWorksheet wsDailyData, int startRow, int startCol, int nextRow, DateTime reportDate, List<DailyComplaintsDetail> complaints)
        {
            var bgFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(255, 255, 255), HorizontalAlignmentStyle.Center, ExcelFont.NormalWeight);

            foreach (DailyComplaintsDetail detail in complaints)
            {
                var nextCol = 0;
                var complains = Helper.ReplaceHtmlTag(detail.Complaints);
                var solution = Helper.ReplaceHtmlTag(detail.Solution);

                wsDailyData.Cells[startRow + nextRow, startCol].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol].Value = detail.FeedbackDate;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.FeedbackTime;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.CustomerName;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.Licensee;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.CommunicationTool.ContactBy;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.ExtNo;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value =
                    Regex.Replace(detail.CommunicationTool.Address, "<[^<>]+>", "");

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.ProductName;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.Country;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.Category;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = complains;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Style.HorizontalAlignment =
                    HorizontalAlignmentStyle.Left;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = solution;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Style.HorizontalAlignment =
                    HorizontalAlignmentStyle.Left;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.Status;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.Response;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.HandlyBy;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.CreationTime;

                startRow++;
            }
        }
        #endregion

        #region export casino summary
        public void ExportExcelCasinoDailySummaryReport(string fromDate, string toDate, string reportTitle)
        {
            try
            {
                var excelFile = new ExcelFile();
                var reportName = "CasinoSummaryReport";

                var getCasinoData = (CasinoDailySummary)SessionExtension.GetSession(HttpContext.Current.Session, _CasinoDailySummary);
                var fDate = CastAide.AsDateTime(fromDate);
                var tDate = CastAide.AsDateTime(toDate);

                if (getCasinoData == null)
                    getCasinoData = GetCasinoDailySummaryReport(fromDate, toDate);
                ExportCasinoDailySummary(getCasinoData, fDate, tDate, reportTitle, excelFile);
                #region response file
                var response = HttpContext.Current.Response;
                response.Clear();
                response.ContentType = "application/vnd.ms-excel";
                response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}-{1:MMddyyyy}-{2:MMddyyyy}.xls", reportName, fDate, tDate));
                excelFile.SaveXls(response.OutputStream);
                #endregion
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(ExportExcelCasinoDailySummaryReport) - {0}", ex.Message));
            }
        }

        private void ExportCasinoDailySummary(CasinoDailySummary casinoDailySummary, DateTime fromDate, DateTime toDate, string reportTitle, ExcelFile excelFile)
        {
            #region private variables

            var startCol = 0;
            var startRow = 2;
            var nextRow = 0;
            var wsReport = excelFile.Worksheets.Add("Casino Summary");
            #endregion

            #region Header

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 8).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 8).Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 8).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 8).Style.Font.
                Color = Color.Blue;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 8).Value = reportTitle;
            #endregion

            if (casinoDailySummary.DailySummary != null && casinoDailySummary.DailySummary.Count > 0)
            {
                #region Content
                ExportCasinoDailySummaryAssignHeader(ref wsReport, startRow, startCol, ref nextRow, fromDate,
                                                     casinoDailySummary, false);
                foreach (CasinoDailySummaryDetail dailySummary in casinoDailySummary.DailySummary)
                {
                    ExportCasinoDailySummaryAssignContent(ref wsReport, startRow, startCol, ref nextRow, dailySummary,
                                                          false);
                }

                #endregion

                #region footer

                ExportCasinoDailySummaryAssignContent(ref wsReport, startRow, startCol, ref nextRow,
                                                      casinoDailySummary.GrandTotal, true);

                ExportCasinoDailySummaryAssignTotal(ref wsReport, startRow, startCol, ref nextRow,
                                                    casinoDailySummary.GrandTotal, casinoDailySummary.SumAllFeedbacks);


                #endregion
            }
            else
                wsReport.Cells[startRow + nextRow, startCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata");

            nextRow++;
        }

        private void ExportCasinoDailySummaryAssignHeader(ref ExcelWorksheet wsReport, int startRow, int startCol, ref int nextRow, DateTime reportDate, CasinoDailySummary casinoDailySummary, bool isWeekly)
        {
            var weeklyHeaderColor = isWeekly ? Color.FromArgb(30, 134, 218) : Color.FromArgb(130, 199, 255);
            var bgHeaderNormalFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var bgHeaderMainFormat = new ExcelUtils().GetStyleTitle(weeklyHeaderColor);
            var mergeCol = 0;

            wsReport.Columns[startCol].Width = 255 * 30;
            #region Merge the header
            if (casinoDailySummary != null && casinoDailySummary.DailySummary != null && casinoDailySummary.DailySummary.Count > 0)
            {
                var totalCells = 0;
                for (int i = 0; i < casinoDailySummary.DailySummary[0].SystemSummary.Count; i++)
                {
                    totalCells += casinoDailySummary.DailySummary[0].SystemSummary[i].Cells.Count;
                }
                mergeCol = totalCells;
            }
            #endregion

            if (mergeCol > 1)
            {
                var nextCol = 0;

                #region left col - main cateogry

                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + nextCol, startRow + nextRow + 1,
                                                   startCol + nextCol).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + nextCol, startRow + nextRow + 1,
                                                   startCol + nextCol).Style = bgHeaderNormalFormat;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + nextCol, startRow + nextRow + 1,
                                                   startCol + nextCol).Value =
                    ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCategory");

                #endregion

                #region right cols - system data

                for (int i = 0; i < casinoDailySummary.DailySummary[0].SystemSummary.Count; i++)
                {
                    var systemSummary = casinoDailySummary.DailySummary[0].SystemSummary[i];
                    var totalMergeCols = systemSummary.Cells.Count - 1;

                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol += 1), startRow + nextRow,
                                                       startCol + nextCol + totalMergeCols).Merged = true;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol), startRow + nextRow,
                                                       startCol + nextCol + totalMergeCols).Style = bgHeaderNormalFormat;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol), startRow + nextRow,
                                                       startCol + nextCol + totalMergeCols).Value = systemSummary.Sytem;


                    var nextCol2 = 0;
                    for (int j = 0; j < systemSummary.Cells.Count; j++)
                    {
                        wsReport.Cells[startRow + nextRow + 1, startCol + (nextCol2) + nextCol].Style = bgHeaderNormalFormat;
                        wsReport.Cells[startRow + nextRow + 1, startCol + (nextCol2) + nextCol].Value =
                            systemSummary.Cells[j].Title;
                        nextCol2 += 1;
                    }

                    nextCol += totalMergeCols;
                }
                nextRow++;

                #endregion
            }
            else
                wsReport.Cells[startRow + nextRow, startCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata");
            nextRow++;
        }

        private void ExportCasinoDailySummaryAssignContent(ref ExcelWorksheet wsReport, int startRow, int startCol, ref int nextRow, CasinoDailySummaryDetail summaryDetail, bool isGrandTotal)
        {
            if (summaryDetail != null && summaryDetail.SystemSummary.Count > 0)
            {
                var color = isGrandTotal ? Color.FromArgb(205, 222, 255) : Color.FromArgb(255, 255, 255);
                var bgNormalFormat = new ExcelUtils().GetStyleIntNumber("", color, isGrandTotal, false);
                wsReport.Cells[startRow + nextRow, startCol].Style = bgNormalFormat;
                wsReport.Cells[startRow + nextRow, startCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                wsReport.Cells[startRow + nextRow, startCol].Value = summaryDetail.MainCategory;

                var nextCol = 0;
                for (int i = 0; i < summaryDetail.SystemSummary.Count; i++)
                {
                    var systemSummary = summaryDetail.SystemSummary[i];
                    for (int j = 0; j < systemSummary.Cells.Count; j++)
                    {
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgNormalFormat;
                        wsReport.Cells[startRow + nextRow, startCol + nextCol].Value =
                            systemSummary.Cells[j].Value;
                    }
                }

                nextRow++;
            }
        }

        private void ExportCasinoDailySummaryAssignTotal(ref ExcelWorksheet wsReport, int startRow, int startCol, ref int nextRow, CasinoDailySummaryDetail grandTotal, List<int> groupTotal)
        {
            if (grandTotal != null && grandTotal.SystemSummary != null)
            {
                var nextCol = 0;
                var bgNormalFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(255, 255, 255));
                wsReport.Cells[startRow + nextRow, startCol + nextCol].Style = bgNormalFormat;
                bgNormalFormat.Font.Color = Color.Blue;

                for (int i = 0; i < grandTotal.SystemSummary.Count; i++)
                {
                    var mergeCols = grandTotal.SystemSummary[i].Cells.Count - 1;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol += 1), startRow + nextRow,
                                                       startCol + nextCol + mergeCols).Merged = true;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol), startRow + nextRow,
                                                       startCol + nextCol + mergeCols).Style = bgNormalFormat;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol), startRow + nextRow,
                                                       startCol + nextCol + mergeCols).Value = groupTotal[i];
                    nextCol += mergeCols;
                }
                nextRow++;
            }
        }
        #endregion

        #region export casino detail
        public void ExportExcelCasinoDailyDetailReport(string fromDate, string toDate, string reportTitle, string email, bool isSendMail)
        {
            try
            {
                var excelFile = new ExcelFile();
                var reportName = "CasinoDetailReport";
                var getData = CacheService.Get<DailyComplaintsDTO>(_CasinoDailyDetail);
                var fDate = CastAide.AsDateTime(fromDate);
                var tDate = CastAide.AsDateTime(toDate);

                if (isSendMail)
                {
                    DataTable sessionData = null;
                    if (getData == null)
                        sessionData = (DataTable)SessionExtension.GetSession(HttpContext.Current.Session, _CasinoDailyDetailNotMapped);

                    #region export excel file to Hardware and send mail confirm to CSD
                    var dailyComplaintsData = new DailyComplaintsDTO();
                    dailyComplaintsData.DailyComplaints = new List<DailyComplaintsDetail>();

                    Task.Factory.StartNew(() =>
                    {
                        Thread.Sleep(200);
                        var fileName = string.Format("{0}-{1:MMddyyyy}-{2:MMddyyyy}", reportName, fDate, tDate);
                        getData = getData ?? MapperCasinoDailyDetailReport(sessionData.AsEnumerable().Select(c => c));
                        if (getData != null)
                        {
                            ExportCasinoDailyComplaintsDetail(getData, fDate, tDate, excelFile, reportTitle);

                            if (!Directory.Exists(WebConfigApplicationSettings.ReportFolderPath))
                                Directory.CreateDirectory(WebConfigApplicationSettings.ReportFolderPath);
                            excelFile.SaveXls(string.Format(@"{0}\{1}.xls", WebConfigApplicationSettings.ReportFolderPath, fileName));

                            SendMailAndDeleteFile(email, fileName, fromDate, toDate, "Daily");
                        }
                    });
                    #endregion

                }
                else
                {
                    if (getData != null)
                    {
                        ExportCasinoDailyComplaintsDetail(getData, fDate, tDate, excelFile, reportTitle);

                        #region response file
                        var response = HttpContext.Current.Response;
                        response.Clear();
                        response.ContentType = "application/vnd.ms-excel";
                        response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", string.Format("{0}-{1:MMddyyyy}-{2:MMddyyyy}", reportName, fDate, tDate)));
                        excelFile.SaveXls(response.OutputStream);
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(ExportExcelCasinoDailyDetailReport) - {0}", ex.Message));
            }
        }

        private void ExportCasinoDailyComplaintsDetail(DailyComplaintsDTO complaints, DateTime fromDate, DateTime toDate, ExcelFile excelFile, string title)
        {
            #region private variables
            var startCol = 0;
            var startRow = 2;
            var nextRow = 0;
            var wsDailyData = excelFile.Worksheets.Add("Casino Detail");
            #endregion

            #region Header
            ExportCasinoDailyComplaintsDetailAssignHeader(ref wsDailyData, startRow, startCol, fromDate, title);
            #endregion

            #region Content
            ExportCasinoDailyComplaintsDetailAssignContent(ref wsDailyData, startRow, startCol, nextRow, fromDate, complaints);
            #endregion

        }
        private void ExportCasinoDailyComplaintsDetailAssignHeader(ref ExcelWorksheet wsDailyData, int startRow, int startCol, DateTime reportDate, string title)
        {
            var bgHeaderNormalFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var bgHeaderMainFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(130, 199, 255));


            #region sencond row
            var nextCol = 0;
            wsDailyData.Rows[startRow - 2].Height = 255 * 2;
            wsDailyData.Rows[startRow - 1].Height = 255 * 2;

            wsDailyData.Cells[startRow - 1, startCol].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "date");
            wsDailyData.Columns[startCol].Width = 255 * 10;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Time");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 10;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCustomer");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 15;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value = "Site";
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 15;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "productname");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 15;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "typeofcontact");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 15;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "ExtNo");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 10;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "ContactMSNSKypeEmail");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 30;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "country");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 15;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCategory");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 30;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "complaintsEnquiries");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 60;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "solution");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 60;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblStatus");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 15;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value = "Response";
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 15;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "handleBy");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 15;

            wsDailyData.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsDailyData.Cells[startRow - 1, startCol + nextCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "CreationTime");
            wsDailyData.Columns[startCol + nextCol].Width = 255 * 15;
            #endregion

            #region first row

            wsDailyData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Merged = true;
            wsDailyData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Style = bgHeaderMainFormat;
            wsDailyData.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Value = title.ToUpper();
            #endregion
        }
        private void ExportCasinoDailyComplaintsDetailAssignContent(ref ExcelWorksheet wsDailyData, int startRow, int startCol, int nextRow, DateTime reportDate, DailyComplaintsDTO complaints)
        {
            var bgFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(255, 255, 255), HorizontalAlignmentStyle.Center, ExcelFont.NormalWeight);

            foreach (DailyComplaintsDetail detail in complaints.DailyComplaints)
            {
                var nextCol = 0;
                var complains = Helper.ReplaceHtmlTag(detail.Complaints);
                var solution = Helper.ReplaceHtmlTag(detail.Solution);

                wsDailyData.Cells[startRow + nextRow, startCol].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol].Value = detail.FeedbackDate;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.FeedbackTime;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.CustomerName;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.Licensee;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.ProductName;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.CommunicationTool.ContactBy;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.ExtNo;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = Regex.Replace(detail.CommunicationTool.Address, "<[^<>]+>", "");

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.Country;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.Category;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = complains;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = solution;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.Status;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.Response;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.HandlyBy;

                wsDailyData.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgFormat;
                wsDailyData.Cells[startRow + nextRow, startCol + nextCol].Value = detail.CreationTime;

                startRow++;
            }
        }
        #endregion

        #region export casino weekly summary
        public void ExportExcelCasinoWeeklySummaryReport(string fromDate, string toDate, string reportTitle)
        {
            try
            {
                var excelFile = new ExcelFile();
                var reportName = "CasinoSummaryReport";
                var getCasinoData = (DailySummaryBO)SessionExtension.GetSession(HttpContext.Current.Session, _CasinoWeeklySummary);
                if (getCasinoData == null)
                    getCasinoData = GetCasinoWeeklySummaryReport(fromDate, toDate);
                var fDate = CastAide.AsDateTime(fromDate);
                var tDate = CastAide.AsDateTime(toDate);

                ExportCasinoWeeklySummaryReport(getCasinoData, fDate, tDate, excelFile, reportTitle);

                #region response file
                var response = HttpContext.Current.Response;
                response.Clear();
                response.ContentType = "application/vnd.ms-excel";
                response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}-{1:MMddyyyy}-{2:MMddyyyy}.xls", reportName, fDate, tDate));
                excelFile.SaveXls(response.OutputStream);
                #endregion
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(ExportExcelCasinoWeeklySummaryReport) - {0}", ex.Message));
            }
        }

        private void ExportCasinoWeeklySummaryReport(DailySummaryBO weeklySummaryBO, DateTime fromDate, DateTime toDate, ExcelFile excelFile, string reportTitle)
        {
            #region private variables
            var startCol = 0;
            var startRow = 2;
            var nextRow = 0;
            var wsDailySummary = excelFile.Worksheets.Add("Casino Weekly Summary");
            #endregion

            #region Content IBC
            ExportCasinoWeeklySummaryReportAssignHeader(ref wsDailySummary, startRow, startCol, ref nextRow, fromDate, toDate, weeklySummaryBO, reportTitle);
            if (weeklySummaryBO != null && weeklySummaryBO.DailySummaries != null && weeklySummaryBO.DailySummaries.Count > 0)
            {
                foreach (var dailySummaryDetail in weeklySummaryBO.DailySummaries)
                {
                    ExportCasinoDailySummaryReportAssignContent(ref wsDailySummary, startRow, startCol, ref nextRow, dailySummaryDetail.Total, false, true);
                }
                ExportCasinoDailySummaryReportAssignContent(ref wsDailySummary, startRow, startCol, ref nextRow, weeklySummaryBO.GrandTotal, true, false);

            }
            nextRow++;

            #endregion

        }

        private void ExportCasinoWeeklySummaryReportAssignHeader(ref ExcelWorksheet wsDailySummary, int startRow, int startCol, ref int nextRow, DateTime fromDate, DateTime toDate, DailySummaryBO dalySummaryBO, string title)
        {
            var bgHeaderNormalFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var bgHeaderMainFormat = new ExcelUtils().GetStyleTitle(Color.FromArgb(130, 199, 255));
            var mergeCols = 0;
            if (dalySummaryBO != null && dalySummaryBO.DailySummaries != null && dalySummaryBO.DailySummaries.Count > 0)
                mergeCols = dalySummaryBO.DailySummaries[0].Total.Cells.Count + 1;

            wsDailySummary.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + mergeCols).Merged = true;
            wsDailySummary.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + mergeCols).Style = bgHeaderMainFormat;
            wsDailySummary.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + mergeCols).Value =
                 title;

            if (mergeCols > 1)
            {
                nextRow++;
                var nextCol = 0;
                wsDailySummary.Columns[startCol].Width = 255 * 30;
                wsDailySummary.Cells[startRow + nextRow, startCol].Style = bgHeaderNormalFormat;
                wsDailySummary.Cells[startRow + nextRow, startCol].Value = ResourceExtension.GetResourceValue(
                                                                                   ResourceExtension.Resources.Language,
                                                                                   "exportComplaintsEnquiriesTitle");
                var cells = dalySummaryBO.DailySummaries[0].Total.Cells;

                for (int i = 0; i < cells.Count; i++)
                {
                    wsDailySummary.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
                    wsDailySummary.Cells[startRow + nextRow, startCol + (nextCol)].Value = cells[i].Title;
                }

                wsDailySummary.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
                wsDailySummary.Cells[startRow + nextRow, startCol + (nextCol)].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "grandTotal");
            }
            nextRow++;
        }

        private void ExportCasinoDailySummaryReportAssignContent(ref ExcelWorksheet wsDailySummary, int startRow, int startCol, ref int nextRow, DailySummaryDetail summaryDatail, bool isGrandTotal, bool isTotal)
        {
            var color = isGrandTotal ? Color.FromArgb(130, 199, 255) : (isTotal ? Color.FromArgb(245, 238, 211) : Color.FromArgb(255, 255, 255));
            var bgNormalFormat = new ExcelUtils().GetStyleIntNumber("", color, isGrandTotal || isTotal, false);

            wsDailySummary.Cells[startRow + nextRow, startCol].Style = bgNormalFormat;
            wsDailySummary.Cells[startRow + nextRow, startCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            wsDailySummary.Cells[startRow + nextRow, startCol].Value = string.Format("{0}{1}", (isTotal || isGrandTotal ? "" : " - "), summaryDatail.MainCategory);
            var cells = summaryDatail.Cells;

            var nextCol = 0;
            for (int i = 0; i < cells.Count; i++)
            {
                wsDailySummary.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgNormalFormat;
                wsDailySummary.Cells[startRow + nextRow, startCol + (nextCol)].Value = cells[i].Value;
            }
            wsDailySummary.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgNormalFormat;
            wsDailySummary.Cells[startRow + nextRow, startCol + (nextCol)].Value = summaryDatail.Total;

            nextRow++;
        }
        #endregion


        public Result AllowExportIbcStatisticDetail()
        {
            var result = new Result();
            try
            {

                var getSessionData = CacheService.Get<DailyComplaintsDTO>(_IbcDailyDetail);
                if (getSessionData != null && getSessionData.DailyComplaints.Count > 0)
                {
                    result.ErrCode = 0;
                    result.ErrMsg = "Succesfully";
                }
                else
                {
                    result.ErrCode = 1;
                    result.ErrMsg = "Data has not mapped yet";
                    
                }
            }
            catch (Exception ex)
            {
                result.ErrCode = 1;
                result.ErrMsg = ex.Message;
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(AllowExportIbcStatisticDetail) - {0}", ex.Message));
            }
            return result;
        }

        public Result AllowExportCasinoStatisticDetail()
        {
            var result = new Result();
            try
            {
                var getSessionData = CacheService.Get<DailyComplaintsDTO>(_CasinoDailyDetail);
                if (getSessionData != null && getSessionData.DailyComplaints.Count > 0)
                {
                    result.ErrCode = 0;
                    result.ErrMsg = "Succesfully";
                }
                else
                {
                    result.ErrCode = 1;
                    result.ErrMsg = "Data has not mapped yet";
                }

            }
            catch (Exception ex)
            {
                result.ErrCode = 1;
                result.ErrMsg = ex.Message;
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(AllowExportCasinoStatisticDetail) - {0}", ex.Message));
            }
            return result;
        }

        public Result AllowExportLicenseeStatisticDetail()
        {
            var result = new Result();
            try
            {
                var getSessionData = CacheService.Get<DailyComplaintsDTO>(_licenseeDailyDetail);
                if (getSessionData != null && getSessionData.DailyComplaints.Count > 0)
                {
                    result.ErrCode = 0;
                    result.ErrMsg = "Succesfully";
                }
                else
                {
                    result.ErrCode = 1;
                    result.ErrMsg = "Data has not mapped yet";
                }

            }
            catch (Exception ex)
            {
                result.ErrCode = 1;
                result.ErrMsg = ex.Message;
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(AllowExportLicenseeStatisticDetail) - {0}", ex.Message));
            }
            return result;
        }

        private void SendMailAndDeleteFile(string email, string reportName, string fromDate, string toDate, string reportType)
        {
            var mailContent = new MailSend()
            {
                HostName = MailSection.HostName.Gmail,
                To = email,
                Cc = string.Empty,
                Bcc = string.Empty,
                Title = string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "reportMailTitle"), reportType),
                Subject = string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "reportMailSubject"), reportType, string.Format("{0}-{1}", fromDate, toDate)),
                Body = string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "reportMailBody"), 
                    WebConfigApplicationSettings.ReportDownloadUrl, reportName)

            };

            SendMailExtension.SendMail(mailContent);

            //Task.Factory.StartNew(() => {
            //    ////set waiting time to delete file
            //    Thread.Sleep((WebConfigApplicationSettings.ReportDeleteFileTime * 1000) * 60);
            //    var reportFilePath = string.Format("{0}\\{1}.xls", WebConfigApplicationSettings.ReportFolderPath, reportName);
            //    if (File.Exists(reportFilePath))
            //        File.Delete(reportFilePath);
            //});
            

        }
    }
}
