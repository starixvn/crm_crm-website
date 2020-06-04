namespace Fanex.CRM.BackgroundJob.Services
{
    using System.Threading.Tasks;
    using Fanex.CRM.BackgroundJob.Repositories;
    using Microsoft.Extensions.Logging;

    public interface IJobRunningChecker
    {
        Task<bool> IsRunning(string jobName);

        Task MarkRunning(string jobName);

        Task ClearRunning(string jobName);
    }

    public class JobRunningChecker : IJobRunningChecker
    {
        private readonly IRedisRepository redisRepository;
        private readonly ILogger mlogger;
        private const string NAME_SPACE = "RunningJobs";
        private const string TRUE = "true";
        private const string FALSE = "false";

        public JobRunningChecker(
            IRedisRepository redisRepository,
            ILogger<JobRunningChecker> mlogger)
        {
            this.redisRepository = redisRepository;
            this.mlogger = mlogger;
        }

        public async Task<bool> IsRunning(string jobName)
        {
            var value = await redisRepository.Get(GetKey(jobName));
            return value == TRUE;
        }

        public async Task MarkRunning(string jobName)
        {
            mlogger.LogInformation(jobName + ": Running");
            await redisRepository.Set(GetKey(jobName), TRUE);
        }

        public async Task ClearRunning(string jobName)
        {
            mlogger.LogInformation(jobName + ": Completed");
            await redisRepository.Set(GetKey(jobName), FALSE);
        }

        private static string GetKey(string jobName)
        {
            return string.Format("{0}:{1}", NAME_SPACE, jobName);
        }
    }
}