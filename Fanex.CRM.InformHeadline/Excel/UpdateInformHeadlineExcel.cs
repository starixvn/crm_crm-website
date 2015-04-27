// -----------------------------------------------------------------------
// <copyright file="UpdateInformHeadlineExcel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.InformHeadline.Excel
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Web;
    using Fanex.CRM.Common.Excel;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.InformHeadline.Models;
    using Fanex.CRM.InformHeadline.Resources;
    using Fanex.CRM.InformHeadline.Services;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Models;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateInformHeadlineExcel : BaseExcel
    {
        private const int KeptForReferenceHeadlineId = 45007;

        private readonly string[] _columnHeaders = new string[]
            {
                Labels.DirectAccount, 
                Labels.Status,
                Labels.UpdatedTime,
                Labels.Remark,
                Labels.Handler,
                Labels.Extension,
                Labels.ProductStatus
            };

        private readonly Color _updatedBackColor = Color.FromArgb(246, 211, 207);
        private readonly Color _headlineBackColor = Color.FromArgb(208, 230, 244);
        private readonly Color _normalBackColor = Color.FromArgb(241, 241, 241);
        private readonly IInformHeadlineService _informService;

        private IList<InformStatus> _informStatuses;
        private IList<Status> _productStatuses;
        private IList<Station> _stations;
        private string _informHeadlineTitle;
        private IList<UpdateInform> _data;

        private ICellStyle _updatedLeftAlignStyle;
        private ICellStyle _updatedCenterAlignStyle;

        private ICellStyle _headlineLeftAlignStyle;
        private ICellStyle _headlineCenterAlignStyle;

        private ICellStyle _normalLeftAlignStyle;
        private ICellStyle _normalCenterAlignStyle;

        public UpdateInformHeadlineExcel(string informHeadlineTitle, IList<UpdateInform> data, string excelFileName)
            : base(excelFileName)
        {
            _informHeadlineTitle = informHeadlineTitle;
            _data = data;
            _informService = new InformHeadlineService();
            _informStatuses = _informService.GetInformStatuses();
            _productStatuses = _informService.GetProductStatuses();
            _stations = _informService.GetStations();
        }

        /// <summary>
        /// Build excel workbook.
        /// </summary>
        public override void BuildWorkBook()
        {
            ExportExcelInfo exportExcelInfo = new ExportExcelInfo
            {
                ExcelFile = WorkBook,
                Worksheet = WorkBook.CreateSheet(ExcelFileName),
                StartRow = 0,
                StartColumn = 0,
                CurrentRow = 0,
                MaxColumnIndex = _columnHeaders.Length - 1
            };

            GenerateTitle(exportExcelInfo, Labels.InformHeadlinesCases);
            GenerateHeader(exportExcelInfo);
            if (_data == null || _data.Count == 0)
            {
                NpoiExcelExtensions.RenderNoInfoMessage(exportExcelInfo, exportExcelInfo.CurrentRow);
            }
            else
            {
                GenerateBody(exportExcelInfo, _data);
            }
        }

        /// <summary>
        /// Initialize Body Row Styles.
        /// </summary>
        /// <param name="exportExcelInfo">Export Excel Info.</param>
        private void InitializeBodyRowStyles(ExportExcelInfo exportExcelInfo)
        {
            NpoiExcelExtensions.SetColorAtIndex(exportExcelInfo, _updatedBackColor, HSSFColor.Coral.Index);
            NpoiExcelExtensions.SetColorAtIndex(exportExcelInfo, _headlineBackColor, HSSFColor.LightYellow.Index);
            NpoiExcelExtensions.SetColorAtIndex(exportExcelInfo, _normalBackColor, HSSFColor.Grey25Percent.Index);

            _updatedLeftAlignStyle = GetCellStyle(exportExcelInfo.ExcelFile, HSSFColor.Coral.Index);
            _updatedCenterAlignStyle = GetCellStyle(exportExcelInfo.ExcelFile, HSSFColor.Coral.Index, CRMHorizontalAlignment.Center);

            _headlineLeftAlignStyle = GetCellStyle(exportExcelInfo.ExcelFile, HSSFColor.LightYellow.Index);
            _headlineCenterAlignStyle = GetCellStyle(exportExcelInfo.ExcelFile, HSSFColor.LightYellow.Index, CRMHorizontalAlignment.Center);

            _normalLeftAlignStyle = GetCellStyle(exportExcelInfo.ExcelFile, HSSFColor.Grey25Percent.Index);
            _normalCenterAlignStyle = GetCellStyle(exportExcelInfo.ExcelFile, HSSFColor.Grey25Percent.Index, CRMHorizontalAlignment.Center);
        }

        /// <summary>
        /// Generates the casino weekly summary title.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="title">The title.</param>
        private void GenerateTitle(ExportExcelInfo exportExcelInfo, string title)
        {
            NpoiExcelExtensions.SetHeaderColorIndex(exportExcelInfo);
            exportExcelInfo.ResetCurrentColumn();
            exportExcelInfo.SetColumnWidthByFontWidth(exportExcelInfo.CurrentColumn++, 280);
            for (int i = 0; i < exportExcelInfo.MaxColumnIndex; i++)
            {
                exportExcelInfo.SetColumnWidthByFontWidth(exportExcelInfo.CurrentColumn++, 120);
            }

            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam() { Alignment = CRMHorizontalAlignment.Center });
            exportExcelInfo.RenderHeaderTitle(title, headerStyle);
            GenerateInformHeadlineTitle(exportExcelInfo, _informHeadlineTitle);
        }

        private void GenerateInformHeadlineTitle(ExportExcelInfo exportExcelInfo, string title)
        {
            ICellStyle headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam() { Alignment = CRMHorizontalAlignment.Center });
            var cellRangeAddress = new CellRangeAddress(
                exportExcelInfo.CurrentRow, 
                exportExcelInfo.CurrentRow + 1, 
                exportExcelInfo.StartColumn, 
                exportExcelInfo.MaxColumnIndex);
            NpoiExcelExtensions.RenderMergedCells(exportExcelInfo, cellRangeAddress, HttpUtility.HtmlDecode(title), headerStyle);
            exportExcelInfo.CurrentRow += 2;
        }

        /// <summary>
        /// Generates the casino weekly summary header.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        private void GenerateHeader(ExportExcelInfo exportExcelInfo)
        {
            int currentRow = exportExcelInfo.CurrentRow;
            int currentColumn = exportExcelInfo.StartColumn;

            var headerStyle = NpoiExcelExtensions.HeaderStyle(exportExcelInfo.ExcelFile, new ExcelStyleParam());
            headerStyle.WrapText = true;
            var row = exportExcelInfo.Worksheet.CreateRow(currentRow);
            row.HeightInPoints = 30;
            foreach (var columnHeader in _columnHeaders)
            {
                NpoiExcelExtensions.RenderCell(row, currentColumn, columnHeader, headerStyle);
                currentColumn++;
            }

            currentRow += 1;
            exportExcelInfo.CurrentRow = currentRow;
        }

        /// <summary>
        /// Generates the casino weekly summary body.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel info.</param>
        /// <param name="informs">The weekly categories.</param>
        private void GenerateBody(ExportExcelInfo exportExcelInfo, IList<UpdateInform> informs)
        {
            exportExcelInfo.ResetCurrentColumn();
            int currentRow = exportExcelInfo.CurrentRow;
            int currentColumn = exportExcelInfo.CurrentColumn;
            ISheet worksheet = exportExcelInfo.Worksheet;
            InitializeBodyRowStyles(exportExcelInfo);
            foreach (var inform in informs)
            { 
                var row = worksheet.CreateRow(currentRow);
                RenderBodyRow(inform, row, currentColumn);

                currentRow++;
                currentColumn = exportExcelInfo.StartColumn;
            }

            exportExcelInfo.CurrentRow = currentRow;
        }

        /// <summary>
        /// Render body rows.
        /// </summary>
        /// <param name="inform">Inform headline data.</param>
        /// <param name="row">Excel row.</param>
        /// <param name="currentColumn">Current column index.</param>
        private void RenderBodyRow(UpdateInform inform, IRow row, int currentColumn)
        {
            ICellStyle leftAlignStyle;
            ICellStyle centerAlignStyle;

            if (inform.Case > 0)
            {
                leftAlignStyle = _updatedLeftAlignStyle;
                centerAlignStyle = _updatedCenterAlignStyle;
            }
            else if (inform.GroupId > 0)
            {
                leftAlignStyle = _headlineLeftAlignStyle;
                centerAlignStyle = _headlineCenterAlignStyle;
            }
            else
            {
                leftAlignStyle = _normalLeftAlignStyle;
                centerAlignStyle = _normalCenterAlignStyle;
            }

            var username = string.IsNullOrEmpty(inform.UserName) == false ? inform.UserName :
                    (inform.Case == 1 || inform.Case == 5 ? inform.OldUserName : inform.NewUserName);

            var isKeepForReference = inform.InformHeadlineStatusId == KeptForReferenceHeadlineId;

            var informStatusName = isKeepForReference ? Labels.KeptForReference : GetInformStatusName(inform.InformHeadlineStatusId);
            var stationName = isKeepForReference ? Labels.KeptForReference : GetStationName(inform.Extension);
            var productStatusName = isKeepForReference ? Labels.KeptForReference : GetProductStatusName(inform.ProductStatus);

            NpoiExcelExtensions.RenderCell(row, currentColumn++, username, leftAlignStyle);
            NpoiExcelExtensions.RenderCell(row, currentColumn++, informStatusName, centerAlignStyle);
            NpoiExcelExtensions.RenderCell(row, currentColumn++, string.Format("{0:MMM dd, yyyy}", inform.UpdatedTime), leftAlignStyle);
            NpoiExcelExtensions.RenderCell(row, currentColumn++, HttpUtility.HtmlDecode(inform.Remark), leftAlignStyle);
            NpoiExcelExtensions.RenderCell(row, currentColumn++, inform.Handler, leftAlignStyle);
            NpoiExcelExtensions.RenderCell(row, currentColumn++, stationName, centerAlignStyle);
            NpoiExcelExtensions.RenderCell(row, currentColumn++, productStatusName, centerAlignStyle);
        }

        private ICellStyle GetCellStyle(HSSFWorkbook workbook, short color = -1, CRMHorizontalAlignment align = CRMHorizontalAlignment.Left)
        {
            return NpoiExcelExtensions.TextStyle(
                workbook,
                color < 0 ? NpoiExcelExtensions.BackgroundColorIndex : color,
                false,
                align);
        }

        private string GetInformStatusName(int id)
        {
            var item = _informStatuses.Where(x => x.SettingId == id).FirstOrDefault();
            return item == null ? string.Empty : item.SettingValue;
        }

        private string GetProductStatusName(int id)
        {
            var item = _productStatuses.Where(x => x.StatusId == id).FirstOrDefault();
            return item == null ? Labels.NA : item.Name;
        }

        private string GetStationName(int id)
        {
            var item = _stations.Where(x => x.StationId == id).FirstOrDefault();
            return item == null ? Labels.NA : item.Name;
        }
    }
}
