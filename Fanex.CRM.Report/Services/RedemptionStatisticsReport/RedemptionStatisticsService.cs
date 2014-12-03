// --------------------------------------------------------------------------
// <copyright file="RedemptionStatisticsService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.11.24">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.DynamicReport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Fanex.Common;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Helpers;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Criteria.RedemptionStatisticsReport;
    using Fanex.CRM.Report.Entities.RedemptionStatistics;
    using Fanex.CRM.Report.Resources;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Services;
    using Fanex.Excel;
    using Fanex.Excel.Enums;
    using Fanex.Excel.Models;
    using Fanex.NAP.Core.Service.Impl;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Dynamic Report Service.
    /// </summary>
    public class RedemptionStatisticsService : BaseService, IRedemptionStatisticsService
    {
        private const string SummaryReportSessionKey = "Fanex.CRM.Report.Services.RedemptionStatisticsReport.SummaryReport";
        private const string DetailReportSessionKey = "Fanex.CRM.Report.Services.RedemptionStatisticsReport.DetailReport";

        private ISettingService _serviceSetting;

        public RedemptionStatisticsService()
            : this(null)
        {
        }

        public RedemptionStatisticsService(
            ISettingService serviceSetting)
        {
            _serviceSetting = serviceSetting ?? new SettingService();
        }

        /// <summary>
        /// Gets all status.
        /// </summary>
        /// <returns>All status.</returns>
        public List<SettingInfo> AllStatus()
        {
            try
            {
                var statusList = _serviceSetting.AllStatus(ViewTypeConstants.Redemption);

                int deleteIndex = -1;
                for (int i = 0; i < statusList.Count; i++)
                {
                    if (statusList[i].Name == "Deleted")
                    {
                        deleteIndex = i;
                        break;
                    }
                }

                if (deleteIndex >= 0)
                {
                    statusList.RemoveAt(deleteIndex);
                }

                return statusList;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// All the report by.
        /// </summary>
        /// <returns>The Setting List.</returns>
        public List<BaseModel> AllReportBy()
        {
            return GetAllReportByRedemptionCriteria.ReportByList;
        }

        /// <summary>
        /// All the currency redemption.
        /// </summary>
        /// <returns>The Setting Info List.</returns>
        public List<SettingInfo> AllCurrencyRedemption()
        {
            try
            {
                var allCurrency = new List<SettingInfo>();
                var rawData = _serviceSetting.AllCurrencyRedemption(new GetAllCurrencyRedemptionCriteria());

                allCurrency.Add(new SettingInfo
                {
                    Id = SettingConstants.NotApply,
                    Name = Labels.All
                });

                foreach (var rawStatus in rawData)
                {
                    allCurrency.Add(rawStatus);
                }

                return allCurrency;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the redemption summary report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The Grid Options.</returns>
        public GridOptions GetRedemptionSummaryReport(GetRedemptionStatisticsSummaryCriteria criteria)
        {
            var gridOptions = new GridOptions();
            gridOptions.Columns = this.GenSummaryHeaderRow();
            gridOptions.ShowDistinctRow = true;
            gridOptions.ShowHighlightRow = true;
            gridOptions.ShowTitle = true;
            gridOptions.ShowFooter = true;
            gridOptions.TitleInfoText = string.Format(
                Labels.RedemptionStatisticsReportTitle,
                criteria.FromDate.ToShortFormatedDate(),
                criteria.ToDate.ToShortFormatedDate());

            List<RedemptionStatisticsEntity> summaryReportData = new List<RedemptionStatisticsEntity>();
            criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
            criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);
            try
            {
                //// Body Row.
                summaryReportData = this.GetData<RedemptionStatisticsEntity>(criteria).ToList();
                foreach (var item in summaryReportData)
                {
                    var dataRow = GenSummaryDataRow(item);
                    gridOptions.BodyRows.Add(dataRow);
                }

                //// Total Row.
                var currencyGroup = summaryReportData.GroupBy(i => i.Currency).Select(d => new
                {
                    Currency = d.Key,
                    Amount = d.Sum(x => x.Amount)
                }).ToList();

                int totalCurrency = currencyGroup.Count;
                foreach (var item in currencyGroup)
                {
                    var dataRow = GenSummaryTotalRow(item.Currency, item.Amount, ref totalCurrency);
                    gridOptions.FooterRows.Add(dataRow);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }

            return gridOptions;
        }

        /// <summary>
        /// Gets the redemption detail report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The Grid Options.</returns>
        public GridOptions GetRedemptionDetailReport(GetRedemptionStatisticsDetailCriteria criteria)
        {
            var gridOptions = new GridOptions();
            gridOptions.Columns = this.GenDetailHeaderRow();
            gridOptions.ShowDistinctRow = true;
            gridOptions.ShowHighlightRow = true;
            gridOptions.ShowTitle = true;
            gridOptions.ShowFooter = true;
            gridOptions.TitleInfoText = Labels.RedemptionStatisticsDetailReportTitle;

            List<RedemptionStatisticsDetailEntity> detailReportData = new List<RedemptionStatisticsDetailEntity>();
            //// Get data from DB.
            try
            {
                //// Body Row.
                detailReportData = this.GetData<RedemptionStatisticsDetailEntity>(criteria).ToList();
                foreach (var item in detailReportData)
                {
                    var dataRow = GenDetailDataRow(item);
                    gridOptions.BodyRows.Add(dataRow);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }

            return gridOptions;
        }

        /// <summary>
        /// Generates the work book.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>The Workbook.</returns>
        public IWorkbook GenerateWorkBook(FanexExcel result)
        {
            var wb = result.ToExcel2003();
            HSSFPalette palette = ((HSSFWorkbook)wb).GetCustomPalette();
            palette.SetColorAtIndex(
                NpoiExcelExtensions.GrandTotalRowColorIndex, 
                NpoiExcelExtensions.GrandTotalRowColor.R, 
                NpoiExcelExtensions.GrandTotalRowColor.G, 
                NpoiExcelExtensions.GrandTotalRowColor.B);

            var footer = result.GetFooterCellStyles;
            ICellStyle rightGeneral = wb.GetCellStyleAt(footer.General.Right.UpperStyle.Index);
            rightGeneral.FillForegroundColor = NpoiExcelExtensions.GrandTotalRowColorIndex;
            rightGeneral.FillPattern = FillPattern.SolidForeground;
            rightGeneral.BorderTop = BorderStyle.None;

            return wb;
        }

        /// <summary>
        /// Gens the summary header row.
        /// </summary>
        /// <returns>The Header Columns List.</returns>
        private List<HeaderColumns> GenSummaryHeaderRow()
        {
            var headerRow = new List<HeaderColumns>();
            headerRow.Add(new HeaderColumns() { HeaderText = Labels.Date, Name = Labels.Date, Width = 200 });
            headerRow.Add(new HeaderColumns() { HeaderText = Labels.Currency, Name = Labels.Currency, Width = 150 });
            headerRow.Add(new HeaderColumns(Labels.Amount, Labels.Amount, 150, HAlignment.Left, "center", ExcelCellType.Double));
            return headerRow;
        }

        /// <summary>
        /// Gens the detail header row.
        /// </summary>
        /// <returns>The Header Columns List.</returns>
        private List<HeaderColumns> GenDetailHeaderRow()
        {
            var headerRow = new List<HeaderColumns>();
            headerRow.Add(new HeaderColumns() { HeaderText = Labels.ClaimedDate, Name = Labels.ClaimedDate, Width = 150 });
            headerRow.Add(new HeaderColumns() { HeaderText = Labels.Customer, Name = Labels.Customer, Width = 150 });
            headerRow.Add(new HeaderColumns() { HeaderText = Labels.Subject, Name = Labels.Subject, Width = 350 });
            headerRow.Add(new HeaderColumns() { HeaderText = Labels.Currency, Name = Labels.Currency, Width = 100 });
            headerRow.Add(new HeaderColumns(Labels.Amount, Labels.Amount, 100, HAlignment.Left, "center", ExcelCellType.Double));
            headerRow.Add(new HeaderColumns() { HeaderText = Labels.Status, Name = Labels.Status, Width = 100 });
            headerRow.Add(new HeaderColumns() { HeaderText = Labels.DoneBy, Name = Labels.DoneBy, Width = 100 });
            return headerRow;
        }

        /// <summary>
        /// Gens the summary data row.
        /// </summary>
        /// <param name="item">The Redemption Statistics Entity item.</param>
        /// <returns>The Grid Data Row.</returns>
        private GridDataRow GenSummaryDataRow(RedemptionStatisticsEntity item)
        {
            var dataRow = new GridDataRow();
            int currentColIndex = 0;
            var align = HAlignment.Center;

            var clickDateFormat = "<span class=\"detail-date link\" data-subject=\"{0}\" " +
                                    "data-currency=\"{1}\" data-currency-id=\"{2}\" data-month=\"{3}\" data-year=\"{4}\">{5}</span>";
            var clickCurrencyFormat = "<span class=\"detail-currency link\" data-subject=\"{0}\" " +
                                    "data-currency=\"{1}\" data-currency-id=\"{2}\" data-month=\"{3}\" data-year=\"{4}\">{5}</span>";
            var dateIdFormat = "{0} {1}";

            var dateId = string.Format(dateIdFormat, DateTimeUtilize.GetMonthName(item.Month), item.Year);
            var dateSpan = string.Format(clickDateFormat, string.Empty, string.Empty, 0, item.Month, item.Year, dateId);
            var currencySpan = string.Format(
                clickCurrencyFormat,
                string.Empty,
                item.Currency,
                item.CurrencyId,
                item.Month,
                item.Year,
                item.Currency);

            dataRow.Columns.Add(this.CreateColumns(currentColIndex++, Labels.Date, dateSpan, dateId, string.Empty, align));
            dataRow.Columns.Add(this.CreateColumns(currentColIndex++, Labels.Currency, currencySpan, item.Currency, string.Empty, align));
            dataRow.Columns.Add(this.CreateColumns(currentColIndex++, Labels.Amount, item.Amount, null, string.Empty, HAlignment.Right));

            return dataRow;
        }

        /// <summary>
        /// Gens the summary total row.
        /// </summary>
        /// <param name="currency">The currency.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="mergeNum">The merge number.</param>
        /// <returns>The Grid Data Row.</returns>
        private GridDataRow GenSummaryTotalRow(string currency, decimal amount, ref int mergeNum)
        {
            var dataRow = new GridDataRow();
            int currentColIndex = 0;
            var align = HAlignment.Center;

            if (mergeNum > 0)
            {
                dataRow.Columns.Add(this.CreateColumns(currentColIndex++, Labels.Date, Labels.Total, null, string.Empty, HAlignment.Right, true, mergeNum));
                mergeNum = 0;
            }

            dataRow.Columns.Add(this.CreateColumns(currentColIndex++, Labels.Currency, currency, null, string.Empty, align, true));
            dataRow.Columns.Add(this.CreateColumns(currentColIndex++, Labels.Amount, amount, null, string.Empty, HAlignment.Right, true));

            return dataRow;
        }

        /// <summary>
        /// Gens the detail data row.
        /// </summary>
        /// <param name="item">The Redemption Statistics Detail Entity item.</param>
        /// <returns>The Grid Data Row.</returns>
        private GridDataRow GenDetailDataRow(RedemptionStatisticsDetailEntity item)
        {
            var dataRow = new GridDataRow();
            int currentColIndex = 0;
            var align = HAlignment.Center;

            dataRow.Columns.Add(this.CreateColumns(currentColIndex++, Labels.ClaimedDate, item.CreateDate.ToShortFormatedDate(), null, string.Empty, HAlignment.Center));
            dataRow.Columns.Add(this.CreateColumns(currentColIndex++, Labels.Customer, item.UserName, null, string.Empty, HAlignment.Left));
            dataRow.Columns.Add(this.CreateColumns(
                currentColIndex++,
                Labels.Subject,
                HttpContext.Current.Server.HtmlEncode(item.Subject),
                item.Subject,
                string.Empty,
                HAlignment.Left));
            dataRow.Columns.Add(this.CreateColumns(currentColIndex++, Labels.Currency, item.Currency, null, string.Empty, align));
            dataRow.Columns.Add(this.CreateColumns(currentColIndex++, Labels.Amount, item.Amount, null, string.Empty, HAlignment.Right));
            dataRow.Columns.Add(this.CreateColumns(currentColIndex++, Labels.Status, item.Status, null, string.Empty, align));
            dataRow.Columns.Add(this.CreateColumns(currentColIndex++, Labels.DoneBy, item.DoneBy, null, string.Empty, HAlignment.Left));

            return dataRow;
        }

        /// <summary>
        /// Creates the columns.
        /// </summary>
        /// <param name="colIndex">Index of the col.</param>
        /// <param name="name">The column name.</param>
        /// <param name="value">The column value.</param>
        /// <param name="excelValue">The excel value.</param>
        /// <param name="cellClass">The cell class.</param>
        /// <param name="cellAlign">The cell align.</param>
        /// <param name="isBold">If set to <c>true</c> [is bold].</param>
        /// <param name="mergeNum">The merge number.</param>
        /// <returns>The Created Column.</returns>
        private Columns CreateColumns(
            int colIndex,
            string name,
            object value,
            object excelValue,
            string cellClass,
            HAlignment cellAlign,
            bool isBold = false,
            int mergeNum = 1)
        {
            string displayValue = excelValue == null ? Converter.ToString2(value) : Converter.ToString2(excelValue);
            return new Columns(colIndex)
            {
                Name = name ?? string.Empty,
                Value = value ?? string.Empty,
                DisplayValue = displayValue,
                TDClass = cellClass ?? string.Empty,
                CellAlignment = cellAlign,
                Bold = isBold,
                MergeRowNumber = mergeNum
            };
        }
    }
}