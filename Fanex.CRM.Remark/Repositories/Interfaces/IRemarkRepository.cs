// --------------------------------------------------------------------------
// <copyright file="IRemarkRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.02.10">Created</change>
// </history>

namespace Fanex.CRM.Remark.Repositories.Interfaces
{
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Remark.Models;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Remark repository interface.
    /// </summary>
    public interface IRemarkRepository
    {
        Remarks GetRemarks(ICriteria criteria);

        Result UpdateRemarks(ICriteria criteria);
    }
}
