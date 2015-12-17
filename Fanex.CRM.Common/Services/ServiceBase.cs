// --------------------------------------------------------------------------
// <copyright file="ServiceBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.10.9">Created</change>
//   <change who="Ricky.Nguyen" date="2012.11.6">Move</change>
// </history>

namespace Fanex.CRM.Common.Services
{
    using System.Collections.Generic;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.Data.Repository;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Service Base.
    /// </summary>
    public class ServiceBase : IServiceBase
    {
        private readonly IDynamicRepository _dynamicRepository;

        public ServiceBase(IDynamicRepository repository = null)
        {
            _dynamicRepository = repository ?? new DynamicRepository();
        }

        /// <summary>
        /// Get Report Data with Criteria.
        /// </summary>
        /// <typeparam name="T">Type of Object return.</typeparam>
        /// <param name="criteria">Criteria to Query DB.</param>
        /// <returns>Enumerable of Type T.</returns>
        public virtual IEnumerable<T> GetDynamicData<T>(ICriteria criteria)
        {
            ArgumentChecker.IsNotNull(criteria, "criteria");

            if (criteria.IsValid())
            {
                return _dynamicRepository.Fetch<T>(criteria);
            }

            return new List<T>();
        }

        /// <summary>
        /// Exports the header title.
        /// Include empty row.
        /// </summary>
        /// <param name="exportExcelInfo">The export excel information.</param>
        /// <param name="reportTitle">The report title.</param>
        /// <param name="headerStyle">The header style.</param>
        public void ExportHeaderTitle(
            ExportExcelInfo exportExcelInfo,
            string reportTitle,
            ICellStyle headerStyle)
        {
            var cellRangeAddress = exportExcelInfo.GetCellRangeBySpanning(1);
            NpoiExcelExtensions.RenderMergedCells(exportExcelInfo, cellRangeAddress, reportTitle, headerStyle);
            exportExcelInfo.CurrentRow += 2;

            NpoiExcelExtensions.RenderEmptyRow(exportExcelInfo, exportExcelInfo.CurrentRow);
            exportExcelInfo.CurrentRow++;
        }
    }
}