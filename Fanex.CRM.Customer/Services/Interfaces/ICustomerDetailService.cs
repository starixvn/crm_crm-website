namespace Fanex.CRM.Customer.Services
{
    using Common.Enumerations;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Customer.Criteria.DetailInfo;
    using Fanex.CRM.Customer.Models;

    public interface ICustomerDetailService
    {
        CustomerInfo GetCustomerInfoDetailById(GetCustomerInfoDetailByIdCriteria criteria);

        CommandResult UpdateCustomerInfoDetail(UpdateCustomerInfoDetailCriteria criteria);

        dynamic GetAllSettings(UserRole userRole);
    }
}