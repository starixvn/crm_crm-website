namespace Fanex.CRM.Report.Excels.PICAssignment
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CRM.Common.Enumerations;
    using Fanex.CRM.Common.Excel;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Report.Models.PicAssignmentReport;
    using Fanex.CRM.Report.Resources;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using static NPOI.HSSF.Util.HSSFColor;

    public class PICAssignmentExportExcel : BaseExcel
    {
        private readonly IEnumerable<GroupHeadlineOfPic> data;
        private const int groupHeadlineColNumber = 1;
        private const int headlineTypeColNumber = 2;
        private const int picColNumber = 3;
        private const int directAccountColNumber = 4;
        private const int statusColNumber = 5;
        private const int siteColNumber = 6;
        private const int currencyColNumber = 7;
        private const int positionColNumber = 8;

        private const int startRow = 4;

        public PICAssignmentExportExcel(IEnumerable<GroupHeadlineOfPic> data, string excelFileName)
            : base(excelFileName)
        {
            this.data = data;
        }

        public override void BuildWorkBook()
        {
            var exportExcelInfo = new ExportExcelInfo
            {
                ExcelFile = new HSSFWorkbook(),
                MaxColumnIndex = 8
            };

            RenderExcelReport(exportExcelInfo);
            NpoiExtensions.ResponseStreamExcel(exportExcelInfo.ExcelFile, NpoiExtensions.StandardizeExcelReportName(ExcelFileName));
        }

        private void RenderExcelReport(ExportExcelInfo exportExcelInfo)
        {
            exportExcelInfo.Worksheet = exportExcelInfo.ExcelFile.CreateSheet(ExcelFileName);
            exportExcelInfo.Worksheet.VerticallyCenter = true;
            RenderHeader(exportExcelInfo);
            RenderBody(exportExcelInfo);
        }

        private void RenderHeader(ExportExcelInfo exportExcelInfo)
        {
            const int largeWidth = 180;
            const int mediumWidth = 120;
            const int smallWidth = 50;

            NpoiExtensions.SetHeaderColorIndex(exportExcelInfo);
            var headerStyle = NpoiExtensions.GetHeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());
            headerStyle.WrapText = true;
            var currentColumn = exportExcelInfo.StartColumn;
            var columnWidths = new List<int>
            {
                smallWidth, mediumWidth, mediumWidth, largeWidth, largeWidth, mediumWidth, mediumWidth, mediumWidth, mediumWidth
            };

            foreach (var item in columnWidths)
            {
                exportExcelInfo.Worksheet.SetColumnWidth(currentColumn++, item * NpoiExtensions.FontWidth);
            }

            string reportTitle = ExcelFileName;
            exportExcelInfo.RenderHeaderTitle(reportTitle, headerStyle);
            var firstHeaderRowIndex = exportExcelInfo.CurrentRow;
            var columnHeaders = new List<string>
            {
                Labels.NumberColumn,
                Labels.Groupheadline,
                Labels.HeadlineType,
                Labels.PIC,
                Labels.DirectAccount,
                Labels.Status,
                Labels.Site,
                Labels.Currency,
                Labels.Position
            };
            IRow row = exportExcelInfo.Worksheet.CreateRow(firstHeaderRowIndex);

            for (int i = 0; i <= exportExcelInfo.MaxColumnIndex; i++)
            {
                NpoiExtensions.RenderCell(row, i, columnHeaders[i], headerStyle);
            }
            exportExcelInfo.CurrentRow = firstHeaderRowIndex + 1;
        }

        private void RenderBody(ExportExcelInfo exportExcelInfo)
        {
            if (!data.Any())
            {
                NpoiExtensions.RenderNoInfoMessage(exportExcelInfo, exportExcelInfo.CurrentRow);
                return;
            }

            var centerTextStyle = NpoiExtensions.GetTextStyle(
                exportExcelInfo.ExcelFile,
                NpoiExtensions.BackgroundColorIndex,
                false,
                ExcelHorizontalAlignment.Center);
            centerTextStyle.WrapText = true;

            foreach (var headline in data)
            {
                var groupHeadlineTotalRow = headline.Pics.Sum(p => p.DirectAccounts.Count());
                int currentRow = exportExcelInfo.CurrentRow;
                var rows = new List<IRow>();
                for (int i = 0; i < groupHeadlineTotalRow; i++)
                {
                    IRow row = exportExcelInfo.Worksheet.CreateRow(currentRow + i);
                    NpoiExtensions.RenderCell(row, 0, currentRow + i + 1 - startRow, centerTextStyle);
                    rows.Add(row);
                }

                RenderHeadlineColumn(exportExcelInfo, currentRow, groupHeadlineTotalRow, headline);

                RenderPicColumn(exportExcelInfo, currentRow, headline.Pics);

                RenderUnmergeColumns(exportExcelInfo, rows, headline.Pics);

                exportExcelInfo.CurrentRow = currentRow + groupHeadlineTotalRow;
            }
        }

        private static void RenderHeadlineColumn(ExportExcelInfo exportExcelInfo, int currentRowNumber, int groupHeadlineTotalRow, GroupHeadlineOfPic headline)
        {
            var headlineCellStyle = NpoiExtensions.GetTextStyle(
               exportExcelInfo.ExcelFile,
               NpoiExtensions.BackgroundColorIndex,
               false,
               ExcelHorizontalAlignment.Left);
            headlineCellStyle.WrapText = true;
            headlineCellStyle.VerticalAlignment = VerticalAlignment.Top;

            var groupHeadlineMergeCell = new CellRangeAddress(currentRowNumber,
                   currentRowNumber + groupHeadlineTotalRow - 1,
                   groupHeadlineColNumber,
                   groupHeadlineColNumber);
            NpoiExtensions.RenderMergedCells(exportExcelInfo, groupHeadlineMergeCell, headline.GroupHeadline, headlineCellStyle);

            var headlineTypeMergeCell = new CellRangeAddress(currentRowNumber,
                currentRowNumber + groupHeadlineTotalRow - 1,
                headlineTypeColNumber,
                headlineTypeColNumber);
            NpoiExtensions.RenderMergedCells(exportExcelInfo, headlineTypeMergeCell, headline.HeadlineType, headlineCellStyle);
        }

        private static void RenderPicColumn(ExportExcelInfo exportExcelInfo, int currentRow, IEnumerable<PicOfDirectAccount> pics)
        {
            var picCellStyle = NpoiExtensions.GetTextStyle(
               exportExcelInfo.ExcelFile,
               NpoiExtensions.BackgroundColorIndex,
               false,
               ExcelHorizontalAlignment.Left);
            picCellStyle.WrapText = true;
            picCellStyle.VerticalAlignment = VerticalAlignment.Top;

            var lastPicTotalColumn = 0;
            foreach (var pic in pics)
            {
                var picMergeCell = new CellRangeAddress(currentRow + lastPicTotalColumn,
                                    currentRow + lastPicTotalColumn + pic.DirectAccounts.Count() - 1,
                                    picColNumber,
                                    picColNumber);
                NpoiExtensions.RenderMergedCells(exportExcelInfo, picMergeCell,
                    pic.Pic != null ? string.Join(", ", pic.Pic.Select(p => $"{p.PicName} ({p.Role})")) : string.Empty,
                    picCellStyle);
                lastPicTotalColumn += pic.DirectAccounts.Count();
            }
        }

        private static void RenderUnmergeColumns(ExportExcelInfo exportExcelInfo, IList<IRow> rows, IEnumerable<PicOfDirectAccount> pics)
        {
            var style = NpoiExtensions.GetTextStyle(
               exportExcelInfo.ExcelFile,
               NpoiExtensions.BackgroundColorIndex,
               false,
               ExcelHorizontalAlignment.Left);
            style.WrapText = true;
            var directAccounts = new List<DirectAccountInfo>();

            foreach (var pic in pics)
            {
                directAccounts.AddRange(pic.DirectAccounts);
            }

            for (int index = 0; index < directAccounts.Count; index++)
            {
                var directAccount = directAccounts[index];
                var row = rows[index];
                style.FillBackgroundColor = GetCellColor(directAccount.Status);

                NpoiExtensions.RenderCell(row, directAccountColNumber, directAccount.DirectAccount, style);
                NpoiExtensions.RenderCell(row, statusColNumber, directAccount.Status, style);
                NpoiExtensions.RenderCell(row, siteColNumber, directAccount.Site, style);
                NpoiExtensions.RenderCell(row, currencyColNumber, directAccount.Currency, style);
                NpoiExtensions.RenderCell(row, positionColNumber, directAccount.Position, style);
            }
        }

        private static short GetCellColor(string accountStatus)
        {
            short color = White.Index;
            if (accountStatus.ToLowerInvariant().Equals("closed"))
            {
                color = Grey25Percent.Index;
            }

            if (accountStatus.ToLowerInvariant().Equals("disabled"))
            {
                color = Grey25Percent.Index;
            }

            if (accountStatus.ToLowerInvariant().Equals("suppended"))
            {
                color = Pink.Index;
            }

            return color;
        }
    }
}