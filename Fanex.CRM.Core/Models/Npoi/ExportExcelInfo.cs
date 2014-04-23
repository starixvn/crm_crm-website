// --------------------------------------------------------------------------
// <copyright file="ExportExcelInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Infrastructure.Models.Npoi
{
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Base Model.
    /// </summary>
    public class ExportExcelInfo
    {
        public HSSFWorkbook ExcelFile { get; set; }

        public ISheet Worksheet { get; set; }

        public int StartRow { get; set; }

        public int StartColumn { get; set; }

        public int CurrentRow { get; set; }

        public int CurrentColumn { get; set; }

        public int MaxColumnIndex { get; set; }

        public ICellStyle TextStyle { get; set; }
    }
}
