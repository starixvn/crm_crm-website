// --------------------------------------------------------------------------
// <copyright file="CustomerReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.Implements
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Report.Criteria.CustomerReport;
    using Fanex.CRM.Report.Models.CustomerReport;
    using Fanex.CRM.Report.Repositories.Interfaces;
    using Fanex.CRM.Report.Resources;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Models;
    using Fanex.CRM.Setting.Services.Implements;
    using Fanex.CRM.Setting.Services.Interfaces;

    /// <summary>
    /// Customer Report Service.
    /// </summary>
    public class CustomerReportService : ICustomerReportService
    {
        private ICustomerReportRepository _customerReportRepository;

        private ISettingService _serviceSetting;

        public CustomerReportService()
            : this(null, null)
        {
        }

        public CustomerReportService(
            ICustomerReportRepository customerReportRepository,
            ISettingService serviceSetting)
        {
            _customerReportRepository = customerReportRepository ?? new CustomerReportRepository();
            _serviceSetting = serviceSetting ?? new SettingService();
        }

        public List<CustomerInfo> GetCustomerReport(GetCustomerReportCriteria criteria)
        {
            var customerReport = new List<CustomerInfo>();
            try
            {
                customerReport = _customerReportRepository.GetCustomerReport(criteria);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return customerReport;
        }

        /// <summary>
        /// Gets all site.
        /// </summary>
        /// <returns>All site value.</returns>
        public List<SiteInfo> GetAllSite()
        {
            var allSite = new List<SiteInfo>();
            try
            {
                allSite = _serviceSetting.GetAllSite();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allSite;
        }

        /// <summary>
        /// Gets all title.
        /// </summary>
        /// <returns>All title.</returns>
        public List<SettingInfo> GetAllTitle()
        {
            var allTitle = new List<SettingInfo>();
            try
            {
                allTitle = _serviceSetting.GetAllTitle();
                allTitle.Insert(0, new SettingInfo
                {
                    Value = Labels.All,
                    Id = TitleConstants.All,
                    DisplayOrder = 0
                });
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allTitle;
        }

        /// <summary>
        /// Gets all country.
        /// </summary>
        /// <returns>All country.</returns>
        public List<CountryInfo> GetAllCountry()
        {
            var allCountry = new List<CountryInfo>();
            try
            {
                allCountry = _serviceSetting.GetAllCountry();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allCountry;
        }

        /// <summary>
        /// Gets all language.
        /// </summary>
        /// <returns>All language.</returns>
        public List<SettingInfo> GetAllLanguage()
        {
            var allLanguage = new List<SettingInfo>();
            try
            {
                allLanguage = _serviceSetting.GetAllLanguage();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allLanguage;
        }

        /// <summary>
        /// Gets all customer type.
        /// </summary>
        /// <returns>All customer type.</returns>
        public List<SettingInfo> GetAllCustomerType()
        {
            var allCustomerType = new List<SettingInfo>();
            try
            {
                allCustomerType = _serviceSetting.GetAllCustomerType();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allCustomerType;
        }

        /// <summary>
        /// Gets all VIP type.
        /// </summary>
        /// <returns>All VIP type.</returns>
        public List<SettingInfo> GetAllVIPType()
        {
            var allVIPType = new List<SettingInfo>();
            try
            {
                allVIPType = _serviceSetting.GetAllVIPType();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allVIPType;
        }
    }
}
