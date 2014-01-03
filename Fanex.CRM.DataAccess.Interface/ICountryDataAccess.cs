// --------------------------------------------------------------------------
// <copyright file="ICountryDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.DataAccess.Interface
{
    using System.Collections.Generic;
    using Fanex.CRM.Entities;

    public interface ICountryDataAccess
    {
        IEnumerable<Country> FindAll();
    }
}
