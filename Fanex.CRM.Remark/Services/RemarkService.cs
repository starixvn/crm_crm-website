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
    using System.Collections.Generic;
    using System.Text;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Remark.Criteria;
    using Fanex.CRM.Remark.Models;
    using Fanex.CRM.Remark.Repositories;
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

        public Remarks GetRemarks(ICriteria criteria)
        {
            return _remarkRepository.GetRemarks(criteria);
        }

        public Result UpdateRemarks(Remarks criteria)
        {
            var xmlContent = new StringBuilder(1024);
            xmlContent.Append("<Remarks>");
            if (criteria.SpecialRemarks != null)
            {
                BuildRemarksXml(xmlContent, criteria.SpecialRemarks, true);
            }

            if (criteria.NormalRemarks != null)
            {
                BuildRemarksXml(xmlContent, criteria.NormalRemarks, false);
            }

            xmlContent.Append("</Remarks>");

            UpdateRemarkCriteria updateCriteria = new UpdateRemarkCriteria
            {
                CustId = criteria.CustId,
                Remarks = xmlContent.ToString()
            };

            return _remarkRepository.UpdateRemarks(updateCriteria);
        }

        private static void BuildRemarksXml(StringBuilder xmlContent, IEnumerable<string> remarks, bool isSpecial)
        {
            var isSpecialText = isSpecial ? " IsSpecial=\"1\" />" : " IsSpecial=\"0\" />";
            foreach (var remark in remarks)
            {
                if (!string.IsNullOrWhiteSpace(remark))
                {
                    xmlContent.Append("<Remark Content=\"").Append(HtmlHelper.EncodeXml(remark)).Append("\"")
                        .Append(isSpecialText);
                }
            }
        }

        #endregion
    }
}
