// -----------------------------------------------------------------------
// <copyright file="InformHeadlineService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.InformHeadline.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Fanex.CRM.InformHeadline.Criteria;
    using Fanex.CRM.InformHeadline.Excel;
    using Fanex.CRM.InformHeadline.Models;
    using Fanex.CRM.InformHeadline.Repositories;
    using Fanex.CRM.InformHeadline.Resources;
    using Fanex.CRM.Infrastructure.Helpers;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Models;
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
        /// Get informs for new case.
        /// </summary>
        /// <param name="criteria">Get informs criteria.</param>
        /// <returns>List of informs.</returns>
        public IList<NewInform> GetNewInforms(GetNewInformsCriteria criteria)
        {
            try
            {
                return _informHeadlineRepository.GetNewInforms(criteria);
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
            var productStatuses = _settingServices.GetStatuses(ViewTypeConstants.InformHeadline);
            productStatuses.Insert(
                0,
                new Status()
            {
                StatusId = 0,
                Name = Labels.NA,
                DisplayOrder = 0
            });

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
                ProductStatuses = productStatuses,
                Stations = _settingServices.GetStations()
            };
        }

        /// <summary>
        /// Gets all settings.
        /// </summary>
        /// <returns>The object with all settings.</returns>
        public dynamic GetContactInformSettings()
        {
            var productStatuses = _settingServices.GetStatuses(ViewTypeConstants.InformHeadline);
            productStatuses.Insert(
                0,
                new Status()
                {
                    StatusId = 0,
                    Name = Labels.NA,
                    DisplayOrder = 0
                });

            return new
            {
                Statuses = _informHeadlineRepository.GetInformStatuses(new GetInformStatusesCriteria()),
                ProductStatuses = productStatuses,
                Stations = _settingServices.GetStations()
            };
        }

        /// <summary>
        /// Get informs for new case.
        /// </summary>
        /// <param name="criteria">Get informs criteria.</param>
        /// <returns>List of informs.</returns>
        public IList<UpdateInform> GetUpdateInforms(GetUpdateInformsCriteria criteria)
        {
            try
            {
                return _informHeadlineRepository.GetUpdateInforms(criteria);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Get informs for contact page.
        /// </summary>
        /// <param name="criteria">Get informs criteria.</param>
        /// <returns>List of informs.</returns>
        public IList<UpdateInform> GetContactInforms(GetContactInformsCriteria criteria)
        {
            try
            {
                criteria.UserName = criteria.UserName ?? string.Empty;
                return _informHeadlineRepository.GetContactInforms(criteria);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Get title detail.
        /// </summary>
        /// <param name="criteria">Criteria object.</param>
        /// <returns>Title detail.</returns>
        public TitleDetail GetTitleDetail(GetTitleDetailCriteria criteria)
        {
            try
            {
                return _informHeadlineRepository.GetTileDetail(criteria);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Update content of inform headlines.
        /// </summary>
        /// <param name="criteria">Criteria object.</param>
        /// <returns>Result object.</returns>
        public Result UpdateContentInformHeadline(UpdateContentInformHeadlineCriteria criteria)
        {
            try
            {
                criteria.Content = BuildUpdateInformsXml(criteria.Informs);
                return _informHeadlineRepository.UpdateContentInformHeadline(criteria);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Insert inform headlines.
        /// </summary>
        /// <param name="criteria">Criteria object.</param>
        /// <returns>Result object.</returns>
        public Result InsertInformHeadline(InsertInformHeadlineCriteria criteria)
        {
            try
            {
                var result = _informHeadlineRepository.InsertInformHeadline(criteria);

                if (result.Code != 0)
                {
                    return result;
                }

                var updateContentCriteria = new UpdateContentInformHeadlineCriteria();
                updateContentCriteria.Informs = criteria.Informs;
                updateContentCriteria.TitleId = (int)result.ExtendedData;

                return _informHeadlineRepository.UpdateContentInformHeadline(updateContentCriteria);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Update inform headline.
        /// </summary>
        /// <param name="criteria">Criteria object.</param>
        /// <returns>Result object.</returns>
        public Result UpdateInformHeadline(UpdateInformHeadlineCriteria criteria)
        {
            try
            {
                criteria.Content = BuildUpdateInformsXml(criteria.Informs);
                return _informHeadlineRepository.UpdateInformHeadline(criteria);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Build update inform XML.
        /// </summary>
        /// <param name="informs">List of inform.</param>
        /// <returns>XML string.</returns>
        private static string BuildUpdateInformsXml(IList<UpdateInform> informs)
        {
            if (informs == null)
            {
                return string.Empty;
            }

            var builder = new StringBuilder("<Content>");
            foreach (var item in informs)
            {
                builder.AppendFormat(
                    "<Headline HeadlineID = '{0}' StatusID = '{1}' Remark = '{2}' Extension = '{3}' ProductStatus = '{4}' />",
                    item.HeadlineId == 0 ? item.NewCustId : item.HeadlineId,
                    item.InformHeadlineStatusId,
                    item.Remark,
                    item.Extension,
                    item.ProductStatus);
            }

            return builder.Append("</Content>").ToString();
        }
    }
}
