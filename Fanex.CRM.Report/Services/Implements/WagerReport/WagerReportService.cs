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
                ReportTitle = Labels.WagerReportTitle,
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
                    report.ReportTitle = Labels.WagerReportTitle;

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
        public void ExportWagerReport(GetWagerReportCriteria criteria)
        {
            WagerReportModel report = GetWagerReport(criteria);
            
            HSSFWorkbook excelFile = new HSSFWorkbook();
            ISheet worksheet = excelFile.CreateSheet(string.Format(Labels.WagerReportExcelName, criteria.Year));
        }

        #region Export excel helper

        private void ExportHeader(HSSFWorkbook excelFile, ISheet worksheet, string reportTitle)
        {
            /*int startColumn = currentColumn = 0;
            int currentRow = 0;
            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(excelFile);

            int fontWidth = NpoiExcelExtensions.FontWidth;
            worksheet.SetColumnWidth(currentColumn++, 200 * fontWidth);
            worksheet.SetColumnWidth(currentColumn++, 200 * fontWidth);
            worksheet.SetColumnWidth(currentColumn++, 200 * fontWidth);
            worksheet.SetColumnWidth(currentColumn++, 200 * fontWidth);

            CellRangeAddress cellRangeAddress = new CellRangeAddress(
                currentRow,
                currentRow + 1,
                startColumn,
                3);
            NpoiExcelExtensions.RenderMergedCells(ref exportExcelInfo, cellRangeAddress, title, headerStyle);
            exportExcelInfo.CurrentRow += 2;

            NpoiExcelExtensions.RenderEmptyRow(ref exportExcelInfo, exportExcelInfo.CurrentRow);
            exportExcelInfo.CurrentRow++;

            IRow row = worksheet.CreateRow(exportExcelInfo.CurrentRow);
            currentColumn = exportExcelInfo.StartColumn;
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Title, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.First_Name, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Last_Name, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.VIP, headerStyle);

            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Contact_Number, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Mobile, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Fax, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Skype, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Email, headerStyle);

            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Birthday, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Site, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Language, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Line_1, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Line_2, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Line_3, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.City, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Country, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.State, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Post_Code, headerStyle);
            exportExcelInfo.CurrentRow++;*/
        }

        private void ExportBody()
        { }

        private void ExportGrandTotal()
        { }

        #endregion
    }
}
