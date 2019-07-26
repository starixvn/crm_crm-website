namespace Fanex.CRM.Customer.Repositories
{
    using Fanex.CRM.Customer.Criteria.DetailInfo;
    using Fanex.CRM.Customer.Models;

    public interface ICustomerDetailRepository
    {
        CustomerInfo GetCustomerInfoDetailById(GetCustomerInfoDetailByIdCriteria criteria);
    }
}