namespace Fanex.CRM.BackgroundJob.Repositories
{
    using System.Threading.Tasks;
    using Fanex.CRM.BackgroundJob.Models;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public interface ICustomerRepository
    {
        Task<CustomerInfo> GetCustomerInfoByUsernameAsync(ICriteria criteria);
    }

    public class CustomerRepository : ICustomerRepository
    {
        public async Task<CustomerInfo> GetCustomerInfoByUsernameAsync(ICriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (IMultiResultReader multi = await db.QueryMultipleAsync(criteria))
                {
                    var customer = await multi.ReadFirstOrDefaultAsync<CustomerInfo>();
                    if (customer != null)
                    {
                        customer.LanguageId = await multi.ReadFirstAsync<int>();
                    }

                    return customer;
                }
            }
        }
    }
}