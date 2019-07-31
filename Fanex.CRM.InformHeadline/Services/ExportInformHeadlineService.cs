namespace Fanex.CRM.InformHeadline.Services
{
    using System;
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
        byte[] Export(IEnumerable<InformHeadline> informHeadlines);
    }

    public partial class ExportInformHeadlineService : IExportInformHeadlineService
    {
        private readonly string[] ColumnHeaders = new[]
        {
            Labels.Title,
            Labels.CreatedDate,
            Labels.NoDirectAccounts,
            Labels.Status,
            Labels.Done,
            Labels.RequestToCallBack,
            Labels.NoNeedToCall,
            Labels.NoReply,
            Labels.Uncontactable,
            Labels.InternalAccount,
            Labels.NeverTouch,
            Labels.KeptForReference
        };

        public byte[] Export(IEnumerable<InformHeadline> informHeadlines)
        {
            const byte TotalColumns = 12;
            const byte HeaderRowIndex = 3;
            const byte BodyRowIndex = 4;
            const byte OpenRowIndex = 4;
            const byte ClosedRowIndex = 5;
            const byte NoDataRowIndex = 6;
            const byte HeaderRowHeight = 30;
            const byte MinimumWidth = 20;
            const byte MaximumWidth = 50;
            var templatePath = HttpContext.Current.Server.MapPath(@"~/ExcelTemplates/InformHeadline.xlsx");
            using (var templatePackage = new ExcelPackage(new FileInfo(templatePath)))
            {
                using (var xlPackage = new ExcelPackage())
                {
                    ExcelWorksheet template = templatePackage.Workbook.Worksheets[1];
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add(Labels.InformHeadlines);
                    var rowIndex = 1;
                    var colIndex = 1;

                    // title
                    template.Cells[rowIndex, colIndex, rowIndex + 1, TotalColumns].Copy(worksheet.Cells[rowIndex, colIndex, rowIndex + 1, TotalColumns]);
                    worksheet.Cells[rowIndex, colIndex].Value = Labels.InformHeadlinesCases;
                    worksheet.Row(rowIndex).Height = HeaderRowHeight;

                    // header
                    rowIndex = HeaderRowIndex;
                    template.Cells[rowIndex, colIndex, rowIndex, TotalColumns].Copy(worksheet.Cells[rowIndex, colIndex, rowIndex, TotalColumns]);
                    foreach (string columnHeader in ColumnHeaders)
                    {
                        worksheet.Cells[rowIndex, colIndex++].Value = columnHeader;
                    }
                    worksheet.Row(rowIndex).Height = HeaderRowHeight;
                    rowIndex++;

                    // body
                    if (!informHeadlines.Any())
                    {
                        template.Cells[NoDataRowIndex, colIndex, NoDataRowIndex, TotalColumns].Copy(worksheet.Cells[rowIndex, colIndex, rowIndex, TotalColumns]);
                        worksheet.Cells[rowIndex, colIndex].Value = CommonLabels.ResultMessage_NoInfo;
                    }
                    else
                    {
                        foreach (InformHeadline informHeadline in informHeadlines)
                        {
                            colIndex = 1;
                            if (informHeadline.Status.Equals("Open", StringComparison.OrdinalIgnoreCase))
                            {
                                template.Cells[OpenRowIndex, colIndex, OpenRowIndex, TotalColumns].Copy(worksheet.Cells[rowIndex, colIndex, rowIndex, TotalColumns]);
                            }
                            else
                            {
                                template.Cells[ClosedRowIndex, colIndex, ClosedRowIndex, TotalColumns].Copy(worksheet.Cells[rowIndex, colIndex, rowIndex, TotalColumns]);
                            }

                            worksheet.Cells[rowIndex, colIndex++].Value = HttpUtility.HtmlDecode(informHeadline.Title);
                            worksheet.Cells[rowIndex, colIndex++].Value = string.Format("{0:MMM dd, yyyy}", informHeadline.ClientCreatedDate);
                            worksheet.Cells[rowIndex, colIndex++].Value = informHeadline.Total;
                            worksheet.Cells[rowIndex, colIndex++].Value = informHeadline.Status;
                            worksheet.Cells[rowIndex, colIndex++].Value = informHeadline.Done;
                            worksheet.Cells[rowIndex, colIndex++].Value = informHeadline.RequestToCallBack;
                            worksheet.Cells[rowIndex, colIndex++].Value = informHeadline.NoNeedToCall;
                            worksheet.Cells[rowIndex, colIndex++].Value = informHeadline.NoReply;
                            worksheet.Cells[rowIndex, colIndex++].Value = informHeadline.Uncontactable;
                            worksheet.Cells[rowIndex, colIndex++].Value = informHeadline.InternalAccount;
                            worksheet.Cells[rowIndex, colIndex++].Value = informHeadline.NeverTouch;
                            worksheet.Cells[rowIndex, colIndex].Value = informHeadline.KeptForReference;

                            rowIndex++;
                        }
                    }

                    worksheet.Cells.AutoFitColumns(MinimumWidth, MaximumWidth);
                    worksheet.View.FreezePanes(BodyRowIndex, 1);
                    return xlPackage.GetAsByteArray();
                }
            }
        }
    }
}