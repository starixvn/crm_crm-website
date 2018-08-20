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
    using System.Collections.Generic;

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

            exportExcelInfo.ResetCurrentColumn();
            exportExcelInfo.CreateCurrentRow();
            exportExcelInfo.RenderCell(info.StartDateString, centerTextStyle);
            exportExcelInfo.RenderCell(info.StartTime, centerTextStyle);
            exportExcelInfo.RenderCell(info.ResolutionTimeString, centerTextStyle);
            exportExcelInfo.RenderCell(durationText, centerTextStyle);
            exportExcelInfo.RenderCell(info.Username, centerTextStyle);
            exportExcelInfo.RenderCell(info.SiteInfo.Name, centerTextStyle);
            exportExcelInfo.RenderCell(NameMapping.GetCommunicationName(info.CommunicationID, _communicationToolList), centerTextStyle);
            exportExcelInfo.RenderCell(NameMapping.GetExtensionName(info.StationID, _extensionList), centerTextStyle);
            exportExcelInfo.RenderCell(info.Role, textStyle);
            exportExcelInfo.RenderCell(HtmlHelper.RemoveHtmlTags(info.Contact), textStyle);
            exportExcelInfo.RenderCell(NameMapping.GetProductName(info.ProductID, _productList), textStyle);
            exportExcelInfo.RenderCell(NameMapping.GetSubProductName(info.SubProductID, _subProductList), textStyle);
            exportExcelInfo.RenderCell(info.Country.Name, centerTextStyle);
            exportExcelInfo.RenderCell(NameMapping.GetMainCategoryName(info.CategoryID, _mainCategory), textStyle);
            exportExcelInfo.RenderCell(NameMapping.GetSubCategoryName(info.CategoryID, info.SubCategoryID, _mainCategory), textStyle);
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
            exportExcelInfo.RenderCell(Labels.Product, headerStyle);
            exportExcelInfo.RenderCell(Labels.SubProduct, headerStyle);
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
