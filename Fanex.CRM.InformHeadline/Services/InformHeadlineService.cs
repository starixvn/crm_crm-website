namespace Fanex.CRM.InformHeadline.Services
{
    using System.Collections.Generic;
    using System.Text;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.InformHeadline.Criteria;
    using Fanex.CRM.InformHeadline.Models;
    using Fanex.CRM.InformHeadline.Repositories;
    using Fanex.CRM.InformHeadline.Resources;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Models;
    using Fanex.CRM.Setting.Services;

    public class InformHeadlineService : IInformHeadlineService
    {
        private readonly IInformHeadlineRepository _informHeadlineRepository;
        private readonly ISettingService _settingServices;
        private readonly IBaseService baseService;

        public InformHeadlineService() : this(null, null, null)
        {
        }

        public InformHeadlineService(
            IInformHeadlineRepository informHeadlineRepository,
            IBaseService baseService,
            ISettingService settingServices)
        {
            _informHeadlineRepository = informHeadlineRepository ?? new InformHeadlineRepository();
            _settingServices = settingServices ?? new SettingService();
            this.baseService = baseService ?? new BaseService();
        }

        #region Inform headline list

        public IEnumerable<InformStatus> GetInformStatuses(bool isIncludeKeptForRefence = false)
        {
            return baseService.GetData<InformStatus>(new GetInformStatusesCriteria
            {
                IsIncludeKeptForRefence = isIncludeKeptForRefence
            });
        }

        public IEnumerable<InformHeadline> GetInforms(GetInformHeadlinesCriteria criteria)
        {
            return baseService.GetData<InformHeadline>(criteria);
        }

        public CommandResult UpdateInformStatus(UpdateInformStatusCriteria criteria)
        {
            return baseService.ExecuteNonQuery(criteria);
        }

        #endregion Inform headline list

        #region New Inform headline

        public CommandResult InsertInformHeadline(InsertInformHeadlineCriteria criteria)
        {
            var result = _informHeadlineRepository.InsertInformHeadline(criteria);

            if (result.Code != 0)
            {
                return result;
            }

            var updateContentCriteria = new UpdateContentInformHeadlineCriteria
            {
                Informs = criteria.Informs,
                TitleId = (int)result.ExtendedData
            };

            return UpdateContentInformHeadline(updateContentCriteria);
        }

        public IEnumerable<NewInform> GetNewInforms(GetNewInformsCriteria criteria)
        {
            return baseService.GetData<NewInform>(criteria);
        }

        public dynamic GetNewInformSettings()
        {
            return new
            {
                Sites = _settingServices.GetAllSites(new GetAllSiteCriteria
                {
                    LicenseeType = LicenseeTypeConstants.Credit
                }),
                Currencies = _settingServices.GetAllCurrencies(
                    new GetAllCurrencyCriteria
                    {
                        IsTest = true
                    },
                    false),
                Statuses = GetInformStatuses(),
                ProductStatuses = GetProductStatuses(),
                Stations = GetStations()
            };
        }

        #endregion New Inform headline

        #region Update Inform headline

        public CommandResult UpdateInformHeadline(UpdateInformHeadlineCriteria criteria)
        {
            criteria.Content = BuildUpdateInformsXml(criteria.Informs);
            return baseService.ExecuteNonQuery(criteria);
        }

        public IEnumerable<UpdateInform> GetUpdateInforms(GetUpdateInformsCriteria criteria)
        {
            return baseService.GetData<UpdateInform>(criteria);
        }

        public CommandResult UpdateContentInformHeadline(UpdateContentInformHeadlineCriteria criteria)
        {
            criteria.Content = BuildContentInformsXml(criteria.Informs);
            return baseService.ExecuteNonQuery(criteria);
        }

        #endregion Update Inform headline

        #region Contact Inform headline

        public IEnumerable<UpdateInform> GetContactInforms(GetContactInformsCriteria criteria)
        {
            criteria.UserName = criteria.UserName ?? string.Empty;
            return baseService.GetData<UpdateInform>(criteria);
        }

        public dynamic GetContactInformSettings()
        {
            return new
            {
                FilterStatuses = GetInformStatuses(true),
                Statuses = GetInformStatuses(),
                ProductStatuses = GetProductStatuses(),
                Stations = GetStations()
            };
        }

        #endregion Contact Inform headline

        #region Common

        public TitleDetail GetTitleDetail(int titleId)
        {
            var criteria = new GetTitleDetailCriteria
            {
                TitleId = titleId
            };
            return baseService.GetDataEntity<TitleDetail>(criteria);
        }

        public IEnumerable<Status> GetProductStatuses()
        {
            var productStatuses = _settingServices.GetInformHeadlineStatuses();
            if (productStatuses.Count > 0 && productStatuses[0].StatusId != 0)
            {
                productStatuses.Insert(
                    0,
                    new Status
                    {
                        StatusId = 0,
                        Name = Labels.NA,
                        DisplayOrder = 0
                    });
            }

            return productStatuses;
        }

        public IEnumerable<Station> GetStations()
        {
            return _settingServices.GetStations();
        }

        #endregion Common

        public IEnumerable<RemovedAccountLog> GetRemovedAccountLogs(GetRemovedAccountLogsCriteria criteria)
        {
            return baseService.GetData<RemovedAccountLog>(criteria);
        }

        public IEnumerable<RemovedAccountLogDetails> GetRemovedAccountLogDetails(GetRemovedAccountLogDetailsCriteria criteria)
        {
            return baseService.GetData<RemovedAccountLogDetails>(criteria);
        }

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