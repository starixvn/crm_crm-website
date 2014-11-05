// --------------------------------------------------------------------------
// <copyright file="RemarkService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.02.10">Created</change>
// </history>

namespace Fanex.CRM.Remark.Services
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Infrastructure.Helpers;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Remark.Criteria;
    using Fanex.CRM.Remark.Models;
    using Fanex.CRM.Remark.Repositories;
    using Fanex.CRM.Remark.Resources;
    using Fanex.Data.Repository;

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
            try
            {
                Remarks customerRemarks = null;
                customerRemarks = _remarkRepository.GetRemarks(criteria);
                return customerRemarks;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Updates the remarks.
        /// </summary>
        /// <param name="criteria">The model.</param>
        /// <returns>The result object.</returns>
        public Result UpdateRemarks(Remarks criteria)
        {
            try
            {
                StringBuilder remarkBuilder = new StringBuilder(1024);
                remarkBuilder.Append("<Remarks>");
                if (criteria.SpecialRemarks != null)
                {
                    foreach (var remark in criteria.SpecialRemarks)
                    {
                        if (remark == null)
                        {
                            continue;
                        }

                        remarkBuilder.Append("<Remark Content=\"").Append(HtmlHelper.EncodeXml(remark)).Append("\"")
                            .Append(" IsSpecial=\"1\" />");
                    }
                }

                if (criteria.NormalRemarks != null)
                {
                    foreach (var remark in criteria.NormalRemarks)
                    {
                        if (remark == null)
                        {
                            continue;
                        }

                        remarkBuilder.Append("<Remark Content=\"").Append(HtmlHelper.EncodeXml(remark)).Append("\"")
                            .Append(" IsSpecial=\"0\" />");
                    }
                }

                remarkBuilder.Append("</Remarks>");

                UpdateRemarkCriteria updateCriteria = new UpdateRemarkCriteria
                {
                    CustId = criteria.CustId,
                    Remarks = remarkBuilder.ToString()
                };

                return _remarkRepository.UpdateRemarks(updateCriteria);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        #endregion
    }
}
