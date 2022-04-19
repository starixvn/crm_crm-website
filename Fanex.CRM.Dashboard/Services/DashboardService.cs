namespace Fanex.CRM.Dashboard.Services
{
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Dashboard.Criteria;
    using Fanex.CRM.Dashboard.Models;
    using Fanex.CRM.Dashboard.Repositories;
    using Fanex.CRM.Dashboard.Resources;

    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepositoryInstance;
        private readonly IBaseService baseService;

        public DashboardService() : this(null, null)
        {
        }

        public DashboardService(IDashboardRepository dashboardRepositoryInstance, IBaseService baseService)
        {
            _dashboardRepositoryInstance = dashboardRepositoryInstance ?? new DashboardRepository();
            this.baseService = baseService ?? new BaseService();
        }

        public IEnumerable<string> GetCustomerRemarks(GetCustomerRemarksCriteria criteria)
        {
            return baseService.GetData<string>(criteria);
        }

        public IEnumerable<NewRemarkInfo> GetCustomerNewRemarks(GetCustomerNewRemarksCriteria criteria)
        {
            return baseService.GetData<NewRemarkInfo>(criteria);
        }

        public CustomerInfo GetCustomerInfo(GetCustomerInfoCriteria criteria)
        {
            return _dashboardRepositoryInstance.GetCustomerInfo(criteria);
        }

        public IEnumerable<RedemptionLogInfo> GetRedemptionLogs(GetRedemptionLogsCriteria criteria)
        {
            return _dashboardRepositoryInstance.GetRedemptionLogs(criteria);
        }

        public IEnumerable<CommunicationLogInfo> GetCommunicationLogs(GetCommunicationLogsCriteria criteria)
        {
            return _dashboardRepositoryInstance.GetCommunicationLogs(criteria);
        }

        public CommandResult UpdateCustomerVerifyFlag(UpdateCustomerVerifyFlagCriteria criteria)
        {
            CommandResult result = baseService.ExecuteNonQuery(criteria);
            if (result.IsSuccessful)
            {
                result.Message = Labels.Dashboard_VerifyFlag_UpdateSuccess;
            }

            return result;
        }
    }
}