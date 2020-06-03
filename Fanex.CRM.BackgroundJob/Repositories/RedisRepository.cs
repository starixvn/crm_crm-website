namespace Fanex.CRM.BackgroundJob.Repositories
{
    using System.Threading.Tasks;
    using Fanex.CRM.BackgroundJob.Models;
    using Microsoft.Extensions.Options;
    using StackExchange.Redis;

    public interface IRedisRepository
    {
        Task Set(string key, string value);

        Task<string> Get(string key);
    }

    public class RedisRepository : IRedisRepository
    {
        private readonly IOptions<RedisConfig> redisConfig;
        private readonly ConnectionMultiplexer connectionMultiplexer;

        public RedisRepository(IOptions<RedisConfig> redisConfig)
        {
            this.redisConfig = redisConfig;
            connectionMultiplexer = ConnectionMultiplexer.Connect(this.redisConfig.Value.Host);
        }

        public async Task<string> Get(string key)
        {
            var db = connectionMultiplexer.GetDatabase();
            return await db.StringGetAsync(GetKey(key));
        }

        public Task Set(string key, string value)
        {
            var db = connectionMultiplexer.GetDatabase();
            return db.StringSetAsync(GetKey(key), value);
        }

        private string GetKey(string jobName)
        {
            return string.Format("{0}:{1}", this.redisConfig.Value.Namespace, jobName);
        }
    }
}