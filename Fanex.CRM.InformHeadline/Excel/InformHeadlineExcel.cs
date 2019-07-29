namespace Fanex.CRM.InformHeadline.Excel
{
    using System;
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
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;

    public class InformHeadlineExcel : BaseExcel
    {
        private readonly string[] _columnHeaders = new[]
            {
                Labels.Title,
                Labels.CreatedDate,
                Labels.NoDirectAccounts,
                Labels.Status,
                Labels.Done,
                Labels.RequestToCallBack,
                Labels.NoNeedToCall,
                Labels.NoReply,
                Labels.Uncontactable,
                Labels.InternalAccount,
                Labels.NeverTouch,
                Labels.KeptForReference
            };

        private readonly Color _openedStatusColor = Color.FromArgb(240, 237, 196);
        private readonly Color _closedStatusColor = Color.FromArgb(242, 242, 242);

        private IEnumerable<InformHeadline> _data;

        public InformHeadlineExcel(IEnumerable<InformHeadline> data, string excelFileName)
            : base(excelFileName)
        {
            _data = data;
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
            if (_data.Count() == 0)
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
            exportExcelInfo.SetColumnWidthByFontWidth(exportExcelInfo.CurrentColumn++, 280);
            for (int i = 0; i < exportExcelInfo.MaxColumnIndex; i++)
            {
                exportExcelInfo.SetColumnWidthByFontWidth(exportExcelInfo.CurrentColumn++, 120);
            }

            ICellStyle headerStyle = NpoiExtensions.GetHeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam() { Alignment = CRMHorizontalAlignment.Center });
            exportExcelInfo.RenderHeaderTitle(title, headerStyle);
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

        private void GenerateBody(ExportExcelInfo exportExcelInfo, IEnumerable<InformHeadline> informs)
        {
            exportExcelInfo.ResetCurrentColumn();
            int currentRow = exportExcelInfo.CurrentRow;
            int currentColumn = exportExcelInfo.CurrentColumn;
            ISheet worksheet = exportExcelInfo.Worksheet;

            NpoiExtensions.SetColorAtIndex(exportExcelInfo, _openedStatusColor, HSSFColor.LightYellow.Index);
            NpoiExtensions.SetColorAtIndex(exportExcelInfo, _closedStatusColor, HSSFColor.Grey25Percent.Index);

            ICellStyle openedLeftAlignStyle = GetCellStyle(exportExcelInfo.ExcelFile, HSSFColor.LightYellow.Index);
            ICellStyle openedCenterAlignStyle = GetCellStyle(exportExcelInfo.ExcelFile, HSSFColor.LightYellow.Index, CRMHorizontalAlignment.Center);
            ICellStyle closedLeftAlignStyle = GetCellStyle(exportExcelInfo.ExcelFile, HSSFColor.Grey25Percent.Index);
            ICellStyle closedCenterAlignStyle = GetCellStyle(exportExcelInfo.ExcelFile, HSSFColor.Grey25Percent.Index, CRMHorizontalAlignment.Center);

            foreach (var inform in informs)
            {
                var row = worksheet.CreateRow(currentRow);
                ICellStyle leftAlignStyle;
                ICellStyle centerAlignStyle;

                if (inform.Status.Equals("Open", StringComparison.OrdinalIgnoreCase))
                {
                    leftAlignStyle = openedLeftAlignStyle;
                    centerAlignStyle = openedCenterAlignStyle;
                }
                else
                {
                    leftAlignStyle = closedLeftAlignStyle;
                    centerAlignStyle = closedCenterAlignStyle;
                }

                NpoiExtensions.RenderCell(row, currentColumn++, HttpUtility.HtmlDecode(inform.Title), leftAlignStyle);
                NpoiExtensions.RenderCell(row, currentColumn++, string.Format("{0:MMM dd, yyyy}", inform.ClientCreatedDate), leftAlignStyle);
                NpoiExtensions.RenderCell(row, currentColumn++, inform.Total.ToString(IntegerFormat), centerAlignStyle);
                NpoiExtensions.RenderCell(row, currentColumn++, inform.Status, centerAlignStyle);
                NpoiExtensions.RenderCell(row, currentColumn++, inform.Done.ToString(IntegerFormat), centerAlignStyle);
                NpoiExtensions.RenderCell(row, currentColumn++, inform.RequestToCallBack.ToString(IntegerFormat), centerAlignStyle);
                NpoiExtensions.RenderCell(row, currentColumn++, inform.NoNeedToCall.ToString(IntegerFormat), centerAlignStyle);
                NpoiExtensions.RenderCell(row, currentColumn++, inform.NoReply.ToString(IntegerFormat), centerAlignStyle);
                NpoiExtensions.RenderCell(row, currentColumn++, inform.Uncontactable.ToString(IntegerFormat), centerAlignStyle);
                NpoiExtensions.RenderCell(row, currentColumn++, inform.InternalAccount.ToString(IntegerFormat), centerAlignStyle);
                NpoiExtensions.RenderCell(row, currentColumn++, inform.NeverTouch.ToString(IntegerFormat), centerAlignStyle);
                NpoiExtensions.RenderCell(row, currentColumn++, inform.KeptForReference.ToString(IntegerFormat), centerAlignStyle);

                currentRow++;
                currentColumn = exportExcelInfo.StartColumn;
            }

            exportExcelInfo.CurrentRow = currentRow;
        }

        private ICellStyle GetCellStyle(HSSFWorkbook workbook, short color = -1, CRMHorizontalAlignment align = CRMHorizontalAlignment.Left)
        {
            return NpoiExtensions.GetTextStyle(
                workbook,
                color < 0 ? NpoiExtensions.BackgroundColorIndex : color,
                false,
                align);
        }
    }
}