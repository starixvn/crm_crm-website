// --------------------------------------------------------------------------
// <copyright file="FirstGrandTotalRowExcelWriter.cs" company="Nexcel Solutions Vietnam">
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

    /// <summary>
    /// First Grand Total Row Excel Writer.
    /// </summary>
    public class FirstGrandTotalRowExcelWriter
    {
        private ICellStyle _intStyle;
        private ICellStyle _textStyle;
        private ICellStyle _centerTextStyle;
        private ExportExcelInfo _exportExcelInfo;
        private short _rowBackgroundColorIndex;

        public FirstGrandTotalRowExcelWriter(ExportExcelInfo exportExcelInfo)
        {
            _exportExcelInfo = exportExcelInfo;
            _rowBackgroundColorIndex = NpoiExcelExtensions.GrandTotalRowColorIndex;

            _intStyle = NpoiExcelExtensions.IntNumberStyle(
               _exportExcelInfo.ExcelFile,
               _rowBackgroundColorIndex,
               true,
               HorizontalAlignment.Right,
               NpoiExcelExtensions.GrandTotalTextColorIndex);
            _textStyle = NpoiExcelExtensions.TextStyle(
                 _exportExcelInfo.ExcelFile,
                 _rowBackgroundColorIndex,
                 true,
                 CRMHorizontalAlignment.Left,
                 NpoiExcelExtensions.GrandTotalTextColorIndex);
            _centerTextStyle = NpoiExcelExtensions.TextStyle(
                _exportExcelInfo.ExcelFile,
                _rowBackgroundColorIndex,
                true,
                CRMHorizontalAlignment.Center,
                NpoiExcelExtensions.GrandTotalTextColorIndex);
        }

        /// <summary>
        /// Exports the dynamic report row.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        /// <param name="info">The information.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public void WriteRow(
            ref ExportExcelInfo exportExcelInfo,
            StatisticReportInfo info)
        {
            var row = exportExcelInfo.Worksheet.GetRow(exportExcelInfo.CurrentRow) ?? exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
            var currentColumn = exportExcelInfo.CurrentColumn;
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, info.ShortName, _textStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, string.Empty, _centerTextStyle);

            var countryLength = info.ChildReports.Count;
            for (var i = 0; i < countryLength; i++)
            {
                var countryReport = info.ChildReports[i];
                NpoiExcelExtensions.RenderCell(
                    ref row,
                    ref currentColumn,
                    countryReport.NumberOfIssues,
                    _intStyle);
            }

            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, info.NumberOfIssues, _intStyle);
            exportExcelInfo.CurrentRow += 1;
        }
    }
}
