namespace Fanex.CRM.BackgroundJob.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.CRM.BackgroundJob.Criteria;
    using Fanex.CRM.BackgroundJob.Models;
    using Fanex.CRM.BackgroundJob.Repositories;
    using global::CRM.Core.Services;

    public interface ICustomerService
    {
        Task<CustomerInfo> GetCustomerInfoAsync(string username, bool isGuest);

        Task<IEnumerable<GroupHeadline>> GetGroupHeadlineAsync(IEnumerable<int> custIds);
    }

    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository repository;
        private readonly IBaseService baseService;

        public CustomerService(
            ICustomerRepository repository,
            IBaseService baseService)
        {
            this.repository = repository;
            this.baseService = baseService;
        }

        public async Task<CustomerInfo> GetCustomerInfoAsync(string username, bool isGuest)
        {
            var criteria = new GetCustomerInfoByUsernameCriteria
            {
                Username = username,
                IsGuest = isGuest
            };
            return await repository.GetCustomerInfoByUsernameAsync(criteria);
        }

        public async Task<IEnumerable<GroupHeadline>> GetGroupHeadlineAsync(IEnumerable<int> custIds)
        {
            var criteria = new GetGroupHeadlineCriteria
            {
                CustIds = string.Join(",", custIds)
            };
            return await baseService.GetDataAsync<GroupHeadline>(criteria);
        }
    }
}