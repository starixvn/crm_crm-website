namespace Fanex.CRM.Customer.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Customer.Criteria;
    using Fanex.CRM.Customer.Models;
    using Fanex.CRM.Customer.Repositories;
    using Fanex.CRM.Customer.Resources;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Services;

    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ISettingService _settingService;
        private readonly IBaseService baseService;

        public CustomerService() : this(null, null, null)
        {
        }

        public CustomerService(
            ICustomerRepository customerRepositoryInstance,
            IBaseService baseService,
            ISettingService settingService)
        {
            _customerRepository = customerRepositoryInstance ?? new CustomerRepository();
            _settingService = settingService ?? new SettingService();
            this.baseService = baseService ?? new BaseService();
        }

        public CustomerInfo GetCustomerInfoByUsername(GetCustomerInfoByUsernameCriteria criteria)
        {
            var customerInfo = _customerRepository.GetCustomerInfoByUsername(criteria);
            if (customerInfo != null)
            {
                customerInfo.Role.Name = RoleSettingService.GetRoleName(customerInfo.Role.Id);
                var siteSetting = _settingService.GetSettingById(new GetSettingByIdCriteria
                {
                    SettingId = customerInfo.SiteSetting.Id
                });
                customerInfo.SiteSetting.Name = siteSetting.Value;
                if (!criteria.IsGuest)
                {
                    customerInfo.UplineInfo = GetUplineInfoOfUser(new GetUplineInfoOfUserCriteria
                    {
                        CustomerId = customerInfo.Id
                    });
                }

                AppSession.CurrentCustomer = new SearchedCustomer
                {
                    Name = criteria.Username,
                    IsGuest = criteria.IsGuest
                };
            }
            else
            {
                AppSession.CurrentCustomer = null;
            }

            return customerInfo;
        }

        public IEnumerable<BaseCustomer> GetUplineInfoOfUser(GetUplineInfoOfUserCriteria criteria)
        {
            var uplineInfoResult = new List<BaseCustomer>();

            var rawData = _customerRepository.GetUplineInfoOfUser(criteria).OrderByDescending(r => r.Role.Id);
            foreach (var item in rawData)
            {
                if (criteria.CustomerId == item.Id || item.Role.Id == RoleIdConstants.Headline)
                {
                    continue;
                }

                item.Role.Name = RoleSettingService.GetRoleName(item.Role.Id);
                uplineInfoResult.Add(item);
            }

            var headline = rawData.Where(i => i.Role.Id == RoleIdConstants.Headline)
                                  .Select(i =>
                                  {
                                      i.Role.Name = RoleSettingService.GetRoleName(i.Role.Id);
                                      return i;
                                  })
                                  .FirstOrDefault() ?? NewUnAssignedCustomerInfo();
            uplineInfoResult.Insert(0, headline);

            return uplineInfoResult;
        }

        public CommandResult UpdateCustomerInfo(UpdateCustomerInfoCriteria criteria)
        {
            CommandResult result = baseService.ExecuteNonQuery(criteria);
            if (result.IsSuccessful)
            {
                result.Message = Labels.CustomerInfo_UpdateSuccess;
            }

            return result;
        }

        public IEnumerable<string> QueryCustomerByUsername(CustomerSuggestionCriteria criteria)
        {
            return baseService.GetData<string>(criteria);
        }

        public bool UpdateCustomerSuggestion(UpdateCustomerSuggestionCriteria criteria)
        {
            return _customerRepository.UpdateCustomerSuggestion(criteria);
        }

        public IEnumerable<BaseModel> VerifyUsername(VerifyRelatedCustomerCriteria criteria)
        {
            return _customerRepository.VerifyUsername(criteria);
        }

        private CustomerInfo NewUnAssignedCustomerInfo()
        {
            return new CustomerInfo
            {
                Name = "Unassigned",
                IsActive = false,
                IsClosed = false,
                IsSuspended = false,
                Role = new BaseModel
                {
                    Id = RoleIdConstants.Headline,
                    Name = RoleSettingService.GetRoleName(RoleIdConstants.Headline)
                }
            };
        }
    }
}