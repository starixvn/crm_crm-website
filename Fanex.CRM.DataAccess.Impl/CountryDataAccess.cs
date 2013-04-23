namespace Fanex.CRM.DataAccess.Impl
{
    using System.Collections.Generic;
    using System.Data;
    using DNAide.Core;
    using Fanex.CRM.DataAccess.Interface;
    using Fanex.CRM.Entities;
    using Fanex.Data;

    public class CountryDataAccess : ICountryDataAccess
    {
        public IEnumerable<Country> FindAll()
        {
            var entityList = new List<Country>();
            using (var obj = new ObjectDb("CountrySel"))
            {
                var ds = obj.ExecuteDataSet();
                if(ds!=null && ds.Tables.Count>0 && ds.Tables[0].Rows.Count >0)
                {
                    var dtCountry = ds.Tables[0];
                    foreach (DataRow country in dtCountry.Rows)
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
