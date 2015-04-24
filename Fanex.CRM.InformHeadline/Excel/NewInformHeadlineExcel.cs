// -----------------------------------------------------------------------
// <copyright file="NewInformHeadlineExcel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.InformHeadline.Excel
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using Fanex.CRM.Common.Excel;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.InformHeadline.Models;
    using Fanex.CRM.InformHeadline.Resources;
    using Fanex.CRM.Infrastructure.Models;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class NewInformHeadlineExcel : BaseExcel
    {
        private readonly string[] _columnHeaders = new string[]
            {
                Labels.DirectAccount, 
                Labels.Status,
                Labels.UpdatedTime,
                Labels.Remark,
                Labels.Handler,
                Labels.Extension,
                Labels.ProductStatus
            };

        private readonly Color _headlineStatusColor = Color.FromArgb(208, 230, 244);
        private readonly Color _normalStatusColor = Color.FromArgb(241, 241, 241);
        private string _informHeadlineTitle;
        private IList<NewInform> _data;

        public NewInformHeadlineExcel(string informHeadlineTitle, IList<NewInform> data, string excelFileName)
            : base(excelFileName)
        {
            _informHeadlineTitle = informHeadlineTitle;
            _data = data;
        }

        /// <summary>
        /// Build excel workbook.
        /// </summary>
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
            if (_data == null || _data.Count == 0)
            {
                NpoiExcelExtensions.RenderNoInfoMessage(exportExcelInfo, exportExcelInfo.CurrentRow);
            }
            else
            {
                GenerateBody(exportExcelInfo, _data);
            }
        }

        /// <summary>
        /// Generates the casino weekly summary title.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="title">The title.</param>
        private void GenerateTitle(ExportExcelInfo exportExcelInfo, string title)
        {
            NpoiExcelExtensions.SetHeaderColorIndex(exportExcelInfo);
            exportExcelInfo.ResetCurrentColumn();
            exportExcelInfo.SetColumnWidthByFontWidth(exportExcelInfo.CurrentColumn++, 280);
            for (int i = 0; i < exportExcelInfo.MaxColumnIndex; i++)
            {
                exportExcelInfo.SetColumnWidthByFontWidth(exportExcelInfo.CurrentColumn++, 120);
            }

            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam() { Alignment = CRMHorizontalAlignment.Center });
            exportExcelInfo.RenderHeaderTitle(title, headerStyle);
            GenerateInformHeadlineTitle(exportExcelInfo, _informHeadlineTitle);
        }

        private void GenerateInformHeadlineTitle(ExportExcelInfo exportExcelInfo, string title)
        {
            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam() { Alignment = CRMHorizontalAlignment.Center });
            var cellRangeAddress = new CellRangeAddress(
                exportExcelInfo.CurrentRow, 
                exportExcelInfo.CurrentRow + 1, 
                exportExcelInfo.StartColumn, 
                exportExcelInfo.MaxColumnIndex);
            NpoiExcelExtensions.RenderMergedCells(exportExcelInfo, cellRangeAddress, title, headerStyle);
            exportExcelInfo.CurrentRow += 2;
        }

        /// <summary>
        /// Generates the casino weekly summary header.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        private void GenerateHeader(ExportExcelInfo exportExcelInfo)
        {
            int currentRow = exportExcelInfo.CurrentRow;
            int currentColumn = exportExcelInfo.StartColumn;

            var headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());
            headerStyle.WrapText = true;
            var row = exportExcelInfo.Worksheet.CreateRow(currentRow);
            row.HeightInPoints = 30;
            foreach (var columnHeader in _columnHeaders)
            {
                NpoiExcelExtensions.RenderCell(row, currentColumn, columnHeader, headerStyle);
                currentColumn++;
            }

            currentRow += 1;
            exportExcelInfo.CurrentRow = currentRow;
        }

        /// <summary>
        /// Generates the casino weekly summary body.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="informs">The weekly categories.</param>
        private void GenerateBody(ExportExcelInfo exportExcelInfo, IList<NewInform> informs)
        {
            exportExcelInfo.ResetCurrentColumn();
            int currentRow = exportExcelInfo.CurrentRow;
            int currentColumn = exportExcelInfo.CurrentColumn;
            ISheet worksheet = exportExcelInfo.Worksheet;

            NpoiExcelExtensions.SetColorAtIndex(exportExcelInfo, _headlineStatusColor, HSSFColor.LightYellow.Index);
            NpoiExcelExtensions.SetColorAtIndex(exportExcelInfo, _normalStatusColor, HSSFColor.Grey25Percent.Index);

            ICellStyle headlineLeftAlignStyle = GetCellStyle(exportExcelInfo.ExcelFile, HSSFColor.LightYellow.Index);
            ICellStyle headlineCenterAlignStyle = GetCellStyle(exportExcelInfo.ExcelFile, HSSFColor.LightYellow.Index, CRMHorizontalAlignment.Center);

            ICellStyle normalLeftAlignStyle = GetCellStyle(exportExcelInfo.ExcelFile, HSSFColor.Grey25Percent.Index);
            ICellStyle normalCenterAlignStyle = GetCellStyle(exportExcelInfo.ExcelFile, HSSFColor.Grey25Percent.Index, CRMHorizontalAlignment.Center);

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

                NpoiExcelExtensions.RenderCell(row, currentColumn++, inform.UserName, leftAlignStyle);
                NpoiExcelExtensions.RenderCell(row, currentColumn++, inform.InformHeadlineStatusId, leftAlignStyle);
                NpoiExcelExtensions.RenderCell(row, currentColumn++, string.Empty, centerAlignStyle);
                NpoiExcelExtensions.RenderCell(row, currentColumn++, string.Empty, centerAlignStyle);
                NpoiExcelExtensions.RenderCell(row, currentColumn++, string.Empty, centerAlignStyle);
                NpoiExcelExtensions.RenderCell(row, currentColumn++, string.Empty, centerAlignStyle);
                NpoiExcelExtensions.RenderCell(row, currentColumn++, string.Empty, centerAlignStyle);

                currentRow++;
                currentColumn = exportExcelInfo.StartColumn;
            }

            exportExcelInfo.CurrentRow = currentRow;
        }

        private ICellStyle GetCellStyle(HSSFWorkbook workbook, short color = -1, CRMHorizontalAlignment align = CRMHorizontalAlignment.Left)
        {
            return NpoiExcelExtensions.TextStyle(
                workbook,
                color < 0 ? NpoiExcelExtensions.BackgroundColorIndex : color,
                false,
                align);
        }
    }
}
