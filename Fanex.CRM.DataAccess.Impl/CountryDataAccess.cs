// --------------------------------------------------------------------------
// <copyright file="CountryDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.DataAccess.Impl
{
    using System.Collections.Generic;
    using System.Data;
    using DNAide.Core;
    using Fanex.CRM.DataAccess.Interface;
    using Fanex.CRM.Entities;
    using Fanex.Data;

    /// <summary>
    /// Country Data Access.
    /// </summary>
    public class CountryDataAccess : ICountryDataAccess
    {
        /// <summary>
        /// Finds all countries.
        /// </summary>
        /// <returns>All countries.</returns>
        public IEnumerable<Country> FindAll()
        {
            var entityList = new List<Country>();
            using (var obj = new ObjectDb("Setting_SettingInfo_GetAllCountry"))
            {
                var ds = obj.ExecuteDataSet();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var dataTableCountry = ds.Tables[0];
                    foreach (DataRow country in dataTableCountry.Rows)
                    {
                        entityList.Add(new Country()
                                {
                                    Id = CastAide.AsInt32(country["CountryID"]),
                                    CountryId = CastAide.AsInt32(country["CountryID"]),
                                    CountryCode = CastAide.AsInt32(country["CountryCode"]),
                                    CountryName = CastAide.AsString(country["CountryName"]),
                                    DisplayOrder = CastAide.AsInt32(country["DisplayOrder"]),
                                    IsActive = CastAide.AsBoolean(country["isActive"])
                                });
                    }
                }
            }

            return entityList;
        }
    }
}
