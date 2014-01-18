// --------------------------------------------------------------------------
// <copyright file="CountryService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

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
    using Fanex.CRM.Infrastructure.Service.Implements;
    using Fanex.CRM.Service.Interface;

    /// <summary>
    /// Country service.
    /// </summary>
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

        private IEnumerable<Country> AllCountries
        {
            get { return GetAll(); }
        }

        /// <summary>
        /// Get all countries.
        /// </summary>
        /// <returns>Return result.</returns>
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
                    {
                        CacheService.Set(Constants.AllCountriesKey, countries);
                    }                        
                }
            }
            catch (Exception ex)
            {
                CacheService.Delete(Constants.AllCountriesKey);
                countries = this._repository.FindAll();
                if (countries != null && countries.Count() > 0)
                {
                    CacheService.Set(Constants.AllCountriesKey, countries);
                }           
 
                LogExtensions.ExceptionHandler(ex);
            }

            return countries;
        }

        /// <summary>
        /// Get country by id.
        /// </summary>
        /// <param name="countryId">Country id.</param>
        /// <returns>Return result.</returns>
        public Country GetById(int countryId)
        {
            var country = new Country();
            try
            {
                ////get from cache
                var countries = AllCountries;
                var query = countries.Where(s => s.CountryId == countryId).ToList();
                if (query != null && query.Count > 0)
                {
                    country = query.First();
                }
                else
                {
                    country = new Country()
                    {
                        Id = 0,
                        CountryName = "N/A",
                        CountryCode = 0
                    };
                }                    
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
