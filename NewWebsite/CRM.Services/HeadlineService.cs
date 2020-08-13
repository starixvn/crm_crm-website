using System.Collections.Generic;
using System.Threading.Tasks;
using CRM.Core.Services;
using CRM.Criteria.Headline;
using CRM.Models;

namespace CRM.Services
{
    public interface IHeadlineService
    {
        Task<IEnumerable<BaseModel>> GetAll(string username);
    }

    public class HeadlineService : IHeadlineService
    {
        private readonly IBaseService baseService;

        public HeadlineService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        public async Task<IEnumerable<BaseModel>> GetAll(string username)
        {
            var criteria = new GetAllCriteria { Username = username };
            var result = await baseService.GetDataAsync<BaseModel>(criteria);
            return result;
        }
    }
}