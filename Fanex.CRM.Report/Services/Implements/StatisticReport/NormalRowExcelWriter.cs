// --------------------------------------------------------------------------
// <copyright file="NormalRowExcelWriter.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.Implements.DynamicReport
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Infrastructure.Models.Npoi;
    using Fanex.CRM.Report.Models.StatisticReport;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// Normal Row Excel Writer.
    /// </summary>
    public class NormalRowExcelWriter
    {
        private ICellStyle _intStyle;
        private ICellStyle _textStyle;
        private ICellStyle _centerTextStyle;
        private ICellStyle _boldIntStyle;
        private ExportExcelInfo _exportExcelInfo;
        private short _rowBackgroundColorIndex;

        public NormalRowExcelWriter(ExportExcelInfo exportExcelInfo)
        {
            _exportExcelInfo = exportExcelInfo;
            _rowBackgroundColorIndex = NpoiExcelExtensions.BackgroundColorIndex;

            _intStyle = NpoiExcelExtensions.IntNumberStyle(
               _exportExcelInfo.ExcelFile,
               _rowBackgroundColorIndex,
               false,
               CRMHorizontalAlignment.Right);
            _boldIntStyle = NpoiExcelExtensions.IntNumberStyle(
               _exportExcelInfo.ExcelFile,
               _rowBackgroundColorIndex,
               true,
               CRMHorizontalAlignment.Right);
            _textStyle = NpoiExcelExtensions.TextStyle(_exportExcelInfo.ExcelFile, _rowBackgroundColorIndex, false, CRMHorizontalAlignment.Left);
            _centerTextStyle = NpoiExcelExtensions.TextStyle(_exportExcelInfo.ExcelFile, _rowBackgroundColorIndex, false, CRMHorizontalAlignment.Center);
        }

        /// <summary>
        /// Exports the dynamic report row.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        /// <param name="info">The information.</param>
        /// <param name="groupInfo">The group information.</param>
        /// <param name="infoIndex">Index of the information.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public void WriteRow(
            ref ExportExcelInfo exportExcelInfo,
            StatisticReportInfo info,
            StatisticReportInfo groupInfo,
            int infoIndex)
        {
            CellRangeAddress cellRangeAddress;
            var row = exportExcelInfo.Worksheet.GetRow(exportExcelInfo.CurrentRow) ?? exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
            var currentColumn = exportExcelInfo.CurrentColumn;

            if (infoIndex == 0)
            {
                cellRangeAddress = new CellRangeAddress(
                    exportExcelInfo.CurrentRow,
                    exportExcelInfo.CurrentRow + groupInfo.ChildReports.Count - 1,
                    currentColumn,
                    currentColumn);
                NpoiExcelExtensions.RenderMergedCells(
                    exportExcelInfo,
                    cellRangeAddress,
                    groupInfo.ShortName,
                    _centerTextStyle);
            }

            currentColumn++;
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, info.ShortName, _centerTextStyle);
            var countryLength = info.ChildReports.Count;
            for (var i = 0; i < countryLength; i++)
            {
                var countryReport = info.ChildReports[i];
                if (countryReport.HasData)
                {
                    NpoiExcelExtensions.RenderCell(
                        ref row,
                        ref currentColumn,
                        countryReport.NumberOfIssues,
                        _intStyle);
                }
                else
                {
                    NpoiExcelExtensions.RenderCell(
                       ref row,
                       ref currentColumn,
                       string.Empty,
                       _textStyle);
                }
            }

            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, info.NumberOfIssues, _boldIntStyle);
            exportExcelInfo.CurrentRow += 1;
        }
    }
}
