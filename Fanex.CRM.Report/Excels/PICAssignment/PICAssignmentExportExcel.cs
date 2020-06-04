namespace Fanex.CRM.Report.Excels.PICAssignment
{
    using System.Collections.Generic;
    using Fanex.CRM.Common.Excel;
    using Fanex.CRM.Models.PicAndHeadline;

    public class PICAssignmentExportExcel : BaseExcel
    {
        private readonly IEnumerable<PICAssignmentInfo> data;

        public PICAssignmentExportExcel(IEnumerable<PICAssignmentInfo> data, string excelFileName)
            : base(excelFileName)
        {
            this.data = data;
        }

        public override void BuildWorkBook()
        {
            throw new System.NotImplementedException();
        }
    }
}