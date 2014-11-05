// --------------------------------------------------------------------------
// <copyright file="FirstGrandTotalRowExcelWriter.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.StatisticReport
{
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// First Grand Total Row Excel Writer.
    /// </summary>
    public class FirstGrandTotalRowExcelWriter : TotalRowExcelWriter
    {
        private ExportExcelInfo _exportExcelInfo;
        private short _rowBackgroundColorIndex;

        public FirstGrandTotalRowExcelWriter()
        {
        }

        public FirstGrandTotalRowExcelWriter(ExportExcelInfo exportExcelInfo)
        {
            _exportExcelInfo = exportExcelInfo;
            _rowBackgroundColorIndex = NpoiExcelExtensions.GrandTotalRowColorIndex;

            IntStyle = NpoiExcelExtensions.IntNumberStyle(
               _exportExcelInfo.ExcelFile,
               _rowBackgroundColorIndex,
               true,
               CRMHorizontalAlignment.Right,
               NpoiExcelExtensions.GrandTotalTextColorIndex);
            TextStyle = NpoiExcelExtensions.TextStyle(
                 _exportExcelInfo.ExcelFile,
                 _rowBackgroundColorIndex,
                 true,
                 CRMHorizontalAlignment.Left,
                 NpoiExcelExtensions.GrandTotalTextColorIndex);
            CenterTextStyle = NpoiExcelExtensions.TextStyle(
                _exportExcelInfo.ExcelFile,
                _rowBackgroundColorIndex,
                true,
                CRMHorizontalAlignment.Center,
                NpoiExcelExtensions.GrandTotalTextColorIndex);
        }
    }
}
