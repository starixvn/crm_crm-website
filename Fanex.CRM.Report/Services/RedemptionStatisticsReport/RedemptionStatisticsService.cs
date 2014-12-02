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
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Helpers;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Criteria.RedemptionStatisticsReport;
    using Fanex.CRM.Report.Entities.RedemptionStatistics;
    using Fanex.CRM.Report.Models.RedemptionStatistics;
    using Fanex.CRM.Report.Resources;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Services;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.Excel.Models;
    using Fanex.Excel.Enums;

    /// <summary>
    /// Dynamic Report Service.
    /// </summary>
    public class RedemptionStatisticsService : BaseService, IRedemptionStatisticsService
    {
        private const string SummaryReportSessionKey = "Fanex.CRM.Report.Services.RedemptionStatisticsReport.SummaryReport";
        private const string DetailReportSessionKey = "Fanex.CRM.Report.Services.RedemptionStatisticsReport.DetailReport";

        private GridOptions _gridOptions { get; set; }

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
                return _serviceSetting.AllStatus(ViewTypeConstants.CommunicationTool);
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

        public RedemptionStatisticsInfo GetRedemptionSummaryReport(GetRedemptionStatisticsSummaryCriteria criteria)
        {
            string key = SummaryReportSessionKey;
            List<RedemptionStatisticsEntity> summaryReportData = null;
            if (criteria.IsGetFromCached)
            {
                summaryReportData = (List<RedemptionStatisticsEntity>)SessionExtension.GetSession(HttpContext.Current.Session, key);
            }

            string title = string.Format("Redemption Statistics Report from {0} to {1}", criteria.FromDate.ToShortFormatedDate(), criteria.ToDate.ToShortFormatedDate());
            int totalItems = 0;
            if (summaryReportData == null)
            {
                summaryReportData = new List<RedemptionStatisticsEntity>();
                // Get data from DB.
                criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
                criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);
                try
                {
                    summaryReportData = this.GetData<RedemptionStatisticsEntity>(criteria, out totalItems).ToList();
                    SessionExtension.AddSession(HttpContext.Current.Session, key, summaryReportData);
                }
                catch (Exception ex)
                {
                    LogExtensions.ExceptionHandler(ex);
                    throw;
                }
            }

            return new RedemptionStatisticsInfo
            {
                Title = title,
                ReportData = summaryReportData,
                TotalData = null,
                CurrentPage = criteria.PageIndex,
                ItemsPerPage = criteria.PageSize
            };
        }

        public GridOptions GetRedemptionSummaryReportV2(GetRedemptionStatisticsSummaryCriteria criteria)
        {
            var gridOptions = new GridOptions();
            gridOptions.Columns = this.GenSummaryHeaderRow();
            gridOptions.ShowDistinctRow = true;
            gridOptions.ShowHighlightRow = true;
            gridOptions.ShowTitle = true;
            gridOptions.ShowFooter = true;
            gridOptions.TitleInfoText = string.Format("Redemption Statistics Report from {0} to {1}", criteria.FromDate.ToShortFormatedDate(), criteria.ToDate.ToShortFormatedDate());

            string key = SummaryReportSessionKey;
            List<RedemptionStatisticsEntity> summaryReportData = null;

            string title = string.Format("Redemption Statistics Report from {0} to {1}", criteria.FromDate.ToShortFormatedDate(), criteria.ToDate.ToShortFormatedDate());
            int totalItems = 0;

            summaryReportData = new List<RedemptionStatisticsEntity>();
            // Get data from DB.
            criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
            criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);
            try
            {
                summaryReportData = this.GetData<RedemptionStatisticsEntity>(criteria, out totalItems).ToList();

                foreach (var item in summaryReportData)
                {
                    var dataRow = GenSummaryDataRow(item);
                    this._gridOptions.BodyRows.Add(dataRow);
                }
                SessionExtension.AddSession(HttpContext.Current.Session, key, this._gridOptions);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
            return this._gridOptions;
        }

        public IEnumerable<RedemptionStatisticsDetailEntity> GetRedemptionDetailReport(GetRedemptionStatisticsDetailCriteria criteria)
        {
            string key = DetailReportSessionKey;
            IEnumerable<RedemptionStatisticsDetailEntity> detailReportData = null;
            if (criteria.IsGetFromCached)
            {
                detailReportData = (List<RedemptionStatisticsDetailEntity>)SessionExtension.GetSession(HttpContext.Current.Session, key);
            }

            string title = string.Format("Redemption Statistics Detail Report", criteria.Month, criteria.Year);
            int totalItems = 0;
            if (detailReportData == null)
            {
                detailReportData = new List<RedemptionStatisticsDetailEntity>();
                // Get data from DB.
                try
                {
                    detailReportData = this.GetData<RedemptionStatisticsDetailEntity>(criteria, out totalItems).ToList();
                    SessionExtension.AddSession(HttpContext.Current.Session, key, detailReportData);
                }
                catch (Exception ex)
                {
                    LogExtensions.ExceptionHandler(ex);
                    throw;
                }
            }

            return detailReportData;
        }

        private List<HeaderColumns> GenSummaryHeaderRow()
        {
            var headerRow = new List<HeaderColumns>();
            headerRow.Add(new HeaderColumns() { HeaderText = Labels.Date, Name = Labels.Date, Width = 200 });
            headerRow.Add(new HeaderColumns() { HeaderText = Labels.Currency, Name = Labels.Currency, Width = 150 });
            headerRow.Add(new HeaderColumns() { HeaderText = Labels.Amount, Name = Labels.Amount, Width = 150 });
            return headerRow;
        }

        private GridDataRow GenSummaryDataRow(RedemptionStatisticsEntity item)
        {
            var dataRow = new GridDataRow();
            int currentColIndex = 0;
            var align = HAlignment.Right;

            dataRow.Columns.Add(this.CreateColumns(currentColIndex++, Labels.Date, "Date", "CustomeCss", align, true));

            return dataRow;
        }

        private Columns CreateColumns(int colIndex, string name, object value, string cellClass, HAlignment cellAlign, bool isBold = false)
        {
            return new Columns(colIndex) { Name = name, Value = value, TDClass = cellClass, CellAlignment = cellAlign, Bold = isBold };
        }
    }
}
