namespace Fanex.CRM.Service.Impl
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CRM.DataAccess.Impl;
    using Fanex.CRM.DataAccess.Interface;
    using Fanex.CRM.Entities;
    using Fanex.CRM.Infrastructure.Helper;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Service.Interface;

    public class CountryService : ICountryService
    {
        private readonly ICountryDataAccess _repository;

        public CountryService() : this(null)
        {
        }

        public CountryService(ICountryDataAccess repository)
        {
            this._repository = repository ?? new CountryDataAccess();
            
        }

        public IEnumerable<Country> GetAll()
        {
            IEnumerable<Country> countries = new List<Country>();
            try
            {
                countries = CacheService.Get<List<Country>>(Constants.AllCountriesKey);
                if (countries == null)
                {
                    countries = this._repository.FindAll();
                    if (countries != null && countries.Count() > 0)                    
                        CacheService.Set(Constants.AllCountriesKey, countries);
                }
            }
            catch (Exception ex)
            {
                CacheService.Delete(Constants.AllCountriesKey);
                countries = this._repository.FindAll();
                if (countries != null && countries.Count() > 0)                
                    CacheService.Set(Constants.AllCountriesKey, countries);
                

                LogExtensions.ExceptionHandler(ex);
            }
            return countries;
        }

        private IEnumerable<Country> AllCountries
        {
            get { return GetAll(); }
        }

        public Country GetById(int countryId)
        {
            var country = new Country();
            try
            {
                ////get from cache
                var countries = AllCountries;
                var query = countries.Where(s => s.CountryId == countryId).ToList();
                if (query != null && query.Count > 0)
                    country = query.First();
                else
                    country = new Country()
                    {
                        Id = 0,
                        CountryName = "N/A",
                        CountryCode = 0
                    };
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(Country GetById: {0}) - {1}", countryId, ex.Message));
            }
            return country;
        }
    }    
}
