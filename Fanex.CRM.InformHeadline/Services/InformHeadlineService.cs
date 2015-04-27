// -----------------------------------------------------------------------
// <copyright file="InformHeadlineService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.InformHeadline.Services
{
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
    using Fanex.Data;

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
            catch (ObjectDbException ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Exports the get inform headlines.
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
        /// Exports the get inform headlines.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="fileName">File name.</param>
        /// <returns>Inform headline excel.</returns>
        public NewInformHeadlineExcel GetNewInformHeadlineExcel(GetNewInformsCriteria criteria, string fileName)
        {
            var data = _informHeadlineRepository.GetNewInforms(criteria);

            return new NewInformHeadlineExcel(criteria.Title, data, fileName);
        }

        /// <summary>
        /// Exports the get inform headlines.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="fileName">File name.</param>
        /// <returns>Inform headline excel.</returns>
        public InformHeadlineExcel GetUpdateInformHeadlineExcel(GetInformHeadlinesCriteria criteria, string fileName)
        {
            var data = _informHeadlineRepository.GetInforms(criteria);

            return new InformHeadlineExcel(data, fileName);
        }

        /// <summary>
        /// Exports the get inform headlines.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="fileName">File name.</param>
        /// <returns>Inform headline excel.</returns>
        public UpdateInformHeadlineExcel GetContactInformHeadlineExcel(GetContactInformsCriteria criteria, string fileName)
        {
            var data = _informHeadlineRepository.GetContactInforms(criteria);
            var titleDetail = _informHeadlineRepository.GetTileDetail(new GetTitleDetailCriteria()
            {
                TitleId = criteria.TitleId
            });
            return new UpdateInformHeadlineExcel(titleDetail.Title, data, fileName);
        }

        /// <summary>
        /// Exports the get inform headlines.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="fileName">File name.</param>
        /// <returns>Inform headline excel.</returns>
        public UpdateInformHeadlineExcel GetUpdateInformHeadlineExcel(GetUpdateInformsCriteria criteria, string fileName)
        {
            var data = _informHeadlineRepository.GetUpdateInforms(criteria);
            var titleDetail = _informHeadlineRepository.GetTileDetail(new GetTitleDetailCriteria()
            {
                TitleId = criteria.TitleId
            });
            return new UpdateInformHeadlineExcel(titleDetail.Title, data, fileName);
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
            catch (ObjectDbException ex)
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
            catch (ObjectDbException ex)
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
                Statuses = GetInformStatuses(),
                ProductStatuses = GetProductStatuses(),
                Stations = GetStations()
            };
        }

        /// <summary>
        /// Gets all settings.
        /// </summary>
        /// <returns>The object with all settings.</returns>
        public dynamic GetContactInformSettings()
        {
            return new
            {
                Statuses = GetInformStatuses(),
                ProductStatuses = GetProductStatuses(),
                Stations = GetStations()
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
            catch (ObjectDbException ex)
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
                return _informHeadlineRepository.GetContactInforms(criteria);
            }
            catch (ObjectDbException ex)
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
            catch (ObjectDbException ex)
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
            catch (ObjectDbException ex)
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

                return UpdateContentInformHeadline(updateContentCriteria);
            }
            catch (ObjectDbException ex)
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
            catch (ObjectDbException ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Get inform statuses.
        /// </summary>
        /// <returns>List of status.</returns>
        public IList<InformStatus> GetInformStatuses()
        {
            try
            {
                return _informHeadlineRepository.GetInformStatuses(new GetInformStatusesCriteria());
            }
            catch (ObjectDbException ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Get product statuses.
        /// </summary>
        /// <returns>List of product status.</returns>
        public IList<Status> GetProductStatuses()
        {
            try
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
                return productStatuses;
            }
            catch (ObjectDbException ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Get stations.
        /// </summary>
        /// <returns>List of stations.</returns>
        public IList<Station> GetStations()
        {
            try
            {
                return _settingServices.GetStations();
            }
            catch (ObjectDbException ex)
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
                    "<Headline HeadlineID = '{0}' StatusID = '{1}' Remark = '{2}' Extension = '{3}' ProductStatus = '{4}' IsKeep='{5}' />",
                    item.HeadlineId != 0 ? item.HeadlineId : (item.Case == 1 || item.Case == 5 ? item.OldCustId : item.NewCustId),
                    item.InformHeadlineStatusId,
                    item.Remark,
                    item.Extension,
                    item.ProductStatus,
                    item.IsKeep ? 1 : 0);
            }

            return builder.Append("</Content>").ToString();
        }
    }
}
