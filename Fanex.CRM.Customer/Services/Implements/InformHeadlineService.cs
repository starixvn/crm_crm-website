// --------------------------------------------------------------------------
// <copyright file="InformHeadlineService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.05.09">Created</change>
// </history>

namespace Fanex.CRM.Customer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Customer.Criteria.InformHeadlines;
    using Fanex.CRM.Customer.Models.InformHeadlines;
    using Fanex.CRM.Customer.Repositories;
    using Fanex.CRM.Customer.Resources;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Helpers;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Models;
    using Fanex.CRM.Setting.Services;
    using Fanex.Data.Repository;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    /// <summary>
    /// InformHeadlineService class.
    /// </summary>
    public class InformHeadlineService : IInformHeadlineService
    {
        private const string InformHeadlineStatisticSessionKey = "Fanex.CRM.Customer.InformHeadlineStatisticSessionKey";

        private readonly IInformHeadlineRepository _informHeadlineRepository;
        private readonly ISettingService _settingService;

        public InformHeadlineService()
            : this(null, null)
        {
        }

        public InformHeadlineService(IInformHeadlineRepository informHeadlineRepository, ISettingService settingService)
        {
            _informHeadlineRepository = informHeadlineRepository ?? new InformHeadlineRepository();
            _settingService = settingService ?? new SettingService();
        }

        public InformHeadlineSetting AllInformHeadlineSettings
        {
            get
            {
                return _settingService.AllInformHeadlineSettings();
            }
        }

        /// <summary>
        /// Gets the inform statistic.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The InformHeadlineStatistic object.</returns>
        public InformHeadlineStatistic GetInformStatistic(GetInformHeadlineStatisticCriteria criteria)
        {
            string sessionKey = InformHeadlineStatisticSessionKey;
            InformHeadlineStatistic statistic = null;
            try
            {
                statistic = criteria.IsCached
                    ? (InformHeadlineStatistic)SessionExtension.GetSession(HttpContext.Current.Session, sessionKey)
                    : null;
                if (statistic == null)
                {
                    statistic = ParseInformHeadlineStatistic(criteria);

                    // Add to session.
                    if (statistic != null)
                    {
                        SessionExtension.AddSession(HttpContext.Current.Session, sessionKey, statistic);
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }

            return statistic;
        }

        /// <summary>
        /// Exports the inform statistic.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public void ExportInformStatistic(GetInformHeadlineStatisticCriteria criteria)
        {
            criteria.IsCached = true;
            InformHeadlineStatistic statistic = GetInformStatistic(criteria);

            string excelFileName = Labels.InformHeadlinesStatisticExcelName;
            string sheetName = Labels.InformHeadlinesStatisticName;
            HSSFWorkbook workbook = new HSSFWorkbook();
            ExportExcelInfo exportExcelInfo = new ExportExcelInfo
            {
                ExcelFile = workbook,
                Worksheet = workbook.CreateSheet(sheetName),
                StartRow = 0,
                StartColumn = 0,
                CurrentRow = 0,
                MaxColumnIndex = 9
            };

            ExportTitle(ref exportExcelInfo, statistic.ReportTitle);
            ExportHeader(ref exportExcelInfo);
            if (statistic.Informs == null || statistic.Informs.Count == 0)
            {
                NpoiExcelExtensions.RenderNoInfoMessage(exportExcelInfo, exportExcelInfo.CurrentRow);
            }
            else
            {
                NpoiExcelExtensions.SetColorAtIndex(exportExcelInfo, NpoiExcelExtensions.EvenRowColor, NpoiExcelExtensions.EvenRowColorIndex);
                NpoiExcelExtensions.SetGrandTotalColorIndex(exportExcelInfo);

                ExportBody(ref exportExcelInfo, statistic.Informs);
            }

            NpoiExcelExtensions.ResponseStreamExcel(exportExcelInfo.ExcelFile, excelFileName);
        }

        /// <summary>
        /// Creates the inform.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The result object.</returns>
        public Result CreateTitle(ICriteria criteria)
        {
            Result result = new Result { Code = 0 };
            try
            {
                _informHeadlineRepository.CreateTitle(criteria);
            }
            catch (Exception ex)
            {
                result.Code = -1;
                result.Message = ex.Message;
                LogExtensions.ExceptionHandler(ex);
                throw;
            }

            return result;
        }

        /// <summary>
        /// Gets the informs.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The InformHeadline list.</returns>
        public IList<InformHeadline> GetInforms(GetInformHeadlineCriteria criteria)
        {
            try
            {
                return _informHeadlineRepository.GetInforms(criteria);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Exports the informs.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public void ExportInforms(GetInformHeadlineCriteria criteria)
        {
            // Get all inform.
            criteria.PageIndex = 1;
            criteria.ItemsPerPage = -1;
            IList<InformHeadline> informHeadlines = GetInforms(criteria);

            string excelFileName = Labels.InformHeadlinesStatusExcelName;
            string sheetName = Labels.InformHeadlinesStatusName;
            HSSFWorkbook workbook = new HSSFWorkbook();
            ExportExcelInfo exportExcelInfo = new ExportExcelInfo
            {
                ExcelFile = workbook,
                Worksheet = workbook.CreateSheet(sheetName),
                StartRow = 0,
                StartColumn = 0,
                CurrentRow = 0,
                MaxColumnIndex = 6
            };

            ExportInformsStatusTitle(ref exportExcelInfo, sheetName);
            ExportInformsStatusHeader(ref exportExcelInfo);
            if (informHeadlines == null || informHeadlines.Count == 0)
            {
                NpoiExcelExtensions.RenderNoInfoMessage(exportExcelInfo, exportExcelInfo.CurrentRow);
            }
            else
            {
                NpoiExcelExtensions.SetColorAtIndex(exportExcelInfo, NpoiExcelExtensions.EvenRowColor, NpoiExcelExtensions.EvenRowColorIndex);
                NpoiExcelExtensions.SetGrandTotalColorIndex(exportExcelInfo);

                ExportInformsStatusBody(ref exportExcelInfo, informHeadlines);
            }

            NpoiExcelExtensions.ResponseStreamExcel(exportExcelInfo.ExcelFile, excelFileName);
        }

        /// <summary>
        /// Updates the informs.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The result object.</returns>
        public Result UpdateInforms(UpdateInformHeadlinesCriteria criteria)
        {
            try
            {
                if (criteria.Informs != null && criteria.Informs.Count != 0)
                {
                    StringBuilder contentXml = new StringBuilder(1024);
                    contentXml.Append("<content>");
                    foreach (InformHeadline inform in criteria.Informs)
                    {
                        contentXml.Append(BuildUpdateInformXml(inform));
                    }

                    contentXml.Append("</content>");
                    criteria.Content = HtmlHelper.ReplaceBreaklineXml(contentXml.ToString());
                }
                else
                {
                    criteria.Content = "<content></content>";
                }

                return _informHeadlineRepository.UpdateInforms(criteria);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        private InformHeadlineStatistic ParseInformHeadlineStatistic(GetInformHeadlineStatisticCriteria criteria)
        {
            var rawData = _informHeadlineRepository.GetInformStatistic(criteria);
            return new InformHeadlineStatistic
            {
                ReportTitle = Labels.InformHeadlinesStatisticName,
                Informs = rawData
            };
        }

        /// <summary>
        /// Builds the update inform XML.
        /// </summary>
        /// <param name="inform">The inform.</param>
        /// <returns>The string of xml content.</returns>
        private string BuildUpdateInformXml(InformHeadline inform)
        {
            string template = "<headline id=\"{0}\" statusid=\"{1}\" headlineid=\"{2}\" remark=\"{3}\" extension=\"{4}\" productstatus=\"{5}\" />";
            return string.Format(
                template,
                inform.InformId,
                inform.StatusId,
                inform.HeadlineId,
                HtmlHelper.EncodeXml(inform.Remark),
                inform.ExtensionId,
                inform.ProductStatus);
        }

        #region Inform statistic - Export excel

        /// <summary>
        /// Exports the title.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="title">The title.</param>
        private void ExportTitle(ref ExportExcelInfo exportExcelInfo, string title)
        {
            var columnWidths = new List<int>() { 280, 100, 150, 100, 150, 150, 100, 150, 150, 100 };
            NpoiExcelExtensions.SetHeaderColorIndex(exportExcelInfo);
            exportExcelInfo.InitColumnWidthByFontWidth(columnWidths);
            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());
            exportExcelInfo.RenderHeaderTitle(title, headerStyle);
        }

        /// <summary>
        /// Exports the header.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        private void ExportHeader(ref ExportExcelInfo exportExcelInfo)
        {
            int currentColumn = exportExcelInfo.StartColumn;
            IRow row = exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());

            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.Title, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.CreatedTime, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.NoOfHeadlines, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.Done, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.RequestCallback, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.NoNeedToCall, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.NoReply, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.Uncontactable, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.InternalAccount, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.NeverTouch, headerStyle);
            currentColumn++;
            exportExcelInfo.CurrentRow++;
        }

        /// <summary>
        /// Exports the body.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="informs">The informs.</param>
        private void ExportBody(ref ExportExcelInfo exportExcelInfo, IList<InformHeadlineStatus> informs)
        {
            ICellStyle titleCellStyle, createdTimeCellStyle, numberCellStyle;
            HSSFWorkbook workbook = exportExcelInfo.ExcelFile;
            int currentColumn, currentRow;
            int statisticLength = informs.Count;

            currentRow = exportExcelInfo.CurrentRow;
            titleCellStyle = GetTitleCellStyle(workbook);
            createdTimeCellStyle = GetCreatedTimeCellStyle(workbook);
            numberCellStyle = GetNumberCellStyle(workbook);
            InformHeadlineStatus inform;
            for (int i = 0; i < statisticLength; i++)
            {
                inform = informs[i];
                currentColumn = exportExcelInfo.StartColumn;

                IRow row = exportExcelInfo.Worksheet.GetRow(currentRow) ?? exportExcelInfo.Worksheet.CreateRow(currentRow);
                NpoiExcelExtensions.RenderCell(row, currentColumn, inform.Title, titleCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, inform.CreatedDateString, createdTimeCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, inform.Total, numberCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, inform.Done, numberCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, inform.RequestToCallback, numberCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, inform.NoNeedToCall, numberCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, inform.NoReply, numberCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, inform.Uncontactable, numberCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, inform.InternalAccount, numberCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, inform.NeverTouch, numberCellStyle);

                currentRow++;
            }

            exportExcelInfo.CurrentRow = currentRow;
        }

        private short GetRowColorIndex()
        {
            return NpoiExcelExtensions.BackgroundColorIndex;
        }

        private ICellStyle GetTitleCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.TextStyle(
                workbook,
                GetRowColorIndex(),
                false,
                CRMHorizontalAlignment.Left);
        }

        private ICellStyle GetCreatedTimeCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.TextStyle(
                workbook,
                GetRowColorIndex(),
                false,
                CRMHorizontalAlignment.Center);
        }

        private ICellStyle GetNumberCellStyle(HSSFWorkbook workbook)
        {
            return NpoiExcelExtensions.IntNumberStyle(
                workbook,
                GetRowColorIndex(),
                false,
                CRMHorizontalAlignment.Right);
        }

        #endregion

        #region Inform headline - Export excel

        /// <summary>
        /// Exports the informs status title.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="title">The title.</param>
        private void ExportInformsStatusTitle(ref ExportExcelInfo exportExcelInfo, string title)
        {
            var columnWidths = new List<int>() { 280, 150, 180, 400, 150, 120, 120 };
            NpoiExcelExtensions.SetHeaderColorIndex(exportExcelInfo);
            exportExcelInfo.InitColumnWidthByFontWidth(columnWidths);
            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());
            exportExcelInfo.RenderHeaderTitle(title, headerStyle);
        }

        /// <summary>
        /// Exports the informs status header.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        private void ExportInformsStatusHeader(ref ExportExcelInfo exportExcelInfo)
        {
            int currentColumn = exportExcelInfo.StartColumn;
            IRow row = exportExcelInfo.Worksheet.CreateRow(exportExcelInfo.CurrentRow);
            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());

            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.Headline, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.Status, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.UpdatedTime, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.Remark, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.Handler, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.Extension, headerStyle);
            currentColumn++;
            NpoiExcelExtensions.RenderCell(row, currentColumn, Labels.ProductStatus, headerStyle);
            currentColumn++;
            exportExcelInfo.CurrentRow++;
        }

        /// <summary>
        /// Exports the informs status body.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="informs">The informs.</param>
        private void ExportInformsStatusBody(ref ExportExcelInfo exportExcelInfo, IList<InformHeadline> informs)
        {
            ICellStyle titleCellStyle, updatedTimeCellStyle;
            HSSFWorkbook workbook = exportExcelInfo.ExcelFile;
            int currentColumn, currentRow;
            int statusLength = informs.Count;

            currentRow = exportExcelInfo.CurrentRow;
            titleCellStyle = GetTitleCellStyle(workbook);
            updatedTimeCellStyle = GetCreatedTimeCellStyle(workbook);

            var productStatus = _settingService.AllInformHeadlineSettings()
                .ProductStatus
                .ToDictionary(k => k.Id, p => p.Name);

            InformHeadline inform;
            for (int i = 0; i < statusLength; i++)
            {
                inform = informs[i];
                currentColumn = exportExcelInfo.StartColumn;

                IRow row = exportExcelInfo.Worksheet.GetRow(currentRow) ?? exportExcelInfo.Worksheet.CreateRow(currentRow);
                NpoiExcelExtensions.RenderCell(row, currentColumn, inform.HeadlineName, titleCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, inform.InformStatus, titleCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, inform.UpdatedTimeString, updatedTimeCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, inform.Remark.HtmlDecode(), titleCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, inform.Handler, titleCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, inform.Extension, titleCellStyle);
                currentColumn++;
                NpoiExcelExtensions.RenderCell(row, currentColumn, productStatus[inform.ProductStatus], titleCellStyle);
                currentColumn++;
                currentRow++;
            }
        }

        #endregion
    }
}
