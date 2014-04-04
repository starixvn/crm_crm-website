// --------------------------------------------------------------------------
// <copyright file="WagerReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.03.28">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.Implements.WagerReport
{
    using System.Collections.Generic;
    using Fanex.CRM.Report.Criteria.WagerReport;
    using Fanex.CRM.Report.Models.WagerReport;
    using Fanex.CRM.Report.Repositories.Implements;
    using Fanex.CRM.Report.Repositories.Interfaces;
    using Fanex.CRM.Report.Resources;
    using Fanex.CRM.Report.Services.Interfaces;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using System.IO;
    using System.Web;
    using Fanex.CRM.Infrastructure.Extension;
    using NPOI.SS.Util;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// WagerReportService service.
    /// </summary>
    public class WagerReportService : IWagerReportService
    {
        private const string WagerReportSessionKey = "Fanex.CRM.Report.WagerReport.WagerReportSessionKey";
        private readonly IWagerReportRepository _wagerRepository;

        public WagerReportService()
            : this(null)
        {
        }

        public WagerReportService(IWagerReportRepository wagerReportRepository)
        {
            _wagerRepository = wagerReportRepository ?? new WagerReportRepository(); 
        }

        /// <summary>
        /// Gets the wager report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>WagerReport model.</returns>
        public WagerReportModel GetWagerReport(GetWagerReportCriteria criteria)
        {
            WagerReportModel report = null;
#if DEBUG
            report = new WagerReportModel
            {
                ReportTitle = string.Format(Labels.WagerReportTitle, criteria.Year),
                WagerByMonths = new List<WagerByMonth>
                {
                    new WagerByMonth 
                    { 
                        Month = "JAN",
                        Wagers = new List<Wager> 
                        {
                            new Wager { Name = "ACCEPT", Amount = 16 },
                            new Wager { Name = "REJECT", Amount = 10 },
                            new Wager { Name = "VOID/REFUND", Amount = 25 }
                        },
                        Total = 51
                    },
                    new WagerByMonth 
                    { 
                        Month = "FEB",
                        Wagers = new List<Wager> 
                        {
                            new Wager { Name = "ACCEPT", Amount = 9 },
                            new Wager { Name = "REJECT", Amount = 6 },
                            new Wager { Name = "VOID/REFUND", Amount = 16 }
                        },
                        Total = 31
                    },
                    new WagerByMonth 
                    { 
                        Month = "MAR",
                        Wagers = new List<Wager> 
                        {
                            new Wager { Name = "ACCEPT", Amount = 11 },
                            new Wager { Name = "REJECT", Amount = 12 },
                            new Wager { Name = "VOID/REFUND", Amount = 34 }
                        },
                        Total = 57
                    },
                    new WagerByMonth 
                    { 
                        Month = "DEC",
                        Wagers = new List<Wager> 
                        {
                            new Wager { Name = "ACCEPT", Amount = 38 },
                            new Wager { Name = "REJECT", Amount = 8 },
                            new Wager { Name = "VOID/REFUND", Amount = 37 }
                        },
                        Total = 83
                    }
                },
                GrandTotal = new WagerByMonth 
                {
                    Month = "Grand Total",
                    Wagers = new List<Wager> 
                        {
                            new Wager { Name = "ACCEPT", Amount = 123 },
                            new Wager { Name = "REJECT", Amount = 456 },
                            new Wager { Name = "VOID/REFUND", Amount = 789 }
                        },
                    Total = 999999
                }
            };
#else
            string sessionKey = WagerReportSessionKey;
            try
            {
                report = (WagerReport)SessionExtension.GetSession(HttpContext.Current.Session, sessionKey);
                if (report == null)
                {
                    report = _wagerRepository.GetWagerReport(criteria);
                    report.ReportTitle = string.Format(Labels.WagerReportTitle, criteria.Year);

                    // Add to session.
                    if (report != null)
                    {
                        SessionExtension.AddSession(HttpContext.Current.Session, sessionKey, report);
                    }
                }
            }
            catch (Exception ex)
            {
                report = null;
                LogExtensions.ExceptionHandler(ex);
            }
#endif
            return report;
        }

        /// <summary>
        /// Exports the wager report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public void ExportWagerReportExcel(GetWagerReportCriteria criteria)
        {
            WagerReportModel report = GetWagerReport(criteria);
            string excelFileName = string.Format(Labels.WagerReportExcelName, criteria.Year);

            HSSFWorkbook workbook = new HSSFWorkbook();
            ExportExcelInfoNPOI exportExcelInfo = new ExportExcelInfoNPOI
            {
                ExcelFile = workbook,
                Worksheet = workbook.CreateSheet(excelFileName),
                StartRow = 0,
                StartColumn = 0,
                CurrentRow = 0,
                MaxColumnIndex = 3
            };

            ExportTitle(ref exportExcelInfo, report.ReportTitle);
            ExportHeader(ref exportExcelInfo);
            if (report.WagerByMonths == null || report.WagerByMonths.Count == 0)
            {
                NpoiExcelExtensions.RenderNoInfoMessage(ref exportExcelInfo, exportExcelInfo.CurrentRow);
            }
            else
            {
                ExportBody(ref exportExcelInfo, report.WagerByMonths);
            }
            
            NpoiExcelExtensions.ResponseStreamExcel(exportExcelInfo.ExcelFile, excelFileName);
        }

        #region Export excel helper

        private void ExportTitle(ref ExportExcelInfoNPOI exportExcelInfo, string title)
        {
            int currentColumn = exportExcelInfo.StartColumn;
            int currentRow = exportExcelInfo.CurrentRow;
            int columnWidth = 200 * NpoiExcelExtensions.FontWidth;

            NpoiExcelExtensions.SetHeaderColorIndex(ref exportExcelInfo);
            ISheet worksheet = exportExcelInfo.Worksheet;
            worksheet.SetColumnWidth(currentColumn++, columnWidth);
            worksheet.SetColumnWidth(currentColumn++, columnWidth);
            worksheet.SetColumnWidth(currentColumn++, columnWidth);
            worksheet.SetColumnWidth(currentColumn++, columnWidth);
            
            CellRangeAddress cellRangeAddress = new CellRangeAddress(
                currentRow,
                currentRow + 1,
                exportExcelInfo.StartColumn,
                exportExcelInfo.MaxColumnIndex);
            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParamNPOI());
            NpoiExcelExtensions.RenderMergedCells(ref exportExcelInfo, cellRangeAddress, title, headerStyle);
            currentRow += 2;

            NpoiExcelExtensions.RenderEmptyRow(ref exportExcelInfo, currentRow);
            currentRow++;

            exportExcelInfo.CurrentRow = currentRow;
        }

        private void ExportHeader(ref ExportExcelInfoNPOI exportExcelInfo)
        {
            int currentColumn = exportExcelInfo.StartColumn;
            IRow row = exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParamNPOI());

            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Month, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Wagers, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Amount, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Total, headerStyle);
            exportExcelInfo.CurrentRow++;
        }

        private void ExportBody(ref ExportExcelInfoNPOI exportExcelInfo, IList<WagerByMonth> wagerByMonths)
        {
            int currentColumn, currentRow, mergeRowNumber;
            int reportLength = wagerByMonths.Count;
            
            WagerByMonth wagerByMonth;
            for (int i = 0; i < reportLength; i++)
            {
                wagerByMonth = wagerByMonths[i];
                currentRow = exportExcelInfo.CurrentRow;
                currentColumn = exportExcelInfo.StartColumn;
                mergeRowNumber = wagerByMonth.Wagers.Count;

                CellRangeAddress monthCellRange = new CellRangeAddress(
                    currentRow,
                    currentRow + mergeRowNumber,
                    currentColumn,
                    currentColumn);
                ICellStyle cellStyle = NpoiExcelExtensions.TextStyle(
                    exportExcelInfo.ExcelFile,
                    NpoiExcelExtensions.BackgroundColorIndex,
                    true,
                    NpoiExcelExtensions.Alignment.Center);
                NpoiExcelExtensions.RenderMergedCells(ref exportExcelInfo, monthCellRange, wagerByMonth.Month, cellStyle);
                currentRow++;
                currentColumn++;

                for (int j = 0; j < mergeRowNumber; j++)
                {
                    IRow row = exportExcelInfo.Worksheet.CreateRow(currentRow);
                    NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, wagerByMonth.Wagers[j].Name, cellStyle);
                    NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, wagerByMonth.Wagers[j].Amount, cellStyle);
                    currentRow++;
                }

                CellRangeAddress totalCellRange = new CellRangeAddress(
                    exportExcelInfo.CurrentRow,
                    exportExcelInfo.CurrentRow + mergeRowNumber,
                    currentColumn,
                    currentColumn);
                NpoiExcelExtensions.RenderMergedCells(ref exportExcelInfo, totalCellRange, wagerByMonth.Total, cellStyle);
                
                exportExcelInfo.CurrentRow = currentRow;
            }
        }

        private void ExportGrandTotal()
        { }

        #endregion
    }
}
