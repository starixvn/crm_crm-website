// --------------------------------------------------------------------------
// <copyright file="ICountryService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Service.Interface
{
    using System.Collections.Generic;
    using Fanex.CRM.Entities;

    public interface ICountryService
    {
        IEnumerable<Country> GetAll();

        Country GetById(int countryId);
    }
}
