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
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models.Npoi;
    using Fanex.CRM.Infrastructure.Helpers;
    using Fanex.Data.Repository;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// Service Base.
    /// </summary>
    public class ServiceBase : IServiceBase
    {
        protected readonly IDynamicRepository DynamicRepository;

        public ServiceBase(IDynamicRepository repository = null)
        {
            DynamicRepository = repository ?? new DynamicRepository();
        }

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <value>
        /// The repository.
        /// </value>
        protected IDynamicRepository Repository
        {
            get { return DynamicRepository; }
        }

        /// <summary>
        /// Handler Exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        public void HandleException(Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }

        /// <summary>
        /// Get Report Data with Criteria.
        /// </summary>
        /// <typeparam name="T">Type of Object return.</typeparam>
        /// <param name="criteria">Criteria to Query DB.</param>
        /// <returns>Enumerable of Type T.</returns>
        public virtual IEnumerable<T> GetDynamicData<T>(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    return DynamicRepository.Fetch<T>(criteria);
                }

                return new List<T>();
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return null;
            }
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
            CellRangeAddress cellRangeAddress = new CellRangeAddress(
                exportExcelInfo.CurrentRow,
                exportExcelInfo.CurrentRow + 1,
                exportExcelInfo.StartColumn,
                exportExcelInfo.MaxColumnIndex);

            NpoiExcelExtensions.RenderMergedCells(exportExcelInfo, cellRangeAddress, reportTitle, headerStyle);
            exportExcelInfo.CurrentRow += 2;

            NpoiExcelExtensions.RenderEmptyRow(exportExcelInfo, exportExcelInfo.CurrentRow);
            exportExcelInfo.CurrentRow++;
        }
    }
}