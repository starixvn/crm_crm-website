// -----------------------------------------------------------------------
// <copyright file="InformHeadlineService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.InformHeadline.Services
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.InformHeadline.Criteria;
    using Fanex.CRM.InformHeadline.Excel;
    using Fanex.CRM.InformHeadline.Models;
    using Fanex.CRM.InformHeadline.Repositories;
    using Fanex.CRM.Infrastructure.Helpers;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class InformHeadlineService : IInformHeadlineService
    {
        private readonly IInformHeadlineRepository _informHeadlineRepository;
        private readonly ISettingService _settingServices;

        public InformHeadlineService()
            : this(null, null)
        {            
        }

        public InformHeadlineService(IInformHeadlineRepository informHeadlineRepository, ISettingService settingServices)
        {
            _informHeadlineRepository = informHeadlineRepository ?? new InformHeadlineRepository();
            _settingServices = settingServices ?? new SettingService();
        }

        /// <summary>
        /// Get informs.
        /// </summary>
        /// <param name="criteria">Criteria data.</param>
        /// <returns>List of informs.</returns>
        public IList<InformHeadline> GetInforms(GetInformHeadlinesCriteria criteria)
        {
            try
            {
                return _informHeadlineRepository.GetInforms(criteria);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Exports the get casino daily summaries.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="fileName">File name.</param>
        /// <returns>Inform headline excel.</returns>
        public InformHeadlineExcel GetInformHeadlineExcel(GetInformHeadlinesCriteria criteria, string fileName)
        {
            var data = _informHeadlineRepository.GetInforms(criteria);

            return new InformHeadlineExcel(data, fileName);
        }

        /// <summary>
        /// Update inform status.
        /// </summary>
        /// <param name="criteria">Criteria data.</param>
        /// <returns>Result object.</returns>
        public Result UpdateInformStatus(UpdateInformStatusCriteria criteria)
        {
            try
            {
                return _informHeadlineRepository.UpdateInformStatus(criteria);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Get direct accounts for new case.
        /// </summary>
        /// <param name="criteria">Get direct account criteria.</param>
        /// <returns>List of direct account.</returns>
        public IList<DirectAccount> GetDirectAccounts(GetDirectAccountsCriteria criteria)
        {
            try
            {
                return _informHeadlineRepository.GetDirectAccounts(criteria);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets all settings.
        /// </summary>
        /// <returns>The object with all settings.</returns>
        public dynamic GetNewInformSettings()
        {
            return new
            {
                Sites = _settingServices.AllSite(new GetAllSiteCriteria()
                {
                    LicenseeType = LicenseeTypeConstants.Credit
                }),
                Currencies = _settingServices.GetAllCurrency(new GetAllCurrencyCriteria()
                {
                    IsTest = 0
                }),
                Statuses = _informHeadlineRepository.GetInformStatuses(new GetInformStatusesCriteria()),
                ProductStatuses = _settingServices.AllStatus(ViewTypeConstants.InformHeadline)
            };
        }
    }
}
