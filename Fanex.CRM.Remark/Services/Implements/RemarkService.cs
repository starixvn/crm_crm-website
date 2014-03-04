// --------------------------------------------------------------------------
// <copyright file="RemarkService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.02.10">Created</change>
// </history>

namespace Fanex.CRM.Remark.Services.Implements
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Remark.Criteria;
    using Fanex.CRM.Remark.Models;
    using Fanex.CRM.Remark.Repositories.Implements;
    using Fanex.CRM.Remark.Repositories.Interfaces;
    using Fanex.CRM.Remark.Resources;
    using Fanex.CRM.Remark.Services.Interfaces;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Remark service class.
    /// </summary>
    public class RemarkService : IRemarkService
    {
        private readonly IRemarkRepository _remarkRepository;

        public RemarkService()
            : this(null)
        {
        }

        public RemarkService(IRemarkRepository remarkRepository)
        {
            _remarkRepository = remarkRepository ?? new RemarkRepository();
        }

        #region IRemarkService Members

        /// <summary>
        /// Gets the remarks.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The remark detail object.</returns>
        public Remarks GetRemarks(ICriteria criteria)
        {
            Remarks customerRemarks = null;
            try
            {
                customerRemarks = _remarkRepository.GetRemarks(criteria);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return customerRemarks;
        }

        /// <summary>
        /// Updates the remarks.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>The result object.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public Result UpdateRemarks(Remarks model)
        {
            try
            {
                StringBuilder remarkBuilder = new StringBuilder(1024);
                remarkBuilder.Append("<Remarks>");
                if (model.SpecialRemarks != null)
                {
                    foreach (var remark in model.SpecialRemarks)
                    {
                        if (remark == null)
                        {
                            continue;
                        }

                        remarkBuilder.Append("<Remark Content=\"").Append(remark).Append("\"")
                            .Append(" IsSpecial=\"1\" />");
                    }
                }

                if (model.NormalRemarks != null)
                {
                    foreach (var remark in model.NormalRemarks)
                    {
                        if (remark == null)
                        {
                            continue;
                        }

                        remarkBuilder.Append("<Remark Content=\"").Append(remark).Append("\"")
                            .Append(" IsSpecial=\"0\" />");
                    }
                }

                remarkBuilder.Append("</Remarks>");

                ICriteria criteria = new UpdateRemarkCriteria
                {
                    CustId = model.CustId,
                    Remarks = remarkBuilder.ToString()
                };

                return _remarkRepository.UpdateRemarks(criteria);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result()
                {
                    Code = (int)Enumerations.ErrorCode.SystemError,
                    Message = Labels.UpdateRemarkUnSuccess
                };
            }
        }

        #endregion
    }
}
