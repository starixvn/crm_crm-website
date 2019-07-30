namespace Fanex.CRM.InformHeadline.Excel
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Web;
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Common.Excel;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.InformHeadline.Models;
    using Fanex.CRM.InformHeadline.Resources;
    using Fanex.CRM.InformHeadline.Services;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    public class NewInformHeadlineExcel : BaseExcel
    {
        private static readonly int[] ColumnWidths = new[] { 280, 120, 120, 300, 120, 120, 120 };

        private static readonly Color HeadlineStatusColor = Color.FromArgb(208, 230, 244);
        private static readonly Color NormalStatusColor = Color.FromArgb(241, 241, 241);

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
        private string _informHeadlineTitle;
        private IEnumerable<NewInform> _data;

        public NewInformHeadlineExcel(string informHeadlineTitle, IEnumerable<NewInform> data, string excelFileName)
            : base(excelFileName)
        {
            _informHeadlineTitle = informHeadlineTitle;
            _data = data;
            _informService = new InformHeadlineService();
            _informStatuses = _informService.GetInformStatuses();
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
            headerStyle.WrapText = true;
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

        private void GenerateBody(ExportExcelInfo exportExcelInfo, IEnumerable<NewInform> informs)
        {
            exportExcelInfo.ResetCurrentColumn();
            int currentRow = exportExcelInfo.CurrentRow;
            int currentColumn = exportExcelInfo.CurrentColumn;
            ISheet worksheet = exportExcelInfo.Worksheet;

            NpoiExtensions.SetColorAtIndex(exportExcelInfo, HeadlineStatusColor, HSSFColor.LightYellow.Index);
            NpoiExtensions.SetColorAtIndex(exportExcelInfo, NormalStatusColor, HSSFColor.Grey25Percent.Index);

            ICellStyle headlineLeftAlignStyle = GetCellStyle(exportExcelInfo.ExcelFile, HSSFColor.LightYellow.Index);
            ICellStyle headlineCenterAlignStyle = GetCellStyle(exportExcelInfo.ExcelFile, HSSFColor.LightYellow.Index, ExcelHorizontalAlignment.Center);

            ICellStyle normalLeftAlignStyle = GetCellStyle(exportExcelInfo.ExcelFile, HSSFColor.Grey25Percent.Index);
            ICellStyle normalCenterAlignStyle = GetCellStyle(exportExcelInfo.ExcelFile, HSSFColor.Grey25Percent.Index, ExcelHorizontalAlignment.Center);

            foreach (var inform in informs)
            {
                var row = worksheet.CreateRow(currentRow);
                ICellStyle leftAlignStyle;
                ICellStyle centerAlignStyle;

                if (inform.GroupId > 0)
                {
                    leftAlignStyle = headlineLeftAlignStyle;
                    centerAlignStyle = headlineCenterAlignStyle;
                }
                else
                {
                    leftAlignStyle = normalLeftAlignStyle;
                    centerAlignStyle = normalCenterAlignStyle;
                }

                NpoiExtensions.RenderCell(row, currentColumn++, inform.UserName, leftAlignStyle);
                NpoiExtensions.RenderCell(row, currentColumn++, GetInformStatusName(inform.InformHeadlineStatusId), centerAlignStyle);
                NpoiExtensions.RenderCell(row, currentColumn++, string.Empty, centerAlignStyle);
                NpoiExtensions.RenderCell(row, currentColumn++, string.Empty, centerAlignStyle);
                NpoiExtensions.RenderCell(row, currentColumn++, string.Empty, centerAlignStyle);
                NpoiExtensions.RenderCell(row, currentColumn++, string.Empty, centerAlignStyle);
                NpoiExtensions.RenderCell(row, currentColumn, string.Empty, centerAlignStyle);

                currentRow++;
                currentColumn = exportExcelInfo.StartColumn;
            }

            exportExcelInfo.CurrentRow = currentRow;
        }

        private static ICellStyle GetCellStyle(HSSFWorkbook workbook, short color = -1, ExcelHorizontalAlignment align = ExcelHorizontalAlignment.Left)
        {
            return NpoiExtensions.GetTextStyle(
                workbook,
                color < 0 ? NpoiExtensions.BackgroundColorIndex : color,
                false,
                align);
        }

        private string GetInformStatusName(int id)
        {
            var item = _informStatuses.FirstOrDefault(x => x.SettingId == id);
            return item == null ? string.Empty : item.SettingValue;
        }
    }
}