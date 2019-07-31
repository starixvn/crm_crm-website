namespace Fanex.CRM.InformHeadline.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Fanex.CRM.InformHeadline.Models;
    using Fanex.CRM.InformHeadline.Resources;
    using OfficeOpenXml;
    using CommonLabels = Common.Resources.Labels;

    public partial interface IExportInformHeadlineService
    {
        byte[] ExportNewInform(
            string informHeadlineTitle,
            IEnumerable<NewInform> informHeadlines,
            IEnumerable<InformStatus> informStatuses);
    }

    public partial class ExportInformHeadlineService : IExportInformHeadlineService
    {
        public byte[] ExportNewInform(
            string informHeadlineTitle,
            IEnumerable<NewInform> informHeadlines,
            IEnumerable<InformStatus> informStatuses)
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
                        foreach (NewInform informHeadline in informHeadlines)
                        {
                            WriteBodyRow(template, worksheet, rowIndex, informHeadline, informStatuses);
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

        private static void WriteBodyRow(
            ExcelWorksheet template,
            ExcelWorksheet worksheet,
            int rowIndex,
            NewInform informHeadline,
            IEnumerable<InformStatus> informStatuses)
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

            worksheet.Cells[rowIndex, colIndex++].Value = informHeadline.UserName;
            worksheet.Cells[rowIndex, colIndex++].Value = GetInformStatus(informStatuses, informHeadline.InformHeadlineStatusId);
            worksheet.Cells[rowIndex, colIndex++].Value = string.Empty;
            worksheet.Cells[rowIndex, colIndex++].Value = string.Empty;
            worksheet.Cells[rowIndex, colIndex++].Value = string.Empty;
            worksheet.Cells[rowIndex, colIndex++].Value = string.Empty;
            worksheet.Cells[rowIndex, colIndex].Value = string.Empty;
        }
    }
}