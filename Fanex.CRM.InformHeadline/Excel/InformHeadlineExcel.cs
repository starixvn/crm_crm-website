// -----------------------------------------------------------------------
// <copyright file="InformHeadlineExcel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.InformHeadline.Excel
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Web;
    using Fanex.CRM.Common.Excel;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.InformHeadline.Models;
    using Fanex.CRM.InformHeadline.Resources;
    using Fanex.CRM.Infrastructure.Models;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class InformHeadlineExcel : BaseExcel
    {
        private readonly string[] _columnHeaders = new string[]
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
                Labels.NeverTouch
            };
        
        private readonly Color _openedStatusColor = Color.FromArgb(240, 237, 196);
        private readonly Color _closedStatusColor = Color.FromArgb(242, 242, 242);

        private IList<InformHeadline> _data;

        public InformHeadlineExcel(IList<InformHeadline> data, string excelFileName)
            : base(excelFileName)
        {
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
        private void GenerateBody(ExportExcelInfo exportExcelInfo, IList<InformHeadline> informs)
        {
            exportExcelInfo.ResetCurrentColumn();
            int currentRow = exportExcelInfo.CurrentRow;
            int currentColumn = exportExcelInfo.CurrentColumn;
            ISheet worksheet = exportExcelInfo.Worksheet;

            NpoiExcelExtensions.SetColorAtIndex(exportExcelInfo, _openedStatusColor, HSSFColor.LightYellow.Index);
            NpoiExcelExtensions.SetColorAtIndex(exportExcelInfo, _closedStatusColor, HSSFColor.Grey25Percent.Index);

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

                NpoiExcelExtensions.RenderCell(row, currentColumn++, HttpUtility.HtmlDecode(inform.Title), leftAlignStyle);
                NpoiExcelExtensions.RenderCell(row, currentColumn++, string.Format("{0:MMM dd, yyyy}", inform.ClientCreatedDate), leftAlignStyle);
                NpoiExcelExtensions.RenderCell(row, currentColumn++, inform.Total, centerAlignStyle);
                NpoiExcelExtensions.RenderCell(row, currentColumn++, inform.Status, centerAlignStyle);
                NpoiExcelExtensions.RenderCell(row, currentColumn++, inform.Done, centerAlignStyle);
                NpoiExcelExtensions.RenderCell(row, currentColumn++, inform.RequestToCallBack, centerAlignStyle);
                NpoiExcelExtensions.RenderCell(row, currentColumn++, inform.NoNeedToCall, centerAlignStyle);
                NpoiExcelExtensions.RenderCell(row, currentColumn++, inform.NoReply, centerAlignStyle);
                NpoiExcelExtensions.RenderCell(row, currentColumn++, inform.Uncontactable, centerAlignStyle);
                NpoiExcelExtensions.RenderCell(row, currentColumn++, inform.InternalAccount, centerAlignStyle);
                NpoiExcelExtensions.RenderCell(row, currentColumn++, inform.NeverTouch, centerAlignStyle);

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
