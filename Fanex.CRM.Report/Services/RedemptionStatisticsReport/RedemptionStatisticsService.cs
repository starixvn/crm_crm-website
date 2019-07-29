namespace Fanex.CRM.Report.Services.DynamicReport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Fanex.Common;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Report.Criteria.RedemptionStatisticsReport;
    using Fanex.CRM.Report.Entities.RedemptionStatistics;
    using Fanex.CRM.Report.Repositories.RedemptionStatisticsReport;
    using Fanex.CRM.Report.Resources;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Services;
    using Fanex.Excel;
    using Fanex.Excel.Enums;
    using Fanex.Excel.Models;
    using Fanex.Excel.Repositories.CustomColors;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;

    public class RedemptionStatisticsService : IRedemptionStatisticsService
    {
        private readonly ISettingService _serviceSetting;
        private readonly IRedemptionStatisticsRepository _redemptionRepository;

        public RedemptionStatisticsService()
            : this(null, null)
        {
        }

        public RedemptionStatisticsService(
            ISettingService serviceSetting,
            IRedemptionStatisticsRepository redemptionRepository)
        {
            _serviceSetting = serviceSetting ?? new SettingService();
            _redemptionRepository = redemptionRepository ?? new RedemptionStatisticsRepository();
        }

        public List<BaseModel> AllStatus()
        {
            try
            {
                var statusList = _serviceSetting.GetAllStatuses(ViewTypeConstants.Redemption);

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

        public List<BaseModel> AllReportBy()
        {
            return GetAllReportByRedemptionCriteria.ReportByList;
        }

        public List<BaseModel> AllCurrencyRedemption()
        {
            try
            {
                var currencies = _serviceSetting.GetAllCurrencyRedemptions(new GetAllCurrencyRedemptionCriteria());
                currencies.Insert(0, new BaseModel { Id = SettingConstants.NotApply, Name = Labels.All });

                return currencies;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        public GridOptions GetRedemptionSummaryReport(GetRedemptionStatisticsSummaryCriteria criteria)
        {
            var gridOptions = new GridOptions
            {
                Columns = this.GenSummaryHeaderRow(),
                ShowDistinctRow = true,
                ShowHighlightRow = true,
                ShowTitle = true,
                ShowFooter = true,
                TitleInfoText = string.Format(
                Labels.RedemptionStatisticsReportTitle,
                criteria.FromDate.ToShortFormatedDate(),
                criteria.ToDate.ToShortFormatedDate())
            };

            IDictionary<string, decimal> totalHash = new Dictionary<string, decimal>();
            criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
            criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);

            // Body Row.
            var rawdata = _redemptionRepository.GetRedemptionSummary(criteria)
                .GroupBy(r => new { r.Month, r.Year, r.CurrencyId, r.Currency })
                .OrderByDescending(r => r.Key.Year).ThenByDescending(r => r.Key.Month);
            foreach (var customGroup in rawdata)
            {
                RedemptionStatisticsEntity entity = new RedemptionStatisticsEntity
                {
                    Month = customGroup.Key.Month,
                    Year = customGroup.Key.Year,
                    CurrencyId = customGroup.Key.CurrencyId,
                    Currency = customGroup.Key.Currency
                };

                foreach (var item in customGroup)
                {
                    entity.Amount += item.Amount;
                }

                gridOptions.BodyRows.Add(GenSummaryDataRow(entity));

                // Calculate total.
                if (!totalHash.ContainsKey(entity.Currency))
                {
                    totalHash.Add(entity.Currency, entity.Amount);
                }
                else
                {
                    totalHash[entity.Currency] += entity.Amount;
                }
            }

            // Total Row.
            int totalCurrency = totalHash.Count;
            foreach (var currency in totalHash.Keys)
            {
                var dataRow = GenSummaryTotalRow(currency, totalHash[currency], ref totalCurrency);
                gridOptions.FooterRows.Add(dataRow);
            }

            return gridOptions;
        }

        public GridOptions GetRedemptionDetailReport(GetRedemptionStatisticsDetailCriteria criteria)
        {
            // PageIndex of SP starts from 1 but Fanex.Grid starts from 0.
            criteria.PageIndex = ++criteria.PageIndex;
            criteria.ItemsPerPage = criteria.ItemsPerPage == 0 ? Pager.DefaultItemsPerPage : criteria.ItemsPerPage;
            criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
            criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);
            var gridOptions = new GridOptions
            {
                Columns = this.GenDetailHeaderRow(),
                ShowDistinctRow = true,
                ShowHighlightRow = true,
                ShowTitle = true,
                ShowFooter = true,
                NumberOfFrozenColumn = 2,
                TitleInfoText = Labels.RedemptionStatisticsDetailReportTitle,
                ShowPager = true,
                CustomPager = true,
                PagerOption = new PagerOption
                {
                    CurrentPage = criteria.PageIndex - 1,
                    ItemsPerPage = criteria.ItemsPerPage,
                    ItemsPerPageArray = Pager.ItemsPerPageArray
                }
            };

            // Body Row.
            var detailReportData = _redemptionRepository.GetRedemptionDetail(criteria);
            if (criteria.TotalRows > 0)
            {
                foreach (var item in detailReportData)
                {
                    var dataRow = GenDetailDataRow(item);
                    gridOptions.BodyRows.Add(dataRow);
                }
            }

            gridOptions.PagerOption.TotalItem = criteria.TotalRows;

            return gridOptions;
        }

        public IWorkbook GenerateSummaryExcelWorkbook(GetRedemptionStatisticsSummaryCriteria criteria)
        {
            GridOptions gridOption = this.GetRedemptionSummaryReport(criteria);

            gridOption.ShowFooterLineBreak = false;
            gridOption.TitleType = Excel.Enums.TitleType.TwoLineBlue;
            FanexExcel result = new FanexExcel(gridOption);
            result.SetColorAtIndex(
                GrandTotalRowColor.Index,
                new short[] { NpoiExtensions.GrandTotalRowColor.R, NpoiExtensions.GrandTotalRowColor.G, NpoiExtensions.GrandTotalRowColor.B });

            var workbook = result.ToExcel2003(Labels.RedemptionStatisticsSheetName);
            var footerFont = result.GetFooterCellStyles.General.Center.Style.GetFont(workbook);
            footerFont.Color = HSSFColor.White.Index;

            return workbook;
        }

        public IWorkbook GenerateDetailExcelWorkbook(GetRedemptionStatisticsDetailCriteria criteria)
        {
            criteria.PageIndex = 0;
            criteria.ItemsPerPage = int.MaxValue;
            GridOptions gridOption = this.GetRedemptionDetailReport(criteria);

            gridOption.ShowFooterLineBreak = false;
            gridOption.TitleType = Excel.Enums.TitleType.TwoLineBlue;
            FanexExcel result = new FanexExcel(gridOption);

            return result.ToExcel2003(Labels.RedemptionStatisticsDetailSheetName);
        }

        private List<HeaderColumns> GenSummaryHeaderRow()
        {
            var headerRow = new List<HeaderColumns>
            {
                new HeaderColumns { HeaderText = Labels.Date, Name = Labels.Date, Width = 200 },
                new HeaderColumns { HeaderText = Labels.Currency, Name = Labels.Currency, Width = 150 },
                new HeaderColumns(Labels.Amount, Labels.Amount, 150, HAlignment.Left, "center", ExcelCellType.Double)
            };
            return headerRow;
        }

        private List<HeaderColumns> GenDetailHeaderRow()
        {
            var headerRow = new List<HeaderColumns>
            {
                new HeaderColumns { HeaderText = Labels.ClaimedDate, Name = Labels.ClaimedDate, Width = 100 },
                new HeaderColumns { HeaderText = Labels.Customer, Name = Labels.Customer, Width = 150 },
                new HeaderColumns { HeaderText = Labels.Subject, Name = Labels.Subject, Width = 350 },
                new HeaderColumns { HeaderText = Labels.Currency, Name = Labels.Currency, Width = 80 },
                new HeaderColumns(Labels.Amount, Labels.Amount, 100, HAlignment.Left, "center", ExcelCellType.Double),
                new HeaderColumns { HeaderText = Labels.Status, Name = Labels.Status, Width = 80 },
                new HeaderColumns { HeaderText = Labels.DoneBy, Name = Labels.DoneBy, Width = 100 }
            };
            return headerRow;
        }

        private GridDataRow GenSummaryDataRow(RedemptionStatisticsEntity item)
        {
            var dataRow = new GridDataRow();
            int currentColIndex = 0;
            var align = HAlignment.Center;

            const string ClickDateFormat = "<span class=\"detail-date link\" " +
                                    "data-currency=\"{0}\" data-currency-id=\"{1}\" data-month=\"{2}\" data-year=\"{3}\">{4}</span>";
            const string ClickCurrencyFormat = "<span class=\"detail-currency link\" " +
                                    "data-currency=\"{0}\" data-currency-id=\"{1}\" data-month=\"{2}\" data-year=\"{3}\">{4}</span>";
            const string DateIdFormat = "{0} {1}";

            var dateId = item.Month == 0 ? item.Year.ToString() : string.Format(DateIdFormat, DateTimeUtilize.GetMonthName(item.Month), item.Year);
            var dateSpan = string.Format(ClickDateFormat, string.Empty, 0, item.Month, item.Year, dateId);
            var currencySpan = string.Format(
                ClickCurrencyFormat,
                item.Currency,
                item.CurrencyId,
                0,
                0,
                item.Currency);

            dataRow.Columns.Add(this.CreateColumns(currentColIndex++, Labels.Date, dateSpan, dateId, string.Empty, align));
            dataRow.Columns.Add(this.CreateColumns(currentColIndex++, Labels.Currency, currencySpan, item.Currency, string.Empty, align));
            dataRow.Columns.Add(this.CreateColumns(currentColIndex++, Labels.Amount, item.Amount, null, string.Empty, HAlignment.Right));

            return dataRow;
        }

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
            else
            {
                currentColIndex++;
            }

            var currencyColumn = this.CreateColumns(currentColIndex, Labels.Currency, currency, null, string.Empty, align, true);
            currencyColumn.ColumnIndex = currentColIndex;
            currentColIndex++;

            var amountColumn = this.CreateColumns(currentColIndex, Labels.Amount, amount, null, string.Empty, HAlignment.Right, true);
            amountColumn.ColumnIndex = currentColIndex;

            dataRow.Columns.Add(currencyColumn);
            dataRow.Columns.Add(amountColumn);
            return dataRow;
        }

        private GridDataRow GenDetailDataRow(RedemptionStatisticsDetailEntity item)
        {
            var dataRow = new GridDataRow();
            int currentColIndex = 0;
            var align = HAlignment.Center;

            var claimTime = DateTimeUtilize.ChangeTimeZoneToClientDateTime(item.ClaimTime).ToShortFormatedDate();
            dataRow.Columns.Add(this.CreateColumns(currentColIndex++, Labels.ClaimedDate, claimTime, null, string.Empty, HAlignment.Center));
            dataRow.Columns.Add(this.CreateColumns(currentColIndex++, Labels.Customer, item.UserName, null, string.Empty, HAlignment.Left));
            dataRow.Columns.Add(this.CreateColumns(
                currentColIndex++,
                Labels.Subject,
                HttpContext.Current.Server.HtmlEncode(item.Subject),
                item.Subject,
                "subject-wordwrap",
                HAlignment.Left));
            dataRow.Columns.Add(this.CreateColumns(currentColIndex++, Labels.Currency, item.Currency, null, string.Empty, align));
            dataRow.Columns.Add(this.CreateColumns(currentColIndex++, Labels.Amount, item.Amount, null, string.Empty, HAlignment.Right));
            dataRow.Columns.Add(this.CreateColumns(currentColIndex++, Labels.Status, item.Status, null, string.Empty, align));
            dataRow.Columns.Add(this.CreateColumns(
                currentColIndex++,
                Labels.DoneBy,
                HttpContext.Current.Server.HtmlEncode(item.DoneBy),
                item.DoneBy,
                string.Empty,
                HAlignment.Left));

            return dataRow;
        }

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
            string displayValue = excelValue == null ? null : Converter.ToString2(excelValue);
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