// --------------------------------------------------------------------------
// <copyright file="StatisticByDateReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.StatisticByDateReport
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Criteria.StatisticByDateReport;
    using Fanex.CRM.Report.Entities.StatisticsReportByDate;
    using Fanex.CRM.Report.Models;
    using Fanex.CRM.Report.Models.StatisticsReportByDate;
    using Fanex.CRM.Report.Repositories.StatisticByDateReport;
    using Fanex.CRM.Report.Resources;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Statistic By Date Report Service.
    /// </summary>
    public class StatisticByDateReportService : ServiceBase, IStatisticByDateReportService
    {
        public StatisticByDateReportService()
            : this(null)
        {
        }

        public StatisticByDateReportService(IStatisticByDateRepository statisticByDateReportRepository)
        {
            StatisticByDateReportRepository = statisticByDateReportRepository ?? new StatisticByDateRepository();
        }

        public IStatisticByDateRepository StatisticByDateReportRepository { get; set; }

        public StatisticsReportByDateModel GetMainReport(GetMainReportCriteria criteria)
        {
            criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
            criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);

            var repositoryResult = StatisticByDateReportRepository.GetMainReport(criteria);
            var reportTitle = string.Format(
                GetMainReportTitle(),
                SiteHelper.GetSiteTypeName(criteria.SiteType),
                DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToShortFormatedDate(),
                DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.ToDate).ToShortFormatedDate());
            var communicationToolLength = repositoryResult.CommunicationTools.Count;
            var result = new StatisticsReportByDateModel
            {
                Id = criteria.SiteType,
                Name = reportTitle,
                AliasName = ChartHelper.ViewChartFromToDateTitle(
                   DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToShortFormatedDate(),
                   DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.ToDate).ToShortFormatedDate()),
                CommunicationTools = repositoryResult.CommunicationTools,
                ChildReports = new List<CategoryIssuesInfo>(),
                NumberOfIssueList = Enumerable.Repeat(0, communicationToolLength).ToList()
            };

            var mainCategoryGroups = repositoryResult.RawData.GroupBy(data =>
                 new
                 {
                     Id = data.MainCategoryId,
                     Name = data.MainCategoryName
                 });

            foreach (var mainCategoryGroup in mainCategoryGroups)
            {
                var categoryIssuesInfo = new CategoryIssuesInfo(mainCategoryGroup.Key.Id, mainCategoryGroup.Key.Name);
                categoryIssuesInfo.NumberOfIssueList = Enumerable.Repeat(0, communicationToolLength).ToList();
                categoryIssuesInfo.ChildReports = MainReportByMainCategoryGroup(mainCategoryGroup, categoryIssuesInfo);
                CategoryIssuesInfo.SumNumberOfIssueList(categoryIssuesInfo, result);
                result.ChildReports.Add(categoryIssuesInfo);
            }

            return result;
        }

        public virtual string GetMainReportTitle()
        {
            return string.Empty;
        }

        /// <summary>
        /// Exports the main report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public void ExportMainReport(GetMainReportCriteria criteria)
        {
            var report = GetMainReport(criteria);
            criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
            criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);
            var exportExcelInfo = new ExportExcelInfo
            {
                ExcelFile = new HSSFWorkbook(),
                MaxColumnIndex = report.CommunicationTools.Count + 1
            };

            exportExcelInfo.CreateSheet(GetMainReportSheetName());
            ExportMainReportHeader(report, exportExcelInfo);
            ExportMainReportBody(report, exportExcelInfo);
            string reportName = string.Format(
                 GetMainReportTitle(),
                 SiteHelper.GetSiteTypeName(criteria.SiteType),
                 DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToShortFormatedDateWithSlash(),
                 DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.ToDate).ToShortFormatedDateWithSlash());
            NpoiExcelExtensions.ResponseStreamExcel(
                exportExcelInfo.ExcelFile,
                NpoiExcelExtensions.StandardizeExcelReportName(reportName));
        }

        public virtual string GetMainReportSheetName()
        {
            return string.Empty;
        }

        public void ExportDetailReport(GetDetailReportCriteria criteria)
        {
            var report = DetailReport(criteria);
            var exportExcelInfo = new ExportExcelInfo
            {
                ExcelFile = new HSSFWorkbook(),
                MaxColumnIndex = 19
            };

            ExportDetailReportBody(report, exportExcelInfo);
            string reportName = string.Format(
                GetMainReportTitle(),
                SiteHelper.GetSiteTypeName(criteria.SiteType),
                DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToShortFormatedDateWithSlash(),
                DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.ToDate).ToShortFormatedDateWithSlash());
            NpoiExcelExtensions.ResponseStreamExcel(exportExcelInfo.ExcelFile, NpoiExcelExtensions.StandardizeExcelReportName(reportName));
        }

        public StatisticsDetailReportByDateModel DetailReport(GetDetailReportCriteria criteria)
        {
            criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
            criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);
            var reportData = StatisticByDateReportRepository.GetDetailReport(criteria);

            var reportTitle = string.Format(
                GetDetailReportTitle(),
                SiteHelper.GetSiteTypeName(criteria.SiteType),
                DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToShortFormatedDate(),
                DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.ToDate).ToShortFormatedDate());
            var result = new StatisticsDetailReportByDateModel
            {
                Name = reportTitle,
                ChildReports = new List<StatisticsDetailbyDateModel>(),
                StartChildIndex = criteria.StartIndex,
                TotalChildReports = reportData.Count
            };

            var pagedData = criteria.IsGetFullReport ?
                reportData :
                reportData.Skip(criteria.StartIndex).Take(criteria.PageSize).ToList();
            foreach (var item in pagedData)
            {
                var feedbackDate = (DateTime)DateTimeUtilize.ChangeTimeZoneToClientDateTime(item.StartDate);
                var resolutionDate = (DateTime)DateTimeUtilize.ChangeTimeZoneToClientDateTime(item.ResolutionTime);
                var timeSpan = resolutionDate - feedbackDate;

                item.StartDateString = feedbackDate.ToShortFormatedDate();
                item.StartTime = feedbackDate.ToHourMinuteMiddayTypeFormated();
                item.Duration = new TimeInfo
                {
                    Days = timeSpan.Days,
                    Hours = timeSpan.Hours,
                    Minutes = timeSpan.Minutes
                };
                item.CreationTimeString = DateTimeUtilize.ChangeTimeZoneToClient(item.CreationTime);
                item.ResolutionTimeString = resolutionDate.ToHourMinuteMiddayTypeFormated();
                result.ChildReports.Add(item);
            }

            return result;
        }

        public virtual string GetDetailReportTitle()
        {
            return string.Empty;
        }

        public List<CategoryIssuesInfo> MainReportByMainCategoryGroup(
            IGrouping<dynamic, StatisticByDateEntity> mainCategoryGroup,
            CategoryIssuesInfo mainCategoryInfo)
        {
            var result = new List<CategoryIssuesInfo>();
            foreach (var subCategory in mainCategoryGroup)
            {
                var subCategoryInfo = new CategoryIssuesInfo
                {
                    Id = subCategory.SubCategoryId,
                    Name = subCategory.SubCategoryName,
                    NumberOfIssueList = subCategory.NumberOfIssues
                };

                CategoryIssuesInfo.SumNumberOfIssueList(subCategoryInfo, mainCategoryInfo);
                result.Add(subCategoryInfo);
            }

            return result;
        }

        /// <summary>
        /// Exports the main report header.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="exportExcelInfo">The export excel information.</param>
        private void ExportMainReportHeader(
            StatisticsReportByDateModel report,
            ExportExcelInfo exportExcelInfo)
        {
            NpoiExcelExtensions.SetHeaderColorIndex(exportExcelInfo);
            var headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());

            exportExcelInfo.ResetCurrentColumn();
            var communicationToolLength = report != null ? report.CommunicationTools.Count : 0;
            var columnWidths = new List<int>();
            int issueColumnWidth = 100;
            columnWidths.Add(300);
            columnWidths.AddRange(Enumerable.Repeat(issueColumnWidth, communicationToolLength));
            columnWidths.Add(issueColumnWidth);
            exportExcelInfo.InitColumnWidthByFontWidth(columnWidths);

            ExportHeaderTitle(exportExcelInfo, report != null ? report.Name : string.Empty, headerStyle);

            exportExcelInfo.ResetCurrentColumn();
            exportExcelInfo.CreateCurrentRow();
            exportExcelInfo.RenderCell(Labels.Complaints_Enquiries, headerStyle);
            for (var i = 0; i < communicationToolLength; i++)
            {
                exportExcelInfo.RenderCell(report.CommunicationTools[i].Name, headerStyle);
            }

            exportExcelInfo.RenderCell(Labels.Grand_Total, headerStyle);
            exportExcelInfo.CurrentRow++;
        }

        /// <summary>
        /// Exports the main report body.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="exportExcelInfo">The export excel information.</param>
        private void ExportMainReportBody(
            StatisticsReportByDateModel report,
            ExportExcelInfo exportExcelInfo)
        {
            var communicationToolLength = report != null ? report.CommunicationTools.Count : 0;
            var mainCategoryLength = report == null ? 0 : report.ChildReports.Count;
            if (mainCategoryLength == 0)
            {
                NpoiExcelExtensions.RenderNoInfoMessage(exportExcelInfo, exportExcelInfo.CurrentRow);
                return;
            }

            var intNumberStyle =
                NpoiExcelExtensions.IntNumberStyle(exportExcelInfo.ExcelFile, NpoiExcelExtensions.BackgroundColorIndex, false, CRMHorizontalAlignment.Center);
            var textStyle =
                NpoiExcelExtensions.TextStyle(exportExcelInfo.ExcelFile, NpoiExcelExtensions.BackgroundColorIndex, false, CRMHorizontalAlignment.Left);
            for (var i = 0; i < mainCategoryLength; i++)
            {
                var mainCategory = report.ChildReports[i];
                ExportMainReportMainCategoryRow(mainCategory, exportExcelInfo, communicationToolLength);
                var subcategoryLength = mainCategory.ChildReports.Count;
                for (var j = 0; j < subcategoryLength; j++)
                {
                    var subCategory = mainCategory.ChildReports[j];
                    ExportMainReportRow(subCategory, exportExcelInfo, intNumberStyle, textStyle, communicationToolLength);
                }
            }

            var grandTotal = new CategoryIssuesInfo
            {
                Name = Labels.Grand_Total,
                NumberOfIssueList = report.NumberOfIssueList
            };

            ExportMainReportGrandTotalRow(grandTotal, exportExcelInfo, communicationToolLength);
        }

        /// <summary>
        /// Exports the main report main category row.
        /// </summary>
        /// <param name="issueInfo">The issue information.</param>
        /// <param name="exportExcelInfo">The export excel information.</param>
        /// <param name="communicationToolLength">Length of the communication tool.</param>
        private void ExportMainReportMainCategoryRow(
            CategoryIssuesInfo issueInfo,
            ExportExcelInfo exportExcelInfo,
            int communicationToolLength)
        {
            NpoiExcelExtensions.SetGroupBackgroundColorIndex(exportExcelInfo);
            var intNumberStyleMainCategory =
                NpoiExcelExtensions.IntNumberStyle(exportExcelInfo.ExcelFile, NpoiExcelExtensions.GroupBackgroundColorIndex, false, CRMHorizontalAlignment.Center);
            var textStyleMainCategory =
                NpoiExcelExtensions.TextStyle(exportExcelInfo.ExcelFile, NpoiExcelExtensions.GroupBackgroundColorIndex, false, CRMHorizontalAlignment.Left);

            var font = exportExcelInfo.ExcelFile.CreateFont();
            font.FontHeightInPoints = NpoiExcelExtensions.FontSize;
            font.FontName = NpoiExcelExtensions.FontName;
            font.Color = (short)HSSFColor.Blue.Index;
            font.Boldweight = (short)FontBoldWeight.Bold;
            intNumberStyleMainCategory.SetFont(font);
            textStyleMainCategory.SetFont(font);
            ExportMainReportRow(issueInfo, exportExcelInfo, intNumberStyleMainCategory, textStyleMainCategory, communicationToolLength);
        }

        /// <summary>
        /// Exports the main report grand total  row.
        /// </summary>
        /// <param name="issueInfo">The issue information.</param>
        /// <param name="exportExcelInfo">The export excel information.</param>
        /// <param name="communicationToolLength">Length of the communication tool.</param>
        private void ExportMainReportGrandTotalRow(
            CategoryIssuesInfo issueInfo,
            ExportExcelInfo exportExcelInfo,
            int communicationToolLength)
        {
            NpoiExcelExtensions.SetGrandTotalColorIndex(exportExcelInfo);
            var intNumberStyle = NpoiExcelExtensions.IntNumberStyleGrandTotal(exportExcelInfo.ExcelFile);
            intNumberStyle.Alignment = HorizontalAlignment.Center;
            var textStyle = NpoiExcelExtensions.TextStyleGrandTotal(exportExcelInfo.ExcelFile);
            ExportMainReportRow(issueInfo, exportExcelInfo, intNumberStyle, textStyle, communicationToolLength);
        }

        /// <summary>
        /// Exports the main report row.
        /// </summary>
        /// <param name="issueInfo">The issue information.</param>
        /// <param name="exportExcelInfo">The export excel information.</param>
        /// <param name="intNumberStyle">The integer number style.</param>
        /// <param name="textStyle">The text style.</param>
        /// <param name="communicationToolLength">Length of the communication tool.</param>
        private void ExportMainReportRow(
            CategoryIssuesInfo issueInfo,
            ExportExcelInfo exportExcelInfo,
            ICellStyle intNumberStyle,
            ICellStyle textStyle,
            int communicationToolLength)
        {
            exportExcelInfo.ResetCurrentColumn();
            exportExcelInfo.CreateCurrentRow();
            exportExcelInfo.RenderCell(issueInfo.Name, textStyle);
            for (var i = 0; i < communicationToolLength; i++)
            {
                exportExcelInfo.RenderCell(issueInfo.NumberOfIssueList[i], intNumberStyle);
            }

            exportExcelInfo.RenderCell(issueInfo.NumberOfIssueGrandTotal, intNumberStyle);
            exportExcelInfo.CurrentRow++;
        }

        /// <summary>
        /// Exports the detail report header.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="exportExcelInfo">The export excel information.</param>
        private void ExportDetailReportHeader(StatisticsDetailReportByDateModel report, ExportExcelInfo exportExcelInfo)
        {
            NpoiExcelExtensions.SetHeaderColorIndex(exportExcelInfo);
            var headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());
            var leftTitleStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());
            leftTitleStyle.Alignment = HorizontalAlignment.Left;

            exportExcelInfo.ResetCurrentColumn();
            var columnWidths = new List<int> { 100, 100, 100, 100, 200, 140, 140, 100, 100, 250, 120, 120, 300, 300, 480, 480, 100, 100, 140, 120 };
            exportExcelInfo.InitColumnWidthByFontWidth(columnWidths);

            ExportHeaderTitle(exportExcelInfo, report != null ? report.Name : string.Empty, leftTitleStyle);

            exportExcelInfo.ResetCurrentColumn();
            exportExcelInfo.CreateCurrentRow();
            exportExcelInfo.RenderCell(Labels.Date, headerStyle);
            exportExcelInfo.RenderCell(Labels.Start_Time, headerStyle);
            exportExcelInfo.RenderCell(Labels.End_Time, headerStyle);
            exportExcelInfo.RenderCell(Labels.Duration, headerStyle);
            exportExcelInfo.RenderCell(Labels.Customer, headerStyle);
            exportExcelInfo.RenderCell(Labels.Site, headerStyle);
            exportExcelInfo.RenderCell(Labels.Type_Of_Contact, headerStyle);
            exportExcelInfo.RenderCell(Labels.Symbol_Extension, headerStyle);
            exportExcelInfo.RenderCell(Labels.Role, headerStyle);
            exportExcelInfo.RenderCell(Labels.ContactInfo, headerStyle);
            exportExcelInfo.RenderCell(Labels.Product, headerStyle);
            exportExcelInfo.RenderCell(Labels.Country, headerStyle);
            exportExcelInfo.RenderCell(Labels.Category, headerStyle);
            exportExcelInfo.RenderCell(Labels.SubCategory, headerStyle);
            exportExcelInfo.RenderCell(Labels.Complaints_Enquiries, headerStyle);
            exportExcelInfo.RenderCell(Labels.Solution, headerStyle);
            exportExcelInfo.RenderCell(Labels.Status, headerStyle);
            exportExcelInfo.RenderCell(Labels.Response, headerStyle);
            exportExcelInfo.RenderCell(Labels.Handle_By, headerStyle);
            exportExcelInfo.RenderCell(Labels.Creation_Time, headerStyle);
            exportExcelInfo.CurrentRow++;
        }

        /// <summary>
        /// Exports the detail report body.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="exportExcelInfo">The export excel information.</param>
        private void ExportDetailReportBody(StatisticsDetailReportByDateModel report, ExportExcelInfo exportExcelInfo)
        {
            var reportSheetName = GetMainReportSheetName();
            if (report == null || report.TotalChildReports == 0)
            {
                exportExcelInfo.CreateSheet(reportSheetName);
                ExportDetailReportHeader(report, exportExcelInfo);
                NpoiExcelExtensions.RenderNoInfoMessage(exportExcelInfo, exportExcelInfo.CurrentRow);
                return;
            }

            var textStyle =
                NpoiExcelExtensions.TextStyle(exportExcelInfo.ExcelFile, NpoiExcelExtensions.BackgroundColorIndex, false, CRMHorizontalAlignment.Left);
            var centerTextStyle =
                NpoiExcelExtensions.TextStyle(exportExcelInfo.ExcelFile, NpoiExcelExtensions.BackgroundColorIndex, false, CRMHorizontalAlignment.Center);
            var childReportLength = report.ChildReports.Count;
            var maxRowPerSheet = NpoiExcelExtensions.MaxRowPerSheet;
            var numberOfSheet = (childReportLength / maxRowPerSheet) +
                (childReportLength % maxRowPerSheet != 0 ? 1 : 0);
            for (var sheetIndex = 0; sheetIndex < numberOfSheet; sheetIndex++)
            {
                var sheetName = numberOfSheet > 1 ?
                    string.Format("{0} - {1}", reportSheetName, sheetIndex + 1) :
                    reportSheetName;
                exportExcelInfo.CreateSheet(sheetName);

                exportExcelInfo.CurrentRow = 0;
                ExportDetailReportHeader(report, exportExcelInfo);
                var startIndex = sheetIndex * maxRowPerSheet;
                var endIndex = startIndex + maxRowPerSheet;
                for (var i = startIndex; i < endIndex && i < childReportLength; i++)
                {
                    ExportDetailReportRow(report.ChildReports[i], exportExcelInfo, textStyle, centerTextStyle);
                }
            }
        }

        /// <summary>
        /// Exports the detail report row.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="exportExcelInfo">The export excel information.</param>
        /// <param name="textStyle">The text style.</param>
        /// <param name="centerTextStyle">The center text style.</param>
        private void ExportDetailReportRow(
            StatisticsDetailbyDateModel info,
            ExportExcelInfo exportExcelInfo,
            ICellStyle textStyle,
            ICellStyle centerTextStyle)
        {
            var duration = string.Format(
                "{0}:{1}:{2}",
                info.Duration.Days.ToString("00"),
                info.Duration.Hours.ToString("00"),
                info.Duration.Minutes.ToString("00"));

            exportExcelInfo.ResetCurrentColumn();
            exportExcelInfo.CreateCurrentRow();
            exportExcelInfo.RenderCell(info.StartDateString, centerTextStyle);
            exportExcelInfo.RenderCell(info.StartTime, centerTextStyle);
            exportExcelInfo.RenderCell(info.ResolutionTimeString, centerTextStyle);
            exportExcelInfo.RenderCell(duration, centerTextStyle);
            exportExcelInfo.RenderCell(info.Username, centerTextStyle);
            exportExcelInfo.RenderCell(info.SiteInfo.Name, centerTextStyle);
            exportExcelInfo.RenderCell(info.CommunicationTool.Name, centerTextStyle);
            exportExcelInfo.RenderCell(info.Extension.Name, centerTextStyle);
            exportExcelInfo.RenderCell(info.Role, textStyle);
            exportExcelInfo.RenderCell(HtmlHelper.ReplaceHtmlTag(info.Contact), textStyle);
            exportExcelInfo.RenderCell(info.Product.Name, centerTextStyle);
            exportExcelInfo.RenderCell(info.Country.Name, centerTextStyle);
            exportExcelInfo.RenderCell(info.MainCategory.Name, centerTextStyle);
            exportExcelInfo.RenderCell(info.SubCategory.Name, centerTextStyle);
            exportExcelInfo.RenderCell(HtmlHelper.ReplaceHtmlTag(info.Remark), textStyle);
            exportExcelInfo.RenderCell(HtmlHelper.ReplaceHtmlTag(info.Solution), textStyle);
            exportExcelInfo.RenderCell(info.StatusName, centerTextStyle);
            exportExcelInfo.RenderCell(info.Response.Name, centerTextStyle);
            exportExcelInfo.RenderCell(info.Handler, centerTextStyle);
            exportExcelInfo.RenderCell(
                info.CreationTime.ToShortFormatedDate() + "\n" + info.CreationTime.ToLongTimeString(),
                centerTextStyle);
            exportExcelInfo.CurrentRow++;
        }
    }
}
