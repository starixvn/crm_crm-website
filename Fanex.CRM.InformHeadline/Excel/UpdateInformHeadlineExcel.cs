namespace Fanex.CRM.InformHeadline.Excel
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Common.Excel;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.InformHeadline.Models;
    using Fanex.CRM.InformHeadline.Resources;
    using Fanex.CRM.InformHeadline.Services;
    using Fanex.CRM.Setting.Models;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    public class UpdateInformHeadlineExcel : BaseExcel
    {
        private const int KeptForReferenceStatusId = 45007;

        private static readonly int[] InformSpecialCases = new[] { 1, 5, 12 };

        private static readonly int[] ColumnWidths = new[] { 280, 120, 170, 500, 120, 120, 120 };

        private static readonly Color UpdatedBackColor = Color.FromArgb(246, 211, 207);
        private static readonly Color HeadlineBackColor = Color.FromArgb(208, 230, 244);
        private static readonly Color NormalBackColor = Color.FromArgb(241, 241, 241);

        private readonly string[] _columnHeaders = new[]
            {
                Labels.DirectAccount,
                Labels.Status,
                Labels.UpdatedTime,
                Labels.Remark,
                Labels.Handler,
                Labels.Extension,
                Labels.ProductStatus
            };

        private readonly IInformHeadlineService _informService;

        private IEnumerable<InformStatus> _informStatuses;
        private IEnumerable<Status> _productStatuses;
        private IEnumerable<Station> _stations;
        private string _informHeadlineTitle;
        private IEnumerable<UpdateInform> _data;

        private ICellStyle _updatedLeftAlignStyle;
        private ICellStyle _updatedCenterAlignStyle;

        private ICellStyle _headlineLeftAlignStyle;
        private ICellStyle _headlineCenterAlignStyle;

        private ICellStyle _normalLeftAlignStyle;
        private ICellStyle _normalCenterAlignStyle;

        public UpdateInformHeadlineExcel(string informHeadlineTitle, IEnumerable<UpdateInform> data, string excelFileName)
            : base(excelFileName)
        {
            _informHeadlineTitle = informHeadlineTitle;
            _data = data;
            _informService = new InformHeadlineService();
            _informStatuses = _informService.GetInformStatuses();
            _productStatuses = _informService.GetProductStatuses();
            _stations = _informService.GetStations();
        }

        public override void BuildWorkBook()
        {
            ExportExcelInfo exportExcelInfo = new ExportExcelInfo
            {
                ExcelFile = WorkBook,
                Worksheet = WorkBook.CreateSheet(ExcelFileName),
                StartRow = 0,
                StartColumn = 0,
                CurrentRow = 0,
                MaxColumnIndex = _columnHeaders.Length - 1
            };

            GenerateTitle(exportExcelInfo, Labels.InformHeadlinesCases);
            GenerateHeader(exportExcelInfo);
            if (_data == null || !_data.Any())
            {
                NpoiExtensions.RenderNoInfoMessage(exportExcelInfo, exportExcelInfo.CurrentRow);
            }
            else
            {
                GenerateBody(exportExcelInfo, _data);
            }
        }

        private static string NormalizeLineBreaks(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            return Regex.Replace(text, @"\r\n?|\n", "\r\n");
        }

        private void InitializeBodyRowStyles(ExportExcelInfo exportExcelInfo)
        {
            NpoiExtensions.SetColorAtIndex(exportExcelInfo, UpdatedBackColor, HSSFColor.Coral.Index);
            NpoiExtensions.SetColorAtIndex(exportExcelInfo, HeadlineBackColor, HSSFColor.LightYellow.Index);
            NpoiExtensions.SetColorAtIndex(exportExcelInfo, NormalBackColor, HSSFColor.Grey25Percent.Index);

            _updatedLeftAlignStyle = GetCellStyle(exportExcelInfo.ExcelFile, HSSFColor.Coral.Index);
            _updatedCenterAlignStyle = GetCellStyle(exportExcelInfo.ExcelFile, HSSFColor.Coral.Index, ExcelHorizontalAlignment.Center);

            _headlineLeftAlignStyle = GetCellStyle(exportExcelInfo.ExcelFile, HSSFColor.LightYellow.Index);
            _headlineCenterAlignStyle = GetCellStyle(exportExcelInfo.ExcelFile, HSSFColor.LightYellow.Index, ExcelHorizontalAlignment.Center);

            _normalLeftAlignStyle = GetCellStyle(exportExcelInfo.ExcelFile, HSSFColor.Grey25Percent.Index);
            _normalCenterAlignStyle = GetCellStyle(exportExcelInfo.ExcelFile, HSSFColor.Grey25Percent.Index, ExcelHorizontalAlignment.Center);
        }

        private void GenerateTitle(ExportExcelInfo exportExcelInfo, string title)
        {
            NpoiExtensions.SetHeaderColorIndex(exportExcelInfo);
            exportExcelInfo.ResetCurrentColumn();
            for (int i = 0; i < ColumnWidths.Length; i++)
            {
                exportExcelInfo.SetColumnWidthByFontWidth(exportExcelInfo.CurrentColumn++, ColumnWidths[i]);
            }

            ICellStyle headerStyle = NpoiExtensions.GetHeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam { Alignment = ExcelHorizontalAlignment.Center });
            exportExcelInfo.RenderHeaderTitle(title, headerStyle);
            GenerateInformHeadlineTitle(exportExcelInfo, _informHeadlineTitle);
        }

        private void GenerateInformHeadlineTitle(ExportExcelInfo exportExcelInfo, string title)
        {
            ICellStyle headerStyle = NpoiExtensions.GetHeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam { Alignment = ExcelHorizontalAlignment.Center });
            var cellRangeAddress = new CellRangeAddress(
                exportExcelInfo.CurrentRow,
                exportExcelInfo.CurrentRow + 1,
                exportExcelInfo.StartColumn,
                exportExcelInfo.MaxColumnIndex);
            NpoiExtensions.RenderMergedCells(exportExcelInfo, cellRangeAddress, HttpUtility.HtmlDecode(title), headerStyle);
            exportExcelInfo.CurrentRow += 2;
        }

        private void GenerateHeader(ExportExcelInfo exportExcelInfo)
        {
            int currentRow = exportExcelInfo.CurrentRow;
            int currentColumn = exportExcelInfo.StartColumn;

            var headerStyle = NpoiExtensions.GetHeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());
            var row = exportExcelInfo.Worksheet.CreateRow(currentRow);
            row.HeightInPoints = 30;
            foreach (var columnHeader in _columnHeaders)
            {
                NpoiExtensions.RenderCell(row, currentColumn, columnHeader, headerStyle);
                currentColumn++;
            }

            currentRow += 1;
            exportExcelInfo.CurrentRow = currentRow;
        }

        private void GenerateBody(ExportExcelInfo exportExcelInfo, IEnumerable<UpdateInform> informs)
        {
            exportExcelInfo.ResetCurrentColumn();
            int currentRow = exportExcelInfo.CurrentRow;
            int currentColumn = exportExcelInfo.CurrentColumn;
            ISheet worksheet = exportExcelInfo.Worksheet;
            InitializeBodyRowStyles(exportExcelInfo);
            foreach (var inform in informs)
            {
                var row = worksheet.CreateRow(currentRow);
                RenderBodyRow(inform, row, currentColumn);

                currentRow++;
                currentColumn = exportExcelInfo.StartColumn;
            }

            exportExcelInfo.CurrentRow = currentRow;
        }

        private void RenderBodyRow(UpdateInform inform, IRow row, int currentColumn)
        {
            ICellStyle leftAlignStyle;
            ICellStyle centerAlignStyle;

            if (inform.Case > 0)
            {
                leftAlignStyle = _updatedLeftAlignStyle;
                centerAlignStyle = _updatedCenterAlignStyle;
            }
            else if (inform.GroupId > 0)
            {
                leftAlignStyle = _headlineLeftAlignStyle;
                centerAlignStyle = _headlineCenterAlignStyle;
            }
            else
            {
                leftAlignStyle = _normalLeftAlignStyle;
                centerAlignStyle = _normalCenterAlignStyle;
            }

            var username = !string.IsNullOrEmpty(inform.UserName) ? inform.UserName :
                    (InformSpecialCases.Contains(inform.Case) ? inform.OldUserName : inform.NewUserName);

            var isKeepForReference = inform.InformHeadlineStatusId == KeptForReferenceStatusId;

            var informStatusName = string.Empty;
            if (isKeepForReference)
            {
                informStatusName = Labels.KeptForReference;
            }
            else if (InformSpecialCases.Contains(inform.Case))
            {
                informStatusName = Labels.KeepThisAccount;
            }
            else
            {
                informStatusName = GetInformStatusName(inform.InformHeadlineStatusId);
            }

            var stationName = isKeepForReference ? Labels.KeptForReference : GetStationName(inform.Extension);
            var productStatusName = isKeepForReference ? Labels.KeptForReference : GetProductStatusName(inform.ProductStatus);
            NpoiExtensions.RenderCell(row, currentColumn++, username, leftAlignStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, informStatusName, leftAlignStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, string.Format("{0:MMM dd, yyyy}", inform.ClientUpdatedTime), leftAlignStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, NormalizeLineBreaks(HttpUtility.HtmlDecode(inform.Remark)), leftAlignStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, inform.Handler, leftAlignStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, stationName, centerAlignStyle);
            NpoiExtensions.RenderCell(row, currentColumn++, productStatusName, centerAlignStyle);
        }

        private static ICellStyle GetCellStyle(HSSFWorkbook workbook, short color = -1, ExcelHorizontalAlignment align = ExcelHorizontalAlignment.Left)
        {
            var cellStyle = NpoiExtensions.GetTextStyle(
                workbook,
                color < 0 ? NpoiExtensions.BackgroundColorIndex : color,
                false,
                align);
            cellStyle.WrapText = true;
            return cellStyle;
        }

        private string GetInformStatusName(int id)
        {
            var item = _informStatuses.FirstOrDefault(x => x.SettingId == id);
            return item == null ? string.Empty : item.SettingValue;
        }

        private string GetProductStatusName(int id)
        {
            var item = _productStatuses.FirstOrDefault(x => x.StatusId == id);
            return item == null ? Labels.NA : item.Name;
        }

        private string GetStationName(int id)
        {
            var item = _stations.FirstOrDefault(x => x.StationId == id);
            return item == null ? Labels.NA : item.Name;
        }
    }
}