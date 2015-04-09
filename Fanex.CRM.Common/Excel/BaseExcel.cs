// -----------------------------------------------------------------------
// <copyright file="BaseExcel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Common.Excel
{
    using Fanex.CRM.Common.Extensions;
    using NPOI.HSSF.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class BaseExcel
    {
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
            NpoiExcelExtensions.ResponseStreamExcel(WorkBook, _excelFileName);
        }
    }
}
