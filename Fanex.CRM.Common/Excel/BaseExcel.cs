namespace Fanex.CRM.Common.Excel
{
    using Fanex.CRM.Common.Extensions;
    using NPOI.HSSF.UserModel;

    public abstract class BaseExcel
    {
        protected const string IntegerFormat = "#,#0";

        private readonly string _excelFileName;

        private readonly HSSFWorkbook _workBook;

        protected BaseExcel(string excelFileName)
        {
            _workBook = new HSSFWorkbook();
            _excelFileName = excelFileName;
        }

        protected string ExcelFileName
        {
            get { return _excelFileName; }
        }

        protected HSSFWorkbook WorkBook
        {
            get { return _workBook; }
        }

        public abstract void BuildWorkBook();

        public virtual void ResponseStreamExcel()
        {
            NpoiExtensions.ResponseStreamExcel(WorkBook, _excelFileName);
        }
    }
}