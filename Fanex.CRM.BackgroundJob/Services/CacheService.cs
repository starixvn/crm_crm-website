namespace Fanex.CRM.BackgroundJob.Services
{
    using System.Threading.Tasks;
    using Fanex.CRM.BackgroundJob.Repositories;
    using Newtonsoft.Json;

    public interface ICacheService
    {
        Task<T> GetAsync<T>(string key);

        Task SetAsync<T>(string key, T value);
    }

    public class CacheService : ICacheService
    {
        private readonly IRedisRepository redisRepository;

        public CacheService(IRedisRepository redisRepository)
        {
            this.redisRepository = redisRepository;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var value = await redisRepository.GetAsync(key);
            if (!value.IsNull)
            {
                try
                {
                    return JsonConvert.DeserializeObject<T>(value);
                }
                catch
                {
                    return default(T);
                }
            }

            return default(T);
        }

        public async Task SetAsync<T>(string key, T value)
        {
            await redisRepository.SetAsync(key, JsonConvert.SerializeObject(value));
        }
    }
}