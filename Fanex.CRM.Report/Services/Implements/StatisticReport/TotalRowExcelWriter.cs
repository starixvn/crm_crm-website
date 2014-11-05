// --------------------------------------------------------------------------
// <copyright file="TotalRowExcelWriter.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.Implements.DynamicReport
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Models.StatisticReport;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Total Row Excel Writer.
    /// </summary>
    public class TotalRowExcelWriter
    {
        private ExportExcelInfo _exportExcelInfo;
        private short _rowBackgroundColorIndex;
        private bool _isBoldRow = true;

        public TotalRowExcelWriter()
        {
        }

        public TotalRowExcelWriter(ExportExcelInfo exportExcelInfo)
        {
            _exportExcelInfo = exportExcelInfo;
            _rowBackgroundColorIndex = NpoiExcelExtensions.TotalRowColorIndex;

            IntStyle = NpoiExcelExtensions.IntNumberStyle(
                _exportExcelInfo.ExcelFile,
                _rowBackgroundColorIndex,
                _isBoldRow,
                CRMHorizontalAlignment.Right);

            TextStyle = NpoiExcelExtensions.TextStyle(
                _exportExcelInfo.ExcelFile,
                _rowBackgroundColorIndex,
                _isBoldRow,
                CRMHorizontalAlignment.Left);
            CenterTextStyle = NpoiExcelExtensions.TextStyle(
                _exportExcelInfo.ExcelFile,
                _rowBackgroundColorIndex,
                _isBoldRow,
                CRMHorizontalAlignment.Center);
        }

        public ICellStyle IntStyle { get; set; }

        public ICellStyle TextStyle { get; set; }

        public ICellStyle CenterTextStyle { get; set; }

        /// <summary>
        /// Exports the dynamic report row.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        /// <param name="info">The information.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public void WriteRow(ref ExportExcelInfo exportExcelInfo, StatisticReportInfo info)
        {
            var row = exportExcelInfo.Worksheet.GetRow(exportExcelInfo.CurrentRow) ?? exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
            var currentColumn = exportExcelInfo.CurrentColumn;
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, info.ShortName, TextStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, string.Empty, CenterTextStyle);

            var countryLength = info.ChildReports.Count;
            for (var i = 0; i < countryLength; i++)
            {
                var countryReport = info.ChildReports[i];
                NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, countryReport.NumberOfIssues, IntStyle);
            }

            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, (int)info.NumberOfIssues, IntStyle);
            exportExcelInfo.CurrentRow += 1;
        }
    }
}
