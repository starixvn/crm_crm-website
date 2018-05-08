// -----------------------------------------------------------------------
// <copyright file="LogStatisticsReportDetailExcel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Excels.LogStatistics
{
    using Fanex.CRM.Common.Excel;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Models.StatisticsReportByDate;
    using Fanex.CRM.Report.Resources;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class LogStatisticsReportDetailExcel : BaseExcel
    {
        private readonly StatisticsDetailReportByDateModel _logStatisticsReportDetail;
        private readonly IEnumerable<Common.Models.Setting> _mainCategory;
        private readonly IEnumerable<Common.Models.Setting> _communicationToolList;
        private readonly IEnumerable<Common.Models.SettingRelation> _extensionList;
        private readonly IEnumerable<Common.Models.Product> _productList;
        private readonly IEnumerable<Common.Models.Product> _subProductList;

        public LogStatisticsReportDetailExcel(
            StatisticsDetailReportByDateModel logStatisticsReportDetail,
            string excelFileName,
            IEnumerable<Common.Models.Setting> mainCategory,
            IEnumerable<Common.Models.Setting> communicationToolList,
            IEnumerable<Common.Models.SettingRelation> extensionList,
            IEnumerable<Common.Models.Product> productList,
            IEnumerable<Common.Models.Product> subProductList
        )
            : base(excelFileName)
        {
            _logStatisticsReportDetail = logStatisticsReportDetail;
            _mainCategory = mainCategory;
            _communicationToolList = communicationToolList;
            _extensionList = extensionList;
            _productList = productList;
            _subProductList = subProductList;
        }

        public override void BuildWorkBook()
        {
            var exportExcelInfo = new ExportExcelInfo
            {
                ExcelFile = WorkBook,
                StartRow = 0,
                StartColumn = 0,
                CurrentRow = 0,
                MaxColumnIndex = 20
            };

            GenerateReportDetail(exportExcelInfo);

            NpoiExtensions.ResponseStreamExcel(exportExcelInfo.ExcelFile, ExcelFileName);
        }

        private string GetMainCategoryName(int mainCategoryId)
        {
            var mainCategory = _mainCategory.FirstOrDefault(c => c.Id == mainCategoryId);
            return mainCategory == null ? string.Empty : mainCategory.Name;
        }

        private string GetSubCategoryName(int mainCategoryId, int subCategoryId)
        {
            var mainCategory = _mainCategory.FirstOrDefault(c => c.Id == mainCategoryId);
            var child = mainCategory == null ? null : mainCategory.ChildSettings.FirstOrDefault(c => c.Id == subCategoryId);
            return child == null ? string.Empty : child.Name;
        }

        private string GetProductName(int id)
        {
            var product = _productList.FirstOrDefault(c => c.Id == id);
            return product == null ? "N/A" : product.Name;
        }

        private string GetSubProductName(int id)
        {
            var subProduct = _subProductList.FirstOrDefault(c => c.Id == id);
            return subProduct == null ? "N/A" : subProduct.Name;
        }

        private string GetCommunicationName(int id)
        {
            var item = _communicationToolList.FirstOrDefault(c => c.Id == id);
            return item == null ? string.Empty : item.Name;
        }

        private string GetExtensionName(int id)
        {
            var item = _extensionList.FirstOrDefault(c => c.Id == id);
            return item == null ? "N/A" : item.Value;
        }


        private void GenerateDetailReportRowData(
            StatisticsDetailbyDateModel info,
            ExportExcelInfo exportExcelInfo,
            ICellStyle textStyle,
            ICellStyle centerTextStyle)
        {
            var duration = info.Duration;
            var durationText = string.Format(
                "{0}:{1}:{2}",
                duration.Days.ToString("00"),
                duration.Hours.ToString("00"),
                duration.Minutes.ToString("00"));

            var categoryContents = info.Settings
                    .Where(i => i.SettingType == "Category")
                    .GroupBy(s => s.SettingIdLv1)
                    .Select(s =>
                    {
                        var con = GetMainCategoryName(s.Key);
                        var subIds = s.Select(e => e.SettingIdLv2).ToList();
                        foreach (var id in subIds)
                        {
                            con = con + "\n  " + GetSubCategoryName(s.Key, id);
                        }

                        return con;
                    })
                    .ToArray();

            var productContents = info.Settings
                    .Where(i => i.SettingType == "Product")
                   .GroupBy(s => s.SettingIdLv1)
                   .Select(s =>
                   {
                       var con = GetProductName(s.Key);
                       var subIds = s.Select(e => e.SettingIdLv2).ToList();
                       foreach (var id in subIds)
                       {
                           con = con + "\n  " + GetSubProductName(id);
                       }

                       return con;
                   })
                   .ToArray();

            var communicationContents = info.Settings
                    .Where(i => i.SettingType == "Communication")
                    .Select(s =>
                    {
                        return GetCommunicationName(s.SettingIdLv1);
                    })
                    .ToArray();

            var extensionContents = info.Settings
                   .Where(i => i.SettingType == "Station")
                   .Select(s =>
                   {
                       return GetExtensionName(s.SettingIdLv1);
                   })
                   .ToArray();

            exportExcelInfo.ResetCurrentColumn();
            exportExcelInfo.CreateCurrentRow();
            exportExcelInfo.RenderCell(info.StartDateString, centerTextStyle);
            exportExcelInfo.RenderCell(info.StartTime, centerTextStyle);
            exportExcelInfo.RenderCell(info.ResolutionTimeString, centerTextStyle);
            exportExcelInfo.RenderCell(durationText, centerTextStyle);
            exportExcelInfo.RenderCell(info.Username, centerTextStyle);
            exportExcelInfo.RenderCell(info.SiteInfo.Name, centerTextStyle);
            exportExcelInfo.RenderCell(String.Join("\n", communicationContents), centerTextStyle);
            exportExcelInfo.RenderCell(String.Join("\n", extensionContents), centerTextStyle);
            exportExcelInfo.RenderCell(info.Role, textStyle);
            exportExcelInfo.RenderCell(HtmlHelper.RemoveHtmlTags(info.Contact), textStyle);
            exportExcelInfo.RenderCell(String.Join("\n\n", productContents), textStyle);
            exportExcelInfo.RenderCell(info.Country.Name, centerTextStyle);
            exportExcelInfo.RenderCell(String.Join("\n\n", categoryContents), textStyle);
            exportExcelInfo.RenderCell(HtmlHelper.RemoveHtmlTags(info.RemarkText), textStyle);
            exportExcelInfo.RenderCell(HtmlHelper.RemoveHtmlTags(info.Solution), textStyle);
            exportExcelInfo.RenderCell(info.StatusName, centerTextStyle);
            exportExcelInfo.RenderCell(info.Response.Name, centerTextStyle);
            exportExcelInfo.RenderCell(info.Handler, centerTextStyle);
            exportExcelInfo.RenderCell(
                info.CreationTime.ToShortFormatedDate() + "\n" + info.CreationTime.ToLongTimeString(),
                centerTextStyle);
            exportExcelInfo.CurrentRow++;
        }

        private void GenerateReportDetail(ExportExcelInfo exportExcelInfo)
        {
            var textStyle =
                NpoiExtensions.GetTextStyle(exportExcelInfo.ExcelFile, NpoiExtensions.BackgroundColorIndex, false, CRMHorizontalAlignment.Left);
            var centerTextStyle =
                NpoiExtensions.GetTextStyle(exportExcelInfo.ExcelFile, NpoiExtensions.BackgroundColorIndex, false, CRMHorizontalAlignment.Center);
            var childReportLength = _logStatisticsReportDetail.ChildReports.Count;
            var maxRowPerSheet = NpoiExtensions.MaxRowPerSheet;
            var numberOfSheet = (childReportLength / maxRowPerSheet) +
                (childReportLength % maxRowPerSheet != 0 ? 1 : 0);

            var generalSheetName = GetSheetname();
            for (var sheetIndex = 0; sheetIndex < numberOfSheet; sheetIndex++)
            {
                var sheetName = numberOfSheet > 1 ?
                    string.Format("{0} - {1}", generalSheetName, sheetIndex + 1) :
                    generalSheetName;
                exportExcelInfo.CreateSheet(sheetName);

                exportExcelInfo.CurrentRow = 0;
                GenerateTitle(exportExcelInfo);
                var startIndex = sheetIndex * maxRowPerSheet;
                var endIndex = startIndex + maxRowPerSheet;
                for (var i = startIndex; i < endIndex && i < childReportLength; i++)
                {
                    GenerateDetailReportRowData(_logStatisticsReportDetail.ChildReports[i], exportExcelInfo, textStyle, centerTextStyle);
                }
            }
        }

        private void GenerateTitle(ExportExcelInfo exportExcelInfo)
        {
            NpoiExtensions.SetHeaderColorIndex(exportExcelInfo);
            var headerStyle = NpoiExtensions.GetHeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());
            var leftTitleStyle = NpoiExtensions.GetHeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());
            leftTitleStyle.Alignment = HorizontalAlignment.Left;

            exportExcelInfo.ResetCurrentColumn();
            var columnWidths = new List<int> { 100, 100, 100, 100, 200, 140, 140, 100, 100, 250, 120, 120, 120, 300, 300, 480, 480, 100, 100, 140, 120 };
            exportExcelInfo.InitColumnWidthByFontWidth(columnWidths);

            var title = _logStatisticsReportDetail != null ? _logStatisticsReportDetail.Name : string.Empty;
            GenerateHeaderTitle(exportExcelInfo, title, leftTitleStyle);

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
            exportExcelInfo.RenderCell(Labels.ProductSubProduct, headerStyle);
            exportExcelInfo.RenderCell(Labels.Country, headerStyle);
            exportExcelInfo.RenderCell(Labels.CategorySubCategory, headerStyle);
            exportExcelInfo.RenderCell(Labels.Complaints_Enquiries, headerStyle);
            exportExcelInfo.RenderCell(Labels.Solution, headerStyle);
            exportExcelInfo.RenderCell(Labels.Status, headerStyle);
            exportExcelInfo.RenderCell(Labels.Response, headerStyle);
            exportExcelInfo.RenderCell(Labels.Handle_By, headerStyle);
            exportExcelInfo.RenderCell(Labels.Creation_Time, headerStyle);
            exportExcelInfo.CurrentRow++;
        }

        public void GenerateHeaderTitle(ExportExcelInfo exportExcelInfo, string reportTitle, ICellStyle headerStyle)
        {
            CellRangeAddress cellRangeAddress = exportExcelInfo.GetCellRangeBySpanning(1);
            NpoiExtensions.RenderMergedCells(exportExcelInfo, cellRangeAddress, reportTitle, headerStyle);
            exportExcelInfo.CurrentRow += 2;

            NpoiExtensions.RenderEmptyRow(exportExcelInfo, exportExcelInfo.CurrentRow);
            exportExcelInfo.CurrentRow++;
        }

        private string GetSheetname()
        {
            var reportType = ExcelFileName.Split('_')[0];
            return reportType + "_Detail";
        }
    }
}
