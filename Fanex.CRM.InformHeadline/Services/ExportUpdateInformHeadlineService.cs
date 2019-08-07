namespace Fanex.CRM.InformHeadline.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Fanex.CRM.InformHeadline.Models;
    using Fanex.CRM.InformHeadline.Resources;
    using Fanex.CRM.Setting.Models;
    using OfficeOpenXml;
    using CommonLabels = Common.Resources.Labels;

    public partial interface IExportInformHeadlineService
    {
        byte[] ExportUpdateInform(
            string informHeadlineTitle,
            IEnumerable<UpdateInform> informHeadlines,
            IEnumerable<InformStatus> informStatuses,
            IEnumerable<Status> productStatuses,
            IEnumerable<Station> stations);
    }

    public partial class ExportInformHeadlineService : IExportInformHeadlineService
    {
        private readonly int[] UpdateColumnWidths = new[] { 20, 15, 20, 60, 15, 15, 15 };

        private readonly string[] UpdateColumnHeaders = new[]
        {
            CommonLabels.DirectAccount,
            CommonLabels.Status,
            CommonLabels.UpdatedTime,
            CommonLabels.Remark,
            CommonLabels.Handler,
            CommonLabels.Extension,
            CommonLabels.ProductStatus
        };

        private const int KeptForReferenceStatusId = 45007;
        private const byte UpdateInformTotalColumns = 7;
        private const byte UpdateInformTitleRowIndex = 3;
        private const byte UpdateInformHeaderRowIndex = 4;
        private const byte UpdateInformBodyRowIndex = 5;
        private const byte UpdateInformHeadlineRowIndex = 6;
        private const byte UpdateInformNormalRowIndex = 7;
        private const byte UpdateInformNoDataRowIndex = 8;
        private const byte UpdateInformHeaderRowHeight = 30;

        public byte[] ExportUpdateInform(
            string informHeadlineTitle,
            IEnumerable<UpdateInform> informHeadlines,
            IEnumerable<InformStatus> informStatuses,
            IEnumerable<Status> productStatuses,
            IEnumerable<Station> stations)
        {
            var templatePath = HttpContext.Current.Server.MapPath(@"~/ExcelTemplates/UpdateInformHeadline.xlsx");
            using (var templatePackage = new ExcelPackage(new FileInfo(templatePath)))
            {
                using (var xlPackage = new ExcelPackage())
                {
                    ExcelWorksheet template = templatePackage.Workbook.Worksheets[1];
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add(Labels.InformHeadlines);
                    var rowIndex = 1;
                    var colIndex = 1;

                    // title
                    template.Cells[1, 1, UpdateInformTitleRowIndex, UpdateInformTotalColumns]
                        .Copy(worksheet.Cells[1, 1, UpdateInformTitleRowIndex, UpdateInformTotalColumns]);
                    worksheet.Cells[1, 1].Value = Labels.InformHeadlinesCases;
                    worksheet.Row(1).Height = UpdateInformHeaderRowHeight;

                    worksheet.Cells[UpdateInformTitleRowIndex, 1].Value = HttpUtility.HtmlDecode(informHeadlineTitle);
                    worksheet.Row(UpdateInformTitleRowIndex).Height = UpdateInformHeaderRowHeight;

                    // header
                    rowIndex = UpdateInformHeaderRowIndex;
                    template.Cells[rowIndex, colIndex, rowIndex, UpdateInformTotalColumns].Copy(worksheet.Cells[rowIndex, colIndex, rowIndex, UpdateInformTotalColumns]);
                    foreach (string columnHeader in UpdateColumnHeaders)
                    {
                        worksheet.Cells[rowIndex, colIndex++].Value = columnHeader;
                    }
                    rowIndex++;

                    // body
                    if (!informHeadlines.Any())
                    {
                        template.Cells[UpdateInformNoDataRowIndex, colIndex, UpdateInformNoDataRowIndex, UpdateInformTotalColumns]
                            .Copy(worksheet.Cells[rowIndex, colIndex, rowIndex, UpdateInformTotalColumns]);
                        worksheet.Cells[rowIndex, colIndex].Value = CommonLabels.ResultMessage_NoInfo;
                    }
                    else
                    {
                        foreach (UpdateInform informHeadline in informHeadlines)
                        {
                            WriteBodyRow(template, worksheet, rowIndex, informHeadline, informStatuses, productStatuses, stations);
                            rowIndex++;
                        }
                    }

                    for (int i = 0; i < UpdateColumnWidths.Length; i++)
                    {
                        worksheet.Column(i + 1).Width = UpdateColumnWidths[i];
                    }

                    worksheet.View.FreezePanes(UpdateInformBodyRowIndex, 1);
                    return xlPackage.GetAsByteArray();
                }
            }
        }

        private void WriteBodyRow(
            ExcelWorksheet template,
            ExcelWorksheet worksheet,
            int rowIndex,
            UpdateInform informHeadline,
            IEnumerable<InformStatus> informStatuses,
            IEnumerable<Status> productStatuses,
            IEnumerable<Station> stations)
        {
            var colIndex = 1;
            if (informHeadline.GroupId > 0)
            {
                template.Cells[UpdateInformHeadlineRowIndex, colIndex, UpdateInformHeadlineRowIndex, UpdateInformTotalColumns]
                    .Copy(worksheet.Cells[rowIndex, colIndex, rowIndex, UpdateInformTotalColumns]);
            }
            else
            {
                template.Cells[UpdateInformNormalRowIndex, colIndex, UpdateInformNormalRowIndex, UpdateInformTotalColumns]
                    .Copy(worksheet.Cells[rowIndex, colIndex, rowIndex, UpdateInformTotalColumns]);
            }
            var isKeepForReference = informHeadline.InformHeadlineStatusId == KeptForReferenceStatusId;

            var informStatus = isKeepForReference ? Labels.KeptForReference : GetInformStatus(informStatuses, informHeadline.InformHeadlineStatusId);

            var station = isKeepForReference ? Labels.KeptForReference : GetStation(stations, informHeadline.Extension);
            var productStatus = isKeepForReference ? Labels.KeptForReference : GetProductStatus(productStatuses, informHeadline.ProductStatus);
            worksheet.Cells[rowIndex, colIndex++].Value = informHeadline.UserName;
            worksheet.Cells[rowIndex, colIndex++].Value = informStatus;
            worksheet.Cells[rowIndex, colIndex++].Value = string.Format("{0:MMM dd, yyyy}", informHeadline.ClientUpdatedTime);
            worksheet.Cells[rowIndex, colIndex++].Value = HttpUtility.HtmlDecode(informHeadline.Remark);
            worksheet.Cells[rowIndex, colIndex++].Value = informHeadline.Handler;
            worksheet.Cells[rowIndex, colIndex++].Value = station;
            worksheet.Cells[rowIndex, colIndex].Value = productStatus;
        }

        private static string GetInformStatus(IEnumerable<InformStatus> informStatuses, int id)
        {
            var item = informStatuses.FirstOrDefault(x => x.SettingId == id);
            return item == null ? string.Empty : item.SettingValue;
        }

        private static string GetProductStatus(IEnumerable<Status> productStatuses, int id)
        {
            var item = productStatuses.FirstOrDefault(x => x.StatusId == id);
            return item == null ? Labels.NA : item.Name;
        }

        private static string GetStation(IEnumerable<Station> stations, int id)
        {
            var item = stations.FirstOrDefault(x => x.StationId == id);
            return item == null ? Labels.NA : item.Name;
        }
    }
}