// --------------------------------------------------------------------------
// <copyright file="IRemarkRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.02.10">Created</change>
// </history>

namespace Fanex.CRM.Remark.Repositories
{
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Remark.Criteria;
    using Fanex.CRM.Remark.Models;
    using Fanex.Data.Repository;

    /// <summary>
    /// Remark repository interface.
    /// </summary>
    public interface IRemarkRepository
    {
        Remarks GetRemarks(ICriteria criteria);

        Result UpdateRemarks(UpdateRemarkCriteria criteria);
    }
}
