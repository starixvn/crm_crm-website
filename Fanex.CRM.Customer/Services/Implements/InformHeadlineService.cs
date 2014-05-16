// --------------------------------------------------------------------------
// <copyright file="InformHeadlineService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.05.09">Created</change>
// </history>

namespace Fanex.CRM.Customer.Services.Implements
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web;
    using Fanex.CRM.Customer.Criteria;
    using Fanex.CRM.Customer.Models;
    using Fanex.CRM.Customer.Repositories.Implements;
    using Fanex.CRM.Customer.Repositories.Interfaces;
    using Fanex.CRM.Customer.Resources;
    using Fanex.CRM.Customer.Services.Interfaces;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Infrastructure.Models.Npoi;
    using Fanex.NAP.Framework.Data;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// InformHeadlineService class.
    /// </summary>
    public class InformHeadlineService : IInformHeadlineService
    {
        private const string InformHeadlineStatisticSessionKey = "Fanex.CRM.Customer.InformHeadlineStatisticSessionKey";

        private readonly IInformHeadlineRepository _informHeadlineRepository;

        public InformHeadlineService()
            : this(null)
        {
        }

        public InformHeadlineService(IInformHeadlineRepository informHeadlineRepository)
        {
            _informHeadlineRepository = informHeadlineRepository ?? new InformHeadlineRepository(); 
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
                NpoiExcelExtensions.RenderNoInfoMessage(ref exportExcelInfo, exportExcelInfo.CurrentRow);
            }
            else
            {
                NpoiExcelExtensions.SetColorAtIndex(
                    ref exportExcelInfo,
                    NpoiExcelExtensions.EvenRowColor,
                    NpoiExcelExtensions.EvenRowColorIndex);
                NpoiExcelExtensions.SetGrandTotalColorIndex(ref exportExcelInfo);

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
                return null;
            }
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
                    criteria.Content = contentXml.ToString();
                }
                
                return _informHeadlineRepository.UpdateInforms(criteria);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return null;
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

        private string BuildUpdateInformXml(InformHeadline inform)
        {
            string template = "<headline id=\"{0}\" statusid=\"{1}\" headlineid=\"{2}\" remark=\"{3}\" />";
            return string.Format(template, inform.InformId, inform.StatusId, inform.HeadlineId, inform.Remark);
        }

        #region Export excel helper

        /// <summary>
        /// Exports the title.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="title">The title.</param>
        private void ExportTitle(ref ExportExcelInfo exportExcelInfo, string title)
        {
            int currentColumn = exportExcelInfo.StartColumn;
            int currentRow = exportExcelInfo.CurrentRow;
            int columnWidth100 = 100 * NpoiExcelExtensions.FontWidth;
            int columnWidth150 = 150 * NpoiExcelExtensions.FontWidth;

            NpoiExcelExtensions.SetHeaderColorIndex(ref exportExcelInfo);
            ISheet worksheet = exportExcelInfo.Worksheet;
            worksheet.SetColumnWidth(currentColumn++, 280 * NpoiExcelExtensions.FontWidth);
            worksheet.SetColumnWidth(currentColumn++, 180 * NpoiExcelExtensions.FontWidth);
            worksheet.SetColumnWidth(currentColumn++, columnWidth150);
            worksheet.SetColumnWidth(currentColumn++, columnWidth100);
            worksheet.SetColumnWidth(currentColumn++, columnWidth150);
            worksheet.SetColumnWidth(currentColumn++, columnWidth150);
            worksheet.SetColumnWidth(currentColumn++, columnWidth100);
            worksheet.SetColumnWidth(currentColumn++, columnWidth150);
            worksheet.SetColumnWidth(currentColumn++, columnWidth150);
            worksheet.SetColumnWidth(currentColumn++, columnWidth100);

            CellRangeAddress cellRangeAddress = new CellRangeAddress(
                currentRow,
                currentRow + 1,
                exportExcelInfo.StartColumn,
                exportExcelInfo.MaxColumnIndex);
            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());
            NpoiExcelExtensions.RenderMergedCells(ref exportExcelInfo, cellRangeAddress, title, headerStyle);
            currentRow += 2;

            NpoiExcelExtensions.RenderEmptyRow(ref exportExcelInfo, currentRow);
            currentRow++;

            exportExcelInfo.CurrentRow = currentRow;
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

            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Title, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.CreatedTime, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.NoOfHeadlines, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Done, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.RequestCallback, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.NoNeedToCall, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.NoReply, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.Uncontactable, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.InternalAccount, headerStyle);
            NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, Labels.NeverTouch, headerStyle);
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
                NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, inform.Title, titleCellStyle);
                NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, inform.CreatedTimeString, createdTimeCellStyle);
                NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, inform.Total, numberCellStyle);
                NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, inform.Done, numberCellStyle);
                NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, inform.RequestToCallback, numberCellStyle);
                NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, inform.NoNeedToCall, numberCellStyle);
                NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, inform.NoReply, numberCellStyle);
                NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, inform.Uncontactable, numberCellStyle);
                NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, inform.InternalAccount, numberCellStyle);
                NpoiExcelExtensions.RenderCell(ref row, ref currentColumn, inform.NeverTouch, numberCellStyle);
                
                currentRow++;
            }

            exportExcelInfo.CurrentRow = currentRow;
        }

        private short GetRowColorIndex(bool isEven)
        {
            ////return isEven ? NpoiExcelExtensions.EvenRowColorIndex : NpoiExcelExtensions.BackgroundColorIndex;
            return NpoiExcelExtensions.BackgroundColorIndex;
        }

        private ICellStyle GetTitleCellStyle(HSSFWorkbook workbook, bool isEvent = true)
        {
            return NpoiExcelExtensions.TextStyle(
                workbook,
                GetRowColorIndex(isEvent),
                false,
                CRMHorizontalAlignment.Left);
        }

        private ICellStyle GetCreatedTimeCellStyle(HSSFWorkbook workbook, bool isEvent = true)
        {
            return NpoiExcelExtensions.TextStyle(
                workbook,
                GetRowColorIndex(isEvent),
                false,
                CRMHorizontalAlignment.Center);
        }

        private ICellStyle GetNumberCellStyle(HSSFWorkbook workbook, bool isEven = true)
        {
            return NpoiExcelExtensions.IntNumberStyle(
                workbook,
                GetRowColorIndex(isEven),
                false,
                CRMHorizontalAlignment.Right);
        }

        #endregion
    }
}
