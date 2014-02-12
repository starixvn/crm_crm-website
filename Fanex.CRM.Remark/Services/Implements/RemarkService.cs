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
    using System.Collections.Generic;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Remark.Models;
    using Fanex.CRM.Remark.Repositories.Implements;
    using Fanex.CRM.Remark.Repositories.Interfaces;
    using Fanex.CRM.Remark.Services.Interfaces;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Remark service class.
    /// </summary>
    public class RemarkService : IRemarkService
    {
        private readonly IRemarkRepository _repositoryInstance;

        public RemarkService()
            : this(null)
        {
        }

        public RemarkService(IRemarkRepository repositoryInstance)
        {
            _repositoryInstance = repositoryInstance ?? new RemarkRepository();
        }

        #region IRemarkService Members

        public IList<RemarkDetail>[] GetRemarks(ICriteria criteria)
        {
            IList<RemarkDetail> specialRemarkList = new List<RemarkDetail>();
            IList<RemarkDetail> remarkList = new List<RemarkDetail>();

            var remarks = _repositoryInstance.GetRemarks(criteria);
            foreach (RemarkDetail remark in remarks)
            {
                if (remark.IsSpecial)
                {
                    specialRemarkList.Add(remark);
                }
                else
                {
                    remarkList.Add(remark);
                }
            }

            return new IList<RemarkDetail>[] { specialRemarkList, remarkList };
        }

        public Result UpdateRemarks(ICriteria criteria)
        {
            return _repositoryInstance.UpdateRemarks(criteria);
        }

        #endregion
    }
}
