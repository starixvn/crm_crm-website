// -----------------------------------------------------------------------
// <copyright file="InformHeadlineService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.InformHeadline.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.CRM.Common.Helpers;
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

        #region Inform headline list

        public IEnumerable<InformStatus> GetInformStatuses(bool isIncludeKeptForRefence = false)
        {
            try
            {
                return _informHeadlineRepository.GetInformStatuses(new GetInformStatusesCriteria()
                {
                    IsIncludeKeptForRefence = isIncludeKeptForRefence
                });
            }
            catch (ObjectDbException ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        public IEnumerable<InformHeadline> GetInforms(GetInformHeadlinesCriteria criteria)
        {
            return _informHeadlineRepository.GetInforms(criteria);
        }

        public Result UpdateInformStatus(UpdateInformStatusCriteria criteria)
        {
            return _informHeadlineRepository.UpdateInformStatus(criteria);
        }

        public InformHeadlineExcel GetInformHeadlineExcel(GetInformHeadlinesCriteria criteria, string fileName)
        {
            var data = _informHeadlineRepository.GetInforms(criteria);

            return new InformHeadlineExcel(data, fileName);
        }

        #endregion Inform headline list

        #region New Inform headline

        public Result InsertInformHeadline(InsertInformHeadlineCriteria criteria)
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

        public IEnumerable<NewInform> GetNewInforms(GetNewInformsCriteria criteria)
        {
            return _informHeadlineRepository.GetNewInforms(criteria);
        }

        public dynamic GetNewInformSettings()
        {
            return new
            {
                Sites = _settingServices.GetAllSites(new GetAllSiteCriteria()
                {
                    LicenseeType = LicenseeTypeConstants.Credit
                }),
                Currencies = _settingServices.GetAllCurrencies(
                    new GetAllCurrencyCriteria()
                    {
                        IsTest = true
                    },
                    false),
                Statuses = GetInformStatuses(),
                ProductStatuses = GetProductStatuses(),
                Stations = GetStations()
            };
        }

        public NewInformHeadlineExcel GetNewInformHeadlineExcel(GetNewInformsCriteria criteria, string fileName)
        {
            var data = _informHeadlineRepository.GetNewInforms(criteria);

            return new NewInformHeadlineExcel(criteria.Title, data, fileName);
        }

        #endregion New Inform headline

        #region Update Inform headline

        public Result UpdateInformHeadline(UpdateInformHeadlineCriteria criteria)
        {
            criteria.Content = BuildUpdateInformsXml(criteria.Informs);
            return _informHeadlineRepository.UpdateInformHeadline(criteria);
        }

        public IEnumerable<UpdateInform> GetUpdateInforms(GetUpdateInformsCriteria criteria)
        {
            return _informHeadlineRepository.GetUpdateInforms(criteria);
        }

        public Result UpdateContentInformHeadline(UpdateContentInformHeadlineCriteria criteria)
        {
            criteria.Content = BuildContentInformsXml(criteria.Informs);
            return _informHeadlineRepository.UpdateContentInformHeadline(criteria);
        }

        public UpdateInformHeadlineExcel GetUpdateInformHeadlineExcel(GetUpdateInformsCriteria criteria, string fileName)
        {
            var data = _informHeadlineRepository.GetUpdateInforms(criteria);
            var titleDetail = _informHeadlineRepository.GetTileDetail(new GetTitleDetailCriteria()
            {
                TitleId = criteria.TitleId
            });
            return new UpdateInformHeadlineExcel(titleDetail.Title, data, fileName);
        }

        #endregion Update Inform headline

        #region Contact Inform headline

        /// <summary>
        /// Get informs for contact page.
        /// </summary>
        /// <param name="criteria">Get informs criteria.</param>
        /// <returns>List of informs.</returns>
        public IEnumerable<UpdateInform> GetContactInforms(GetContactInformsCriteria criteria)
        {
            return _informHeadlineRepository.GetContactInforms(criteria);
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
        /// Gets all settings.
        /// </summary>
        /// <returns>The object with all settings.</returns>
        public dynamic GetContactInformSettings()
        {
            try
            {
                return new
                {
                    FilterStatuses = GetInformStatuses(true),
                    Statuses = GetInformStatuses(),
                    ProductStatuses = GetProductStatuses(),
                    Stations = GetStations()
                };
            }
            catch (ObjectDbException ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        #endregion Contact Inform headline

        #region Common

        public TitleDetail GetTitleDetail(GetTitleDetailCriteria criteria)
        {
            return _informHeadlineRepository.GetTileDetail(criteria);
        }

        public IEnumerable<Status> GetProductStatuses()
        {
            var productStatuses = _settingServices.GetInformHeadlineStatuses().ToList();
            if (productStatuses.Count > 0 && productStatuses[0].StatusId != 0)
            {
                productStatuses.Insert(
                    0,
                    new Status()
                    {
                        StatusId = 0,
                        Name = Labels.NA,
                        DisplayOrder = 0
                    });
            }

            return productStatuses;
        }

        /// <summary>
        /// Get stations.
        /// </summary>
        /// <returns>List of stations.</returns>
        public IEnumerable<Station> GetStations()
        {
            return _settingServices.GetStations();
        }

        #endregion Common

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
                    "<Headline HeadlineID = '{0}' GroupID = '{1}' StatusID = '{2}' Remark = '{3}' Extension = '{4}' ProductStatus = '{5}' IsKeep='{6}' />",
                    item.HeadlineId != 0 ? item.HeadlineId : (item.Case == 1 || item.Case == 5 ? item.OldCustId : item.NewCustId),
                    item.GroupId,
                    item.InformHeadlineStatusId,
                    HtmlHelper.ReplaceBreaklineXml(item.Remark),
                    item.Extension,
                    item.ProductStatus,
                    item.IsKeep ? 1 : 0);
            }

            return builder.Append("</Content>").ToString();
        }

        /// <summary>
        /// Build update inform XML.
        /// </summary>
        /// <param name="informs">List of inform.</param>
        /// <returns>XML string.</returns>
        private static string BuildContentInformsXml(IList<UpdateInform> informs)
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
                    item.HeadlineId != 0 ? item.HeadlineId : (item.Case == 1 || item.Case == 5 ? item.OldCustId : item.NewCustId),
                    item.InformHeadlineStatusId,
                    HtmlHelper.ReplaceBreaklineXml(item.Remark),
                    item.Extension,
                    item.ProductStatus);
            }

            return builder.Append("</Content>").ToString();
        }
    }
}
